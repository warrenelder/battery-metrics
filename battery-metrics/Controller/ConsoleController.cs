using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using MoreLinq;
using batterymetrics.Model;
using batterymetrics.Components;

namespace batterymetrics.Controller
{
    public class ConsoleController
    {
        
        private static List<Metric> DeviceMetricList = new List<Metric>();

        public void Read(string filename)
        {
            try
            {
                //StreamReader sr = File.OpenText(filename);
                //Console.WriteLine("The first line of this file is {0}", sr.ReadLine());
                //sr.ReadToEnd().Skip(1).Select(x=>DeviceFactory.AddDeviceReading(x));
                //sr.Close();
                foreach( var reading in File.ReadAllLines(filename).Skip(1) )
                {
                    DeviceFactory.AddDevice(reading);
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error reading the file: '{0}'", ex);
            }
        }

        public void View()
        {
            Console.Write($"DeviceId  AccNum  BattLife  ChargeT  Cycles  FlatCycles \n");

            // Implement foreach loop to output each metric then provide download.
            foreach (var item in DeviceFactory.DeviceList.DistinctBy(x => x.deviceId).ToList())
            {
                Metric DeviceMetric = MetricFactory.DeviceBatteryMetric(item.deviceId, item.accntNo);
                string output = $"{DeviceMetric.DeviceId,-10}{DeviceMetric.AccNum,-8}{DeviceMetric.BatteryLifetime,-10}{DeviceMetric.ChargeTime,-9}{DeviceMetric.Cycles,-9}{DeviceMetric.LevelCycles,-8}\n";
                Console.Write(output);
            }
        }

        public void Create()
        {
            foreach (var item in DeviceFactory.DeviceList.DistinctBy(x => x.deviceId).ToList())
            {
                DeviceMetricList.Add(MetricFactory.DeviceBatteryMetric(item.deviceId, item.accntNo));
            }
            using (var writer = new StreamWriter("output.csv"))
            {
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.WriteRecords(DeviceMetricList);
                }
            }
        }
    }
}