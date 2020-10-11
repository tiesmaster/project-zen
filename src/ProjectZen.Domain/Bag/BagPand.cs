using Microsoft.Spatial;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagPand : BagBase
    {
        public BagPand(string id, BagVersion version, int constructionYear, GeometryPolygon geometry) : base(id, version)
        {
            ConstructionYear = constructionYear;
            Geometry = geometry;
        }

        public int ConstructionYear { get; }
        public GeometryPolygon Geometry { get; }
    }
}