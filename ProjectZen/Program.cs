using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace ProjectZen
{
    public static class Program
    {
        public static void Main()
        {
            var pandFiles = Directory.EnumerateFiles("c:/src/projects/project-zen/tmp/small-zips-unpacked/", "9999PND08082020-*.xml");

            pandFiles = pandFiles.Take(1);

            var panden = new List<Pand>();
            var totalSw = Stopwatch.StartNew();
            foreach (var (pandFile, index) in pandFiles.WithIndex())
            {
                Console.WriteLine($"Processing {Path.GetFileName(pandFile)}");
                var singleFileSw = Stopwatch.StartNew();

                var pandenNodes = BagParser.ParsePandenFile(pandFile);
                foreach (XmlNode pandXml in pandenNodes)
                {
                    if (BagParser.IsActive(pandXml))
                    {
                        panden.Add(Pand.From(pandXml));
                    }
                }

                var totalFilesProcessed = index + 1;
                Console.WriteLine($"  Processed in: {singleFileSw.Elapsed} (Average: {totalSw.Elapsed / totalFilesProcessed}) | Total panden: {panden.Count:N0}");
            }
        }
    }

    public class BagParser
    {
        public static XmlNodeList ParsePandenFile(string filename)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);

            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            namespaceManager.AddNamespace("gml", "http://www.opengis.net/gml");
            namespaceManager.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            namespaceManager.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");

            var nodes = xmlDocument.SelectNodes("/xb:BAG-Extract-Deelbestand-LVC/xb:antwoord/xb:producten/product_LVC:LVC-product/bag_LVC:Pand", namespaceManager);

            return nodes;
        }

        public static bool IsActive(XmlNode xmlNode)
        {
            return xmlNode["bag_LVC:aanduidingRecordInactief"].InnerText == "N";
        }
    }
}