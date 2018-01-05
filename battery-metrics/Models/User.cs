using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace batterymetrics.Models
{
    public class User
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
