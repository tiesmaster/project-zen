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
        private readonly BagParser _bagParser;
        private readonly IClock _clock;
        private readonly string _bagXmlFilesPath;
        private readonly int _maxFilesToProcess;
        private readonly ILogger _logger;

        public BuildingBagImporter(BagParser bagParser, IClock clock, string bagXmlFilesPath, int maxFilesToProcess = int.MaxValue)
        {
            _bagParser = bagParser;
            _clock = clock;
            _bagXmlFilesPath = bagXmlFilesPath;
            _maxFilesToProcess = maxFilesToProcess;

            _logger = Log.Logger;
        }

        public IEnumerable<BagPand> ReadPanden() => ReadBagObjecten("PND", "Panden", _bagParser.ParsePanden);
        public IEnumerable<BagVerblijfsobject> ReadVerblijfsobjecten() => ReadBagObjecten("VBO", "Verblijfsobjecten", _bagParser.ParseVerblijfsobjecten);
        public IEnumerable<BagNummeraanduiding> ReadNummeraanduidingen() => ReadBagObjecten("NUM", "Nummeraanduidingen", _bagParser.ParseNummeraanduidingen);
        public IEnumerable<BagOpenbareRuimte> ReadOpenbareRuimten() => ReadBagObjecten("OPR", "OpenbareRuimten", _bagParser.ParseOpenbareRuimten);
        public IEnumerable<BagWoonplaats> ReadWoonplaatsen() => ReadBagObjecten("WPL", "Woonplaatsen", _bagParser.ParseWoonplaatsen);

        public IEnumerable<TBagObject> ReadBagObjecten<TBagObject>(
            string bagObjectCode,
            string bagObjectNamePlural,
            Func<string, IEnumerable<TBagObject>> parseBagObjectsFile) where TBagObject : BagBase
        {
            using var _ = LogContext.PushProperty("BagObjectCode", bagObjectCode);

            var referenceInstant = _clock.GetCurrentInstant();

            var bagObjectPath = Path.Combine(_bagXmlFilesPath, bagObjectCode.ToLowerInvariant());

            var bagObjectFiles = Directory
                .EnumerateFiles(bagObjectPath, $"9999{bagObjectCode}*.xml")
                .Take(_maxFilesToProcess)
                .ToList();

            var totalFilesToRead = bagObjectFiles.Count;

            var totalSw = Stopwatch.StartNew();
            _logger.StartReadingBagObjects(bagObjectNamePlural, totalFilesToRead);

            var totalCountRead = 0;
            foreach (var (bagObjectFile, fileIndex) in bagObjectFiles.WithIndex())
            {
                var singleFileSw = Stopwatch.StartNew();
                _logger.StartReadingBagFile(Path.GetFileName(bagObjectFile), fileIndex, totalFilesToRead);

                var bagObjectsBatch = from bagObject in parseBagObjectsFile(bagObjectFile)
                                      where bagObject.IsActive(referenceInstant)
                                      select bagObject;

                foreach (var bagObject in bagObjectsBatch)
                {
                    yield return bagObject;
                    totalCountRead++;
                }

                _logger.FinishedReadingBagFile(singleFileSw, totalSw, fileIndex + 1, totalCountRead);
            }

            _logger.FinishReadingBagObjects(bagObjectNamePlural, totalCountRead, totalFilesToRead, totalSw);
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
}