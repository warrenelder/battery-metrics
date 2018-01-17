﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Models;

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
            DeleteDeviceList();

            foreach (var reading in File.ReadAllLines(fileName).Skip(1))
            {
                AddDevice(reading);
            }
            MetricFactory.CalculateAllDeviceBatteryMetrics();
        }

        public static void DeleteDeviceList()
        {
            DeviceList.Clear();
        }

        public static DeviceData ExtractJSON(Device item)
        {
            return DeserializeObject<DeviceData>(item.jsonData);
        }
    }
}
