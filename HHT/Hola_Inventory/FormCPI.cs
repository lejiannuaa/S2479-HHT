using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using HolaCore;
using System.IO;
namespace Hola_Inventory
{
    public partial class FormCPI : Form,ISerializable,ConnCallback
    {
        private const string guid = "870F05A6-DBC1-4fa9-B44A-A87BEC4F3ABF";
        private const string OGuid = "00000000000000000000000000000000";
        private Form Child = null;
        private DataSet ds = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private string xmlfile = null;
        #region 子窗口ID

        private enum Form_ID
        {
            CPISKU,
            Null,
        }

        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R70001,
            R70002,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;

        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            PCode,
            CBNO,
            Child,
            DataMax,
        }
        #endregion
        private SerializeClass sc = null;
        private Form_ID ChildID = Form_ID.Null;
        private int TaskBarHeight = 0;
        public FormCPI()
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


                btnReturn.Top = dstHeight - btnReturn.Height;
                btnConfirm.Top = dstHeight - btnConfirm.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnConfirm.Top - pbBar.Height * 3;
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
                        }
                    }
                    if (!bNoData)
                    {
                        for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                        {
                            string rowValue = ds.Tables["detail"].Rows[i][0].ToString();
                            pCode.Items.Add(rowValue);
                        }
                        btnCpiCode.Visible = false;
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
            apiID = API_ID.R70001;
            string msg = "request=700;usr=" + Config.User + ";op=01;sto=" + ShopNO.Text; //+ ";stk_no=" +pCode.Text;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request02()
        {
            apiID = API_ID.R70002;
            string msg = "request=700;usr=" + Config.User + ";op=02;sto=" + ShopNO.Text + ";stk_no=" + pCode.Text +";loc_no="+CBNO.Text;
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
                    case API_ID.R70001:
                        {
                            string file = Config.getApiFile("700", "01");
                            from += "/700/" + file;
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

        private void CBNO_GotFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(true);
            }

        }

        private void CBNO_LostFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(false);
            }
        }

        private void btnCpiCode_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request01();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (pCode.SelectedIndex >= 0 && CBNO.Text != "")
                {
                    request02();
                }
                else
                {
                    MessageBox.Show("请检查柜号或单号!");
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (ChildID == Form_ID.CPISKU)
                {

                }
                else
                {
                    File.Delete(Config.DirLocal + guid);
                }
                Close();
            }
        }

        private void ShowChild(bool bDeserialize)
        {
            try
            {
                if (Child != null)
                {
                    Child.Dispose();
                    Child = null;
                }
                switch (ChildID)
                {
                    case Form_ID.CPISKU:
                        Child = new FormCPISKU();
                        break;
                    default:
                        return;
                }

                Serialize(guid);

                ((ISerializable)Child).init(new object[] { ShopNO.Text, pCode.Text, CBNO.Text });
                if (bDeserialize)
                {
                    ((ISerializable)Child).Deserialize(null);
                }

                pCode.Enabled = false;
                CBNO.Enabled = false;
                if (DialogResult.OK == Child.ShowDialog())
                {
                    pCode.Enabled = true;
                    CBNO.Enabled = true;
                    ChildID = Form_ID.Null;
                    File.Delete(Config.DirLocal + guid);
                }

                Show();
                Child.Dispose();
                Child = null;
            }
            catch (Exception)
            {
            }
        }

        private void CBNO_TextChanged(object sender, EventArgs e)
        {
            int start = CBNO.SelectionStart;
            CBNO.Text = CBNO.Text.Trim();
            CBNO.SelectionStart = start;
        }

        #endregion

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            ShopNO.Text = (string)param[0];
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
                data[(int)DATA_INDEX.PCode] = pCode.SelectedIndex.ToString();
                data[(int)DATA_INDEX.CBNO] = CBNO.Text;
                data[(int)DATA_INDEX.Child] = ChildID.ToString();

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
                    pCode.Text = sc.Data[(int)DATA_INDEX.PCode];
                    CBNO.Text = sc.Data[(int)DATA_INDEX.CBNO];
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);

                    ds = sc.DS;
                    for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                    {
                        string RowValue = ds.Tables["detail"].Rows[i][0].ToString();
                          pCode.Items.Add(RowValue);
                    }
                    if (int.Parse(sc.Data[(int)DATA_INDEX.PCode]) <= pCode.Items.Count)
                    {
                        pCode.SelectedIndex = int.Parse(sc.Data[(int)DATA_INDEX.PCode]);
                        btnCpiCode.Visible = false;
                    }
                   
                    if (ChildID != Form_ID.Null)
                    {
                        ShowChild(true);
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
            this.Invoke(new InvokeDelegate(() =>
            {
                //formDownload.setProgress(total, progress);
            }));
        }
        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.R70001:
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

                    case API_ID.R70002:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ChildID = Form_ID.CPISKU;
                           
                            ShowChild(true);
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