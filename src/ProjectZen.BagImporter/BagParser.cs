using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Xml;

using Microsoft.Spatial;

using NodaTime;
using NodaTime.Text;

using Serilog;

using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public class BagParser
    {
        private readonly InstantPattern _bagInstantPattern = InstantPattern.Create("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);
        private readonly bool _ignoreInvalidBagObjects;
        private readonly GmlFormatter _gmlFormatter = GmlFormatter.Create();

        public BagParser(bool ignoreInvalidBagObjects = true)
        {
            _ignoreInvalidBagObjects = ignoreInvalidBagObjects;
        }

        public IEnumerable<BagPand> ParsePanden(string filename)
            => ParsePanden(XmlReader.Create(filename));

        public IEnumerable<BagVerblijfsobject> ParseVerblijfsobjecten(string filename)
            => ParseVerblijfsobjecten(XmlReader.Create(filename));

        public IEnumerable<BagNummeraanduiding> ParseNummeraanduidingen(string filename)
            => ParseNummeraanduidingen(XmlReader.Create(filename));

        public IEnumerable<BagOpenbareRuimte> ParseOpenbareRuimten(string filename)
            => ParseOpenbareRuimten(XmlReader.Create(filename));

        public IEnumerable<BagWoonplaats> ParseWoonplaatsen(string filename)
            => ParseWoonplaatsen(XmlReader.Create(filename));

        public IEnumerable<BagPand> ParsePanden(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Pand",
                (node, _) => ParsePand(node));
        }

        public IEnumerable<BagVerblijfsobject> ParseVerblijfsobjecten(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Verblijfsobject",
                ParseVerblijfsobject);
        }

        public IEnumerable<BagNummeraanduiding> ParseNummeraanduidingen(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Nummeraanduiding",
                (node, _) => ParseNummeraanduiding(node));
        }

        public IEnumerable<BagOpenbareRuimte> ParseOpenbareRuimten(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "OpenbareRuimte",
                (node, _) => ParseOpenbareRuimte(node));
        }

        public IEnumerable<BagWoonplaats> ParseWoonplaatsen(XmlReader xmlReader)
        {
            return ParseBagObject(
                xmlReader,
                "Woonplaats",
                (node, _) => ParseWoonplaats(node));
        }

        private IEnumerable<TBagObject> ParseBagObject<TBagObject>(
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
                catch (Exception ex) when (_ignoreInvalidBagObjects)
                {
                    Log.Warning(ex, "Failed to parse {BagObjectName}, XML: {InnerXml}", xmlNodeName, node.InnerXml);
                    return null;
                }
            }
        }

        private XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDocument)
        {
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);

            namespaceManager.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            namespaceManager.AddNamespace("gml", "http://www.opengis.net/gml");
            namespaceManager.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            namespaceManager.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");

            return namespaceManager;
        }

        private BagPand ParsePand(XmlNode node)
        {
            return new BagPand(
                id: ParseId(node),
                version: ParseBagVersion(node),
                constructionYear: ParseConstructionYear(node),
                geometry: default); // ParseGeometry(node));
        }

        private BagVerblijfsobject ParseVerblijfsobject(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            return new BagVerblijfsobject(
                id: ParseId(node),
                version: ParseBagVersion(node),
                relatedMainAddress: ParseRelatedMainAddress(node),
                relatedAdditionalAddresses: ParseRelatedAdditionalAddresses(node, namespaceManager).ToImmutableList(),
                relatedPanden: ParseRelatedPanden(node, namespaceManager).ToImmutableList());
        }

        private BagNummeraanduiding ParseNummeraanduiding(XmlNode node)
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

        private BagOpenbareRuimte ParseOpenbareRuimte(XmlNode node)
        {
            return new BagOpenbareRuimte(
                id: ParseId(node),
                version: ParseBagVersion(node),
                name: ParseOpenbareRuimteName(node),
                relatedWoonplaats: ParseRelatedWoonplaats(node));
        }

        private BagWoonplaats ParseWoonplaats(XmlNode node)
        {
            return new BagWoonplaats(
                id: ParseId(node),
                version: ParseBagVersion(node),
                name: ParseWoonplaatsName(node));
        }

        private string ParseId(XmlNode node)
        {
            return node["bag_LVC:identificatie"].InnerText;
        }

        private string ParseOpenbareRuimteName(XmlNode node)
        {
            return node["bag_LVC:openbareRuimteNaam"].InnerText;
        }

        private int ParseHouseNumber(XmlNode node)
        {
            return ParseInt(node["bag_LVC:huisnummer"]);
        }

        private char? ParseHouseLetter(XmlNode node)
        {
            return node["bag_LVC:huisletter"]?.InnerText?.Single();
        }

        private string ParseHouseNumberAddition(XmlNode node)
        {
            return node["bag_LVC:huisnummertoevoeging"]?.InnerText;
        }

        private string ParsePostalCode(XmlNode node)
        {
            return node["bag_LVC:postcode"]?.InnerText;
        }

        private string ParseRelatedOpenbareRuimte(XmlNode node)
        {
            return node["bag_LVC:gerelateerdeOpenbareRuimte"].InnerText;
        }

        private string ParseWoonplaatsName(XmlNode node)
        {
            return node["bag_LVC:woonplaatsNaam"].InnerText;
        }

        private int ParseConstructionYear(XmlNode node)
        {
            return ParseInt(node["bag_LVC:bouwjaar"]);
        }

        private string ParseRelatedWoonplaats(XmlNode node)
        {
            return node["bag_LVC:gerelateerdeWoonplaats"].InnerText;
        }

        private string ParseRelatedMainAddress(XmlNode node)
        {
            return node["bag_LVC:gerelateerdeAdressen"]["bag_LVC:hoofdadres"].InnerText;
        }

        private IEnumerable<string> ParseRelatedAdditionalAddresses(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            var relatedAddresses = node["bag_LVC:gerelateerdeAdressen"];

            var relatedAdditionalAddresses = relatedAddresses.SelectNodes("bag_LVC:nevenadres", namespaceManager);
            foreach (XmlNode relatedAdditionalAddress in relatedAdditionalAddresses)
            {
                yield return relatedAdditionalAddress.InnerText;
            }
        }

        private IEnumerable<string> ParseRelatedPanden(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            var relatedPanden = node.SelectNodes("bag_LVC:gerelateerdPand", namespaceManager);
            foreach (XmlNode relatedPand in relatedPanden)
            {
                yield return relatedPand.InnerText;
            }
        }

        private BagVersion ParseBagVersion(XmlNode node)
        {
            return new BagVersion(
                active: ParseBagActive(node),
                correctionIndex: ParseCorrectionIndex(node),
                validityInterval: ParseValidityInterval(node));
        }

        private bool ParseBagActive(XmlNode node)
        {
            return !ParseBagBoolean(node["bag_LVC:aanduidingRecordInactief"]);
        }

        private int ParseCorrectionIndex(XmlNode node)
        {
            return ParseInt(node["bag_LVC:aanduidingRecordCorrectie"]);
        }

        private Interval ParseValidityInterval(XmlNode node)
        {
            var validityNode = node["bag_LVC:tijdvakgeldigheid"];

            var start = ParseBagInstant(validityNode["bagtype:begindatumTijdvakGeldigheid"]);
            var end = ParseBagInstant(validityNode["bagtype:einddatumTijdvakGeldigheid"]);

            return new Interval(start ?? Instant.MinValue, end ?? Instant.MaxValue);
        }

        private int ParseInt(XmlElement element)
        {
            return int.Parse(element.InnerText);
        }

        private bool ParseBagBoolean(XmlElement element)
        {
            return element.InnerText == "Y";
        }

        private Instant? ParseBagInstant(XmlElement element)
        {
            if (element == null)
            {
                return null;
            }

            return _bagInstantPattern.Parse(element.InnerText).Value;
        }

        private GeometryPolygon ParseGeometry(XmlNode node)
        {
            var gmlNode = node["bag_LVC:pandGeometrie"].FirstChild;

            OverrideSrsNameAttribute(gmlNode);

            var reader = new XmlNodeReader(gmlNode);

            return _gmlFormatter.Read<GeometryPolygon>(reader);
        }

        private static void OverrideSrsNameAttribute(XmlNode gmlNode)
        {
            // This is needed since the Microsoft.Spatial library doesn't support the srsName value
            // to be supplied in URN form (like this: "urn:ogc:def:crs:EPSG::28992").

            gmlNode.Attributes["srsName"].Value = "http://www.opengis.net/def/crs/EPSG/0/28992";
        }
    }
}