using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Muse_Dash
{
    public class WindowHelper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // 定义委托类型，用于传递给EnumWindows函数
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // 枚举所有窗口，并返回窗口句柄列表
        public static Process[] GetAllProcesses()
        {
            var processes = Process.GetProcesses();
            return processes;
        }

        // 通过窗口标题查找指定的窗口，并将其置顶
        public static bool SetWindowTop(string windowTitle)
        {
            bool result = false;

            // 枚举所有窗口，并找到指定标题的窗口
            EnumWindows((hWnd, lParam) => {
                if (GetWindowCaption(hWnd).Contains(windowTitle))
                {
                    // 将找到的窗口置顶
                    SetForegroundWindow(hWnd);
                    result = true;
                    return false;
                }
                return true;
            }, IntPtr.Zero);

            return result;
        }

        // 获取窗口的标题
        private static string GetWindowCaption(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            if (length == 0)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder(length + 1);
            GetWindowText(hWnd, builder, builder.Capacity);
            return builder.ToString();
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);
    }
}
