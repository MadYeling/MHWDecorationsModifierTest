namespace MHWDecorationsModifierTest.Beans
{
    public class DecorationBean
    {
        public string Name { get; set; }

        public int Code { get; set; }

        public int Number { get; set; }

        public long Address { get; set; }

        public DecorationBean(string name, int code, int number, long address)
        {
            Name = name;
            Code = code;
            Number = number;
            Address = address;
        }

        public override string ToString()
        {
            return "珠子名称：" + Name + "    \t珠子代码：" + $"{Code:X4}" + "\t珠子数量：" + Number + "\t内存地址：" + $"{Address:x8}";
        }
    }
}