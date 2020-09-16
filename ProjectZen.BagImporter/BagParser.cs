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
                id: node["bag_LVC:identificatie"].InnerText,
                version: ParseBagVersion(node),
                constructionYear: int.Parse(node["bag_LVC:bouwjaar"].InnerText));
        }

        private static BagVersion ParseBagVersion(XmlNode node)
        {
            return new BagVersion(
                active: !ParseBagBoolean(node["bag_LVC:aanduidingRecordInactief"].InnerText),
                correctionIndex: int.Parse(node["bag_LVC:aanduidingRecordCorrectie"].InnerText),
                validityInterval: ParseValidityInterval(node["bag_LVC:tijdvakgeldigheid"]));
        }

        private static Interval ParseValidityInterval(XmlNode node)
        {
            var start = ParseBagInstant(node["bagtype:begindatumTijdvakGeldigheid"].InnerText);
            var end = ParseBagInstant(node["bagtype:enddatumTijdvakGeldigheid"]?.InnerText);
            return new Interval(start ?? Instant.MinValue, end ?? Instant.MaxValue);
        }

        private static bool ParseBagBoolean(string text)
        {
            return text == "Y";
        }

        private static Instant? ParseBagInstant(string text)
        {
            return text == default
                ? null
                : (Instant?)_bagInstantPattern.Parse(text).Value;
        }
    }
}