using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace ProjectZen
{
    class Program
    {
        static void Main()
        {
            //var firstPandenFile = "c:/src/projects/project-zen/tmp/small-zips-unpacked/9999PND08082020-000001.xml";

            //var count = xmlDocument.DocumentElement.ChildNodes.Count;

            //Console.WriteLine($"Count: {count}");
            //Console.WriteLine($"Name: {xmlDocument.DocumentElement.Name}");

            //Console.WriteLine($"Name: {xmlDocument.DocumentElement.FirstChild.Name}");

            //Console.WriteLine(nodes.Count);

            var pandFiles = Directory.EnumerateFiles("c:/src/projects/project-zen/tmp/small-zips-unpacked/", "9999PND08082020-*.xml"); //.Take(100);

            var panden = new List<Pand>();
            var totalSw = Stopwatch.StartNew();
            foreach (var (pandFile, index) in pandFiles.WithIndex())
            {
                Console.WriteLine($"Processing {Path.GetFileName(pandFile)}");
                var singleFileSw = Stopwatch.StartNew();

                var pandenNodes = GetPandenElements(pandFile);
                foreach (XmlNode pandXml in pandenNodes)
                {
                    panden.Add(Pand.From(pandXml));
                    //if (IsActive(pandXml))
                    //{
                    //}
                }

                var totalFilesProcessed = index + 1;
                Console.WriteLine($"  Processed in: {singleFileSw.Elapsed} (Average: {totalSw.Elapsed / totalFilesProcessed}) | Total panden: {panden.Count:N0}");
            }
        }

        private static bool IsActive(XmlNode xmlNode)
        {
            return xmlNode["bag_LVC:aanduidingRecordInactief"].InnerText == "N";
        }

        private static XmlNodeList GetPandenElements(string filename)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);

            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            namespaceManager.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            namespaceManager.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");

            var nodes = xmlDocument.SelectNodes("/xb:BAG-Extract-Deelbestand-LVC/xb:antwoord/xb:producten/product_LVC:LVC-product/bag_LVC:Pand", namespaceManager);

            return nodes;
        }
    }

    public class Pand
    {
        public string Id { get; set; }
        public int ConstructionYear { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, ConstructionYear: {ConstructionYear}";
        }

        public static Pand From(XmlNode xmlNode)
        {
            return new Pand
            {
                Id = xmlNode["bag_LVC:identificatie"].InnerText,
                ConstructionYear = int.Parse(xmlNode["bag_LVC:bouwjaar"].InnerText)
            };
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> source)
        {
            var i = 0;
            foreach (var item in source)
            {
                yield return (item, i++);
            }
        }
    }
}