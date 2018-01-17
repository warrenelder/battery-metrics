namespace batterymetrics.Models
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
        public bool Charging { get; set; }
        public double PredHalfCycleTime { get; set; }
    }

}
