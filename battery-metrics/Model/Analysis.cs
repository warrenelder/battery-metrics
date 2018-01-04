using System;
namespace batterymetrics.Model
{
    public class Metric
    {
        public int DeviceId { get; set; }
        public int AccNum { get; set; }
        public int BatteryLifetime { get; set; }
        public int ChargeTime { get; set; }
        public int Cycles { get; set; }
        public int LevelCycles { get; set; }
    }

    public class Cycle
    { 
        public int DeviceId { get; set; }
        public int AccNum { get; set; }
        public bool Charging { get; set; }
        public double PredHalfCycleTime { get; set; }
    }

    public class Processor
    {
        public int DeviceId { get; set; }
        public int AccountId { get; set; }
        public DateTime Timestamp { get; set; }
        public int level { get; set; }
        public bool charging { get; set; }
    }
}
