using System;

using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace HolaShell
{
    static class Program
    {
        const string suffixEXE = ".exe";
        const string suffixDLL = ".dll";
        const string suffixUPDATE = ".update";

        const string dllNamespace = "HolaCore";
        const string formUpdate = ".FormUpdate";
        const string formLogin = ".FormLogin";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            try
            {
                AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                string dir = Path.GetDirectoryName(assemblyName.CodeBase) + "\\";
                string exeName = assemblyName.Name + suffixEXE;
                string dllName = dllNamespace + suffixDLL;
                string domainName = AppDomain.CurrentDomain.FriendlyName;

                //首次运行
                if (domainName.Equals(exeName))
                {
                    //在新的域内二次运行
                    AppDomain ad = AppDomain.CreateDomain(exeName + suffixUPDATE);
                    ad.ExecuteAssembly(exeName);
                    AppDomain.Unload(ad);
                }
                //二次运行
                else //if (domainName.Equals(exeName + suffixUPDATE))
                {
                    //下载新版
                    Assembly assembly = Assembly.LoadFrom(dllName);
                    Type type = assembly.GetType(dllNamespace + formUpdate);
                    Application.Run((Form)Activator.CreateInstance(type));
                }

                //首次运行
                if (domainName.Equals(exeName))
                {
                    //如已更新
                    if (File.Exists(dir + dllName + suffixUPDATE))
                    {
                        //拷贝新版覆盖旧版、删除新版
                        File.Copy(dir + dllName + suffixUPDATE, dir + dllName, true);
                        File.Delete(dir + dllName + suffixUPDATE);
                    }

                    //正式运行
                    Assembly assembly = Assembly.LoadFrom(dllName);
                    Type type = assembly.GetType(dllNamespace + formLogin);
                    Application.Run((Form)Activator.CreateInstance(type));
                }
            }
            catch (Exception)
            {
            }
        }
    }
}