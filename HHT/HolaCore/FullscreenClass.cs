using System;
using System.Runtime.InteropServices;

namespace HolaCore
{
    public class FullscreenClass
    {
        public static uint SIPF_OFF = 0x00;
        public static uint SIPF_ON = 0x01;

        [DllImport("coredll.dll")]
        public extern static void SipShowIM(uint dwFlag);

        [DllImport("coredll.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpWindowName, string lpClassName);

        [DllImport("coredll.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);

        public const int SW_SHOW = 5; //显示窗口常量
        public const int SW_HIDE = 0; //隐藏窗口常量

        public static bool Fullscreen(bool FLAG)
        {
            int Hwnd = FindWindow("HHTaskBar", null);
            if (Hwnd == 0) return false;
            else
            {
                if (FLAG)
                {
                    ShowWindow(Hwnd, SW_HIDE);
                }
                else
                {
                    ShowWindow(Hwnd, SW_SHOW);
                }
            }
            return true;
        }

        public static void ShowSIP(bool bShow)
        {
            if (bShow)
            {
                SipShowIM(SIPF_ON);
            }
            else
            {
                SipShowIM(SIPF_OFF);
            }
        }
    }
}