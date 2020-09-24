using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

using NodaTime;
using NodaTime.Text;

using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public static class BagParser
    {
        private static readonly InstantPattern _bagInstantPattern = InstantPattern.Create("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);

        public static IEnumerable<BagPand> ParsePanden(string filename)
            => ParsePanden(XmlReader.Create(filename));

        public static IEnumerable<BagPand> ParsePanden(XmlReader xmlReader)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);

            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            namespaceManager.AddNamespace("gml", "http://www.opengis.net/gml");
            namespaceManager.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            namespaceManager.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");

            var nodes = xmlDocument.SelectNodes("/xb:BAG-Extract-Deelbestand-LVC/xb:antwoord/xb:producten/product_LVC:LVC-product/bag_LVC:Pand", namespaceManager);

            return from XmlNode node in nodes
                   select ParsePand(node);
        }

        private static BagPand ParsePand(XmlNode node)
        {
            return new BagPand(
                id: ParseId(node),
                version: ParseBagVersion(node),
                constructionYear: ParseConstructionYear(node));
        }

        private static string ParseId(XmlNode node)
        {
            return node["bag_LVC:identificatie"].InnerText;
        }

        private static int ParseConstructionYear(XmlNode node)
        {
            return ParseInt(node["bag_LVC:bouwjaar"]);
        }

        private static BagVersion ParseBagVersion(XmlNode node)
        {
            return new BagVersion(
                active: ParseBagActive(node),
                correctionIndex: ParseCorrectionIndex(node),
                validityInterval: ParseValidityInterval(node));
        }

        private static bool ParseBagActive(XmlNode node)
        {
            return !ParseBagBoolean(node["bag_LVC:aanduidingRecordInactief"]);
        }

        private static int ParseCorrectionIndex(XmlNode node)
        {
            return ParseInt(node["bag_LVC:aanduidingRecordCorrectie"]);
        }

        private static Interval ParseValidityInterval(XmlNode node)
        {
            var validityNode = node["bag_LVC:tijdvakgeldigheid"];

            var start = ParseBagInstant(validityNode["bagtype:begindatumTijdvakGeldigheid"]);
            var end = ParseBagInstant(validityNode["bagtype:einddatumTijdvakGeldigheid"]);

            return new Interval(start ?? Instant.MinValue, end ?? Instant.MaxValue);
        }

        private static int ParseInt(XmlElement element)
        {
            return int.Parse(element.InnerText);
        }

        private static bool ParseBagBoolean(XmlElement element)
        {
            return element.InnerText == "Y";
        }

        private static Instant? ParseBagInstant(XmlElement element)
        {
            if (element == null)
            {
                return null;
            }

            return _bagInstantPattern.Parse(element.InnerText).Value;
        }
    }
}