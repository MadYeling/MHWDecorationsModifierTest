﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MHWDecorationsModifier.Beans;
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
        private readonly byte _fistSignatureCode;
        private readonly string[] _signature;
        private readonly Dictionary<int, string> _codeName;

        /// <summary>
        /// 需要特征码中的第几位用来扫描
        /// </summary>
        private const int Excursion = 0x12;

        private static int _archive;

        public MemoryHandler(int archive = JsonHandler.Archive1)
        {
            _myOperator = new MemoryOperator();
            _jsonHandler = new JsonHandler();
            _archive = archive;
            _signature = _jsonHandler.ReadSignature();
            _fistSignatureCode = GetFistSignatureCode();
            _codeName = _jsonHandler.CodeName;
        }

        /// <summary>
        /// 获得特征码中的一位用来扫描
        /// </summary>
        /// <returns>一位特征码</returns>
        private byte GetFistSignatureCode()
        {
            return Convert.ToByte(_signature[Excursion], 16);
        }

        /// <summary>
        /// 将内存中读出的特征码和json中的进行对比
        /// </summary>
        /// <param name="scannedSignature">内存中读出的特征码</param>
        /// <returns>是否相等</returns>
        private bool CompareWithSignature(IReadOnlyList<byte> scannedSignature)
        {
            for (var i = 0; i < _signature.Length; i++)
            {
                if (_signature[i] == "??") continue;
                var signatureByte = Convert.ToByte(_signature[i], 16);
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
            var startScanAddress = archiveBean.FirstScanAddress + Excursion;
            var lastScanAddress = archiveBean.LastScanAddress + Excursion;
            var interval = archiveBean.Interval;
            var subtraction = archiveBean.Subtraction;

            // 因为在扫描的内存地址中数值为A8的过多，为了避免调用过多次方法出错，添加一个偏移量，扫描另一个地址
            for (var i = startScanAddress; i < lastScanAddress; i += interval)
            {
                var b = _myOperator.ReadMemory(i, 1);
                if (b != _fistSignatureCode || !CompareWithSignature(GetLastBytes(i - Excursion))) continue;
                Logger.Debug("寻找到特征码的地址为：" + $"{i - Excursion:x8}");
                return i - Excursion - subtraction;
            }

            Logger.Error("无法寻找到特征码");
            MessageBox.Show("无法寻找到特征码，请联系作者", "警告");
            return 0;
        }

        /// <summary>
        /// 获得拥有的珠子集合
        /// </summary>
        /// <returns>珠子集合</returns>
        public ArrayList GetArchiveDecorations()
        {
            var list = new ArrayList();
            var address = GetDecorationsAddress();
            if (address == 0) return list;
            var count = 0;

            for (var i = 0; i < 120; i++)
            {
                var deAddress = address + i * 16;
                var code = _myOperator.ReadMemory(deAddress, 4);
                count = code == 0 ? count + 1 : 0;

                var number = _myOperator.ReadMemory(deAddress + 0x4, 4);
                var name = code == 0 ? "空" : GetNameByCode(code);

                list.Add(new DecorationBean(name, code, number, deAddress));

                if (count > 10)
                {
                    break;
                }
            }

            return list;
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

        public int GetCodeByName(string name)
        {
            var code = 0;
            foreach (var codeName in _codeName.Where(codeName => codeName.Value.Equals(name)))
            {
                code = codeName.Key;
            }
            return code;
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
            var bytes = new byte[_signature.Length];
            for (var i = 0; i < _signature.Length; i++)
            {
                bytes[i] = (byte) _myOperator.ReadMemory(address + i, 1);
            }

            return bytes;
        }
    }
}