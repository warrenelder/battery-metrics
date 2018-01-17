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
using Microsoft.AspNetCore.Http;
using batterymetrics.Models;
using batterymetrics.Components;

namespace batterymetrics.Controllers
{
    public class EmailController : Controller
    {
        private static SmtpClient smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("ENTER GMAIL EMAIL", "ENTER GMAIL APP PASSWORD")
        };


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
