using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using batterymetrics.Components;
namespace batterymetrics.Controllers
{
    public class DeviceController : Controller
    {
        [Route("/[controller]")]

        [HttpGet]
        public IActionResult Index()
        {
            var DeviceList = DeviceFactory.DeviceList.ToList();
            if(DeviceList.Count() == 0)
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
                    DeviceFactory.UploadDeviceFromFile(file.FileName);
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


