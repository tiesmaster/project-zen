using System.Collections.Immutable;

using NodaTime;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class BagPand
    {
        public BagPand(string id, BagVersion version, int constructionYear)
        {
            Id = id;
            ConstructionYear = constructionYear;
            Version = version;
        }

        public string Id { get; }
        public BagVersion Version { get; }
        public int ConstructionYear { get; }

        public bool IsActive(Instant referenceInstant) => Version.IsActive(referenceInstant);
    }

    public class BagVerblijfsobject
    {
        public BagVerblijfsobject(string id, BagVersion version, ImmutableList<string> relatedPanden)
        {
            Id = id;
            Version = version;
            RelatedPanden = relatedPanden;
        }

        public BagVerblijfsobject(string id, BagVersion version, params string[] relatedPanden) : this(id, version, relatedPanden.ToImmutableList())
        {
        }

        public string Id { get; }
        public BagVersion Version { get; }
        public ImmutableList<string> RelatedPanden { get; }
    }

    public class BagNummeraanduiding
    {
        public BagNummeraanduiding(string id, BagVersion version)
        {
            Id = id;
            Version = version;
        }

        public string Id { get; }
        public BagVersion Version { get; }
    }

    public readonly struct BagVersion
    {
        public BagVersion(bool active, int correctionIndex, Interval validityInterval)
        {
            Active = active;
            CorrectionIndex = correctionIndex;
            ValidityInterval = validityInterval;
        }

        public bool Active { get; }
        public int CorrectionIndex { get; }
        public Interval ValidityInterval { get; }

        public bool IsActive(Instant referenceInstant)
        {
            return Active && CorrectionIndex == 0 && ValidityInterval.Contains(referenceInstant);
        }
    }
}