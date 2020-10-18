using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

using Serilog;

using Tiesmaster.ProjectZen.BagImporter;
using Tiesmaster.ProjectZen.Domain;
using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen
{
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
                "c:/src/projects/project-zen/tmp/small-zips-unpacked/",
                maxFilesToProcess);

            //var wpls = buildingImporter.ReadWoonplaatsen();
            //PersistToRavenDB(wpls);

            //var oprs = buildingImporter.ReadOpenbareRuimten();
            //PersistToRavenDB(oprs);

            //var nums = buildingImporter.ReadNummeraanduidingen();
            //PersistToRavenDB(nums);

            //var vbos = buildingImporter.ReadVerblijfsobjecten();
            //PersistToRavenDB(vbos);

            var panden = buildingImporter.ReadPanden();

            using var store = OpenDocumentStore();
            using var session = store.OpenSession();

            var bulkInsert = store.BulkInsert();

            foreach (var pand in panden)
            {
                var building = ConstructBuilding(pand, session);
                bulkInsert.Store(building);
            }

            //var buildings = buildingImporter.ReadBuildings();
            //PersistToRavenDB(vbos);

            Log.Logger.FinishedImport(totalSw);
            Log.CloseAndFlush();
        }

        private static Building ConstructBuilding(BagPand pand, IDocumentSession session)
        {
            var relatedVbos = session.Query<BagVerblijfsobject>().Where(x => x.RelatedPanden.Contains(pand.Id));

            var address = relatedVbos.Any()
                ? GetAddress(relatedVbos.First(), session)
                : null;

            return new Building(
                pand.Id,
                pand.ConstructionYear,
                address);
        }

        private static Address GetAddress(BagVerblijfsobject relatedVbo, IDocumentSession session)
        {
            var relatedNum = session.Load<BagNummeraanduiding>(relatedVbo.RelatedMainAddress);
            var relatedOpr = session.Load<BagOpenbareRuimte>(relatedNum.RelatedOpenbareRuimte);
            var relatedWpl = session.Load<BagWoonplaats>(relatedOpr.RelatedWoonplaats);

            return new Address(
                relatedOpr.Name,
                relatedNum.HouseNumber,
                relatedNum.HouseLetter,
                relatedNum.HouseNumberAddition,
                relatedNum.PostalCode,
                relatedWpl.Name);
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
                Log.Information("RavenDB database {DatabaseName} created", databaseName);
            }
            else
            {
                Log.Debug("RavenDB database {DatabaseName} already exists. Skipping creation", databaseName);
            }
        }
    }
}