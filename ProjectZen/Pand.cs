using System.Xml;

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
            return new Pand
            {
                Id = xmlNode["bag_LVC:identificatie"].InnerText,
                ConstructionYear = int.Parse(xmlNode["bag_LVC:bouwjaar"].InnerText)
            };
        }
    }
}