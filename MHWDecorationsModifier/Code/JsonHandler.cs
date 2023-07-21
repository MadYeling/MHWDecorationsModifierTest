using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.MyException;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace MHWDecorationsModifier.Code
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
        private readonly Dictionary<int, string> _codeName;

        public const int Archive1 = 1;
        public const int Archive2 = 2;
        public const int Archive3 = 3;

        /// <summary>
        /// 构造函数，只在这里读取文件
        /// </summary>
        public JsonHandler()
        {
            _jToken = ReadJsonFile();
            _codeName = ReadAllName();
            // 校验读取结果
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
                Logger.Debug("特征码：" + signatureToken);
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
        /// 读取扫描间隔
        /// </summary>
        /// <returns>扫描间隔</returns>
        public int ReadInterval()
        {
            try
            {
                const string keyWord = "interval";
                var token = VerifyKeyWord(keyWord, _jToken);
                return Convert.ToInt32(token.ToString(), 16);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return 0x10000;
            }
        }

        /// <summary>
        /// 读取珠子首位差
        /// </summary>
        /// <returns>首位差</returns>
        public int ReadSubtraction()
        {
            try
            {
                const string keyWord = "subtraction";
                var token = VerifyKeyWord(keyWord, _jToken);
                return Convert.ToInt32(token.ToString(), 16);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return 0x1F38;
            }
        }

        /// <summary>
        /// 读取名字首位差
        /// </summary>
        /// <returns>首位差</returns>
        public int ReadNameSub()
        {
            try
            {
                const string keyWord = "name";
                var token = VerifyKeyWord(keyWord, _jToken);
                return Convert.ToInt32(token.ToString(), 16);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return 0x10000;
            }
        }

        /// <summary>
        /// 通过代码获取名称
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>名称</returns>
        public string GetNameByCode(int code)
        {
            return _codeName.ContainsKey(code) ? _codeName[code] : "未知";
        }

        /// <summary>
        /// 通过名称获取代码
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>代码</returns>
        public int GetCodeByName(string name)
        {
            var code = 0;
            foreach (var codeName in _codeName.Where(codeName => codeName.Value.Contains(name)))
            {
                code = codeName.Key;
            }

            return code;
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

                return new ArchiveBean(firstScanAddress, lastScanAddress);
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
                const string keyWord = "Decorations";
                // 从json中取出需要的那部分内容
                var name = VerifyKeyWord(keyWord, _jToken);
                var jObj = JObject.Parse(name.ToString());

                var codes = new List<string>();
                foreach (var item in jObj)
                {
                    codes.Add(item.Key);
                }

                // 遍历集合
                foreach (var code in codes)
                {
                    // 通过遍历取出的代码来获取珠子名称
                    var decorationName = name[code].ToString();
                    // 使用Convert.ToInt32()将字符串类型的16进制转换为数字
                    var decorationCode = Convert.ToInt32(code, 16);
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