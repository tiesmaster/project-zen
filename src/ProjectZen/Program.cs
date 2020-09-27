using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

using Serilog;

using Tiesmaster.ProjectZen.BagImporter;

namespace Tiesmaster.ProjectZen
{
    public static class Program
    {
        public static void Main()
        {
            ConfigureLogging();

            var maxFilesToProcess = 1;
            var buildingImporter = new BuildingBagImporter(
                SystemClock.Instance,
                "c:/src/projects/project-zen/tmp/small-zips-unpacked/",
                maxFilesToProcess);

            var nums = buildingImporter.ReadNummeraanduidingen();

            //var buildings = buildingImporter.ReadBuildings();
            PersistToRavenDB(nums);
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        private static void PersistToRavenDB(IEnumerable<object> objects)
        {
            Log.Information("Start persisting");
            var totalSw = Stopwatch.StartNew();

            using var store = OpenDocumentStore();

            var bulkInsert = store.BulkInsert();
            foreach (var (objectBatch, index) in objects.Batch(10_000).WithIndex())
            {
                Log.Information($"Saving batch {index}");
                var batchSw = Stopwatch.StartNew();

                foreach (var building in objectBatch)
                {
                    bulkInsert.Store(building);
                }

                Log.Information($"Saved batch {index} of 10.000 (in {batchSw.Elapsed})");
            }

            Log.Information($"Finished persisting objects (in {totalSw.Elapsed})");
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
                Log.Information("RavenDB database {DatabaseName} is absent. Creating database...", databaseName);
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(databaseName)));
                Log.Information("RavenDB database {DatabaseName} created.", databaseName);
            }
            else
            {
                Log.Debug("RavenDB database {DatabaseName} already exists. Skipping creation.", databaseName);
            }
        }
    }
}