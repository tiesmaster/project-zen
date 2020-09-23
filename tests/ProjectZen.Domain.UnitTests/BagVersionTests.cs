using FluentAssertions;

using NodaTime;

using Tiesmaster.ProjectZen.Domain.Bag;

using Xunit;

namespace Tiesmaster.ProjectZen.Domain.UnitTests
{
    public class BagVersionTests
    {
        [Fact]
        public void GivenActiveIsFalse_ThenIsNotActive()
        {
            // arrange
            var bagVersion = A.BagVersion.WithActive(false);

            // act
            var isActive = bagVersion.CallIsActiveWithReferenceWithinValidityInterval();

            // assert
            isActive.Should().BeFalse();
        }

        [Fact]
        public void GivenCorrectionIndexIsNotZero_ThenIsNotActive()
        {
            // arrange
            var bagVersion = A.BagVersion.WithCorrectionIndex(1);

            // act
            var isActive = bagVersion.CallIsActiveWithReferenceWithinValidityInterval();

            // assert
            isActive.Should().BeFalse();
        }

        [Fact]
        public void GivenReferenceTimeBeforeStartOfValidityInterval_ThenIsNotActive()
        {
            // arrange
            var referenceDay = new LocalDate(2020, 09, 09);
            var startDay = referenceDay.PlusDays(1);
            var bagVersion = A.BagVersion.WithValidityInterval(startDay, startDay.PlusDays(1));

            // act
            var isActive = bagVersion.IsActive(referenceDay.AtMidnight().InUtc().ToInstant());

            // assert
            isActive.Should().BeFalse();
        }

        [Fact]
        public void GivenReferenceTimeIsFallingInValidityInterval_ThenIsActive()
        {
            // arrange
            var referenceDay = new LocalDate(2020, 09, 09);
            var startDay = referenceDay.PlusDays(-1);
            var bagVersion = A.BagVersion.WithValidityInterval(startDay, startDay.PlusDays(2));

            // act
            var isActive = bagVersion.IsActive(referenceDay.AtMidnight().InUtc().ToInstant());

            // assert
            isActive.Should().BeTrue();
        }
    }

    public static class BagVersionExtensions
    {
        public static bool CallIsActiveWithReferenceWithinValidityInterval(this BagVersion bagVersion)
            => bagVersion.IsActive(bagVersion.ValidityInterval.Start + Duration.FromSeconds(5));

        public static BagVersion WithActive(this BagVersion bagVersion, bool active)
            => new BagVersion(active, bagVersion.CorrectionIndex, bagVersion.ValidityInterval);

        public static BagVersion WithCorrectionIndex(this BagVersion bagVersion, int correctionIndex)
            => new BagVersion(bagVersion.Active, correctionIndex, bagVersion.ValidityInterval);

        public static BagVersion WithValidityInterval(this BagVersion bagVersion, Interval validityInterval)
            => new BagVersion(bagVersion.Active, bagVersion.CorrectionIndex, validityInterval);

        public static BagVersion WithValidityInterval(this BagVersion bagVersion, Instant startInstant, Instant? endInstant)
            => bagVersion.WithValidityInterval(new Interval(startInstant, endInstant ?? Instant.MaxValue));

        public static BagVersion WithValidityInterval(this BagVersion bagVersion, LocalDate startDay, LocalDate endDay)
            => bagVersion.WithValidityInterval(
                new Interval(
                    startDay.AtMidnight().InUtc().ToInstant(),
                    endDay.AtMidnight().InUtc().ToInstant()));
    }
}