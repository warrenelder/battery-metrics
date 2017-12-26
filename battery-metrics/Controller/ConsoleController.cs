using System;
using System.IO;
using System.Linq;
using batterymetrics.Utilities;
using static Newtonsoft.Json.JsonConvert;
using batterymetrics.Model;
using System.Collections.Generic;
using CsvHelper;

namespace batterymetrics.Controller
{
    public class ConsoleController
    {
        private static List<Device> _devices = new List<Device>();
        private static List<DeviceData> _deviceData = new List<DeviceData>();
        private static List<Metric> _metrics = new List<Metric>();
        private static List<Cycle> _cycles = new List<Cycle>();

        public void Read(string path)
        {
            _devices.AddRange(File.ReadAllLines(path).Skip(1).Select(l => DeviceParser.FromTsv(l)));
        }

        public void Analyse()
        {
            //@TODO Calculate battery metrics for each device
        }

        public void View()
        {
            Console.Write($"DeviceId AccNum BattLife Chargetime Cycles \n");
            foreach (var metric in _metrics)
            {
                Console.Write($"{metric.DeviceId} \t {metric.AccNum} \t {metric.BatteryLifetime} \t {metric.ChargeTime} \t {metric.Cycles} \n");
            }
        }

        public void Create()
        {
            using (var writer = new StreamWriter("output.csv"))
            {
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.WriteRecords(_metrics);
                }
            }
        }
    }
}