using System;
using System.IO;
using batterymetrics.Controller;

namespace battery_metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleController cc = new ConsoleController();

            string filename = null;

            while (File.Exists(filename) != true)
            {
                Console.WriteLine("Please provide upload file path;");
                filename = Console.ReadLine();
            }

            // Upload file (provide file path)
            cc.Read(filename);

            // Calculate Metrics
            cc.Analyse();

            // Output metrics
            cc.View();

            // Output file
            cc.Create();

        }
    }
}
