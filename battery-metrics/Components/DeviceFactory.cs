using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace batterymetrics.Components
{
    public class DeviceFactory
    {
        public static List<Device> DeviceList = new List<Device>();

        public static void AddDevice(string reading)
        {
            DeviceList.Add(DeviceParser.FromTsv(reading));
        }

        public static void UploadDeviceFromFile(string fileName)
        {
            foreach (var reading in File.ReadAllLines(fileName).Skip(1))
            {
                AddDevice(reading);
            }
            MetricFactory.CalculateAllDeviceBatteryMetrics();
        }

        public static DeviceData ExtractJSON(Device item)
        {
            return DeserializeObject<DeviceData>(item.jsonData);
        }
    }
}
