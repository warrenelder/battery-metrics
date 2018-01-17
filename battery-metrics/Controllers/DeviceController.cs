using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using batterymetrics.Components;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace batterymetrics.Controllers
{
    public class DeviceController : Controller
    {
        private IHostingEnvironment hostingEnv;

        public DeviceController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [Route("/[controller]")]

        [HttpGet]
        public IActionResult Index()
        {
            var DeviceList = DeviceFactory.DeviceList.ToList();
            if (DeviceList.Count() == 0)
            {
                ViewData["message"] = "No device data is available.";
            }
            return View(DeviceList);
        }

        [HttpGet]
        public IActionResult Delete()
        {
            DeviceFactory.DeleteDeviceList();
            MetricFactory.DeleteMetricList();
            return Redirect("/Home/Index");
        }


        [HttpGet]
        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FileUpload(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                ViewData["message"] = "No file selected!";
            }
            else
            {
                try
                {
                    var filename = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"');
                    filename = hostingEnv.WebRootPath + $@"\{filename}";

                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    DeviceFactory.UploadDeviceFromFile(filename);


                    ViewData["message"] = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewData["message"] = ex.Message;
                }
            }

            return View();
        }
    }
}