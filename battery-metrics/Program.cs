using System;
using batterymetrics.Controller;

namespace battery_metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            ConsoleController cc = new ConsoleController();

            // Console prompt text
            Console.WriteLine("Please provide file path;");

            // Upload file (provide file path)
            path = Console.ReadLine();
            cc.Read(path);

            // Calculate Metrics

            // Output metrics

        }
    }
}
