using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Xml;

using NodaTime;
using NodaTime.Text;

using Serilog;

using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public static class BagParser
    {
        private static readonly InstantPattern _bagInstantPattern = InstantPattern.Create("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);

        public static IEnumerable<BagPand> ParsePanden(string filename)
            => ParsePanden(XmlReader.Create(filename));

        public static IEnumerable<BagVerblijfsobject> ParseVerblijfsobjecten(string filename)
            => ParseVerblijfsobjecten(XmlReader.Create(filename));

        public static IEnumerable<BagNummeraanduiding> ParseNummeraanduidingen(string filename)
            => ParseNummeraanduidingen(XmlReader.Create(filename));

        public static IEnumerable<BagOpenbareRuimte> ParseOpenbareRuimten(string filename)
            => ParseOpenbareRuimten(XmlReader.Create(filename));

        public static IEnumerable<BagWoonplaats> ParseWoonplaatsen(string filename)
            => ParseWoonplaatsen(XmlReader.Create(filename));

        public static IEnumerable<BagPand> ParsePanden(XmlReader xmlReader)
        {
            xmlReader.ReadToDescendant("product_LVC:LVC-product");

            var productDepth = xmlReader.Depth;
            xmlReader.Read();

            while (xmlReader.Depth > productDepth)
            {
                yield return ParsePand(xmlReader);
                xmlReader.Read();
            }

            //xmlReader.MoveToElement();

            //return ParseBagObject(
            //    xmlReader,
            //    "Pand",
            //    (node, _) => ParsePand(node));
        }

        private static BagPand ParsePand(XmlReader xmlReader)
        {
            string id = default;

            // bag version
            var active = true;
            var correctionIndex = 0;
            var startInstant = Instant.MinValue;
            var endInstant = Instant.MaxValue;

            // other attributes
            var constructionYear = 0;

            var pandDepth = xmlReader.Depth;
            xmlReader.Read();

            while (xmlReader.Depth > pandDepth)
            {
                switch (xmlReader.Name)
                {
                    case "bag_LVC:identificatie":
                        id = xmlReader.ReadElementContentAsString();
                        break;

                    // BAG version
                    case "bag_LVC:aanduidingRecordInactief":
                        active = xmlReader.ReadElementContentAsString() == "N";
                        break;

                    case "bag_LVC:aanduidingRecordCorrectie":
                        correctionIndex = xmlReader.ReadElementContentAsInt();
                        break;

                    case "bagtype:begindatumTijdvakGeldigheid":
                        startInstant = _bagInstantPattern.Parse(xmlReader.ReadElementContentAsString()).Value;
                        break;

                    case "bagtype:einddatumTijdvakGeldigheid":
                        endInstant = _bagInstantPattern.Parse(xmlReader.ReadElementContentAsString()).Value;
                        break;

                    // other attributes
                    case "bag_LVC:bouwjaar":
                        constructionYear = xmlReader.ReadElementContentAsInt();
                        break;

                    default:
                        xmlReader.Read();
                        break;
                }
            }

            return new BagPand(
                id,
                new BagVersion(
                    active,
                    correctionIndex,
                    new Interval(startInstant, endInstant)),
                constructionYear);
        }

        public static IEnumerable<BagVerblijfsobject> ParseVerblijfsobjecten(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Verblijfsobject",
                ParseVerblijfsobject);
        }

        public static IEnumerable<BagNummeraanduiding> ParseNummeraanduidingen(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Nummeraanduiding",
                (node, _) => ParseNummeraanduiding(node));
        }

        public static IEnumerable<BagOpenbareRuimte> ParseOpenbareRuimten(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "OpenbareRuimte",
                (node, _) => ParseOpenbareRuimte(node));
        }

        public static IEnumerable<BagWoonplaats> ParseWoonplaatsen(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Woonplaats",
                (node, _) => ParseWoonplaats(node));
        }

        private static IEnumerable<TBagObject> ParseBagObject<TBagObject>(
            XmlReader xmlReader,
            string xmlNodeName,
            Func<XmlNode, XmlNamespaceManager, TBagObject> parseBagObject) where TBagObject : BagBase
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);

            var namespaceManager = GetNamespaceManager(xmlDocument);

            var nodes = xmlDocument.SelectNodes(
                $"/xb:BAG-Extract-Deelbestand-LVC/xb:antwoord/xb:producten/product_LVC:LVC-product/bag_LVC:{xmlNodeName}",
                namespaceManager);

            return nodes
                .Cast<XmlNode>()
                .Select(TryParseBagObject)
                .Where(x => x != default);

            TBagObject TryParseBagObject(XmlNode node)
            {
                try
                {
                    return parseBagObject(node, namespaceManager);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Failed to parse {BagObjectName}, XML: {InnerXml}", xmlNodeName, node.InnerXml);
                    return null;
                }
            }
        }

        private static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDocument)
        {
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);

            namespaceManager.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            namespaceManager.AddNamespace("gml", "http://www.opengis.net/gml");
            namespaceManager.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            namespaceManager.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");

            return namespaceManager;
        }

        private static BagPand ParsePand(XmlNode node)
        {
            return new BagPand(
                id: ParseId(node),
                version: ParseBagVersion(node),
                constructionYear: ParseConstructionYear(node));
        }

        private static BagVerblijfsobject ParseVerblijfsobject(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            return new BagVerblijfsobject(
                id: ParseId(node),
                version: ParseBagVersion(node),
                relatedMainAddress: ParseRelatedMainAddress(node),
                relatedAdditionalAddresses: ParseRelatedAdditionalAddresses(node, namespaceManager).ToImmutableList(),
                relatedPanden: ParseRelatedPanden(node, namespaceManager).ToImmutableList());
        }

        private static BagNummeraanduiding ParseNummeraanduiding(XmlNode node)
        {
            return new BagNummeraanduiding(
                id: ParseId(node),
                version: ParseBagVersion(node),
                houseNumber: ParseHouseNumber(node),
                houseLetter: ParseHouseLetter(node),
                houseNumberAddition: ParseHouseNumberAddition(node),
                postalCode: ParsePostalCode(node),
                relatedOpenbareRuimte: ParseRelatedOpenbareRuimte(node));
        }

        private static BagOpenbareRuimte ParseOpenbareRuimte(XmlNode node)
        {
            return new BagOpenbareRuimte(
                id: ParseId(node),
                version: ParseBagVersion(node),
                name: ParseOpenbareRuimteName(node),
                relatedWoonplaats: ParseRelatedWoonplaats(node));
        }

        private static BagWoonplaats ParseWoonplaats(XmlNode node)
        {
            return new BagWoonplaats(
                id: ParseId(node),
                version: ParseBagVersion(node),
                name: ParseWoonplaatsName(node));
        }

        private static string ParseId(XmlNode node)
        {
            return node["bag_LVC:identificatie"].InnerText;
        }

        private static string ParseOpenbareRuimteName(XmlNode node)
        {
            return node["bag_LVC:openbareRuimteNaam"].InnerText;
        }

        private static int ParseHouseNumber(XmlNode node)
        {
            return ParseInt(node["bag_LVC:huisnummer"]);
        }

        private static char? ParseHouseLetter(XmlNode node)
        {
            return node["bag_LVC:huisletter"]?.InnerText?.Single();
        }

        private static string ParseHouseNumberAddition(XmlNode node)
        {
            return node["bag_LVC:huisnummertoevoeging"]?.InnerText;
        }

        private static string ParsePostalCode(XmlNode node)
        {
            return node["bag_LVC:postcode"]?.InnerText;
        }

        private static string ParseRelatedOpenbareRuimte(XmlNode node)
        {
            return node["bag_LVC:gerelateerdeOpenbareRuimte"].InnerText;
        }

        private static string ParseWoonplaatsName(XmlNode node)
        {
            return node["bag_LVC:woonplaatsNaam"].InnerText;
        }

        private static int ParseConstructionYear(XmlNode node)
        {
            return ParseInt(node["bag_LVC:bouwjaar"]);
        }

        private static string ParseRelatedWoonplaats(XmlNode node)
        {
            return node["bag_LVC:gerelateerdeWoonplaats"].InnerText;
        }

        private static string ParseRelatedMainAddress(XmlNode node)
        {
            return node["bag_LVC:gerelateerdeAdressen"]["bag_LVC:hoofdadres"].InnerText;
        }

        private static IEnumerable<string> ParseRelatedAdditionalAddresses(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            var relatedAddresses = node["bag_LVC:gerelateerdeAdressen"];

            var relatedAdditionalAddresses = relatedAddresses.SelectNodes("bag_LVC:nevenadres", namespaceManager);
            foreach (XmlNode relatedAdditionalAddress in relatedAdditionalAddresses)
            {
                yield return relatedAdditionalAddress.InnerText;
            }
        }

        private static IEnumerable<string> ParseRelatedPanden(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            var relatedPanden = node.SelectNodes("bag_LVC:gerelateerdPand", namespaceManager);
            foreach (XmlNode relatedPand in relatedPanden)
            {
                yield return relatedPand.InnerText;
            }
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