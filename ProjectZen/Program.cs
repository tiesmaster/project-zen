using System;
using System.Collections.Generic;
using System.Xml;

namespace ProjectZen
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.Load("c:/src/projects/project-zen/tmp/small-zips-unpacked/9999PND08082020-000001.xml");

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            nsmgr.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            nsmgr.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");

            var count = xmlDoc.DocumentElement.ChildNodes.Count;

            Console.WriteLine($"Count: {count}");
            Console.WriteLine($"Name: {xmlDoc.DocumentElement.Name}");

            Console.WriteLine($"Name: {xmlDoc.DocumentElement.FirstChild.Name}");

            var nodes = xmlDoc.SelectNodes("/xb:BAG-Extract-Deelbestand-LVC/xb:antwoord/xb:producten/product_LVC:LVC-product/bag_LVC:Pand", nsmgr);
            Console.WriteLine(nodes.Count);

            var panden = new List<Pand>();

            foreach (XmlNode pandXml in nodes)
            {
                panden.Add(new Pand
                {
                    Id = pandXml["bag_LVC:identificatie"].InnerText,
                    ConstructionYear = int.Parse(pandXml["bag_LVC:bouwjaar"].InnerText)
                });
            }
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
    }
}