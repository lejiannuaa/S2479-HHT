using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using HolaCore;
namespace Hola_Inventory
{
    public partial class FormHHTScan : Form,ISerializable,ConnCallback
    {
        private const string guid = "A52FBF81-E71E-448e-A0F1-6A2BE641AF45";
        private DataSet ds = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private string xmlfile = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            CBNOScan,
            SKUNOScan,
            SKUName,
            PageIndex,
            DataMax,
        }

        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            S0001,
            R80001,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private SerializeClass sc = null;

        private DataTable dtResult = null;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 5;
        private int colIndex = -1;
        Regex reg = new Regex(@"^\d+$");
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormHHTScan()
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
                if (dstWidth > 240)
                {
                    dgTable.Height = dgTable.Height + (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += 1;
                }

                PrePage.Top = dgTable.Top + dgTable.Height +5;
                NexPage.Top = dgTable.Top + dgTable.Height + 5;
                Page.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
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

        private void CreateDS()
        {
            string[] colName = { "loc_no", "sku", "sku_dsc", "sku_test_qty" };
            DataTable dt = new DataTable();
            ds = new DataSet();
            dt.TableName = "detail";
            for (int i = 0; i < colName.Length; i++)
            {
                dt.Columns.Add(colName[i]);
            }
            ds.Tables.Add(dt);
            dt.Dispose();
        }

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
                    DataSet DS = new DataSet();
                    DS.ReadXml(reader);

                    for (int i = 0; i < DS.Tables.Count; i++)
                    {
                        if (DS.Tables[i].TableName.Equals("detail"))
                        {
                            bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {
                        SKUName.Text = DS.Tables["detail"].Rows[0][1].ToString();
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
            DataGridCustomColumnBase style = null;
            DataGridColumnStyle style1 = null;
            for (int i = 0; i < name.Length; i++)
            {
                
                if (name[i].Equals("sku_test_qty"))
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }
                else
                {
                    style1 = new DataGridTextBoxColumn();
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
                string[] colName = new string[] { "loc_no", "sku", "sku_dsc", "sku_test_qty" };
                string[] colValue = new string[] { "柜号", "SKU", "品名", "数量" };
                int[] colWidth = new int[] { 17, 25, 35, 20 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;


                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();
                UpdatedgHeader();
            }
            catch (Exception)
            {
            }
        }

        private void UpdatedgHeader()
        {
            try
            {
                string[] colName = new string[] { "loc_no", "sku", "sku_dsc", "sku_test_qty" };
                string[] colValue = new string[] { "柜号", "SKU", "品名", "数量" };
                int[] colWidth = new int[] { 17, 25, 35, 20 };
                DataTable dtHeader = ds.Tables["detail"].Clone();
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
            DataGridCustomTextBoxColumn TextBoxCol = null;
            dtResult = new DataTable();
            dtResult.TableName = dt.TableName;
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("sku_test_qty"))
                {
                    TextBoxCol = (DataGridCustomTextBoxColumn)styles[i];
                }
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
            if (TextBoxCol != null && to > 0)
            {
                TextBox Tbox = (TextBox)TextBoxCol.HostedControl;
                Tbox.LostFocus += new EventHandler(Tbox_LostFocus);
            }
            int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
        }


        #endregion


        #region 接口请求
        private void requesS01()
        {
            apiID = API_ID.S0001;
            string msg = "request=S00;usr=" + Config.User + ";op=01;sku=" +SKUNOScan.Text + ";sto=" + ShopNO.Text;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR01()
        {
            apiID = API_ID.R80001;
            string msg = "request=800;usr=" + Config.User + ";op=01;sto=" + ShopNO.Text + ";sn=" + Config.MAC + ";hhtip=" + Config.IPLocal + ";json=";
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestXML()
        {
            try
            {
                string from = Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.S0001:
                        {
                            string file = Config.getApiFile("S00", "01");
                            from += "/S00/" + file;
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

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
            checkData();
        }

        private bool CheckSubmit()
        {
            try
            {
                UpdateDsData();
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ;
                }
                else
                {
                    MessageBox.Show("表格内无数据!");
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (CheckSubmit())
                {
                    requestR01();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CBNOScan.Text == "")
            {
                MessageBox.Show("请输入柜号！");
                return;
            }

            if (!regText.IsMatch(CBNOScan.Text))
            {
                MessageBox.Show("输入柜号非法！");
                return;
            }

            if (SKUNOScan.Text == "")
            {
                MessageBox.Show("请输入SKU！");
                return;
            }

            if (!regText.IsMatch(SKUNOScan.Text))
            {
                MessageBox.Show("输入SKU非法！");
                return;
            }

            if (SKUName.Text == "")
            {
                MessageBox.Show("不存在此SKU！");
                return;
            }

            DataRow row = ds.Tables["detail"].NewRow();
            row["loc_no"] = CBNOScan.Text;
            row["sku"] = SKUNOScan.Text;
            row["sku_dsc"] = SKUName.Text;
            row["sku_test_qty"] = "0";
            ds.Tables["detail"].Rows.Add(row);
            UpdatedgTable();
          
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (!busy)
            {
                DataTable dtHeader = (DataTable)dgTable.DataSource;
                if (dtHeader != null && dtHeader.Rows.Count > 0)
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

                            UpdatedgTable();
                        }
                    }
                }
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            DataTable dtt = (DataTable)dgTable.DataSource;
            if (dtt != null)
            {
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                string ColName = dtt.Columns[colIndex].ColumnName;
                if (!ColName.Equals("sku_test_qty"))
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
                }
            }
        }

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (!busy)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null && dt.Rows.Count >= 1)
                {
                    int rowOld = dgTable.CurrentCell.RowNumber;
                    int columnOld = dgTable.CurrentCell.ColumnNumber;

                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);

                    if (columnOld == 3)
                    {
                        //if (hitTest.Row == rowOld && hitTest.Column == columnOld)
                        //{
                        //}
                        //else
                        //{
                        //    checkData();
                        //}

                    }
                }

            }
        }

        private void checkData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = dt.Rows[dgTable.CurrentRowIndex]["sku_test_qty"].ToString();

                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        UpdatedgTable();
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 0)
                        {
                            string subtext = ok.Substring(0, 1);
                            if (UInt64.Parse(subtext) == 0)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdatedgTable();
                            }
                        }
                        else if (UInt64.Parse(ok) == 0)
                        {
                            if (ok.Length >= 2)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdatedgTable();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
                }

            }
            catch (Exception)
            {
            }
        }

        private void UpdateDsData()
        {
            try
            {

                DataTable dts = ds.Tables["detail"].DefaultView.ToTable();
                DataTable dtt = (DataTable)dgTable.DataSource;
                if (dts != null && dtt != null)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                            dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][3] = dtt.Rows[i][3];
                    }
                    ds = new DataSet();
                    ds.Tables.Add(dts);
                }
            }
            catch (Exception)
            {
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

        #region 翻页

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable.DataSource != null)
            {
                int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                UpdateDsData();
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

        private void NexPage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable.DataSource != null)
            {
                int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                UpdateDsData();
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

        #endregion

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            ShopNO.Text = (string)param[0];
            CreateDS();
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
                data[(int)DATA_INDEX.CBNOScan] = CBNOScan.Text;
                data[(int)DATA_INDEX.SKUNOScan] = SKUNOScan.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;

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
                    CBNOScan.Text = sc.Data[(int)DATA_INDEX.CBNOScan];
                    SKUNOScan.Text = sc.Data[(int)DATA_INDEX.SKUNOScan];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        DataTable dt = ds.Tables["detail"];
                        if (dt.Rows.Count > 0)
                        {
                            UpdatedgTable();
                        }
                    }
                    else
                    {
                        CreateDS();
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

                    case API_ID.R80001:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("上传成功!");
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

        private void SKUNOScan_TextChanged(object sender, EventArgs e)
        {
            SKUName.Text = "";
        }

        private void SKUNOScan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SKUNOScan.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                    return;
                }

                if (!regText.IsMatch(SKUNOScan.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                    return;
                }
                SKUNOScan.Text = AddZero(SKUNOScan.Text);
                btnAdd.Focus();
                requesS01();
            }
        }

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

    }
}