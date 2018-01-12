using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Model;
using System.Collections.Generic;

namespace batterymetrics.Components
{
    public class DeviceFactory
    {
        public static List<Device> DeviceList = new List<Device>();

        public static void AddDevice(string reading)
        {
            DeviceList.Add(DeviceParser.FromTsv(reading));
        }

        public static DeviceData ExtractJSON(Device item)
        {
            return DeserializeObject<DeviceData>(item.jsonData);
        }
    }
}
