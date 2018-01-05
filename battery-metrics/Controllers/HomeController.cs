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
            if (_metrics.Count() == 0)
                ViewData["Message"] = "Please upload device data to view battery metrics.";

            return View(_metrics);
        }

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
