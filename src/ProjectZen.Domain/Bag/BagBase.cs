using System.Collections.Immutable;

using NodaTime;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public abstract class BagBase
    {
        protected BagBase(string id, BagVersion version)
        {
            Id = id;
            Version = version;
        }

        public string Id { get; }
        public BagVersion Version { get; }

        public bool IsActive(Instant referenceInstant) => Version.IsActive(referenceInstant);
    }
}