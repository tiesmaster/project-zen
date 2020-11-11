using NodaTime;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public record BagVersion(bool Active, int CorrectionIndex, Interval ValidityInterval)
    {
        public bool IsActive(Instant referenceInstant)
        {
            return Active && CorrectionIndex == 0 && ValidityInterval.Contains(referenceInstant);
        }
    }
}