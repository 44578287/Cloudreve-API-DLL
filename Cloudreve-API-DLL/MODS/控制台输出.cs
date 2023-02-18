using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudreve_API_DLL.MODS
{
    public class ConsolePrint
    {
        /// <summary>
        /// 文件大小转换
        /// </summary>
        public static class SizeConversion
        {
            /// <summary>
            /// 将文件大小(字节)转换为最适合的显示方式
            /// </summary>
            /// <param name="size">文件字节</param>
            /// <returns>返回转换后的字符串</returns>
            public static string AutoSizeConversion(long? size)
            {
                string result = "0KB";
                int? filelength = size.ToString()?.Length;
                if (filelength < 4)
                    result = size + "byte";
                else if (filelength < 7)
                    result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
                else if (filelength < 10)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
                else if (filelength < 13)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
                else
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
                return result;
            }
            /// <summary>
            /// 将文件大小(字节)统一转换为K为单位
            /// </summary>
            /// <param name="size">文件字节</param>
            /// <returns>返回转换后的字符串</returns>
            public static double SizeConversionToK(long? size)
            {
                double result = 0;
                if (size > 0)
                {
                    result = Math.Round(Convert.ToDouble(size / 1024d), 2);
                }
                return result;
            }
            /// <summary>
            /// 将文件大小(字节)统一转换为M为单位
            /// </summary>
            /// <param name="size">文件字节</param>
            /// <returns>返回转换后的字符串</returns>
            public static double SizeConversionToM(long? size)
            {
                double result = 0;
                if (size > 0)
                {
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2);
                }
                return result;
            }
            /// <summary>
            /// 将文件大小(字节)统一转换为G为单位
            /// </summary>
            /// <param name="size">文件字节</param>
            /// <returns>返回转换后的字符串</returns>
            public static double SizeConversionToG(long? size)
            {
                double result = 0;
                if (size > 0)
                {
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2);
                }
                return result;
            }
        }
        /// <summary>
        /// 进度条
        /// </summary>
        public class Consoler
        {
            private static string? lastContext = "";//用于记录上次写的内容
            private static readonly object _lock = new object();//加锁保证只有一个输出
            public static void Write(string context)
            {
                lastContext = context;//记录上次写的内容
                lock (_lock)
                {
                    Console.Write(context);
                }
            }
            /// <summary>
            /// 覆写
            /// </summary>
            /// <param name="context"></param>
            public static void OverWrite(string? context = null)
            {
                lastContext = context;//记录上次写的内容
                var strLen = context?.Length ?? 0;
                //空白格的长度，考虑到内容可能超出一行的宽度，所以求余。
                var blankLen = Console.WindowWidth - strLen % Console.WindowWidth - 1;
                var rowCount = strLen / Console.WindowWidth;
                Console.SetCursorPosition(0, Console.CursorTop - rowCount);
                //空白只需填充最后一行的剩余位置即可。
                lock (_lock)
                {
                    Console.Write(context + new string(' ', blankLen));
                }
            }
            public static void WriteLine(string? context = null)
            {
                ClearConsoleLine();//清除最后一行
                lock (_lock)
                {
                    Console.WriteLine(context);
                    if (!string.IsNullOrWhiteSpace(lastContext))
                        Console.Write(lastContext);//重新输出最后一次的内容，否则有较明显的闪烁
                    lastContext = null;
                }
            }
            public static void ClearConsoleLine(int invertedIndex = 0)
            {
                int currentLineCursor = Console.CursorTop;
                int top = Console.CursorTop - invertedIndex;
                top = top < 0 ? 0 : top;
                Console.SetCursorPosition(0, top);
                Console.Write(new string(' ', Console.WindowWidth - 1));
                Console.SetCursorPosition(0, currentLineCursor);
            }
        }
    }
}
