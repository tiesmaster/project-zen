using System;
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

    public class BagVersion
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

        public bool IsActive => throw new NotImplementedException();
    }
}