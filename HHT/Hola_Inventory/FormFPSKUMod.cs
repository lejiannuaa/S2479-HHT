using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using HolaCore;
namespace Hola_Inventory
{
    public partial class FormFPSKUMod : Form,ISerializable,ConnCallback
    {

        private const string guid = "25D42DCF-5E96-4a43-89B2-D62AF37CB329";
        private const string OGuid = "00000000000000000000000000000000";
        private DataSet ds = null;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        //指示当前正在发送网络请求
        private bool busy = false;
        private string xmlfile = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            CBNO,
            FPCode,
            SKUNO,
            SKUName,
            SKUNum,
            CPNum,
            ReasonDes,
            DataMax,
        }
        #endregion
        private SerializeClass sc = null;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R60201,
            S0001,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        public delegate void GetModDele(int num,string ss);
        public delegate void GetReasonTb(DataTable dt);
        public GetModDele getMod = null;
        public GetReasonTb getReason = null;
        private int TaskBarHeight = 0;
        Regex reg = new Regex(@"^\d+$");
        public FormFPSKUMod()
        {
            InitializeComponent();
            doLayout();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / (float)srcWidth, yTime = dstHeight / (float)srcHeight;
            xTime -= 1.0f;
            yTime -= 1.0f;
            int xOffSet = (int)(label1.Location.X * xTime);
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                btnConfirm.Top = dstHeight - btnConfirm.Height;
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);


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
                            ds.Tables[i].TableName = "Reason";
                        }
                    }
             
                    if (!bNoData)
                    {

                        btnReason.Visible = false;
                        ReasonDes.Items.Clear();
                        for (int i = 0; i < ds.Tables["Reason"].Rows.Count; i++)
                        {
                            string rowValue = ds.Tables["Reason"].Rows[i][1].ToString();
                            ReasonDes.Items.Add(rowValue);
                        }
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
            apiID = API_ID.R60201;
            string msg = "request=602;usr=" + Config.User + ";op=01;sto="+ShopNO.Text;
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
                    case API_ID.R60201:
                        {
                            string file = Config.getApiFile("602", "01");
                            from += "/602/" + file;
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
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (ReasonDes.SelectedIndex >= 0)
                {
                    if (reg.IsMatch(SKUNum.Text))
                    {
                        if (UInt64.Parse(SKUNum.Text) > 999)
                        {
                            MessageBox.Show("不可大于999!");
                        }
                        else
                        {
                            getReason(ds.Tables["Reason"].DefaultView.ToTable());
                            getMod(int.Parse(SKUNum.Text), ReasonDes.SelectedIndex.ToString());
                            Close();
                            File.Delete(Config.DirLocal + guid);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入非负整数!");
                    }
                    
                }
                else
                {
                    MessageBox.Show("请选择原因!");
                }
            }
        }

        private void btnReason_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
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
             ShopNO.Text=(string)param[0];
             CBNO.Text=(string)param[1];
             FPCode.Text=(string)param[2];
             SKUNO.Text=(string)param[3];
             CPNum.Text = (string)param[4];
             SKUName.Text = (string)param[5];
             if ((DataTable)param[6] != null)
             {
                  DataTable dt=(DataTable)param[6];
                  if (dt.Rows.Count > 0)
                  {
                      ds = new DataSet();
                      ds.Tables.Add(dt.DefaultView.ToTable());
                      btnReason.Visible = false;
                      ReasonDes.Items.Clear();
                      for (int i = 0; i < ds.Tables["Reason"].Rows.Count; i++)
                      {
                          string rowValue = ds.Tables["Reason"].Rows[i][1].ToString();
                          ReasonDes.Items.Add(rowValue);
                      }
                  }
             }
            
             Serialize(guid);
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
                data[(int)DATA_INDEX.ShopNO] = ShopNO.Text;
                data[(int)DATA_INDEX.CBNO] = CBNO.Text;
                data[(int)DATA_INDEX.FPCode] = FPCode.Text;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.SKUNum] = SKUNum.Text;
                data[(int)DATA_INDEX.CPNum] = CPNum.Text;
                data[(int)DATA_INDEX.ReasonDes] = ReasonDes.SelectedIndex.ToString();

                sc.Data = data;
                sc.DS = ds;
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


                    ShopNO.Text = sc.Data[(int)DATA_INDEX.ShopNO];
                    CBNO.Text = sc.Data[(int)DATA_INDEX.CBNO];
                    FPCode.Text = sc.Data[(int)DATA_INDEX.FPCode];
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    SKUNum.Text = sc.Data[(int)DATA_INDEX.SKUNum];
                    CPNum.Text = sc.Data[(int)DATA_INDEX.CPNum];
                    ReasonDes.SelectedIndex = int.Parse(sc.Data[(int)DATA_INDEX.ReasonDes]);
                    ds = sc.DS;
                    if (ds != null)
                    {
                        DataTable dt = ds.Tables["Reason"];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            ReasonDes.Items.Clear();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string RowValue = dt.Rows[i][1].ToString();

                                ReasonDes.Items.Add(RowValue);
                            }
                        }
                        btnReason.Visible = false;
                    }

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
                    case API_ID.R60201:
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
                    case API_ID.S0001:
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

    }
}