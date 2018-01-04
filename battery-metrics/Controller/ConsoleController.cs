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
            
            foreach (var _device in _devices.GroupBy(d => d.deviceId).ToList())
            {
                List<Processor> _processor = new List<Processor>();
                List<Cycle> _cycles = new List<Cycle>();
                var _deviceId = _device.Key;
                var _accNum = _device.Select(x => x.accntNo).FirstOrDefault();

                // Deserialise json data in device
                foreach (var _reading in _device.OrderBy(t => t.timestamp).ToList())
                {
                    var data = DeserializeObject<DeviceData>(_reading.jsonData);
                    _processor.Add(
                        new Processor()
                        {
                            DeviceId = _reading.deviceId,
                            AccountId = _reading.accntNo,
                            Timestamp = _reading.timestamp,
                            level = data.Battery.level,
                            charging = data.Battery.charging
                        });
                }

                // Process device cycles
                Dictionary<int, int> _cycleIndex = new Dictionary<int, int>();
                int cycle = 0;
                for (int i = 0; i < _processor.Count(); i++)
                {
                    var cData = _processor[i];
                    if (i == 0)
                    {
                        cycle++;
                    }
                    else
                    {
                        var pData = _processor[i - 1];
                        if ((cData.charging != pData.charging))
                        {
                            cycle++;
                        }
                        else
                        {
                            if ((cData.charging && (cData.level < pData.level)) ||
                               (!cData.charging && (cData.level > pData.level)))
                            {
                                cycle++;
                            }
                        }
                    }
                    _cycleIndex.Add(cData.GetHashCode(), cycle);
                }

                foreach (var item in _processor.Join(_cycleIndex,
                                        (p) => p.GetHashCode(),
                                        (f) => f.Key,
                                        (p, f) => new { p.DeviceId, p.AccountId, p.Timestamp, p.charging, p.level, cycle = f.Value })
                                  .GroupBy((arg) => arg.cycle)
                                  .ToList())
                {

                    var start = item.First();
                    var end = item.Last();
                    double deltaTime = (end.Timestamp - start.Timestamp).TotalSeconds;
                    double deltaLevel = Math.Abs(end.level - start.level);
                    double _PredHalfCycleTime = 0;
                    if (deltaTime > 0 && deltaLevel > 0)
                    {
                        _PredHalfCycleTime = (deltaTime / deltaLevel) * 100;
                    }

                    _cycles.Add(new Cycle()
                    {
                        DeviceId = start.DeviceId,
                        AccNum = start.AccountId,
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