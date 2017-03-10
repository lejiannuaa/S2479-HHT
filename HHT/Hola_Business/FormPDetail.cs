using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using HolaCore;
using System.Text.RegularExpressions;
namespace Hola_Business
{
    public partial class FormPDetail : Form,ISerializable,ConnCallback
    {
        private const string guid = "F2F2962A-1FF6-4ef9-9A0A-9B574D2E473C";
        private const string OGuid = "00000000000000000000000000000000";
        
        private SerializeClass sc = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            SKUNO,
            ActDateFrom,
            ActDateTo,
            Event,
            ActType,
            DataMax
        }
        #endregion
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private DataSet ds = null;
        private string xmlfile = null;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R101301,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private int TaskBarHeight = 0;
        private string ShopNo = "";
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormPDetail()
        {
            InitializeComponent();
            doLayout();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / (float)srcWidth, yTime = dstHeight / (float)srcHeight;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBarI.Top = btnReturn.Top - pbBar.Height * 3;
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

        //SKU补零
        private string AddZero(string OLstring)
        {
            string SKU = OLstring;
            if (SKU.Length < 8)
            {
                StringBuilder addString = new StringBuilder();
                int addStringNo = 9 - SKU.Length;
                addString.Append('0', addStringNo);
                SKU = addString.ToString() + SKU;
            }
            return SKU;
        }

        #region XML加载与显示

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            try
            {
                if (File.Exists(xmlfile))
                {
                    doc = new XmlDocument();
                    doc.Load(xmlfile);
                    reader = new XmlNodeReader(doc);
                    ds = new DataSet();
                    ds.ReadXml(reader);

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("detail"))
                        {
                            bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {
                        DataTable dt = ds.Tables["detail"];
                        if (dt.Rows.Count > 0)
                        {
                            ActDateFrom.Text = dt.Rows[0]["act_start_time"].ToString();
                            ActDateTo.Text = dt.Rows[0]["act_end_time"].ToString();
                            Event.Text = dt.Rows[0]["event"].ToString();
                            ActType.Text = dt.Rows[0]["event_desc"].ToString();

                        }
                        else
                        {
                            MessageBox.Show("无数据！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("请求文件不存在,请重新请求!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                if (reader != null)
                {
                    reader.Close();
                }
                File.Delete(xmlfile);
                GC.Collect();
                Serialize(guid);
            }
        }

        #endregion

        #region 接口请求

        private void request01()
        {
            apiID = API_ID.R101301;
            string msg = "request=1013;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R101301:
                        {
                            string file = Config.getApiFile("1013", "01");
                            from += "/1013/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOAD_XML;
                        }
                        break;
                    default:
                        return;
                }

                new ConnThread(this).Download(from, to, false);

                wait();

            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region UI响应

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                if (e.KeyValue == 13)
                {
                    if (SKUNO.Text == "")
                    {
                        MessageBox.Show("请输入SKU！");
                        return;
                    }

                    if (!regText.IsMatch(SKUNO.Text))
                    {
                        MessageBox.Show("输入SKU非法！");
                        return;
                    }

                    SKUNO.Text = AddZero(SKUNO.Text);
                    ActDateFrom.Text = "";
                    ActDateTo.Text = "";
                    Event.Text = "";
                    ActType.Text = "";
                    btnReturn.Focus();
                    request01();
                }
                QC();
            }
        }
        private void QC()
        {
            ActDateFrom.Text = "";
            ActDateTo.Text = "";
            Event.Text = "";
            ActType.Text = "";
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                    return;
                }

                if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                    return;
                }

                SKUNO.Text = AddZero(SKUNO.Text);
                ActDateFrom.Text = "";
                ActDateTo.Text = "";
                Event.Text = "";
                ActType.Text = "";
                request01();
            }
        }
 

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                File.Delete(Config.DirLocal + guid);
                Close();
            }
        }

        #endregion

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                ShopNo = (string)param[0];
                SKUNO.Text = (string)param[1];
                if (!string.IsNullOrEmpty(SKUNO.Text))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                    {
                        this.Invoke(new InvokeDelegate(() =>
                        {
                            request01();
                        }));
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Serialize(string file)
        {
            try
            {
                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DataMax];
                data[(int)DATA_INDEX.ShopNO] = ShopNo;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.ActDateFrom] = ActDateFrom.Text;
                data[(int)DATA_INDEX.ActDateTo] = ActDateTo.Text;
                data[(int)DATA_INDEX.ActType] = ActType.Text;
                data[(int)DATA_INDEX.Event] = Event.Text;

                sc.Data = data;

                sc.Serialize(Config.DirLocal + file);
            }
            catch (Exception)
            {
            }
        }

        public void Deserialize(string file)
        {
            try
            {
                if (file == null)
                    file = guid;

                sc = SerializeClass.Deserialize(Config.DirLocal + file);

                if (sc != null)
                {
                    ShopNo=sc.Data[(int)DATA_INDEX.ShopNO];
                    SKUNO.Text=sc.Data[(int)DATA_INDEX.SKUNO];
                    ActDateFrom.Text=sc.Data[(int)DATA_INDEX.ActDateFrom];
                    ActDateTo.Text=sc.Data[(int)DATA_INDEX.ActDateTo];
                    ActType.Text= sc.Data[(int)DATA_INDEX.ActType];
                    Event.Text= sc.Data[(int)DATA_INDEX.Event];
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 实现ConnCallback接口

        public void progressCallback(int total, int progress)
        {
        }

        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.R101301:
                        if (result == ConnThread.RESULT_OK)
                        {
                            requestXML();
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

                    case API_ID.DOWNLOAD_XML:
                        if (result == ConnThread.RESULT_FILE)
                        {
                            LoadXML();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SKUNO.Text = "";
            ActDateFrom.Text = "";
            ActDateTo.Text = "";
            Event.Text = "";
            ActType.Text = "";
            SKUNO.Focus();
        }  
    }
}