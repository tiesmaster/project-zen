using FluentAssertions;
using NodaTime;
using Tiesmaster.ProjectZen.Domain.Bag;
using Xunit;

namespace Tiesmaster.ProjectZen.Domain.UnitTests
{
    public class BagVersionTests
    {
        [Fact]
        public void GivenVersionIsNotActive_ThenIsNotActive()
        {
            // arrange
            var bagVersion = new BagVersion(active: false, correctionIndex: 0, new Interval());

            // act
            var isActive = bagVersion.IsActive;

            // assert
            isActive.Should().BeFalse();
        }
    }
}
