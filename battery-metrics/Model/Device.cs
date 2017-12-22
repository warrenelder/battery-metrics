using System;
namespace batterymetrics.Model
{
    public class Device
    {
        public int deviceId { get; set; }
        public int accntNo { get; set; }
        public DateTime timestamp { get; set; } 
        public string jsonData { get; set; }
    }
}
