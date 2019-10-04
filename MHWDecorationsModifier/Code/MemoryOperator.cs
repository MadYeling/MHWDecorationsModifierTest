using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows;
using NLog;

namespace MHWDecorationsModifier.Code
{
    public class MemoryOperator
    {
        #region 申明API

        //打开一个已存在的进程对象，并返回进程的句柄
        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //从指定内存中读取字节集数据
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize,
            IntPtr lpNumberOfBytesRead);

        //从指定内存中写入字节集数据
        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, int[] lpBuffer, int nSize,
            IntPtr lpNumberOfBytesWritten);

        //关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hObject);

        #endregion

        /// <summary>
        /// 日志相关
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static int _pid;

        public MemoryOperator()
        {
            var jsonHandler = new JsonHandler();
            _pid = GetPidByName(jsonHandler.ReadProcessName());
            if (_pid == 0)
            {
                Logger.Error("无法获取进程PID");
                MessageBox.Show("无法获取进程PID，可能是你还没有打开游戏，或者被杀毒软件禁止", "警告");
                Environment.Exit(1);
            }
            else
            {
                Logger.Debug("进程PID：" + _pid);
            }
        }

        private int GetPidByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            return (from process in processes where process.ProcessName == processName select process.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// 向指定内存中写入内容
        /// </summary>
        /// <param name="number">写入的内容</param>
        /// <param name="address">内存地址</param>
        /// <returns>是否成功</returns>
        [HandleProcessCorruptedStateExceptions]
        public bool WriteMemory(long address, int number)
        {
            try
            {
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                var hProcess = OpenProcess(0x1F0FFF, false, _pid);
                //从指定内存中写入字节集数据
                WriteProcessMemory(hProcess, (IntPtr) address, new[] {number}, 4, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
                return true;
            }
            catch (AccessViolationException accessViolationException)
            {
                Logger.Fatal(accessViolationException.ToString());
                // 此错误后果可能非常严重需要关闭进程
                MessageBox.Show("发生严重错误，进程已关闭", "警告");
                Environment.Exit(-1);
                return false;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// 访问指定内存指定长度内容
        /// </summary>
        /// <param name="address">内存地址</param>
        /// <param name="byteLength">内容长度</param>
        /// <returns>内存中内容</returns>
        [HandleProcessCorruptedStateExceptions]
        public int ReadMemory(long address, int byteLength)
        {
            try
            {
                var buffer = new byte[byteLength];
                //获取缓冲区地址
                var byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                var hProcess = OpenProcess(0x1F0FFF, false, _pid);
                //将制定内存中的值读入缓冲区
                ReadProcessMemory(hProcess, (IntPtr) address, byteAddress, byteLength, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
                //从非托管内存中读取一个 32 位带符号整数。
                var nub = Marshal.ReadInt32(byteAddress);
                return nub;
            }
            catch (AccessViolationException accessViolationException)
            {
                Logger.Fatal(accessViolationException.ToString());
                // 此错误后果可能非常严重需要关闭进程
                MessageBox.Show("发生严重错误，进程已关闭", "警告");
                Environment.Exit(-1);
                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                return 0;
            }
        }
    }
}