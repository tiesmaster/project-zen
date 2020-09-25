using System.Collections.Immutable;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagVerblijfsobject : BagBase
    {
        public BagVerblijfsobject(
            string id,
            BagVersion version,
            string relatedMainAddress,
            ImmutableList<string> relatedAdditionalAddresses,
            ImmutableList<string> relatedPanden) : base(id, version)
        {
            RelatedMainAddress = relatedMainAddress;
            RelatedAdditionalAddresses = relatedAdditionalAddresses;
            RelatedPanden = relatedPanden;
        }

        public BagVerblijfsobject(
            string id,
            BagVersion version,
            string relatedMainAddress,
            params string[] relatedPanden) : this(
                id,
                version,
                relatedMainAddress,
                ImmutableList<string>.Empty,
                relatedPanden.ToImmutableList())
        {
        }

        public string RelatedMainAddress { get; }
        public ImmutableList<string> RelatedAdditionalAddresses { get; }
        public ImmutableList<string> RelatedPanden { get; }
    }
}