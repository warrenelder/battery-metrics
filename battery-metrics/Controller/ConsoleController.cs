using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using batterymetrics.Model;
using batterymetrics.Components;

namespace batterymetrics.Controller
{
    public class ConsoleController
    {
        public void Read(string filename)
        {
            try
            {
                DeviceFactory.UploadDeviceFromFile(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error reading the file: '{0}'", ex);
            }
        }

        public void View()
        {
            Console.Write($"DeviceId  AccNum  BattLife  ChargeT  Cycles  FlatCycles \n");

            List<Metric> MetricList = MetricFactory.MetricList;
            foreach (var item in MetricList)
            {
                string output = $"{item.DeviceId,-10}{item.AccNum,-8}{item.BatteryLifetime,-10}{item.ChargeTime,-9}{item.Cycles,-9}{item.LevelCycles,-8}\n";
                Console.Write(output);
            }
        }

        public void Create()
        {
            List<Metric> MetricList = MetricFactory.MetricList;
            using (var writer = new StreamWriter("output.csv"))
            {
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.WriteRecords(MetricList);
                }
            }
        }
    }
}
