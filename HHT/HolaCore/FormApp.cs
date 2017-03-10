using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;

namespace HolaCore
{
    public partial class FormApp : Form, ConnCallback
    {
        const string Hola_In = "1";
        const string Hola_Out = "2";
        const string Hola_Excel = "3";
        const string Hola_Business = "4";
        const string Hola_Inventory = "5"; 

        const string suffixDomain = ".app";

        FormDownload formDownload = null;
        private const string OGuid = "00000000000000000000000000000000";
        //网络请求线程
        private ConnThread connThread = null;

        private ConnThread LogOutThread = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        private int TaskBarHeight = 0;
        #region 应用编号
        public const int APP_IN = 1;
        public const int APP_OUT = 2;
        public const int APP_EXCEL = 3;
        public const int APP_PD = 4;
        public const int APP_YYK = 5;
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            LOGOUT,
            DOWNLOAD_APP,
            TEST,
            API_ID01
        }
        private API_ID apiID = API_ID.NONE;
        #endregion

        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        [DllImport("cellcore.dll", SetLastError = true)]

        private static extern int ConnMgrConnectionStatus(IntPtr hConnection, out long pdwStatus);
        public FormApp()
        {
            InitializeComponent();

            doLayout(); 
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                btnIn.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("_in")));
                btnIn.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("in_p")));
                btnOut.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("_out")));
                btnOut.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("out_p")));
                btnExcel.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("excel")));
                btnExcel.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("excel_p")));
                btnPing.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("ping")));
                btnPing.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("ping_p")));
                btnInventory.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("pd")));
                btnBusiness.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("yyk")));
                Goback.Top = dstHeight - Goback.Height;
                btnLogout.Top = dstHeight - btnLogout.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnLogout.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        public void init(string data)
        {
            if (data.IndexOf(Hola_In) < 0)
            {
                btnIn.Visible = false;
            }

            if (data.IndexOf(Hola_Out) < 0)
            {
                btnOut.Visible = false;
            }

            if (data.IndexOf(Hola_Excel) < 0)
            {
                btnExcel.Visible = false;
            }

            if (data.IndexOf(Hola_Business) < 0)
            {
                btnBusiness.Visible = false;
            }

            if (data.IndexOf(Hola_Inventory) < 0)
            {
                btnInventory.Visible = false;
            }
        }

        //等待网络请求返回
        private void wait()
        {
            Cursor.Current = Cursors.WaitCursor;

            busy = true;
        }

        //网络请求已返回
        private void idle()
        {
            Cursor.Current = Cursors.Default;

            busy = false;
        }

        //注销按钮响应
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                logout();
            }
        }

        //下载指定应用
        private void download(int index)
        {
            if (!busy)
            {
                try
                {
                    apiID = API_ID.DOWNLOAD_APP;

                    if (Config.Server != null && Config.Server.app.Count > index && Config.Server.app[index] != null)
                    {
                        string app = (string)Config.Server.app[index];
                        app = app.Split('.').GetValue(0).ToString() + ".dll";
                        string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + app;
                        string to = Config.DirLocal + (string)(Config.Server.app[index]);

                        connThread = new ConnThread(this);
                        connThread.Download(from, to, true);

                        if (formDownload == null)
                        {
                            formDownload = new FormDownload();
                        }

                        if (DialogResult.Cancel == formDownload.ShowDialog())
                        {
                            connThread.Abort();
                            connThread.Stop();
                            formDownload.Dispose();
                            formDownload = null;
                        }
                        else
                        {
                            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                            AppDomain ad = AppDomain.CreateDomain(assemblyName.Name + suffixDomain);
                            ad.ExecuteAssembly(to);
                            AppDomain.Unload(ad);
                            connThread = null;

                            if (Config.LoginTwice.Equals("True"))
                            {
                                Config.loginTwice = "";
                                Config.save();
                                Close();
                            }
                            else
                            {
                                Show();
                            }

                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    Show();
                    if (MessageBox.Show(ex.ToString()) == DialogResult.OK)
                    {
                        ;
                    }
                }
            }
        }

        private void request01()
        {
            apiID = API_ID.API_ID01;
            string msg = "request=A00;usr=" + Config.User + ";op=01;hhtIp=" + Config.IPLocal;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }


        //下载OR直接运行收货应用
        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    string to = Config.DirLocal + (string)(Config.Server.app[APP_IN]);
                    if (File.Exists(to))
                    {
                        wait();
                        ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                            {
                                AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                                AppDomain ad = AppDomain.CreateDomain(assemblyName.Name + suffixDomain);
                                ad.ExecuteAssembly(to);
                                AppDomain.Unload(ad);
                                this.Invoke(new InvokeDelegate(() =>
                                    {
                                        idle();
                                        if (Config.LoginTwice.Equals("True"))
                                        {
                                            Config.loginTwice = "";
                                            Config.save();
                                            Close();
                                        }
                                        else
                                        {
                                            Show();
                                        }
                                    }
                                ));
                            }));
                    }
                    else
                    {
                        //request01();
                        download(APP_IN);
                    }
                }
                
            }
            catch (Exception ex)
            {
                idle();
                if (MessageBox.Show(ex.ToString()) == DialogResult.OK)
                {
                    ;
                }
            }
        }

        //下载OR直接运行出货应用
        private void btnOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    string to = Config.DirLocal + (string)(Config.Server.app[APP_OUT]);
                    if (File.Exists(to))
                    {
                        wait();
                        ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                        {
                            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                            AppDomain ad = AppDomain.CreateDomain(assemblyName.Name + suffixDomain);
                            ad.ExecuteAssembly(to);
                            AppDomain.Unload(ad);
                           
                            this.Invoke(new InvokeDelegate(() =>
                            {
                                idle();
                                if (Config.LoginTwice.Equals("True"))
                                {
                                    Config.loginTwice = "";
                                    Config.save();
                                    Close();
                                }
                                else
                                {
                                    Show();
                                }

                            }
                            ));
                        }));
                    }
                    else
                    {
                        download(APP_OUT);
                    }
                }
            }
            catch (Exception ex)
            {
                idle();
                if (MessageBox.Show(ex.ToString()) == DialogResult.OK)
                {
                    ;
                }
            }
        }

        //下载OR直接运行查询&报表应用
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    string to = Config.DirLocal + (string)(Config.Server.app[APP_EXCEL]);
                    if (File.Exists(to))
                    {
                        wait();
                        ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                        {
                            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                            AppDomain ad = AppDomain.CreateDomain(assemblyName.Name + suffixDomain);
                            ad.ExecuteAssembly(to);
                            AppDomain.Unload(ad);
                            this.Invoke(new InvokeDelegate(() =>
                            {
                                idle();
                                if (Config.LoginTwice.Equals("True"))
                                {
                                    Config.loginTwice = "";
                                    Config.save();
                                    Close();
                                }
                                else
                                {
                                    Show();
                                }
                            }
                            ));
                        }));
                    }
                    else
                    {
                        download(APP_EXCEL);
                    }
                }
            }
            catch (Exception ex)
            {
                idle();
                if (MessageBox.Show(ex.ToString()) == DialogResult.OK)
                {
                    ;
                }
            }
        }

        private void btnPd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    string to = Config.DirLocal + (string)(Config.Server.app[APP_PD]);
                    if (File.Exists(to))
                    {
                        wait();
                        ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                        {
                            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                            AppDomain ad = AppDomain.CreateDomain(assemblyName.Name + suffixDomain);
                            ad.ExecuteAssembly(to);
                            AppDomain.Unload(ad);
                            this.Invoke(new InvokeDelegate(() =>
                            {
                                idle();
                                if (Config.LoginTwice.Equals("True"))
                                {
                                    Config.loginTwice = "";
                                    Config.save();
                                    Close();
                                }
                                else
                                {
                                    Show();
                                }
                            }
                            ));
                        }));
                    }
                    else
                    {
                        download(APP_PD);
                    }
                }
            }
            catch (Exception ex)
            {
                idle();
                if (MessageBox.Show(ex.ToString()) == DialogResult.OK)
                {
                    ;
                }
            }
        }

        private void btnYyk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    string to = Config.DirLocal + (string)(Config.Server.app[APP_YYK]);
                    if (File.Exists(to))
                    {
                        wait();
                        ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                        {
                            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                            AppDomain ad = AppDomain.CreateDomain(assemblyName.Name + suffixDomain);
                            ad.ExecuteAssembly(to);
                            AppDomain.Unload(ad);
                            this.Invoke(new InvokeDelegate(() =>
                            {
                                idle();
                                if (Config.LoginTwice.Equals("True"))
                                {
                                    Config.loginTwice = "";
                                    Config.save();
                                    Close();
                                }
                                else
                                {
                                    Show();
                                }
                            }
                            ));
                        }));
                    }
                    else
                    {
                        download(APP_YYK);
                    }
                }

            }
            catch (Exception ex)
            {
                idle();
                if (MessageBox.Show(ex.ToString()) == DialogResult.OK)
                {
                    ;
                }
            }
        }

        //注销请求
        public void logout()
        {
            apiID = API_ID.LOGOUT;

            string msg = "request=logout;usr=" + Config.User + ";sn=" + Config.MAC;
            msg = OGuid + msg;
            LogOutThread = new ConnThread(this);
            LogOutThread.Send(msg);
            wait();
        }

        #region 实现ConnCallback接口
        //请求进度回调
        public void progressCallback(int total, int progress)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                if (formDownload != null)
                {
                    formDownload.setProgress(total, progress);
                }
            }));
        }

        //网络请求回调
        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();

                switch (apiID)
                {
                    case API_ID.DOWNLOAD_APP:
                        if (result == ConnThread.RESULT_FILE)
                        {
                            if (formDownload != null)
                            {
                                formDownload.DialogResult = DialogResult.OK;
                                formDownload.Dispose();
                                formDownload = null;
                            }

                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.LOGOUT:
                        {
                            Close();
                        }
                        break;

                    case API_ID.API_ID01:
                        if (result == ConnThread.RESULT_OK)
                        {
                            //Config.stoNO = data;
                        }
                        else if (result == ConnThread.RESULT_DUPLOGIN)
                        {
                            if (MessageBox.Show("已登录，您确定要重新登录吗？", "",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                Config.loginTwice = "True";
                                Config.save();
                                Application.Exit();
                            }
                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;

                    default:
                        break;
                }
            }));
        }
        #endregion

        private void Goback_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (LogOutThread != null)
                    {
                        LogOutThread.Abort();
                        LogOutThread.Stop();
                        LogOutThread = null;
                    }

                    Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    new FormPing().ShowDialog();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}