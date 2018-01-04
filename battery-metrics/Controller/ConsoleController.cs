using System;
using System.IO;
using System.Linq;
using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Model;
using System.Collections.Generic;
using CsvHelper;

namespace batterymetrics.Controller
{
    public class ConsoleController
    {
        private static List<Device> _devices = new List<Device>();
        private static List<Metric> _metrics = new List<Metric>();

        public void Read(string path)
        {
            _devices.AddRange(File.ReadAllLines(path).Skip(1).Select(l => DeviceParser.FromTsv(l)));
        }

        public void Analyse()
        {

            // Extract device JSON data
            var collection = _devices.Select(c => new { c.deviceId, c.accntNo, c.timestamp, batteryData = Device.ExtractJSON(c).Battery}).ToList();

            // Calculate device metrics
            foreach (var _deviceReadings in collection.OrderBy(x => x.timestamp).GroupBy(x => x.deviceId).ToList())
            {
                
                var _deviceId = _deviceReadings.Key;
                var _accNum = _deviceReadings.Select(x => x.accntNo).FirstOrDefault();

                // TODO refactor charge cycle analysis into seperate class
                // Generate charging cycle index
                Dictionary<int, int> _cycleIndex = new Dictionary<int, int>();
                int cycle = 0;
                for (int i = 0; i < _deviceReadings.Count(); i++)
                {
                    var cData = _deviceReadings.ToList()[i];
                    if (i == 0)
                    {
                        cycle++;
                    }
                    else
                    {
                        var pData = _deviceReadings.ToList()[i - 1];
                        if ((cData.batteryData.charging != pData.batteryData.charging))
                        {
                            cycle++;
                        }
                        else
                        {
                            if ((cData.batteryData.charging && (cData.batteryData.level < pData.batteryData.level)) ||
                                (!cData.batteryData.charging && (cData.batteryData.level > pData.batteryData.level)))
                            {
                                cycle++;
                            }
                        }
                    }
                    _cycleIndex.Add(cData.GetHashCode(), cycle);
                }

                // Predict charge/discharge time for each cycle
                List<Cycle> _cycles = new List<Cycle>();
                foreach (var item in _deviceReadings.Join(_cycleIndex,
                                        (p) => p.GetHashCode(),
                                        (f) => f.Key,
                                        (p, f) => new { p.deviceId, p.accntNo, p.timestamp, p.batteryData.charging, p.batteryData.level, cycle = f.Value })
                                  .GroupBy((arg) => arg.cycle)
                                  .ToList())
                {

                    var start = item.First();
                    var end = item.Last();
                    double deltaTime = (end.timestamp - start.timestamp).TotalSeconds;
                    double deltaLevel = Math.Abs(end.level - start.level);
                    double _PredHalfCycleTime = 0;
                    if (deltaTime > 0 && deltaLevel > 0)
                    {
                        _PredHalfCycleTime = (deltaTime / deltaLevel) * 100;
                    }

                    _cycles.Add(new Cycle()
                    {
                        Charging = start.charging,
                        PredHalfCycleTime = _PredHalfCycleTime,
                    });
                }

                // Calculate battery metrics
                _metrics.Add(new Metric()
                {
                    DeviceId = _deviceId,
                    AccNum = _accNum,
                    BatteryLifetime = (int)_cycles.Where(x => x.Charging == false && x.PredHalfCycleTime > 0).Select(x => (int?)Math.Round(x.PredHalfCycleTime)).Average().GetValueOrDefault(),
                    ChargeTime = (int)_cycles.Where(x => x.Charging == true && x.PredHalfCycleTime > 0).Select(x => (int?)Math.Round(x.PredHalfCycleTime)).Average().GetValueOrDefault(),
                    Cycles = _cycles.Count(),
                    LevelCycles = _cycles.Where(x => (int)x.PredHalfCycleTime == 0).Count(),
                });

            }
        }

        public void View()
        {
            Console.Write($"DeviceId  AccNum  BattLife  ChargeT  Cycles  FlatCycles \n");
            foreach (var metric in _metrics)
            {
                string output = $"{metric.DeviceId,-10}{metric.AccNum,-8}{metric.BatteryLifetime,-10}{metric.ChargeTime,-9}{metric.Cycles,-9}{metric.LevelCycles,-8}\n";
                Console.Write(output);
            }
        }

        public void Create()
        {
            using (var writer = new StreamWriter("output.csv"))
            {
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.WriteRecords(_metrics);
                }
            }
        }
    }
}