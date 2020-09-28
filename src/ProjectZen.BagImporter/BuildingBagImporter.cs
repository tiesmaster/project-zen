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

            var totalSw = Stopwatch.StartNew();

            var referenceInstant = _clock.GetCurrentInstant();

            var bagObjectFiles = Directory
                .EnumerateFiles(_bagXmlFilesPath, $"9999{bagObjectCode}*.xml")
                .Take(_maxFilesToProcess)
                .ToList();

            var totalFilesToRead = bagObjectFiles.Count;

            Log.Information("Start reading {BagObjectName} over {TotalFilesCount} files", bagObjectNamePlural, totalFilesToRead);

            var allBagObjects = new List<TBagObject>();
            var batchSw = Stopwatch.StartNew();
            foreach (var (bagObjectFile, index) in bagObjectFiles.WithIndex())
            {
                var indexOneBased = index + 1;

                Log
                    .ForContext("BagObjectFileName", Path.GetFileName(bagObjectFile))
                    .Debug("Processing file {CurrentFileIndex} / {TotalFilesCount}", indexOneBased, totalFilesToRead);

                var singleFileSw = Stopwatch.StartNew();

                allBagObjects.AddRange(from bagObject in parseBagObjectsFile(bagObjectFile)
                                       where bagObject.IsActive(referenceInstant)
                                       select bagObject);

                Log.Debug(
                    "Processed in: {MsPerFile} ms (Average: {MsPerFileAverage} ms) | Total: {BatchCountRead:N0}",
                    singleFileSw.ElapsedMilliseconds,
                    batchSw.ElapsedMilliseconds / indexOneBased,
                    allBagObjects.Count);
            }

            _logger.LogFinishReadingObjects(bagObjectNamePlural, allBagObjects.Count, totalFilesToRead, totalSw);

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
        public static void LogFinishReadingObjects(
            this ILogger logger,
            string bagObjectNamePlural,
            int totalCountRead,
            int totalFilesToRead,
            Stopwatch totalElapsed)
        {
            logger.Information(
                "Finished reading {BagObjectName} ({TotalCountRead:N0} objects across {TotalFilesRead:N0} files " +
                "in {TotalElapsed} | {MsPerFile} ms / file)",
                bagObjectNamePlural,
                totalCountRead,
                totalFilesToRead,
                totalElapsed.Elapsed,
                totalElapsed.ElapsedMilliseconds / totalFilesToRead);
        }
    }
}