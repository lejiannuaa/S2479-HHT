using System;

using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace Hola_Business
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new FormMain()); 
        }
    }
}