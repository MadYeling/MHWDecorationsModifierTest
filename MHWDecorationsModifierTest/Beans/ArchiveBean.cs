namespace MHWDecorationsModifierTest.Beans
{
    public class ArchiveBean
    {
        public ArchiveBean(long firstScanAddress, long lastScanAddress, int interval, int subtraction)
        {
            this.FirstScanAddress = firstScanAddress;
            this.LastScanAddress = lastScanAddress;
            this.Interval = interval;
            this.Subtraction = subtraction;
        }

        public long FirstScanAddress { get; set; }

        public long LastScanAddress { get; set; }

        public int Interval { get; set; }

        public int Subtraction { get; set; }

        public override string ToString()
        {
            return "扫描从 " + $"{FirstScanAddress:x8}" + " 至 " + $"{LastScanAddress:x8}" + "\n"
                   + "扫描间隔 " + $"{Interval:X}" + "\n扫描后减去 " + $"{Subtraction:X}";
        }
    }
}