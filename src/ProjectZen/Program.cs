using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

using Serilog;

using Tiesmaster.ProjectZen.BagImporter;

namespace Tiesmaster.ProjectZen
{
    public class StreetAndCity
    {
        public string Street { get; set; }
        public string City { get; set; }
    }

    public static class Program
    {
        public static void Main()
        {
            ConfigureLogging();
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

            var totalSw = Stopwatch.StartNew();
            Log.Logger.StartImport();

            var maxFilesToProcess = 10_000;
            var buildingImporter = new BuildingBagImporter(
                new BagParser(),
                SystemClock.Instance,
                "c:/bag/small-zips-unpacked/",
                maxFilesToProcess);

            var wpls = buildingImporter.ReadWoonplaatsen().ToDictionary(x => x.Id, x => x.Name);
            var oprs = buildingImporter.ReadOpenbareRuimten().ToDictionary(x => x.Id, x => new StreetAndCity { Street = x.Name, City = wpls[x.RelatedWoonplaats] });

            var postalCodeDict = new Dictionary<string, StreetAndCity>();
            foreach (var num in buildingImporter.ReadNummeraanduidingen())
            {
                if (num.PostalCode != null && !postalCodeDict.ContainsKey(num.PostalCode))
                {
                    postalCodeDict.Add(num.PostalCode, oprs[num.RelatedOpenbareRuimte]);
                }
            }

            using var f = File.OpenWrite(@"c:\bag\postalcodes.csv");
            using var writer = new StreamWriter(f);

            foreach (var kvp in postalCodeDict)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value.City},{kvp.Value.Street}");
            }

            //PersistToRavenDB(buildingImporter.ReadVerblijfsobjecten());
            //PersistToRavenDB(buildingImporter.ReadPanden());

            //var buildings = buildingImporter.ReadBuildings();
            //PersistToRavenDB(vbos);

            Log.Logger.FinishedImport(totalSw);
            Log.CloseAndFlush();
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("ApplicationName", "ProjectZen")
                .Enrich.WithProperty("ApplicationVersion", GetAssemblyVersion())
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .WriteTo.Seq("http://localhost:5342")
                .CreateLogger();
        }

        private static string GetAssemblyVersion()
        {
            var assemblyInformationalVersion = Assembly
                .GetEntryAssembly()
                .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
                .Single();

            return assemblyInformationalVersion.InformationalVersion;
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;

            Log.Fatal(ex, $"{ex.GetType()}: {ex.Message}");
            Log.CloseAndFlush();
        }

        private static void PersistToRavenDB(IEnumerable<object> objects)
        {
            // Expand logging: add name of type of thing we're persisting via generic method argument? (or just a parameter)
            Log.Information("Start persisting");
            var totalSw = Stopwatch.StartNew();

            using var store = OpenDocumentStore();

            var bulkInsert = store.BulkInsert();
            foreach (var (objectBatch, index) in objects.Batch(10_000).WithIndex())
            {
                // Pull name of BAG object in here via PushProperty
                Log.Debug("Saving batch {batchIndex}", index);
                var batchSw = Stopwatch.StartNew();

                foreach (var building in objectBatch)
                {
                    bulkInsert.Store(building);
                }

                Log.Debug("Saved batch {batchIndex} of 10.000 (in {batchElapsed})", index, batchSw.Elapsed);
            }

            Log.Information("Finished persisting objects (in {totalElapsed})", totalSw.Elapsed);
        }

        private static DocumentStore OpenDocumentStore()
        {
            const string databaseName = "ProjectZen";
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8282" },
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
                Log.Information("RavenDB database {DatabaseName} created", databaseName);
            }
            else
            {
                Log.Debug("RavenDB database {DatabaseName} already exists. Skipping creation", databaseName);
            }
        }
    }
}