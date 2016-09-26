using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waes.Diffly.Api.Infrastructure;
using Waes.Diffly.UnitTest;
using Xunit;

namespace Waes.Diffly.UnitTest.Api.Infrastructure
{
    public class DiffSideRouteConstraintTest
    {
        [Theory]
        [InlineData("left", true)]
        [InlineData("Left", true)]
        [InlineData("LEFt", true)]
        [InlineData("right", true)]
        [InlineData("RIGHT", true)]
        [InlineData("something", false)]
        [InlineData("", false)]
        public void DiffSideRouteConstraint_MatchAsExpected(object parameterValue, bool expected)
        {
            // Arrange
            var constraint = new DiffSideRouteConstraint();

            // Act
            var actual = ConstraintsTestHelper.TestConstraint(constraint, parameterValue);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
