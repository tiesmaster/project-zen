using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;

using Tiesmaster.ProjectZen.BagImporter;
using Tiesmaster.ProjectZen.Domain;
using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen
{
    public class VboComparer : IEqualityComparer<(BagVerblijfsobject vbo, string pandId)>
    {
        public bool Equals([AllowNull] (BagVerblijfsobject vbo, string pandId) x, [AllowNull] (BagVerblijfsobject vbo, string pandId) y)
        {
            return x.pandId == y.pandId;
        }

        public int GetHashCode([DisallowNull] (BagVerblijfsobject vbo, string pandId) obj)
        {
            return obj.pandId.GetHashCode();
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var maxFilesToProcess = 1000;
            var buildingImporter = new BuildingBagImporter(
                SystemClock.Instance,
                "c:/src/projects/project-zen/tmp/small-zips-unpacked/",
                maxFilesToProcess);

            var woonplaatsen = buildingImporter.ReadWoonplaatsen().ToDictionary(x => x.Id, x => x);

            var openbareRuimten = buildingImporter.ReadOpenbareRuimten().ToDictionary(x => x.Id, x => x);

            var nummeraanduidingen = buildingImporter.ReadNummeraanduidingen().ToDictionary(x => x.Id, x => x);

            //var pandVboMapping = buildingImporter
            //    .ReadVerblijfsobjecten()
            //    .SelectMany(x => x.RelatedPanden, (vbo, pandId) => (vbo, pandId))
            //    .GroupBy(x => x.pandId)
            //    .ToDictionary(x => x.Key, x => x.Select(x => x.vbo).ToList());

            var verblijfsobjecten = buildingImporter
                .ReadVerblijfsobjecten()
                .SelectMany(x => x.RelatedPanden, (vbo, pandId) => (vbo, pandId))
                .Distinct(new VboComparer())
                .ToDictionary(x => x.pandId, x => x.vbo);

            var panden = buildingImporter.ReadPanden();

            var buildings = panden.Select(pand => ToBuilding(pand, verblijfsobjecten, nummeraanduidingen, openbareRuimten, woonplaatsen));

            var rotterBuildings = buildings.Where(b => b.Address.City == "Rotterdam").ToList();

            //var buildings = panden.Select(x => new Building(x.Id, x.ConstructionYear));

            PersistToRavenDB(buildings);
        }

        private static Building ToBuilding(
            BagPand pand,
            Dictionary<string, BagVerblijfsobject> verblijfsobjecten,
            Dictionary<string, BagNummeraanduiding> nummeraanduidingen,
            Dictionary<string, BagOpenbareRuimte> openbareRuimten,
            Dictionary<string, BagWoonplaats> woonplaatsen)
        {
            var relatedVbo = verblijfsobjecten[pand.Id];
            var relatedNum = nummeraanduidingen[relatedVbo.RelatedMainAddress];
            var relatedOpr = openbareRuimten[relatedNum.RelatedOpenbareRuimte];
            var relatedWpl = woonplaatsen[relatedOpr.RelatedWoonplaats];

            return new Building(
                pand.Id,
                pand.ConstructionYear,
                new Address(
                    relatedOpr.Name,
                    relatedNum.HouseNumber,         // TODO: Join these...
                    relatedNum.HouseLetter,
                    relatedNum.HouseNumberAddition,
                    relatedNum.PostalCode,
                    relatedWpl.Name));
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