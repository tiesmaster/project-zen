using System.Collections.Immutable;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagVerblijfsobject : BagBase
    {
        public BagVerblijfsobject(string id, BagVersion version, ImmutableList<string> relatedPanden) : base(id, version)
        {
            RelatedPanden = relatedPanden;
        }

        public BagVerblijfsobject(string id, BagVersion version, params string[] relatedPanden) : this(id, version, relatedPanden.ToImmutableList())
        {
        }

        public ImmutableList<string> RelatedPanden { get; }
    }
}