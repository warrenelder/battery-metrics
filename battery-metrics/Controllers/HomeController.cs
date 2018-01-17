using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;
using System.Net;
using System.Net.Mail;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using batterymetrics.Components;
namespace batterymetrics.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var MetricList = MetricFactory.MetricList;
            if (MetricList.Count() == 0)
            {
                ViewData["Message"] = "Please upload device data to view battery metrics.";
            }
                
            return View(MetricList);
        }

    }
}
