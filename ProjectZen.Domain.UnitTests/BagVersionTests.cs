using FluentAssertions;
using Xunit;

namespace Tiesmaster.ProjectZen.Domain.UnitTests
{
    public class BagVersionTests
    {
        [Fact]
        public void GivenVersionIsNotActive_ThenIsNotActive()
        {
            // arrange
            var bagVersion = A.BagVersion.WithActive(false);

            // act
            var isActive = bagVersion.IsActive;

            // assert
            isActive.Should().BeFalse();
        }
    }
}