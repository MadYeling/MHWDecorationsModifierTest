using System;
using System.Linq;
using System.Text;
using MHWDecorationsModifierTest.Beans;

namespace MHWDecorationsModifierTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
//            var myOperator = new MemoryOperator();
//            var jsonHandler = new JsonHandler();
//            const long address = 0x9AB6F658;
//
//            var signature = jsonHandler.ReadSignature().Split(' ');
//
//            var b1 = new byte[16];
//            var b2 = new byte[16];
//
//            for (var i = 0; i < 16; i++)
//            {
//                b1[i] = (byte) myOperator.ReadMemory(address + i, 1);
//                b2[i] = Convert.ToByte(signature[i], 16);
//            }
//
//            var sb = new StringBuilder();
//
//            if (b1.SequenceEqual(b2))
//            {
//                Console.WriteLine("ojbk");
//            }
//
//            foreach (var variable in b1)
//            {
//                var s = $"{variable:x2}";
//                sb.Append(s);
//            }
//
//            Console.WriteLine(sb.ToString());
//
//            var code = myOperator.ReadMemory(address - 0xc78, 4);
//            Console.WriteLine("{0:x4}", code);
//            Console.WriteLine(jsonHandler.GetNameByCode(code));

//            var s = new MemoryHandler();
//
//            var a = new byte[] {0xE0, 0x8D, 0xBB, 0x42, 0x01, 00, 00, 00, 00, 00, 00, 00, 0x97, 00, 00, 00};
//
//            Console.WriteLine(s.CompareWithSignature(a));

//            var s = new MemoryHandler();
//            Console.WriteLine("{0:x8}", s.GetDecorationsAddress(JsonHandler.Archive1));

//            var s = new JsonHandler();
//            Console.WriteLine(s.ReadArchiveBean(JsonHandler.Archive3));

            var q = new MemoryHandler(JsonHandler.Archive1);
            var s = q.GetArchiveDecorations();

            for (var i = 0; i < s.Count; i++)
            {
                Console.Write("编号：" + (i + 1) + " \t");
                Console.WriteLine(s[i].ToString());
            }

            Console.WriteLine("编号：");
            var num = Convert.ToInt32(Console.ReadLine());
            var address = ((DecorationBean) s[num - 1]).Address;
            Console.WriteLine("珠子代码：");
            var code = Convert.ToInt32(Console.ReadLine(), 16);
            Console.WriteLine("数量：");
            var nub = Convert.ToInt32(Console.ReadLine());

            var name = new JsonHandler().GetNameByCode(code);
            Console.WriteLine("即将修改的珠子为：" + name + "\n是否确定？(Y/N)");
            var k = Console.Read();
            if (k != 89 && k != 121) return;
            var b = new DecorationBean(name, code, nub, address);
            Console.WriteLine(q.ChangeDecoration(b));
            Console.ReadKey();

//            Console.WriteLine("{0:x2}", new MemoryHandler().GetFistSignatureCode());

//            var a = new DecorationBean("", 0x02d7, 11, 0x99EDE9E0);
//            var s = new MemoryHandler().ChangeDecoration(a);
//            Console.WriteLine(s);
        }
    }
}