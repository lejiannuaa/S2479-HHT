using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using HolaCore;

namespace Hola_Excel
{
    static class Program
    {
        const string suffixEXE = ".exe";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            string exeName = Assembly.GetExecutingAssembly().GetName().Name + suffixEXE;
            string domainName = AppDomain.CurrentDomain.FriendlyName;

            //if (!domainName.Equals(exeName))
            {
                //FullscreenClass.Fullscreen(true);
                try
                {
                    Application.Run(new FormMain());
                }
                catch (Exception)
                {
                }
                //FullscreenClass.Fullscreen(false);               
            }
        }
    }
}