using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NodaTime;

using Serilog;

using Tiesmaster.ProjectZen.Domain;
using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public class BuildingBagImporter
    {
        private readonly IClock _clock;
        private readonly string _bagXmlFilesPath;
        private readonly int _maxFilesToProcess;

        public BuildingBagImporter(IClock clock, string bagXmlFilesPath, int maxFilesToProcess = int.MaxValue)
        {
            _clock = clock;
            _bagXmlFilesPath = bagXmlFilesPath;
            _maxFilesToProcess = maxFilesToProcess;
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
            Log.Information("Start reading {BagObjectName}", bagObjectNamePlural);
            var totalSw = Stopwatch.StartNew();

            var referenceInstant = _clock.GetCurrentInstant();

            var bagObjectFiles = Directory.EnumerateFiles(_bagXmlFilesPath, $"9999{bagObjectCode}*.xml");

            bagObjectFiles = bagObjectFiles.Take(_maxFilesToProcess);

            var allBagObjects = new List<TBagObject>();
            var batchSw = Stopwatch.StartNew();
            foreach (var (bagObjectFile, index) in bagObjectFiles.WithIndex())
            {
                Log.Debug("Processing {BagObjectFileName}", Path.GetFileName(bagObjectFile));
                var singleFileSw = Stopwatch.StartNew();

                allBagObjects.AddRange(from bagObject in parseBagObjectsFile(bagObjectFile)
                                       where bagObject.IsActive(referenceInstant)
                                       select bagObject);

                var totalFilesProcessed = index + 1;
                Log.Debug(
                    "Processed in: {SingleFileElapsed} (Average: {AverageElapsedPerFile}) | Total {BagObjectName}: {TotalBagObjectsRead}",
                    singleFileSw.Elapsed,
                    batchSw.Elapsed / totalFilesProcessed,
                    bagObjectNamePlural,
                    allBagObjects.Count);
            }

            Log.Information("Finished reading {BagObjectName} (in {TotalElapsed})", bagObjectNamePlural, totalSw.Elapsed);

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
}