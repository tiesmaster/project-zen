using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NodaTime;

using Serilog;
using Serilog.Context;

using Tiesmaster.ProjectZen.Domain;
using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public class BuildingBagImporter
    {
        private readonly IClock _clock;
        private readonly string _bagXmlFilesPath;
        private readonly int _maxFilesToProcess;
        private readonly ILogger _logger;

        public BuildingBagImporter(IClock clock, string bagXmlFilesPath, int maxFilesToProcess = int.MaxValue)
        {
            _clock = clock;
            _bagXmlFilesPath = bagXmlFilesPath;
            _maxFilesToProcess = maxFilesToProcess;

            _logger = Log.Logger;
        }

        public IEnumerable<BagPand> ReadPanden() => ReadBagObjecten("PND", "Panden", BagParser.ParsePanden);
        public IEnumerable<BagVerblijfsobject> ReadVerblijfsobjecten() => ReadBagObjecten("VBO", "Verblijfsobjecten", BagParser.ParseVerblijfsobjecten);
        public IEnumerable<BagNummeraanduiding> ReadNummeraanduidingen() => ReadBagObjecten("NUM", "Nummeraanduidingen", BagParser.ParseNummeraanduidingen);
        public IEnumerable<BagOpenbareRuimte> ReadOpenbareRuimten() => ReadBagObjecten("OPR", "OpenbareRuimten", BagParser.ParseOpenbareRuimten);
        public IEnumerable<BagWoonplaats> ReadWoonplaatsen() => ReadBagObjecten("WPL", "Woonplaatsen", BagParser.ParseWoonplaatsen);

        public IEnumerable<TBagObject> ReadBagObjecten<TBagObject>(
            string bagObjectCode,
            string bagObjectNamePlural,
            Func<string, IEnumerable<TBagObject>> parseBagObjectsFile) where TBagObject : BagBase
        {
            using var _ = LogContext.PushProperty("BagObjectCode", bagObjectCode);

            var referenceInstant = _clock.GetCurrentInstant();

            var bagObjectFiles = Directory
                .EnumerateFiles(_bagXmlFilesPath, $"9999{bagObjectCode}*.xml")
                .Take(_maxFilesToProcess)
                .ToList();

            var totalFilesToRead = bagObjectFiles.Count;

            var totalSw = Stopwatch.StartNew();
            _logger.StartReadingBagObjects(bagObjectNamePlural, totalFilesToRead);

            var allBagObjects = new List<TBagObject>();
            foreach (var (bagObjectFile, fileIndex) in bagObjectFiles.WithIndex())
            {
                var singleFileSw = Stopwatch.StartNew();
                _logger.StartReadingBagFile(Path.GetFileName(bagObjectFile), fileIndex, totalFilesToRead);

                allBagObjects.AddRange(from bagObject in parseBagObjectsFile(bagObjectFile)
                                       where bagObject.IsActive(referenceInstant)
                                       select bagObject);

                _logger.FinishedReadingBagFile(singleFileSw, totalSw, fileIndex + 1, allBagObjects.Count);
            }

            _logger.FinishReadingBagObjects(bagObjectNamePlural, allBagObjects.Count, totalFilesToRead, totalSw);

            return allBagObjects;
        }

        // draft code

        public IEnumerable<Building> ReadBuildings()
        {
            var woonplaatsen = ReadWoonplaatsen().ToDictionary(x => x.Id, x => x);

            var openbareRuimten = ReadOpenbareRuimten().ToDictionary(x => x.Id, x => x);

            var nummeraanduidingen = ReadNummeraanduidingen().ToDictionary(x => x.Id, x => x);

            var pandVboMapping = ReadVerblijfsobjecten()
                .SelectMany(x => x.RelatedPanden, (vbo, pandId) => (vbo, pandId))
                .GroupBy(x => x.pandId)
                .ToDictionary(x => x.Key, x => x.Select(x => x.vbo).First());

            var panden = ReadPanden();

            return panden.Select(pand => ToBuilding(pand, pandVboMapping, nummeraanduidingen, openbareRuimten, woonplaatsen));
        }

        private static Building ToBuilding(
            BagPand pand,
            Dictionary<string, BagVerblijfsobject> verblijfsobjecten,
            Dictionary<string, BagNummeraanduiding> nummeraanduidingen,
            Dictionary<string, BagOpenbareRuimte> openbareRuimten,
            Dictionary<string, BagWoonplaats> woonplaatsen)
        {
            Address GetAddress(BagVerblijfsobject relatedVbo)
            {
                var relatedNum = nummeraanduidingen[relatedVbo.RelatedMainAddress];
                var relatedOpr = openbareRuimten[relatedNum.RelatedOpenbareRuimte];
                var relatedWpl = woonplaatsen[relatedOpr.RelatedWoonplaats];

                return new Address(
                    relatedOpr.Name,
                    relatedNum.HouseNumber,         // TODO: Join these...
                    relatedNum.HouseLetter,
                    relatedNum.HouseNumberAddition,
                    relatedNum.PostalCode,
                    relatedWpl.Name);
            }

            var address = verblijfsobjecten.TryGetValue(pand.Id, out var relatedVbo)
                ? GetAddress(relatedVbo)
                : null;

            return new Building(
                pand.Id,
                pand.ConstructionYear,
                address);
        }
    }

    public static class LoggerExtensions
    {
        public static void StartReadingBagFile(this ILogger logger, string fileName, int fileIndex, int totalFilesToRead)
        {
            logger
                .ForContext("BagObjectFileName", fileName)
                .Debug("Processing file {CurrentFileIndex} / {TotalFilesCount}", fileIndex + 1, totalFilesToRead);
        }

        public static void FinishedReadingBagFile(
            this ILogger logger,
            Stopwatch singleFileElapsed,
            Stopwatch totalElapsed,
            int totalFilesRead,
            int totalCountRead)
        {
            logger.Debug(
                "Processed in: {MsPerFile} ms (Average: {MsPerFileAverage} ms) | Total: {TotalCountRead:N0}",
                singleFileElapsed.ElapsedMilliseconds,
                totalElapsed.ElapsedMilliseconds / totalFilesRead,
                totalCountRead);
        }

        public static void StartReadingBagObjects(this ILogger logger, string bagObjectNamePlural, int totalFilesToRead)
        {
            logger.Information("Start reading {BagObjectName} over {TotalFilesCount} files", bagObjectNamePlural, totalFilesToRead);
        }

        public static void FinishReadingBagObjects(
            this ILogger logger,
            string bagObjectNamePlural,
            int totalCountRead,
            int totalFilesRead,
            Stopwatch totalElapsed)
        {
            logger.Information(
                "Finished reading {BagObjectName} ({TotalCountRead:N0} objects across {TotalFilesRead:N0} files " +
                "in {TotalElapsed} | {MsPerFile} ms / file)",
                bagObjectNamePlural,
                totalCountRead,
                totalFilesRead,
                totalElapsed.Elapsed,
                totalElapsed.ElapsedMilliseconds / totalFilesRead);
        }
    }
}