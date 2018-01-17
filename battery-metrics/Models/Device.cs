using System;
namespace batterymetrics.Models
{
    public class Device
    {
        private static int DeviceCount = 1;

        public Device()
        {
            Id = DeviceCount++;
        }

        public int Id { get; set; }
        public int deviceId { get; set; }
        public int accntNo { get; set; }
        public DateTime timestamp { get; set; } 
        public string jsonData { get; set; }
    }

    public class DeviceData
    {
        public Wifi Wifi { get; set; }
        public Battery Battery { get; set; }
        public Cellular Cellular { get; set; }
        public CurrentLocation CurrentLocation { get; set; }
    }

    public class Battery
    {
        public int level { get; set; }
        public bool charging { get; set; }
    }

    public class CurrentLocation
    {
        public string uid { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Altitude { get; set; }
        public double Accuracy { get; set; }
        public int Timestamp { get; set; }
        public int RequestTime { get; set; }
        public int TimeToFix { get; set; }
        public bool IsValid { get; set; }
        public string AsText { get; set; }
    }

    public class Cellular
    {
        public int cellId { get; set; }
        public string ipAddress { get; set; }
        public int mcc { get; set; }
        public string apn { get; set; }
        public int lac { get; set; }
        public string status { get; set; }
        public bool isRoaming { get; set; }
        public int mnc { get; set; }
        public string imei { get; set; }
        public string ipv6Address { get; set; }
        public bool isFlightMode { get; set; }
    }

    public class Wifi
    {
        public string status { get; set; }
        public string ipv6Address { get; set; }
        public string ipAddress { get; set; }
        public int signalStrength { get; set; }
        public string ssid { get; set; }
        public string macAddress{ get; set; }
    }

    public class ChargeCycle
    {
        public int Cycle { get; set; }
        public DateTime Timestamp { get; set; } 
        public int Level { get; set; }
        public bool Charging { get; set; }
    }

}
