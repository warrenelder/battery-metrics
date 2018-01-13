using System;
using Xunit;
using batterymetrics.Components;

namespace batterymetrics
{
    public class BatterCycleTests
    {
        [Fact]
        public void AssertChargeLevelDifferenceIsOneForPositiveDifferenceTest()
        {
            int start = 1;
            int end = 2;
            var result = BatteryCycle.ChargeLevelDifference(start, end);
            Assert.Equal(1, result);
        }

        [Fact]
        public void AssertChargeLevelDifferenceIsOneForNegativeDifferenceTest()
        {
            int start = 3;
            int end = 1;
            var result = BatteryCycle.ChargeLevelDifference(start, end);
            Assert.Equal(2, result);
        }
    }
}
