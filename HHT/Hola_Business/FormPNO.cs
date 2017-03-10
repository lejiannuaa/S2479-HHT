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
    public partial class FormPNO : Form, ISerializable, ConnCallback
    {
        private const string guid = "DA79F26B-CFC2-411b-996D-18DB423C2797";
        private string OGuid = "00000000000000000000000000000000";
        private SerializeClass sc = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            SKUNO,
            DataMax
        }
        #endregion
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private DataSet ds = null;
        private string xmlfile = null;
        private int TABLE_ROWMAX = 6;
        private int pageIndex = 1;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R101201,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private int TaskBarHeight = 0;
        private int colIndex = -1;
        private string ShopNo = "";
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormPNO()
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
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();
                if (dstWidth > 240)
                {
                    int muti = 2;
                    dgTable.Height = dgTable.Height + muti * (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += muti;
                }

                PrePage.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Top = dgTable.Top + dgTable.Height + 5;
                Page.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
               
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
                        UpdatedgTable(true);
                        int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();

                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                        dgTable.DataSource = null;
                        MessageBox.Show("无数据！");
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
                File.Delete(xmlfile);
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

        private void UpdatedgTable(bool Init)
        {
            try
            {
                string[] colValue = new string[] {  "SKU", "国际条码" };
                string[] colName = new string[] { "sku", "code" };
                //int[] colWidth = new int[] { 48,49};
                int[] colWidth = new int[] { 19, 78 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;
                if (!cols.Contains("sku"))
                {
                    cols.Add("sku");
                }
                if (Init)
                {
                    for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                    {

                        rows[i]["sku"] = SKUNO.Text;
                    }
                }
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
                string[] colValue = new string[] { "SKU", "国际条码" };
                string[] colName = new string[] { "sku", "code" };
                int[] colWidth = new int[] { 19, 78 };
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        #endregion

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.R101201;
            string msg = "request=1012;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text;
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
                    case API_ID.R101201:
                        {
                            string file = Config.getApiFile("1012", "01");
                            from += "/1012/" + file;
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
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                }
                else if (e.KeyValue == 13)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    btnReturn.Focus();
                    dgTable.DataSource = null;
                    request01();
                }
                dgTable.DataSource = null;
            }
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dtHeader = (DataTable)dgTable.DataSource;
                    if (dtHeader != null && dtHeader.Rows.Count >= 1)
                    {
                        DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {

                            string colName = dtHeader.Columns[hitTest.Column].ColumnName;

                            DataTable dt = ds.Tables["detail"];
                            DataColumn col = dt.Columns[colName];
                            if (dt.Rows.Count >= 1)
                            {
                                if (colIndex == hitTest.Column)
                                {
                                    if (dt.DefaultView.Sort.IndexOf(" asc") > 0)
                                    {
                                        dt.DefaultView.Sort = col.ColumnName.ToString() + " desc";
                                    }
                                    else
                                    {
                                        dt.DefaultView.Sort = col.ColumnName.ToString() + " asc";
                                    }
                                }
                                else
                                {
                                    colIndex = hitTest.Column;
                                    dt.DefaultView.Sort = col.ColumnName.ToString() + " asc";
                                }

                                UpdatedgTable(false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (dt != null && dt.Rows.Count >= 1)
                    {
                        int rowOld = dgTable.CurrentCell.RowNumber;
                        int columnOld = dgTable.CurrentCell.ColumnNumber;

                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            dgTable.Select(hitTest.Row);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtt = (DataTable)dgTable.DataSource;
                if (dtt != null)
                {
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    this.dgTable.DataSource = null;
                    Close();
                    File.Delete(Config.DirLocal + guid);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region 翻页

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable.DataSource != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdatedgTable(false);
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
            if (!busy && dgTable.DataSource != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {

                        UpdatedgTable(false);
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
                Serialize(guid);
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
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNO];
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];

                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        if (ds.Tables["detail"].Rows.Count > 0)
                        {
                            UpdatedgTable(false);
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
            this.Invoke(new InvokeDelegate(() =>
            {
                
            }));
        }

        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.R101201:
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

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                }
                else
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    btnReturn.Focus();
                    dgTable.DataSource = null;
                    request01();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SKUNO.Text = "";
            dgTable.DataSource = null;
            SKUNO.Focus();
        }
    }
}