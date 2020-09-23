using System;

using NodaTime;
using NodaTime.Extensions;

using Tiesmaster.ProjectZen.Domain.Bag;

namespace Tiesmaster.ProjectZen.TestHelpers
{
    public static class A
    {
        public static BagVersion BagVersion =
            new BagVersion(
                active: true,
                correctionIndex: 0,
                validityInterval: new Interval(
                    DateTime.UtcNow.ToInstant(),
                    DateTime.UtcNow.AddDays(1).ToInstant()));
    }
}