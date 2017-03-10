using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace HolaCore
{
    public partial class FormLogin : Form, ConnCallback
    {
        //输入格式正则表达式
        Regex reg = new Regex("[^\\da-zA-Z]");
        
        //指示当前正在发送网络请求
        private bool busy = false;
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private int TaskBarHeight = 0;

        private string FtpUpLoadPath = "/HHTLog/" + Config.MAC;//上传日志文件路径
        private bool bUpLoad = false;//是否上传成功

        private Thread FtpThread = null;
        private ConnThread loginThread = null;
        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            LOGIN,
            LOGIN2,
            API_ID01
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormLogin()
        {
            InitializeComponent();

            doLayout();              
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                string picBG = Screen.PrimaryScreen.Bounds.Width == srcWidth ? "bg" : "bg2";
                picFormBG.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject(picBG)));
                picFormBG.Size = new Size(dstWidth, dstHeight);
                lbVersion.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();

                ResumeLayout(false);
            }
            catch (Exception)
            {
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

        private void exit()
        {
            if (loginThread != null)
            {
                loginThread.Abort();
                loginThread.Stop();
                loginThread.Dispose();
                loginThread = null;
            }
            
            Application.Exit();
            GC.Collect();
        }

        //退出按钮响应
        private void btnExit_Click(object sender, EventArgs e)
        {
            //Config.stoNO = "";
            exit();
        }

        //用户名格式合法性监测
        private void tbUsr_TextChanged(object sender, EventArgs e)
        {
            if (reg.IsMatch(tbUsr.Text))
            {
                MessageBox.Show("用户名输入格式有错！");
            }
        }

        //密码格式合法性监测
        private void tbPwd_TextChanged(object sender, EventArgs e)
        {
            if (reg.IsMatch(tbPwd.Text))
            {
                MessageBox.Show("密码输入格式有错！");
            }
        }

        //检查用户名/密码输入合法性
        bool validateUsrPwd()
        {
            if (tbUsr.Text.Length <= 0)
            {
                MessageBox.Show("请输入用户名！");
                return false;
            }

            if (reg.IsMatch(tbUsr.Text))
            {
                MessageBox.Show("用户名输入格式有错！");
                return false;
            }

            if (tbPwd.Text.Length <= 0)
            {
                MessageBox.Show("请输入密码！");
                return false;
            }

            if (reg.IsMatch(tbPwd.Text))
            {
                MessageBox.Show("密码输入格式有错！");
                return false;
            }


            Config.User = tbUsr.Text.ToUpper();

            return true;
        }

        private void tbUsr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!busy && e.KeyChar == (char)13)
            {
                tbPwd.Focus();
            }
        }

        private void tbPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!busy && e.KeyChar == (char)13 && validateUsrPwd())
            {
                btnLogin.Focus();
                login();
            }
        }

        //登录按钮响应
        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            if (!busy && validateUsrPwd())
            {
                //删除历史系统日志
                if (Directory.Exists(Config.DirLocal +  Config.LogFile))
                {
                    string[] files = Directory.GetFiles(Config.DirLocal +  Config.LogFile,"*.txt");
                    if (files.Length > 0)
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            File.Delete(Config.DirLocal + Config.LogFile + files[i]);
                        }
                    }
                    
                }

                login();
                //获取店号保存到Config.stoNO
                //request01();
            }
        }

        private void tbUsr_GotFocus(object sender, EventArgs e)
        {
            FullscreenClass.ShowSIP(true);
        }

        private void tbUsr_LostFocus(object sender, EventArgs e)
        {
            FullscreenClass.ShowSIP(false);
        }

        //上下方向键切换焦点
        private void tbUsr_KeyDown(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                if (e.KeyValue == (char)Keys.Up)
                {
                    btnExit.Focus();
                }
                else if (e.KeyValue == (char)Keys.Down)
                {
                    tbPwd.Focus();
                }
            }
        }

        //上下方向键切换焦点
        private void tbPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                if (e.KeyValue == (char)Keys.Up)
                {
                    tbUsr.Focus();
                }
                else if (e.KeyValue == (char)Keys.Down)
                {
                    btnLogin.Focus();
                }
            }
        }

        //同一用户名在其他地方登录时，需用户确认是否重新登录
        private void loginConfirm()
        {
            if (MessageBox.Show("已登录，您确定要重新登录吗？", "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                login2();
            }
        }

        #region 实现ConnCallback接口
        //登录请求回调
        private void connCallback_login(string data, int result, string date)
        {
            if (result == ConnThread.RESULT_OK)
            {
                Config.Date = date;
                Config.save();

                //登陆成功上传日志
                //FtpThread = new Thread(new ThreadStart(FtpUpLoad));
                //FtpThread.Start();

                FormApp form = new FormApp();
                form.init(data);
                form.ShowDialog();
                Show();
            }
            else if (result == ConnThread.RESULT_DUPLOGIN)
            {
                loginConfirm();
            }
            else
            {
                MessageBox.Show(data);
            }
        }

        //请求进度回调
        public void progressCallback(int total, int progress)
        {

        }

        //网络请求回调
        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();

                switch (apiID)
                {
                    case API_ID.API_ID01:
                        if (result == ConnThread.RESULT_OK)
                        {
                             //Config.stoNO= data;
                        }
                        break;
                    case API_ID.LOGIN:
                    case API_ID.LOGIN2:
                        connCallback_login(data, result, date);
                        break;

                    default:
                        break;
                }
            }));
        }
        #endregion

        #region 接口请求
        //登录请求
        private void login()
        {
            apiID = API_ID.LOGIN;

            string msg = "request=login;usr=" + Config.User + ";pwd=" + tbPwd.Text + ";sn=" + Config.MAC;
           
            msg = OGuid + msg;
            loginThread= new ConnThread(this);
            loginThread.Send(msg);
            wait();
        }

        //重新登录请求
        private void login2()
        {
            apiID = API_ID.LOGIN2;

            string msg = "request=login2;usr=" + Config.User + ";pwd=" + tbPwd.Text + ";sn=" + Config.MAC;

            msg = OGuid + msg;
            loginThread = new ConnThread(this);
            loginThread.Send(msg);
            wait();
        }

        //接口请求获取店号
        private void request01()
        {
            apiID = API_ID.API_ID01;
            string msg = "request=A00;usr=" + Config.User + ";op=01;hhtIp=" + Config.IPLocal;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
        }
        #endregion

        #region Ftp上传日志
        //删除日志
        private void ClearLog()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Config.DirLocal);
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Name.Substring(0, 7).Equals(Config.LogFile))
                    {
                        if (!fi.Name.Equals(Config.LogFile + Config.LogFileSuffix))
                        {
                            fi.Delete();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        //文件属性
        public class FileData
        {
            public int fileAttributes = 0;
            public int creationTime_lowDateTime = 0;
            public int creationTime_highDateTime = 0;
            public int lastAccessTime_lowDateTime = 0;
            public int lastAccessTime_highDateTime = 0;
            public int lastWriteTime_lowDateTime = 0;
            public int lastWriteTime_highDateTime = 0;
            public int nFileSizeHigh = 0;
            public int nFileSizeLow = 0;
            public int dwReserved0 = 0;
            public int dwReserved1 = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String fileName = null;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public String alternateFileName = null;
        }

        public bool FtpFileOrDirectoryExist(String ftpFile)
        {
            //变量声明   
            bool result = false;
            int lnghInet = 0;
            int lnghConnect = 0;

            try
            {
                //初期化FTP的连接   
                lnghInet = APIs.InternetOpen("FtpOperator", 1, null, null, 0);

                //连接FTP   
                lnghConnect = APIs.InternetConnect(lnghInet, Config.IPServer, 21, "", "", 1, 0, 0);

                //结果的判断   
                if (lnghInet == 0 || lnghConnect == 0)
                {
                    result = false;
                    return result;
                }

                //FTP文件查询   
                FileData foundData = new FileData();
                int hFind = APIs.FtpFindFirstFile(lnghConnect, ftpFile, foundData, 0x80000000, 0);

                //查询结果判断   
                if (hFind != 0)
                {
                    result = true;
                }
                else
                {
                    bool bcreate = APIs.FtpCreateDirectory(lnghConnect, ftpFile);
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
                return result;
            }
            finally
            {
                //FTP连接关闭   
                if (lnghInet != 0)
                {
                    APIs.InternetCloseHandle(lnghInet);
                }

                //FTP连接关闭   
                if (lnghConnect != 0)
                {
                    APIs.InternetCloseHandle(lnghConnect);
                }
            }

            //返回执行结果   
            return result;
        }

        public bool FtpFileUpLoad(string localFileFullName)
        {
            try
            {

                //设定上传文件在FTP上的保存路径和名称   
                string ftpUpFullName = FtpUpLoadPath + "/" + Config.LogFile + Config.LogFileSuffix;

                //检查本地文件是否存在   
                if (File.Exists(localFileFullName) == false)
                {
                    return false;
                }

                //初始FTP连接   
                int lnghInet = APIs.InternetOpen("FtpOperator", 1, null, null, 0);

                //连接FTP   
                int lnghConnect = APIs.InternetConnect(lnghInet, Config.IPServer, 21, "", "", 1, 0, 0);

                //文件上传   

                long ret = APIs.FtpPutFile(lnghConnect, localFileFullName, ftpUpFullName, 0x80000000, 0);

                //上传结果的判断   
                if (ret == 0)
                {
                    return false;
                }

                //关闭FTP的连接   
                APIs.InternetCloseHandle(lnghInet);
                APIs.InternetCloseHandle(lnghConnect);
            }
            catch (Exception)
            {
                return false;
            }

            //返回结果   
            return true;
        }

        public static class APIs
        {

            [DllImport("wininet.dll")]
            public static extern int InternetOpen(string lpszCallerName, int dwAccessType, string lpszProxyName, string lpszProxyBypass, int dwFlags);

            [DllImport("wininet.dll")]
            public static extern int InternetConnect(int hInternetSession, string lpszServerName, int nProxyPort, string lpszUsername, string lpszPassword, int dwService, int dwFlags, int dwContext);

            [DllImport("wininet.dll", CharSet = CharSet.Auto)]
            public static extern int FtpFindFirstFile(int hConnect, string lpszSearchFile, [In, Out] FileData dirData, ulong ulFlags, ulong ulContext);

            [DllImport("wininet.dll")]
            public static extern int InternetCloseHandle(int hInet);

            [DllImport("wininet.dll")]
            public static extern int FtpPutFile(int hConnect, string lpszLocalFile, string lpszNewRemoteFile, uint dwFlags, int dwContext);

            [DllImport("wininet.dll", CharSet = CharSet.Auto)]
            public static extern bool FtpCreateDirectory(int hInternet, String lpszDirectory);

        }

        public void FtpUpLoad()
        {
            try
            {
                while (!bUpLoad)
                {
                    string path = "";
                    foreach (string dir in FtpUpLoadPath.Split('/'))
                    {
                        path += "/" + dir;

                        FtpFileOrDirectoryExist(path);
                    }
                    bUpLoad = FtpFileUpLoad(Config.DirLocal + Config.LogFile + Config.LogFileSuffix);

                    ClearLog();
                }

            }
            catch (Exception)
            {
                ClearLog();
            }
            finally
            {
                ClearLog();
            }
        }

        private void FtpAbort()
        {
            try
            {
                bUpLoad = true;
                FtpThread.Abort();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}