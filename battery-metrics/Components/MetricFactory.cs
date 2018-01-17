using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using batterymetrics.Models;
namespace batterymetrics.Components
{
    public class MetricFactory
    {
        public static List<Metric> MetricList = new List<Metric>();

        public static void CalculateAllDeviceBatteryMetrics()
        {
            DeleteMetricList();
            foreach (var item in DeviceFactory.DeviceList.DistinctBy(x => x.deviceId).ToList())
            {
                MetricList.Add(DeviceBatteryMetric(item.deviceId, item.accntNo));
            }
        }

        public static Metric DeviceBatteryMetric(int DeviceId, int AccNum)
        {
            var ChargeCycles = BatteryCycle.CalculateDeviceBatteryChargeCycle(DeviceId);
            Metric metric = new Metric
            {
                DeviceId = DeviceId,
                AccNum = AccNum,
                BatteryLifetime = (int)ChargeCycles.Where(x => x.Charging == false && x.PredHalfCycleTime > 0).Select(x => (int?)Math.Round(x.PredHalfCycleTime)).Average().GetValueOrDefault(),
                ChargeTime = (int)ChargeCycles.Where(x => x.Charging == true && x.PredHalfCycleTime > 0).Select(x => (int?)Math.Round(x.PredHalfCycleTime)).Average().GetValueOrDefault(),
                Cycles = ChargeCycles.Count(),
                LevelCycles = ChargeCycles.Where(x => (int)x.PredHalfCycleTime == 0).Count(),
            };

            return metric;
        }

        public static void DeleteMetricList()
        {
            MetricList.Clear();
        }
    }
}
