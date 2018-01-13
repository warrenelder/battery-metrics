using System;
using System.Linq;
using batterymetrics.Model;
using System.Collections.Generic;
using MoreLinq;

namespace batterymetrics.Components
{
    public class BatteryCycle
    {
        public static List<Cycle> CalculateDeviceBatteryChargeCycle(int DeviceID)
        {
            List<Cycle> CycleList = new List<Cycle>();

            var DeviceReadings = DeviceFactory
                .DeviceList
                .Where(x => x.deviceId == DeviceID)
                .OrderBy(x => x.timestamp)
                .Select(x => new
                {
                    x.Id,
                    x.timestamp,
                    batteryData = DeviceFactory.ExtractJSON(x).Battery
                })
                .ToList();

            var DeviceReadingCycleIndex = CreateChargeCycleIndex(DeviceID);

            var GroupedDeviceReadingByCycle = DeviceReadings
                         .Join(
                                DeviceReadingCycleIndex,
                    (a) => a.Id,
                    (b) => b.Key,
                    (a, b) => new
                    {
                        Timestamp = a.timestamp,
                        Charging = a.batteryData.charging,
                        Level = a.batteryData.level,
                        Cycle = b.Value
                    })
                        .GroupBy(x => x.Cycle)
                        .Select(g => g.ToList())
                .ToList();

            foreach (var CycleGroup in GroupedDeviceReadingByCycle)
            {
                var FirstReading = CycleGroup.First();
                var LastReading = CycleGroup.Last();
                CycleList.Add(new Cycle()
                {
                    Charging = FirstReading.Charging,
                    PredHalfCycleTime = TotalBatteryChargingDischargeTime(LastReading.Timestamp, FirstReading.Timestamp, LastReading.Level, FirstReading.Level),
                });
            }

            return CycleList;
        }

        public static double TotalBatteryChargingDischargeTime(DateTime t1, DateTime t2, int l1, int l2)
        {
            double DeltaTime = TimeDifference(t2, t1);
            double DeltaLevel = ChargeLevelDifference(l2, l1);
            double TotalChargeDischargeTime = 0;
            if ((DeltaTime > 0) && (DeltaLevel > 0))
            {
                TotalChargeDischargeTime = (DeltaTime / DeltaLevel) * 100;
            }
            return TotalChargeDischargeTime;
        }

        public static double ChargeLevelDifference(int start, int end)
        {
            return Math.Abs(end - start);
        }

        static double TimeDifference(DateTime start, DateTime end)
        {
            return (end - start).TotalSeconds;
        }

        private static Dictionary<int, int> CreateChargeCycleIndex(int DeviceID)
        {
            Dictionary<int, int> CycleIndex = new Dictionary<int, int>();
            var BatteryReadings = DeviceFactory.DeviceList
                                               .Where(x => x.deviceId == DeviceID)
                                               .OrderBy(x => x.timestamp)
                                               .Select(x => new { x.Id, x.timestamp, batteryData = DeviceFactory.ExtractJSON(x).Battery })
                                               .ToList();
            // Compare each reading with previous to determine if they are in the same or different Charge cycle.
            int NumBatteryReadings = BatteryReadings.Count();
            int Cycle = 0;
            for (int i = 0; i < NumBatteryReadings; i++)
            {
                var cData = BatteryReadings.ToList()[i];
                if (i > 0)
                {
                    var pData = BatteryReadings.ToList()[i - 1];

                    if ((cData.batteryData.charging != pData.batteryData.charging))
                    {
                        Cycle++;
                    }
                    else
                    {
                        if ((cData.batteryData.charging && (cData.batteryData.level < pData.batteryData.level)) ||
                            (!cData.batteryData.charging && (cData.batteryData.level > pData.batteryData.level)))
                        {
                            Cycle++;
                        }
                    }
                }
                else
                {
                    Cycle++;
                }
                CycleIndex.Add(cData.Id, Cycle);
            }

            return CycleIndex;
        }
    }
}
