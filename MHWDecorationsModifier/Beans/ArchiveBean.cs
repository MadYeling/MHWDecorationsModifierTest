namespace MHWDecorationsModifier.Beans
{
    public class ArchiveBean
    {
        public ArchiveBean(long firstScanAddress, long lastScanAddress)
        {
            FirstScanAddress = firstScanAddress;
            LastScanAddress = lastScanAddress;
        }

        public long FirstScanAddress { get; set; }

        public long LastScanAddress { get; set; }

        public override string ToString()
        {
            return $"扫描从 {FirstScanAddress:x8} 至 {LastScanAddress:x8}";
        }
    }
}