using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.MyException;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace MHWDecorationsModifier
{
    public class JsonHandler
    {
        /// <summary>
        /// 日志相关
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// json文件名称
        /// </summary>
        private const string FileName = "Decorations.json";

        /// <summary>
        /// JSON处理相关，读取的文件保存在当中。
        /// </summary>
        private readonly JToken _jToken;

        /// <summary>
        /// 字典
        /// </summary>
        private readonly Dictionary<int, string> _codeToName;

        public readonly int SignatureLength;

        public const int Archive1 = 0;
        public const int Archive2 = 1;
        public const int Archive3 = 2;

        /// <summary>
        /// 构造函数，只在这里读取文件
        /// </summary>
        public JsonHandler()
        {
            _jToken = ReadJsonFile();
            // 校验读取结果
            _codeToName = ReadAllName();
            SignatureLength = ReadSignature().Length;
            if (_jToken != null) return;
            Logger.Error("无法读取JSON文件");
            Environment.Exit(1);
        }

        /// <summary>
        /// 读取进程名称
        /// </summary>
        /// <returns>进程名称</returns>
        public string ReadProcessName()
        {
            try
            {
                const string keyWord = "ProcessName";
                var token = VerifyKeyWord(keyWord, _jToken);
                return token.ToString();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 读取特征码
        /// </summary>
        /// <returns>特征码字符串</returns>
        public string[] ReadSignature()
        {
            try
            {
                const string keyWord = "Signature";
                var signatureToken = VerifyKeyWord(keyWord, _jToken);
                var signature = signatureToken.ToString().Split(' ');
                return signature;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 读取珠子代码
        /// </summary>
        /// <returns>珠子代码集合</returns>
        public ArrayList ReadCode()
        {
            var list = new ArrayList();
            try
            {
                const string keyWord = "Codes";
                var codeToken = VerifyKeyWord(keyWord, _jToken);

                // 将读取的内容转换为JSON数组
                var jArray = JArray.Parse(codeToken.ToString());
                // 遍历数组，添加进集合
                foreach (var variable in jArray)
                {
                    list.Add(variable);
                }

                return list;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 读取扫描信息
        /// </summary>
        /// <param name="archive">存档号</param>
        /// <returns></returns>
        public ArchiveBean ReadArchiveBean(int archive)
        {
            try
            {
                const string acKey = "Archive";
                var acToken = VerifyKeyWord(acKey, _jToken);

                string keyWord;
                switch (archive)
                {
                    case Archive1:
                        keyWord = "Archive1";
                        break;
                    case Archive2:
                        keyWord = "Archive2";
                        break;
                    case Archive3:
                        keyWord = "Archive3";
                        break;
                    default:
                        keyWord = "Archive1";
                        break;
                }

                var archiveToken = VerifyKeyWord(keyWord, acToken);

                var firstScanAddress = Convert.ToInt64(archiveToken["firstScanAddress"].ToString(), 16);
                var lastScanAddress = Convert.ToInt64(archiveToken["lastScanAddress"].ToString(), 16);
                var interval = Convert.ToInt32(archiveToken["interval"].ToString(), 16);
                var subtraction = Convert.ToInt32(archiveToken["subtraction"].ToString(), 16);

                return new ArchiveBean(firstScanAddress, lastScanAddress, interval, subtraction);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 读取字典
        /// </summary>
        /// <returns>代码名称对照字典</returns>
        private Dictionary<int, string> ReadAllName()
        {
            var map = new Dictionary<int, string>();
            try
            {
                const string keyWord = "Names";
                // 获取代码集合
                var codes = ReadCode();
                // 从json中取出需要的那部分内容
                var name = VerifyKeyWord(keyWord, _jToken);

                // 遍历集合
                foreach (var code in codes)
                {
                    // 通过遍历取出的代码来获取珠子名称
                    var decorationName = name[code.ToString()].ToString();
                    // 使用Convert.ToInt32()将字符串类型的16进制转换为数字
                    var decorationCode = Convert.ToInt32(code.ToString(), 16);
                    // 将珠子代码和名称以一一对应的方式添加至字典中，以便日后使用
                    map.Add(decorationCode, decorationName);
                }

                return map;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 通过代码获取名称
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>名称</returns>
        public string GetNameByCode(int code)
        {
            return _codeToName.ContainsKey(code) ? _codeToName[code] : null;
        }

        /// <summary>
        /// 校验关键字
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <param name="iJToken">json令牌</param>
        /// <returns>通过返回取出的数据，不通过返回null</returns>
        private JToken VerifyKeyWord(string keyWord, JToken iJToken)
        {
            // 校验
            var codeToken = iJToken[keyWord];
            if (codeToken != null) return codeToken;
            Logger.Error("无法找到字段：\"" + keyWord + "\"");
            throw new InvalidKeyWordException("无效的JSON字段！");
        }

        /// <summary>
        /// 读取JSON文件
        /// </summary>
        /// <returns>存储文件内容的令牌</returns>
        private static JToken ReadJsonFile()
        {
            try
            {
                // 获取文件的完整路径
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)
                               ?.Replace("file:\\", "") + "\\" + FileName;
                JToken jToken;
                /*
                 某些类型的非托管对象有数量限制或很耗费系统资源，在代码使用完它们后，尽可能快的释放它们时非常重要的。
                 using语句有助于简化该过程并确保这些资源被适当的处置（dispose）。
                 */
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            jToken = JToken.ReadFrom(jsonReader);
                        }
                    }
                }

                return jToken;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }
    }
}