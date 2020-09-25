using System.Collections.Immutable;

using NodaTime;

namespace Tiesmaster.ProjectZen.Domain.Bag
{

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