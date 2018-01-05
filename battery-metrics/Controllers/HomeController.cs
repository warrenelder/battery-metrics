using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using battery_metrics.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using batterymetrics.Model;
using batterymetrics.Utilities;
using System.Collections.ObjectModel;

namespace battery_metrics.Controllers
{
    public class HomeController : Controller
    {

        private static List<Device> _devices = new List<Device>();
        private static List<Metric> _metrics = new List<Metric>();

        public IActionResult Index()
        {
            // Extract device JSON data
            var collection = _devices.Select(c => new { c.deviceId, c.accntNo, c.timestamp, batteryData = Device.ExtractJSON(c).Battery }).ToList();

            // Calculate device metrics
            foreach (var _deviceReadings in collection.OrderBy(x => x.timestamp).GroupBy(x => x.deviceId).ToList())
            {
                List<Cycle> _cycles = new List<Cycle>();
                var _deviceId = _deviceReadings.Key;
                var _accNum = _deviceReadings.Select(x => x.accntNo).FirstOrDefault();

                // Process device cycles
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

            if (_metrics.Count() == 0)
                ViewData["Message"] = "Please upload device data to view battery metrics.";

            return View(_metrics);
        }

        //[HttpGet("{id}")]
        //public IActionResult ViewMetric(int id)
        //{
        //    var metric = _metrics.SingleOrDefault(p => p.DeviceId == id);

        //    if (metric == null)
        //        return NotFound();

        //    return View(metric);
        //}

        [HttpPost]
        public IActionResult ViewMetric()
        {
            return View(_metrics);
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            var result = new List<string>();
            
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() > 0)
                    result.Add(reader.ReadLine());
            }              

            _devices.AddRange(result.Skip(1).Select(x => DeviceParser.FromTsv(x)));

            return RedirectToAction("Index");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
