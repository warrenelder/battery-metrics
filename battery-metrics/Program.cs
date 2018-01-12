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
                Console.WriteLine("Please provide upload filename");
                filename = Console.ReadLine();
            }

            // Upload file (provide file path)
            cc.Read(filename);

            // Output metrics in Console
            cc.View();

            // Output file
            cc.Create();

        }
    }
}
