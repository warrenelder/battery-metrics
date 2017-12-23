using System;
namespace batterymetrics.Model
{
    public class Metric
    {
        public int DeviceId { get; set; }
        public int AccNum { get; set; }
        public int BatteryLifetime { get; set; }
        public int Cycles { get; set; }
    }

    public class Cycle
    {
        public int DeviceId { get; set; }
        public enum Type { C, D, L }
        public int ChargeStart { get; set; }
        public int ChargeEnd { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
