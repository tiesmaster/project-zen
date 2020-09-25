using System;
using System.Collections.Generic;
using System.Diagnostics;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;

using Tiesmaster.ProjectZen.BagImporter;
using Tiesmaster.ProjectZen.Domain;

namespace Tiesmaster.ProjectZen
{
    public static class Program
    {
        public static void Main()
        {
            var maxFilesToProcess = 10;
            var buildingImporter = new BuildingBagImporter(
                SystemClock.Instance,
                "c:/src/projects/project-zen/tmp/small-zips-unpacked/",
                maxFilesToProcess);

            var panden = buildingImporter.ReadPanden();
            var verblijfsobjecten = buildingImporter.ReadVerblijfsobjecten();

            //var buildings = panden.Select(x => new Building(x.Id, x.ConstructionYear));

            //PersistBuildings(buildings);
        }

        private static void PersistBuildings(IEnumerable<Building> buildings)
        {
            Console.WriteLine("Start persisting Buildings");
            var totalSw = Stopwatch.StartNew();

            using var store = OpenDocumentStore();

            foreach (var (buildingBatch, index) in buildings.Batch(10_000).WithIndex())
            {
                Console.WriteLine($"Saving batch {index}");
                var batchSw = Stopwatch.StartNew();

                using var session = store.OpenSession();
                foreach (var building in buildingBatch)
                {
                    session.Store(building);
                }

                session.SaveChanges();

                Console.WriteLine($"Saved batch {index} (in {batchSw.Elapsed})");
            }

            Console.WriteLine($"Finished persisting Buildings (in {totalSw.Elapsed})");
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