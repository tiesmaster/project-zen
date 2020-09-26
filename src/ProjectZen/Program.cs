using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

using Tiesmaster.ProjectZen.BagImporter;

namespace Tiesmaster.ProjectZen
{
    public static class Program
    {
        public static void Main()
        {
            var maxFilesToProcess = 1;
            var buildingImporter = new BuildingBagImporter(
                SystemClock.Instance,
                "c:/src/projects/project-zen/tmp/small-zips-unpacked/",
                maxFilesToProcess);

            var nums = buildingImporter.ReadNummeraanduidingen();

            //var buildings = buildingImporter.ReadBuildings();
            PersistToRavenDB(nums);
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
            const string databaseName = "ProjectZen";
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = databaseName
            };

            store.Initialize();
            CreateDatabaseIfAbsent(store, databaseName);

            return store;
        }

        private static void CreateDatabaseIfAbsent(DocumentStore documentStore, string databaseName)
        {
            var existingDatabases = documentStore.Maintenance.Server.Send(new GetDatabaseNamesOperation(0, 128));
            if (!existingDatabases.Any(name => name == databaseName))
            {
                //_logger.LogInformation("RavenDB database '{DatabaseName}' is absent. Creating database...", databaseName);
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(databaseName)));
                //_logger.LogInformation("RavenDB database '{DatabaseName}' created.", databaseName);
            }
            else
            {
                //_logger.LogDebug("RavenDB database '{DatabaseName}' already exists. Skipping creation.", databaseName);
            }
        }
    }
}