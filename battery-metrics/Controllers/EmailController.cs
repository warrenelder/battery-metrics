using System;
using Microsoft.AspNetCore.Mvc;
using batterymetrics.Models;
namespace batterymetrics.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet]
        public IActionResult Email()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Email([FromForm] User user)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
                
            try
            {
                Components.Email.SendEmail(user.Email);
                ViewData["message"] = "Email Sent successfully";
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
            }

            return View("Email");
        }

    }
}
