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
using System.Text.RegularExpressions;
namespace Hola_Business
{
    public partial class FormPSKU : Form, ISerializable, ConnCallback
    {

        private const string guid = "DF303FCA-5906-41cf-83A5-75B231F243D1";
        private const string OGuid = "00000000000000000000000000000000";
        private string ShopNO = "";
        private string PSKUNO = "";
        private SerializeClass sc = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            SKUNO,
            PSKUNO,
            DataMax
        }
        #endregion
        private string xmlfile = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
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

        private DataSet ds = null;
        private int TABLE_ROWMAX = 6;
        private int pageIndex = 1;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R101101,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private int TaskBarHeight = 0;
        private int colIndex = -1;
        public delegate void getRowSku(string sku);
        public getRowSku GetRowSku;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormPSKU()
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
            int xOffSet = (int)(SKUNO.Location.X * xTime);
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

        #region XML加载与显示

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

                if (name[i].Equals("subsku"))
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
                string[] colValue = new string[] {  "SKU", "类型" };
                string[] colName = new string[] { "subsku", "sku_type" };
                //int[] colWidth = new int[] { 48,49};
                int[] colWidth = new int[] { 19, 78 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;
                if (!cols.Contains("sku_type"))
                {
                    cols.Add("sku_type");
                }
                if (Init)
                {
                    for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                    {
                            rows[i]["sku_type"] = "子商品";
                    }
                    DataRow Prow = ds.Tables["detail"].NewRow();
                    Prow["subsku"] = PSKUNO;
                    Prow["sku_type"] = "母商品";
                    ds.Tables["detail"].Rows.Add(Prow);
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

        private void UpdatedgHeader()
        {
            try
            {
                string[] colValue = new string[] { "SKU", "类型" };
                string[] colName = new string[] { "subsku", "sku_type" };
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
            catch (Exception)
            {
            }
        }

        private void UpdateRow()
        {
            DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
            DataTable  dtResult = new DataTable();

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

        #endregion

        #region UI响应

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SKUNO.Text = "";
            dgTable.DataSource = null;
            SKUNO.Focus();
        }

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
                    btn01.Focus();
                    dgTable.DataSource = null;
                    request01();
                }
            }
        }

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
                    btn01.Focus();
                    dgTable.DataSource = null;
                    request01();
                }
                dgTable.DataSource = null;
            }
        }

        private void QC()
        {
            throw new NotImplementedException();
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
                            GetRowSku(dt.Rows[hitTest.Row]["subsku"].ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                Close();
                File.Delete(Config.DirLocal + guid);
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
                        if (ds.Tables[i].TableName.Equals("info"))
                        {
                            bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {
                        if (ds.Tables["detail"] != null)
                        {
                            DataTable dtSubSku = ds.Tables["detail"].DefaultView.ToTable();
                            init(new object[] { ShopNO, SKUNO.Text, ds.Tables["info"].Rows[0]["parent_sku"].ToString(), dtSubSku });
                        }
                        else
                        {
                            MessageBox.Show("没有子商品!");
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
            apiID = API_ID.R101101;
            string msg = "request=1011;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNO;
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
                    case API_ID.R101101:
                        {
                            string file = Config.getApiFile("1011", "01");
                            from += "/1011/" + file;
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
                    case API_ID.R101101:
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

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                ShopNO = (string)param[0];
                SKUNO.Text = (string)param[1];
                PSKUNO = (string)param[2];
                DataTable dt = (DataTable)param[3];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                    UpdatedgTable(true);
                }
                Serialize(guid);
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
                data[(int)DATA_INDEX.ShopNO] = ShopNO;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.PSKUNO] = PSKUNO;

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
                    ShopNO = sc.Data[(int)DATA_INDEX.ShopNO];
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    PSKUNO = sc.Data[(int)DATA_INDEX.PSKUNO];
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
    }
}