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

        public IEnumerable<BagPand> ReadPanden()
        {
            Console.WriteLine("Start reading Panden");
            var totalSw = Stopwatch.StartNew();

            var referenceInstant = _clock.GetCurrentInstant();

            var pandFiles = Directory.EnumerateFiles(_bagXmlFilesPath, "9999PND*.xml");

            pandFiles = pandFiles.Take(_maxFilesToProcess);

            var allPanden = new List<BagPand>();
            var batchSw = Stopwatch.StartNew();
            foreach (var (pandFile, index) in pandFiles.WithIndex())
            {
                Console.WriteLine($"Processing {Path.GetFileName(pandFile)}");
                var singleFileSw = Stopwatch.StartNew();

                allPanden.AddRange(from pand in BagParser.ParsePanden(pandFile)
                                   where pand.IsActive(referenceInstant)
                                   select pand);

                var totalFilesProcessed = index + 1;
                Console.WriteLine($"Processed in: {singleFileSw.Elapsed} (Average: {batchSw.Elapsed / totalFilesProcessed}) | Total panden: {allPanden.Count:N0}");
            }

            Console.WriteLine($"Finished reading Panden (in {totalSw.Elapsed})");

            return allPanden;
        }
    }
}