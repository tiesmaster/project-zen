using System;
using System.Collections.Generic;
using System.Diagnostics;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;

using Tiesmaster.ProjectZen.BagImporter;

namespace Tiesmaster.ProjectZen
{
    public static class Program
    {
        public static void Main()
        {
            var maxFilesToProcess = 20;
            var buildingImporter = new BuildingBagImporter(
                SystemClock.Instance,
                "c:/src/projects/project-zen/tmp/small-zips-unpacked/",
                maxFilesToProcess);

            var woonplaatsen = buildingImporter.ReadWoonplaatsen();
            PersistToRavenDB(woonplaatsen);

            //var panden = buildingImporter.ReadPanden();
            //var verblijfsobjecten = buildingImporter.ReadVerblijfsobjecten();
            //var nummeraanduidingen = buildingImporter.ReadNummeraanduidingen();
            //var openbareRuimten = buildingImporter.ReadOpenbareRuimten();

            //var buildings = panden.Select(x => new Building(x.Id, x.ConstructionYear));

            //PersistToRavenDB(buildings);
        }

        private static void PersistToRavenDB(IEnumerable<object> objects)
        {
            Console.WriteLine("Start persisting");
            var totalSw = Stopwatch.StartNew();

            using var store = OpenDocumentStore();

            var bulkInsert = store.BulkInsert();
            foreach (var (objectBatch, index) in objects.Batch(10_000).WithIndex())
            {
                Console.WriteLine($"Saving batch {index}");
                var batchSw = Stopwatch.StartNew();

                foreach (var building in objectBatch)
                {
                    bulkInsert.Store(building);
                }

                Console.WriteLine($"Saved batch {index} of 10.000 (in {batchSw.Elapsed})");
            }

            Console.WriteLine($"Finished persisting objects (in {totalSw.Elapsed})");
        }

        private static DocumentStore OpenDocumentStore()
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "ProjectZen"
            };

            store.Initialize();

            return store;
        }
    }
}