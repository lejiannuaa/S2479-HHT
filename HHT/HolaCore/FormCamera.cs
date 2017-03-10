using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsMobile.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using HolaCore;


namespace HolaCore
{
    public partial class FormCamera : Form, ConnCallback
    {
        private string ShopNO = "";

        private enum API_ID
        {
            NONE = 0,
            API_ID01,
            API_ID02,
            API_ID03
        }
        private API_ID api_Id = API_ID.NONE;

        private const string guid = "07AE8A78-0E59-4f1a-B6F8-88A2AF1524D1";

        private const string OGuid = "00000000000000000000000000000000";

        string abc = "";

        private int quantity;

        public delegate void MyPhotoDelegate(string text1, string text2, string text3);

        public FormCamera(string name1, string name2, string name3, bool ispo)
        {
            InitializeComponent();
            pictureName1 = name1;
            pictureName2 = name2;
            pictureName3 = name3;
            isPO = ispo;
            request01();
        }

        private string pictureName1 = "";
        private string pictureName2 = "";
        private string pictureName3 = "";
        private bool isPO = false;

        private bool bUpLoad = false;//是否上传成功
        private Thread FtpThread = null;

        public void getTitle(Form form)
        {
            Title = form.Text;
            BottonCanUse();
        }


        //上传，打印按钮显示
        public void BottonCanUse()
        {
            if (Title == "调拨收货" || Title == "收货-1.5.0.1" || Title == "收货明细")
            {
                DYbutton.Enabled = false;
                SCbutton.Enabled = true;
            }
            else
            {
                DYbutton.Enabled = true;
                SCbutton.Enabled = false;
            }
        }

        private string Title = null;
        private bool Select = false;

        private string JPGshort1;
        private string JPGshort2;
        private string JPGshort3;

        #region UI响应
        void btn_Click(object sender, System.EventArgs e)
        {

            JPG = Guid.NewGuid().ToString() + ".jpg";
            string JGPlong = BDpath + JPG;

            bool Shoot = true;

            if (pictureBox1.Image != null && pictureBox2.Image != null && pictureBox3.Image != null)
            {
                MessageBox.Show("最多只能照三张");
                Shoot = false;
            }

            if (Shoot)
            {
                if (CameraCaptureManager.Shoot())
                {
                    //照片名称
                    System.IO.File.Delete(JGPlong);
                    System.IO.File.Move(CameraCaptureManager.FileName, JGPlong);
                    if (pictureBox1.Image == null)
                    {

                        pictureBox1.Image = new Bitmap(JGPlong);
                        pictureName1 = JGPlong;
                        JPGshort1 = JPG;
                        Select = true;
                    }

                    if (pictureBox1.Image != null && pictureBox2.Image == null && !Select)
                    {
                        pictureBox2.Image = new Bitmap(JGPlong);
                        pictureName2 = JGPlong;
                        JPGshort2 = JPG;
                        Select = true;
                    }

                    if (pictureBox3.Image == null && pictureBox1.Image != null && pictureBox2.Image != null && !Select)
                    {
                        pictureBox3.Image = new Bitmap(JGPlong);
                        pictureName3 = JGPlong;
                        JPGshort3 = JPG;
                    }
                    Select = false;
                }
            }
        }

        //public event MyPhotoDelegate MyPhotoEvent;

        //返回的图片名
        public string photoName()
        {
            return abc;
        }

        void button1_Click(object sender, System.EventArgs e)
        {
            //if (Title == "调拨收货")
            //{
            //    MyPhotoEvent(pictureName1, pictureName2, pictureName3);
            //}
            //清空照片名
            //pictureName1 = "";
            //pictureName2 = "";
            //pictureName3 = "";

            this.Close();


            if (File.Exists(@"\Program Files\hhtiii\photos"))
            {
                File.Delete(@"\Program Files\hhtiii\photos");
            }

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除该张吗？", "",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                pictureBox1.Image = null;
                File.Delete(pictureName1);
                pictureName1 = "";
            }
            else
            {

            }

        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除该张吗？", "",
         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                pictureBox2.Image = null;
                File.Delete(pictureName2);
                pictureName2 = "";
            }
            else
            {

            }
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除该张吗？", "",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                pictureBox3.Image = null;
                File.Delete(pictureName3);
                pictureName3 = "";
            }
            else
            {
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认全部删除吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
                pictureName1 = "";
                pictureName2 = "";
                pictureName3 = "";
            }
            else
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

        //上传之后记录图片名，在确认时才提交数据
        private void SCbutton_Click(object sender, EventArgs e)
        {


            if (isPO)
            {
                if (pictureName1 == "" && pictureName2 == "" && pictureName3 == "")
                {
                    MessageBox.Show("没有照片可以打印");
                }
                else
                {
                    if (ShopNO == "")
                    {
                        MessageBox.Show("没有店号");
                    }
                    else
                    {
                        //int i = 0;
                        if (pictureName1 != null && pictureName1 != "")
                        {
                            FtpFileUpLoad(pictureName1);
                            if (File.Exists(pictureName1))
                            {
                                File.Delete(pictureName1);
                            }

                            //i = i + 1;
                        }
                        else
                        {
                        }

                        if (pictureName2 != null && pictureName2 != "")
                        {
                            FtpFileUpLoad(pictureName2);
                            if (File.Exists(pictureName2))
                            {
                                File.Delete(pictureName2);
                            }
                            //i = i + 1;
                        }
                        else
                        {
                        }


                        if (pictureName3 != null && pictureName3 != "")
                        {
                            FtpFileUpLoad(pictureName3);
                            if (File.Exists(pictureName3))
                            {
                                File.Delete(pictureName3);
                            }
                            //i = i + 1;
                        }
                        else
                        {

                        }

                        //quantity = i;

                        StringBuilder a = new StringBuilder();

                        if (pictureName1 != null && pictureName1 != "")
                        {
                            a.Append(pictureName1.Substring(pictureName1.Length - 40, 40));
                        }
                        if (pictureName2 != null && pictureName2 != "")
                        {
                            a.Append("," + pictureName2.Substring(pictureName2.Length - 40, 40));
                        }
                        if (pictureName3 != null && pictureName3 != "")
                        {
                            a.Append("," + pictureName3.Substring(pictureName3.Length - 40, 40));
                        }
                        abc = a.ToString();


                        //判断是否有逗号
                        if (abc.StartsWith(","))
                        {
                            abc = abc.Substring(2);
                        }
                        request03();
                    }
                }

            }
            else
            {
                if (pictureName1 == "" && pictureName2 == "" && pictureName3 == "")
                {
                    MessageBox.Show("没有照片可以上传");
                }
                else
                {
                    //int i = 0;
                    if (pictureName1 != null && pictureName1 != "")
                    {
                        FtpFileUpLoad(pictureName1);
                        if (File.Exists(pictureName1))
                        {
                            File.Delete(pictureName1);
                        }

                        //i = i + 1;
                    }
                    else
                    {
                    }

                    if (pictureName2 != null && pictureName2 != "")
                    {
                        FtpFileUpLoad(pictureName2);
                        if (File.Exists(pictureName2))
                        {
                            File.Delete(pictureName2);
                        }
                        //i = i + 1;
                    }
                    else
                    {
                    }


                    if (pictureName3 != null && pictureName3 != "")
                    {
                        FtpFileUpLoad(pictureName3);
                        if (File.Exists(pictureName3))
                        {
                            File.Delete(pictureName3);
                        }
                        //i = i + 1;
                    }
                    else
                    {

                    }

                    //quantity = i;

                    StringBuilder a = new StringBuilder();

                    if (pictureName1 != null && pictureName1 != "")
                    {
                        a.Append(pictureName1.Substring(pictureName1.Length - 40, 40));
                    }
                    if (pictureName2 != null && pictureName2 != "")
                    {
                        a.Append("," + pictureName2.Substring(pictureName2.Length - 40, 40));
                    }
                    if (pictureName3 != null && pictureName3 != "")
                    {
                        a.Append("," + pictureName3.Substring(pictureName3.Length - 40, 40));
                    }
                    abc = a.ToString();


                    //判断是否有逗号
                    if (abc.StartsWith(","))
                    {
                        abc = abc.Substring(2);
                    }
                    if (quantity == 0)
                    {
                        MessageBox.Show("上传失败");
                    }
                    else
                    {
                        MessageBox.Show("当前" + quantity + "张上传成功");
                    }
                }
            }
        }

        //指示当前正在发送网络请求
        private bool busy = false;

        //接口请求获取店号
        private void request01()
        {
            api_Id = API_ID.API_ID01;
            string msg = "request=A00;usr=" + Config.User + ";op=01;hhtIp=" + Config.IPLocal;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
        }

        //打印请求接口
        private void request02()
        {
            api_Id = API_ID.API_ID02;
            string msg = "request=207;usr=" + Config.User + ";op=01;photo=" + abc;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        //po收货请求接口
        private void request03()
        {
            api_Id = API_ID.API_ID03;
            string msg = "request=006;usr=" + Config.User + ";op=01;photo=" + abc;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        //记录打印照片
        private void DYbutton_Click(object sender, EventArgs e)
        {

            if (pictureName1 == "" && pictureName2 == "" && pictureName3 == "")
            {
                MessageBox.Show("没有照片可以打印");
            }
            else
            {
                if (ShopNO == "")
                {
                    MessageBox.Show("没有店号");
                }
                else
                {
                    //int i = 0;
                    if (pictureName1 != null && pictureName1 != "")
                    {
                        FtpFileUpLoad(pictureName1);
                        if (File.Exists(pictureName1))
                        {
                            File.Delete(pictureName1);
                        }

                        //i = i + 1;
                    }
                    else
                    {
                    }

                    if (pictureName2 != null && pictureName2 != "")
                    {
                        FtpFileUpLoad(pictureName2);
                        if (File.Exists(pictureName2))
                        {
                            File.Delete(pictureName2);
                        }
                        //i = i + 1;
                    }
                    else
                    {
                    }


                    if (pictureName3 != null && pictureName3 != "")
                    {
                        FtpFileUpLoad(pictureName3);
                        if (File.Exists(pictureName3))
                        {
                            File.Delete(pictureName3);
                        }
                        //i = i + 1;
                    }
                    else
                    {

                    }

                    //quantity = i;

                    StringBuilder a = new StringBuilder();

                    if (pictureName1 != null && pictureName1 != "")
                    {
                        a.Append(pictureName1.Substring(pictureName1.Length - 40, 40));
                    }
                    if (pictureName2 != null && pictureName2 != "")
                    {
                        a.Append("," + pictureName2.Substring(pictureName2.Length - 40, 40));
                    }
                    if (pictureName3 != null && pictureName3 != "")
                    {
                        a.Append("," + pictureName3.Substring(pictureName3.Length - 40, 40));
                    }
                    abc = a.ToString();


                    //判断是否有逗号
                    if (abc.StartsWith(","))
                    {
                        abc = abc.Substring(2);
                    }

                    request02();
                }
            }
        }
        #endregion

        #region Ftp上传日志
        //private const string userName = "crmftp";
        //private const string passWord = "asdf1234";
        //private string IPServer = "172.16.251.121";

        private string userName = Config.FtpUser;
        private string passWord = Config.FtpPassword;
        private string IPServer = Config.FtpHost;

        //private const string userName = "hhttest";
        //private const string passWord = "test654321";
        //private string IPServer = "172.16.251.144";

        private string JPG = null;
        private const string BDpath = "\\Program Files\\hhtiii\\photos\\";


        //上传日志文件路径
        private string ftpUpLoadPath;
        public string FtpUpLoadPath
        {
            get
            {
                //ftpUpLoadPath = @"/home/crmftp/" +Config.User+"//";
                if (Title == "调拨收货" || Title == "PO收货" || Title == "收货明细")
                {
                    ftpUpLoadPath = Config.User + "//";
                }
                else
                {
                    ftpUpLoadPath = ShopNO + "//";
                }



                return ftpUpLoadPath;
            }
        }

        //删除日志
        private void ClearLog()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(FtpUpLoadPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Name.Substring(0, 7).Equals(FtpUpLoadPath))
                    {
                        if (!fi.Name.Equals(FtpUpLoadPath))
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
                lnghConnect = APIs.InternetConnect(lnghInet, IPServer, 21, userName, passWord, 1, 0, 0);

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
            int lnghInet = 0;
            int lnghConnect = 0;
            try
            {
                //设定上传文件在FTP上的保存路径和名称  
                //localFileFullName.Substring(localFileFullName.LastIndexOf("\\"));
                string file = localFileFullName.Substring(localFileFullName.Length - 40, 40);
                string ftpUpFullName = FtpUpLoadPath + file;

                //检查本地文件是否存在   
                if (File.Exists(localFileFullName) == false)
                {
                    return false;
                }

                //初始FTP连接   
                lnghInet = APIs.InternetOpen("FtpOperator", 1, null, null, 0);

                //连接FTP   
                lnghConnect = APIs.InternetConnect(lnghInet, IPServer, 21, userName, passWord, 1, 0, 0);

                if (lnghConnect == 0)
                {
                    MessageBox.Show("网络异常，无法连接ftp服务器");
                }

                //删除以该用户名命名的文件夹
                //bool DelDir = APIs.FtpDeleteFile(lnghConnect, userName);


                if (Title == "调拨收货" || Title == "PO收货" || Title == "收货明细")
                {
                    //创建以用户名命名的文件夹
                    //bool dir = APIs.FtpCreateDirectory(lnghConnect, userName);
                    bool dir = APIs.FtpCreateDirectory(lnghConnect, Config.User);


                    //文件上传   
                    long ret = APIs.FtpPutFile(lnghConnect, localFileFullName, ftpUpFullName, 0x80000000, 0);

                    //上传结果的判断   
                    if (ret == 0)
                    {
                        return false;
                    }
                    quantity++;
                }
                else
                {
                    //创建以用户名命名的文件夹
                    //bool dir = APIs.FtpCreateDirectory(lnghConnect, userName);
                    bool dir = APIs.FtpCreateDirectory(lnghConnect, ShopNO);

                    //文件上传   
                    long ret = APIs.FtpPutFile(lnghConnect, localFileFullName, ftpUpFullName, 0x80000000, 0);

                    //上传结果的判断   
                    if (ret == 0)
                    {
                        return false;
                    }

                    quantity++;
                }

                //关闭FTP的连接   
                APIs.InternetCloseHandle(lnghInet);
                APIs.InternetCloseHandle(lnghConnect);
            }
            catch (Exception)
            {
                return false;
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

            [DllImport("wininet.dll", CharSet = CharSet.Auto)]
            public static extern bool FtpDeleteFile(int hInternet, String lpszDirectory);

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
                    bUpLoad = FtpFileUpLoad(FtpUpLoadPath);

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


        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        #region ConnCallback 成员

        void ConnCallback.progressCallback(int total, int progress)
        {
            throw new NotImplementedException();
        }

        void ConnCallback.requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (api_Id)
                {

                    case API_ID.API_ID01:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ShopNO = data;
                        }
                        else
                        {
                            //MessageBox.Show("没有取到店号");
                        }
                        break;

                    case API_ID.API_ID02:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("当前" + quantity + "张打印成功");
                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.API_ID03:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("当前" + quantity + "上传印成功");
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

    }
}