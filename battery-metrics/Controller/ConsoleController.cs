using System;
using System.IO;
using System.Linq;
using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Model;
using System.Collections.Generic;

namespace batterymetrics.Controller
{
    public class ConsoleController
    {
        private static List<Device> _devices = new List<Device>();
        private static List<Metric> _metrics = new List<Metric>();
        private static List<Cycle> _cycles = new List<Cycle>();

        public void Read(string path)
        {
            _devices.AddRange(File.ReadAllLines(path).Skip(1).Select(l => DeviceParser.FromTsv(l)));
        }

        public void Analyse()
        {
            var groupedDevices = _devices.GroupBy(d => d.deviceId).ToList();
            foreach (var _device in groupedDevices)
            {
                int count = 0;
                var deviceId = _device.Key;
                Console.WriteLine(deviceId + " device id");
                var test = _device.OrderBy(t => t.timestamp);
                foreach(var _reading in _device) 
                {
                    count++;
                    var deviceData = DeserializeObject<DeviceData>(_reading.jsonData);

                    Console.WriteLine(_reading.timestamp);
                    Console.WriteLine(deviceData.Battery.charging);
                    Console.WriteLine(deviceData.Battery.level);

                }

                // Metric calculation
            }
        }

        public void View()
        {
            
        }

        public void Create()
        {
            
        }

    }
}