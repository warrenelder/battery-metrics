using System.ComponentModel.DataAnnotations;
namespace batterymetrics.Models
{
    public class User
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
