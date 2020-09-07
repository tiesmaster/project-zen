using NodaTime;

namespace Tiesmaster.ProjectZen.Domain.Bag
{
    public class Pand
    {
        public Pand(string id, BagVersion version, int constructionYear)
        {
            Id = id;
            ConstructionYear = constructionYear;
            Version = version;
        }

        public string Id { get; }
        public BagVersion Version { get; }
        public int ConstructionYear { get; }
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

        public bool IsActive => Active;

        public BagVersion WithActive(bool active)
            => new BagVersion(active, CorrectionIndex, ValidityInterval);

        public BagVersion WithCorrectionIndex(int correctionIndex)
            => new BagVersion(Active, correctionIndex, ValidityInterval);

        public BagVersion WithValidityInterval(Interval validityInterval)
            => new BagVersion(Active, CorrectionIndex, validityInterval);
    }
}