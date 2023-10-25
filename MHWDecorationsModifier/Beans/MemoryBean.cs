namespace MHWDecorationsModifier.Beans
{
    public class MemoryBean
    {
        public long NameAddress { get; set; }

        public long DecorationAddress { get; set; }

        public MemoryBean(long nameAddress, long decorationAddress)
        {
            NameAddress = nameAddress;
            DecorationAddress = decorationAddress;
        }
    }
}