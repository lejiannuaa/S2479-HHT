using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using HolaCore;
using System.Threading;
using System.Text.RegularExpressions;
namespace Hola_Business
{
    public partial class FormInvTD : Form,ISerializable,ConnCallback
    {
        private const string guid = "11505E0D-7ACE-402c-8CAE-2B3A91BC69C7";
        private const string OGuid = "00000000000000000000000000000000";
        private DataSet ds = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            SKUNO,
            SKUName,
            ShopNo,
            PageIndex,
            DataMax
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R102401,
            DOWNLOADXML,
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private string ShopNo = null;
        private string xmlfile = null;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 7;
        private SerializeClass sc = null;
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormInvTD()
        {
            InitializeComponent();

            doLayout();
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
                if (dstWidth > 240)
                {
                    int muti = 1;
                    dgTable.Height = dgTable.Height + muti * (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += muti;
                }
                PrePage.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Top = dgTable.Top + dgTable.Height + 5;
                Page.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
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

                        if (ds.Tables[i].TableName.Equals("info"))
                        {
                            SKUName.Text = ds.Tables[i].Rows[0]["sku_dsc"].ToString();
                        }
                    }

                    if (!bNoData)
                    {
                        DataTable dt = ds.Tables["detail"];
                        if (dt.Rows.Count > 0)
                        {
                            UpdatedgTable();
                        }
                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                        dgTable.DataSource = null;
                        MessageBox.Show("无数据!");
                    }
                }
                else
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
                    dgTable.DataSource = null;
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
                //File.Delete(xmlfile);
                GC.Collect();
                Serialize(guid);
            }
        }

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool GridNo)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables["detail"];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;

            DataGridColumnStyle style1 = null;
            DataGridCustomTextBoxColumn style = null;

            for (int i = 0; i < name.Length; i++)
            {
                style1 = new DataGridTextBoxColumn();

                if (name[i].Equals("sku"))
                {
                    if (GridNo)
                    {
                        style = new DataGridCustomTextBoxColumn();

                        style.Owner = dgTable;

                        style.Alignment = HorizontalAlignment.Right;
                        style.ReadOnly = true;
                        style.MappingName = name[i];
                        style.Width = (int)(dstWidth * width[i] / 100);
                        ts.GridColumnStyles.Add(style);
                    }
                    else
                    {
                        style1.MappingName = name[i];
                        style1.Width = (int)(dstWidth * width[i] / 100);
                        ts.GridColumnStyles.Add(style1);
                    }

                }
                else
                {
                    style1.MappingName = name[i];
                    style1.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style1);
                }

            }

            return ts;
        }

        private void UpdatedgTable()
        {
            try
            {
                string[] colName = new string[] { "sku", "sto", "inv" };
                string[] colValue = new string[] { "SKU", "门店编号", "库存量" };
                int[] colWidth = new int[] { 19, 51, 27 };

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));

                UpdateRow();
                UpdatedgHeader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateRow()
        {
            try
            {
                DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;

                DataTable dtResult = new DataTable();
                dtResult.TableName = dt.TableName;
                for (int i = 0; i < styles.Count; i++)
                {
                    dtResult.Columns.Add(styles[i].MappingName);
                }
                int from = (pageIndex - 1) * TABLE_ROWMAX;
                int to = from + TABLE_ROWMAX;
                if (to > dt.Rows.Count)
                {
                    to = dt.Rows.Count;
                }

                for (int i = from; i < to; i++)
                {
                    DataRow rowNew = dtResult.NewRow();

                    for (int j = 0; j < styles.Count; j++)
                    {
                        string name = styles[j].MappingName;
                        rowNew[name] = dt.Rows[i][name];
                    }

                    dtResult.Rows.Add(rowNew);
                }
                dgTable.DataSource = dtResult;
                int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                if (pageCount == 0)
                {
                    pageCount = 1;
                }
                Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdatedgHeader()
        {
            try
            {
                string[] colName = new string[] { "sku", "sto", "inv" };
                string[] colValue = new string[] { "SKU", "门店编号", "库存量" };
                int[] colWidth = new int[] { 19, 51, 27 };
                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();//ds.Tables["detail"].Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colValue.Length; i++)
                {
                    row[i] = colValue[i];
                }
                dtHeader.Rows.Add(row);
                dgHeader.Controls.Clear();
                dgHeader.TableStyles.Clear();
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, false));
                dgHeader.DataSource = dtHeader;
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region 接口请求

        private void requestR01()
        {
            SKUName.Text = "";
            apiID = API_ID.R102401;
            string msg = "request=1024;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text+";sto="+ShopNo;
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

                    case API_ID.R102401:
                        {
                            string file = Config.getApiFile("1024", "01");
                            from += "/1024/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOADXML;
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
                    btn01.Focus();
                    requestR01();
                }
                SKUName.Text = "";
                dgTable.DataSource = null;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                Close();
                //File.Delete(Config.DirLocal + guid);
            }
        }
        #endregion

        #region 翻页

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdatedgTable();
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex++;
                        MessageBox.Show("已是首页!");
                    }
                }
            }
        }

        private void NexPage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {

                        UpdatedgTable();
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex--;
                        MessageBox.Show("已是最后一页!");
                    }
                }
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
                SKUName.Text = (string)param[2];
                Serialize(guid);
                if (!string.IsNullOrEmpty(SKUNO.Text))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                    {
                        this.Invoke(new InvokeDelegate(() =>
                        {
                            requestR01();
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
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.ShopNo] = ShopNo;
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
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
                     SKUNO.Text=sc.Data[(int)DATA_INDEX.SKUNO];
                     SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                     ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                     pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                     ds = sc.DS;
                     if (ds != null && ds.Tables["detail"] != null)
                     {
                         if (ds.Tables["detail"].Rows.Count > 0)
                         {
                             UpdatedgTable();
                         }
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
                    case API_ID.R102401:
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

                    case API_ID.DOWNLOADXML:
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
                requestR01();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SKUNO.Text = "";
            SKUName.Text = "";
            dgTable.DataSource = null;
            SKUNO.Focus();
        }   
    }
}