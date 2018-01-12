using System;
using System.IO;
using System.Linq;
using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Model;
using System.Collections.Generic;
using CsvHelper;
using MoreLinq;
namespace batterymetrics.Components
{
    public class MetricFactory
    {

        public MetricFactory(DeviceFactory deviceFactory)
        {
            deviceFactory = new DeviceFactory();
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
    }
}
