using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MHWDecorationsModifier.Beans;
using System.Text;
using NLog;

namespace MHWDecorationsModifier.Code
{
    public class MemoryHandler
    {
        /// <summary>
        /// 日志相关
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly MemoryOperator _myOperator;
        private readonly JsonHandler _jsonHandler;

        /// <summary>
        /// 配置中特征码的其中一位
        /// </summary>
        private readonly byte _oneByteConfigSignature;

        /// <summary>
        /// 配置中的特征码字符串数组<br/>
        /// 因为有"??"的存在，必须使用字符串
        /// </summary>
        private readonly string[] _configSignature;

        /// <summary>
        /// 需要特征码中的第几位用来扫描
        /// </summary>
        private const int Deviation = 4;

        private readonly long _decorationAddress = 0;

        private static int _archive;

        public MemoryHandler(int archive = JsonHandler.Archive1)
        {
            _myOperator = new MemoryOperator();
            _jsonHandler = new JsonHandler();
            _archive = archive;
            _configSignature = _jsonHandler.ReadSignature();
            _oneByteConfigSignature = GetOneByteConfigSignature();
            _decorationAddress = GetDecorationsAddress();
        }

        /// <summary>
        /// 获得特征码中的一位用来扫描
        /// </summary>
        /// <returns>一位特征码</returns>
        private byte GetOneByteConfigSignature()
        {
            return Convert.ToByte(_configSignature[Deviation], 16);
        }

        /// <summary>
        /// 将内存中读出的特征码和json中的进行对比
        /// </summary>
        /// <param name="scannedSignature">内存中读出的特征码</param>
        /// <returns>是否相等</returns>
        private bool CompareWithSignature(IReadOnlyList<byte> scannedSignature)
        {
            for (var i = 0; i < _configSignature.Length; i++)
            {
                if (_configSignature[i] == "??") continue;
                var signatureByte = Convert.ToByte(_configSignature[i], 16);
                var scannedSignatureByte = scannedSignature[i];
                if (signatureByte != scannedSignatureByte) return false;
            }

            return true;
        }

        /// <summary>
        /// 通过扫描指定内存段，获取珠子所在内存段的起始地址
        /// </summary>
        /// <returns>地址</returns>
        private long GetDecorationsAddress()
        {
            var archiveBean = _jsonHandler.ReadArchiveBean(_archive);
            var startScanAddress = archiveBean.FirstScanAddress + Deviation;
            var lastScanAddress = archiveBean.LastScanAddress + Deviation;
            var interval = _jsonHandler.ReadInterval();
            var subtraction = _jsonHandler.ReadSubtraction();

            // 因为在扫描的内存地址中数值为A8的过多，为了避免调用过多次方法出错，添加一个偏移量，扫描另一个地址
            for (var i = startScanAddress; i < lastScanAddress; i += interval)
            {
                var b = _myOperator.ReadMemory(i, 1);
                if (b != _oneByteConfigSignature || !CompareWithSignature(GetLastBytes(i - Deviation))) continue;
                Logger.Debug("寻找到特征码的地址为：" + $"{i - Deviation:X8}");
                return i - Deviation - subtraction;
            }

            Logger.Error("无法寻找到特征码");
            return 0;
        }

        /// <summary>
        /// 获得拥有的珠子集合
        /// </summary>
        /// <returns>珠子集合</returns>
        public ArrayList GetArchiveDecorations()
        {
            Logger.Debug($"起始地址：{_decorationAddress:X8}, 开始获取珠子列表");
            var list = new ArrayList();
            if (_decorationAddress == 0) return list;
            var count = 0;

            // 目前一共应该是404个珠子
            for (var i = 0; i < 410; i++)
            {
                var deAddress = _decorationAddress + i * 16;
                var code = _myOperator.ReadMemory(deAddress, 4);
                count = code == 0 ? count + 1 : 0;
                var number = _myOperator.ReadMemory(deAddress + 0x4, 4);
                var name = code == 0 ? "空" : _jsonHandler.GetNameByCode(code);
                list.Add(new DecorationBean(name, code, number, deAddress));

                if (count > 5) break;
            }

            return list;
        }

        /// <summary>
        /// 访问玩家名称
        /// </summary>
        /// <returns>玩家名称</returns>
        public string GetPlayerName()
        {
            var address = _decorationAddress - _jsonHandler.ReadNameSub();
            const int byteLength = 64;
            var nameByte = new byte[byteLength];
            for (var i = 0; i < byteLength; i++)
            {
                nameByte[i] = Convert.ToByte(_myOperator.ReadMemory(address + i, 1));
            }

            return Encoding.UTF8.GetString(nameByte);
        }

        /// <summary>
        /// 修改珠子信息
        /// </summary>
        /// <param name="decorationBean">珠子bean</param>
        /// <returns>是否成功</returns>
        public bool ChangeDecoration(DecorationBean decorationBean)
        {
            var address = decorationBean.Address;
            var code = decorationBean.Code;
            var number = decorationBean.Number;

            var b = _myOperator.WriteMemory(address, code);
            return b && _myOperator.WriteMemory(address + 0x4, number);
        }

        /// <summary>
        /// 获得内存中疑似特征码完整内容用以对比
        /// </summary>
        /// <param name="address">地址</param>
        /// <returns>疑似特征码完整内容</returns>
        private byte[] GetLastBytes(long address)
        {
            var bytes = new byte[_configSignature.Length];
            for (var i = 0; i < _configSignature.Length; i++)
            {
                bytes[i] = (byte)_myOperator.ReadMemory(address + i, 1);
            }

            return bytes;
        }
    }
}