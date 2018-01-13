using System;
using Xunit;
using batterymetrics.Components;

namespace batterymetrics
{
    public class BatteryCycleTests
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

        [Fact]
        public void AssertTimeDifferenceIsOneTest()
        {
            DateTime start = new DateTime(2018, 01, 01, 0, 0, 0);
            DateTime end = new DateTime(2018, 01, 01, 0, 0, 1);
            var result = BatteryCycle.TimeDifference(start, end);
            Assert.Equal(1, result);
        }

        [Fact]
        public void AssertTotalBatteryChargingTimeIsTenTest()
        {
            DateTime t1 = new DateTime(2018, 01, 01, 0, 0, 0);
            DateTime t2 = new DateTime(2018, 01, 01, 0, 0, 1);
            int l1 = 10;
            int l2 = 20;
            var result = BatteryCycle.TotalBatteryChargingDischargeTime(t1, t2, l1, l2);
            Assert.Equal(10, result);
        }

        [Fact]
        public void AssertTotalBatteryDischargeTimeIsTenTest()
        {
            DateTime t1 = new DateTime(2018, 01, 01, 0, 0, 0);
            DateTime t2 = new DateTime(2018, 01, 01, 0, 0, 1);
            int l1 = 20;
            int l2 = 10;
            var result = BatteryCycle.TotalBatteryChargingDischargeTime(t1, t2, l1, l2);
            Assert.Equal(10, result);
        }

        [Fact]
        public void AssertTotalBatteryChargingTimeIsZeroForZeroDeltaTimeTest()
        {
            DateTime t1 = new DateTime(2018, 01, 01, 0, 0, 0);
            DateTime t2 = new DateTime(2018, 01, 01, 0, 0, 0);
            int l1 = 10;
            int l2 = 20;
            var result = BatteryCycle.TotalBatteryChargingDischargeTime(t1, t2, l1, l2);
            Assert.Equal(0, result);
        }

        [Fact]
        public void AssertTotalBatteryChargingTimeIsZeroForZeroDeltaBatteryLevelTest()
        {
            DateTime t1 = new DateTime(2018, 01, 01, 0, 0, 0);
            DateTime t2 = new DateTime(2018, 01, 01, 0, 0, 1);
            int l1 = 10;
            int l2 = 10;
            var result = BatteryCycle.TotalBatteryChargingDischargeTime(t1, t2, l1, l2);
            Assert.Equal(0, result);
        }
    }
}
