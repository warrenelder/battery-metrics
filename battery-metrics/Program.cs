using System;
using batterymetrics.Controller;

namespace battery_metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleController cc = new ConsoleController();

            // Console prompt text
            Console.WriteLine("Please provide upload file path;");

            // Upload file (provide file path)
            string path = Console.ReadLine();
            cc.Read(path);

            // Calculate Metrics
            cc.Analyse();

            // Output metrics
            cc.View();

            // Output file
            cc.Create();

        }
    }
}
