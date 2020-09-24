﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using MoreLinq;

using NodaTime;

using Raven.Client.Documents;

using Tiesmaster.ProjectZen.BagImporter;
using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen
{
    public static class Program
    {
        public static void Main()
        {
            var panden = ReadPanden();

            PersistPanden(panden);
        }

        private static IEnumerable<BagPand> ReadPanden()
        {
            Console.WriteLine("Start reading Panden");
            var totalSw = Stopwatch.StartNew();

            var referenceInstant = SystemClock.Instance.GetCurrentInstant();

            var pandFiles = Directory.EnumerateFiles("c:/src/projects/project-zen/tmp/small-zips-unpacked/", "9999PND08082020-*.xml");

            pandFiles = pandFiles.Take(100);

            var allPanden = new List<BagPand>();
            var batchSw = Stopwatch.StartNew();
            foreach (var (pandFile, index) in pandFiles.WithIndex())
            {
                Console.WriteLine($"Processing {Path.GetFileName(pandFile)}");
                var singleFileSw = Stopwatch.StartNew();

                var panden2 = BagParser.ParsePanden(pandFile);
                foreach (var pand in panden2)
                {
                    if (pand.Version.IsActive(referenceInstant))
                    {
                        allPanden.Add(pand);
                    }
                }

                var totalFilesProcessed = index + 1;
                Console.WriteLine($"Processed in: {singleFileSw.Elapsed} (Average: {batchSw.Elapsed / totalFilesProcessed}) | Total panden: {allPanden.Count:N0}");
            }

            Console.WriteLine($"Finished reading Panden (in {totalSw.Elapsed})");

            return allPanden;
        }

        private static void PersistPanden(IEnumerable<BagPand> panden)
        {
            Console.WriteLine("Start persisting Panden");
            var totalSw = Stopwatch.StartNew();

            using var store = OpenDocumentStore();

            foreach (var (batch, index) in panden.Batch(10_000).WithIndex())
            {
                Console.WriteLine($"Saving batch {index}");
                var batchSw = Stopwatch.StartNew();

                using var session = store.OpenSession();
                foreach (var pand in batch)
                {
                    session.Store(pand);
                }

                session.SaveChanges();

                Console.WriteLine($"Saved batch {index} (in {batchSw.Elapsed})");
            }

            Console.WriteLine($"Finished persisting Panden (in {totalSw.Elapsed})");
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