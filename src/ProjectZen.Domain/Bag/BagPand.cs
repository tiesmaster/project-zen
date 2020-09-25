using System.Collections.Immutable;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagPand : BagBase
    {
        public BagPand(string id, BagVersion version, int constructionYear) : base(id, version)
        {
            ConstructionYear = constructionYear;
        }

        public int ConstructionYear { get; }
    }
}