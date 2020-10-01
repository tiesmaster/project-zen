namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagPand : BagBase
    {
        public BagPand(string id, BagVersion version, int constructionYear, string geometry) : base(id, version)
        {
            ConstructionYear = constructionYear;
            Geometry = geometry;
        }

        public int ConstructionYear { get; }
        public string Geometry { get; }
    }
}