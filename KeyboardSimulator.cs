using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Muse_Dash
{
    public class KeyboardSimulator
    {
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte virtualKey, byte scanCode, int flags, int extraInfo);

        public static void KeyPress(byte keyCode)
        {
            keybd_event(keyCode, 0, 0, 0); // Press key
        }
        public static void KeyRelese(byte keyCode)
        {
            keybd_event(keyCode, 0, KEYEVENTF_KEYUP, 0); // Release key
        }
    }
}
