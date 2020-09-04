using System.Xml;
using Microsoft.Spatial;

namespace ProjectZen
{

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
            var gmlFormatter = GmlFormatter.Create();
            var gmlNode = xmlNode["bag_LVC:pandGeometrie"].FirstChild;
            gmlNode.Attributes["srsName"].Value = "http://www.opengis.net/def/crs/EPSG/0/28992";
            var reader = new XmlNodeReader(gmlNode);
            var polygon = gmlFormatter.Read<GeometryPolygon>(reader);
            return new Pand
            {
                Id = xmlNode["bag_LVC:identificatie"].InnerText,
                ConstructionYear = int.Parse(xmlNode["bag_LVC:bouwjaar"].InnerText)
            };
        }
    }
}