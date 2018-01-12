using System;
using System.Globalization;
using batterymetrics.Model;

namespace batterymetrics.Utilities
{
    public static class DeviceParser
    {
        public static Device FromTsv(string tsvLine)
        {
            char[] quotes = { '\"', ' ' };
            string[] values = tsvLine.Split('\t');


            Device device = new Device
            {
                deviceId = int.Parse(values[0]),
                accntNo = int.Parse(values[1]),
                timestamp = DateTime.ParseExact(values[2].Trim(quotes), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                jsonData = values[3].Trim(quotes).Replace("\"\"", "'"),
            };

            return device;
        }
    }
}