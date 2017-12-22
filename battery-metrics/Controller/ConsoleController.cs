using System.IO;
using System.Linq;
using batterymetrics.Utilities;

namespace batterymetrics.Controller
{
    public class ConsoleController
    {
        public void Read(string path)
        {
            FileInfo file = new FileInfo(path);

            var devices = File.ReadAllLines(path).Skip(1)
                                 .Select(l => DeviceParser.FromTsv(l))
                                 .GroupBy(d => d.deviceId)
                                 .ToArray();
        }
    }
}