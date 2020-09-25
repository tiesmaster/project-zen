using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NodaTime;

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

        public IEnumerable<Building> ReadBuildings() => throw new NotImplementedException();

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
            Console.WriteLine($"Start reading {bagObjectNamePlural}");
            var totalSw = Stopwatch.StartNew();

            var referenceInstant = _clock.GetCurrentInstant();

            var bagObjectFiles = Directory.EnumerateFiles(_bagXmlFilesPath, $"9999{bagObjectCode}*.xml");

            bagObjectFiles = bagObjectFiles.Take(_maxFilesToProcess);

            var allBagObjects = new List<TBagObject>();
            var batchSw = Stopwatch.StartNew();
            foreach (var (bagObjectFile, index) in bagObjectFiles.WithIndex())
            {
                Console.WriteLine($"Processing {Path.GetFileName(bagObjectFile)}");
                var singleFileSw = Stopwatch.StartNew();

                allBagObjects.AddRange(from bagObject in parseBagObjectsFile(bagObjectFile)
                                       where bagObject.IsActive(referenceInstant)
                                       select bagObject);

                var totalFilesProcessed = index + 1;
                Console.WriteLine($"Processed in: {singleFileSw.Elapsed} (Average: {batchSw.Elapsed / totalFilesProcessed}) | Total {bagObjectNamePlural}: {allBagObjects.Count:N0}");
            }

            Console.WriteLine($"Finished reading {bagObjectNamePlural} (in {totalSw.Elapsed})");

            return allBagObjects;
        }
    }
}