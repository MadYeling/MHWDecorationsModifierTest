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
            // new Program().m();
            var mh = new MemoryHandler();
            Console.WriteLine(mh.GetDecorationsAddress());
        }

        private void m()
        {
            Console.WriteLine("输入存档号（1或2或3）");
            var archive = Console.ReadLine();
            var userArc = Convert.ToInt32(archive);
            var arc = userArc;
            if (userArc != JsonHandler.Archive1 && userArc != JsonHandler.Archive2 && userArc != JsonHandler.Archive3)
            {
                arc = JsonHandler.Archive1;
            }
            
            

            //while (true)
            //{
            //    var q = new MemoryHandler(arc);
            //    var s = q.GetArchiveDecorations();
            //    for (var i = 0; i < s.Count; i++)
            //    {
            //        Console.Write("编号：" + (i + 1) + " \t");
            //        Console.WriteLine(s[i].ToString());
            //    }

            //    Console.WriteLine("提示！如果是4脚双技能珠，请输入完整名称");

            //    Console.WriteLine("编号：");
            //    var num = Convert.ToInt32(Console.ReadLine());
            //    var address = ((DecorationBean) s[num - 1]).Address;
            //    Console.WriteLine("珠子名称：");
            //    var codeName = Console.ReadLine();
            //    Console.WriteLine("数量：");
            //    var nub = Convert.ToInt32(Console.ReadLine());
            //    nub = nub < 0 ? 1 : nub;

            //    var code = q.GetCodeByName(codeName);
            //    if (code == 0)
            //    {
            //        Console.WriteLine("无法找到你需要的珠子");
            //        Console.ReadKey();
            //        return;
            //    }

            //    var name = q.GetNameByCode(code);
            //    Console.WriteLine("即将修改的珠子为：【" + name + "】\n是否确定？(Y/N)");
            //    var k = Console.Read();
            //    if (k != 'Y' && k != 'y') return;
            //    var b = new DecorationBean(name, code, nub, address);
            //    Console.WriteLine(q.ChangeDecoration(b));
            //    Console.ReadLine();
            //    Console.Clear();
            //}
        }
    }
}