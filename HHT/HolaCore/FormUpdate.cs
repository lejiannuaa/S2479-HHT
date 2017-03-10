using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace HolaCore
{
    public partial class FormUpdate : NonFullscreenForm, ConnCallback
    {
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        //config文件更新线程
        private ConnThread XMLThread = null;
        //App更新线程
        private ConnThread APPThread = null;
        #region 应用编号
        public const int APP_SHELL = 0;
        #endregion

        #region 接口请求编号
        private enum API_ID
        {
            NONE = 0,
            UPDATE_XML,
            UPDATE_APP
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormUpdate()
        {
            InitializeComponent();
            updateXML();
            ClearLog();
        }
		
        //清除非当天日志
        private void ClearLog()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Config.DirLocal);
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Name.Substring(0,7).Equals(Config.LogFile))
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

        private void exit()
        {
            try
            {
                if (XMLThread != null)
                {
                    XMLThread.Abort();
                    XMLThread.Stop();
                    XMLThread.Dispose();
                    XMLThread = null;
                }
                if (APPThread != null)
                {
                    APPThread.Abort();
                    APPThread.Stop();
                    APPThread.Dispose();
                    APPThread = null;
                }
				
                Application.Exit();
            }
            catch (Exception)
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            exit();
        }

        #region 实现ConnCallback接口

        //下载最新Shell回调
        private void connCallbackUpdateAPP(string data, int result, string date)
        {
            try
            {
                if (result == ConnThread.RESULT_FILE)
                {
                    pbDownload.Value = 100;
                    exit();
                }
                else if (result == ConnThread.RESULT_EXCEPTION)
                {
                    MessageBox.Show(data);
                    exit();
                }
                else if (result == ConnThread.RESULT_UPDATE)
                {
                    pbDownload.Value = 100;
                    exit();
                }
                else
                {
                    MessageBox.Show("下载最新应用程序失败！");
                    exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                exit();
            }
        }

        //下载最新Config回调
        private void connCallbackUpdateXML(string data, int result, string date)
        {
            try
            {
                if (result == ConnThread.RESULT_FILE)
                {
                    pbDownload.Value = 50;

                    string ver = Config.getVer(data);
                    if (int.Parse(ver) > int.Parse(Config.Server.ver))
                    {
                        File.Copy(data, Config.DirLocal + Config.XML, true);
                        File.Delete(data);
                        Config.reset(Config.DirLocal + Config.XML);
                        File.Delete(Config.DirLocal + (string)(Config.Server.app[Config.APPIn]));
                        File.Delete(Config.DirLocal + (string)(Config.Server.app[Config.APPOut]));
                        File.Delete(Config.DirLocal + (string)(Config.Server.app[Config.APPExcel]));

                        File.Delete(Config.DirLocal + (string)(Config.Server.app[Config.APPInventory]));
                        File.Delete(Config.DirLocal + (string)(Config.Server.app[Config.APPBusiness]));

                        updateAPP();
                    }
                    else
                    {
                        File.Delete(data);
                        pbDownload.Value = 100;
                        exit();
                    }
                }
                else if (result == ConnThread.RESULT_EXCEPTION)
                {
                    MessageBox.Show(data);
                    exit();
                }
                else
                {
                    MessageBox.Show("下载最新配置文件失败！");
                    exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                exit();
            }
        }

        //请求进度回调
        public void progressCallback(int total, int progress)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                try
                {
                    switch (apiID)
                    {
                        case API_ID.UPDATE_XML:
                            if (total > 0)
                            {
                                pbDownload.Value = progress * 50 / total;
                            }
                            break;

                        case API_ID.UPDATE_APP:
                            if (total > 0)
                            {
                                pbDownload.Value = 50 + progress * 50 / total;
                            }
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }));
        }

        //网络请求回调
        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                switch (apiID)
                {
                    case API_ID.UPDATE_XML:
                        connCallbackUpdateXML(data, result, date);
                        break;

                    case API_ID.UPDATE_APP:
                        connCallbackUpdateAPP(data, result, date);
                        break;

                    default:
                        break;
                }
            }));
        }

        #endregion

        #region  接口请求
        //下载Config请求
        public void updateXML()
        {
            try
            {
                apiID = API_ID.UPDATE_XML;

                string from = "Http://" + Config.IPHttp + "/" + Config.Server.dir + "/" + Config.XML;

                string to = Config.DirLocal + Config.XMLUpdate;
                XMLThread = new ConnThread(this);
                XMLThread.Download(from, to, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                exit();
            }
        }

        //下载Shell请求
        private void updateAPP()
        {
            try
            {
                if (!string.IsNullOrEmpty(Config.IPServer))
                {
                    apiID = API_ID.UPDATE_APP;

                    string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + (string)Config.Server.app[APP_SHELL];
                   
                    string to = Config.DirLocal + Config.APPUpdate;
                    APPThread = new ConnThread(this);
                    APPThread.Download(from, to, false);                   
                }
                else
                {
                    MessageBox.Show("无法匹配服务器!");
                    exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                exit();
            }
        }

        #endregion
    }
}