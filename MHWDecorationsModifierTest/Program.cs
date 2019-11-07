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
            var q = new MemoryHandler(JsonHandler.Archive1);
            var s = q.GetArchiveDecorations();
            for (var i = 0; i < s.Count; i++)
            {
                Console.Write("编号：" + (i + 1) + " \t");
                Console.WriteLine(s[i].ToString());
            }
            Console.WriteLine("提示！程序已经更新，可以做到在下方输入珠子名称而不是代码来刷珠子，请输入较为完整的名称且无错别字");

            Console.WriteLine("编号：");
            var num = Convert.ToInt32(Console.ReadLine());
            var address = ((DecorationBean) s[num - 1]).Address;
            Console.WriteLine("珠子名称：");
            var codeName = Console.ReadLine();
            Console.WriteLine("数量：");
            var nub = Convert.ToInt32(Console.ReadLine());

            var code = q.GetCodeByName(codeName);
            if (code == 0)
            {
                Console.WriteLine("无法找到你需要的珠子");
                return;
            }

            var name = q.GetNameByCode(code);
            Console.WriteLine("即将修改的珠子为：【" + name + "】\n是否确定？(Y/N)");
            var k = Console.Read();
            if (k != 'Y' && k != 'y') return;
            var b = new DecorationBean(name, code, nub, address);
            Console.WriteLine(q.ChangeDecoration(b));
            Console.ReadKey();
        }
    }
}