using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Xml;
using HolaCore;
using System.Text.RegularExpressions;

namespace Hola_Business
{
    public partial class FormDJSKU : Form,ISerializable,ConnCallback
    {
        private const string guid = "D6DE705C-9152-4879-821E-57FBC34DF7D3";
        private const string OGuid = "00000000000000000000000000000000";
        private string DataGuid = "";
        #region 序列化索引
        private enum DATA_INDEX
        {
            GroupID,
            SKUNO,
            SKUName,
            ShopNo,
            DataGuid,
            PageIndex,
            DataMax
        }
        #endregion
        private SerializeClass sc = null;
        private DataSet ds = null;
        private string ShopNo = "";
        private string xmlfile = null;
        private bool bCheckAll = false;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 5;
        private int colIndex = -1;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        Regex reg = new Regex(@"^\d+$");
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            S0001,
            R104301,
            R104302,
            DOWNLOAD_S01,
            DOWNLOAD_XML,
            DOWNLOAD104301,
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormDJSKU()
        {
            InitializeComponent();

            doLayout();
            //requestR01();
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
                    dgTable.Height = dgTable.Height + (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += 1;
                }
                PrePage.Top = dgTable.Top + dgTable.Height + 8;
                NexPage.Top = dgTable.Top + dgTable.Height + 8;
                Page.Top = dgTable.Top + dgTable.Height + 8;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                btnReturn.Top = dstHeight - btnReturn.Height;
                btn02.Top = dstHeight - btnReturn.Height;
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

        private void CreateDS()
        {
            try
            {
                string[] colName = { "sku", "sku_dsc", "sku_qty", "CheckBox" };
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool LoadS01(XmlNodeReader reader)
        {
            bool bNoData = true;
            DataSet dss = new DataSet();
            dss.ReadXml(reader);

            for (int i = 0; i < dss.Tables.Count; i++)
            {
                if (dss.Tables[i].TableName.Equals("detail"))
                {
                    bNoData = false;
                    DataTable dt = dss.Tables["detail"];
                    if (dt.Rows.Count > 0)
                    {
                        SKUNO.Text = dt.Rows[0][0].ToString();
                        SKUName.Text = dt.Rows[0][1].ToString();
                    }
                }
            }
            return bNoData;
        }
        string skunumberL;
        private bool LoadR01(XmlNodeReader reader)
        {
            bool bNoData = true;
            ds = new DataSet();
            ds.ReadXml(reader);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i].TableName.Equals("detail"))
                {
                    bNoData = false;

                    for (int j = 0; j < ds.Tables["detail"].Rows.Count; j++)
                    {
                        skunumberL = ds.Tables["detail"].Rows[j][0].ToString();
                        sku_Number.Add(skunumberL);
                    }

                }
            }
            return bNoData;
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
                }
                switch (apiID)
                {
                    case API_ID.DOWNLOAD_S01:
                        if (File.Exists(xmlfile))
                        {
                            bNoData = LoadS01(reader);
                            if (!bNoData)
                            {
                                ;
                            }
                            else
                            {
                                SKUName.Text = "";
                                MessageBox.Show("无数据!");
                            }
                        }
                        else
                        {
                            SKUName.Text = "";
                            MessageBox.Show("请求文件不存在,请重新请求!");
                        }
                        break;
                    case API_ID.DOWNLOAD_XML:
                        if (File.Exists(xmlfile))
                        {
                            bNoData = LoadR01(reader);
                            if (!bNoData)
                            {
                                DataTable dt = ds.Tables["detail"];
                                if (dt.Rows.Count > 0)
                                {
                                    UpdatedgTable(false);
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
                        break;

                    case API_ID.DOWNLOAD104301:
                        if (File.Exists(xmlfile))
                        {
                            bNoData = LoadR01(reader);
                            if (!bNoData)
                            {
                                DataTable dt = ds.Tables["detail"];
                                if (dt.Rows.Count > 0)
                                {
                                    UpdatedgTable(false);
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
                        break;
                    default:
                        break;
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

            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = null;
                DataGridColumnStyle style1 = null;
                if (name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }
                else if (name[i].Equals("sku_qty"))
                {
                    style = new DataGridCustomTextBoxColumn();

                    style.Owner = dgTable;
                    style.ReadOnly = false;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }
                else if (name[i].Equals("sku"))
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
                        style1 = new DataGridTextBoxColumn();
                        style1.MappingName = name[i];
                        style1.Width = (int)(dstWidth * width[i] / 100);
                        ts.GridColumnStyles.Add(style1);
                    }
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

        private void UpdatedgTable(bool bSerialize)
        {
            try
            {
                string[] colName = new string[] { "sku", "sku_dsc", "sku_qty", "CheckBox" };
                string[] colValue = new string[] { "SKU", "品名", "数量", "false" };
                //int[] colWidth = new int[] { 30, 60, 0, 7 };
                int[] colWidth = new int[] { 19, 71, 0, 7 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;
                if (!cols.Contains("CheckBox"))
                {
                    cols.Add("CheckBox");
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["CheckBox"] = "false";
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

        private void UpdatedgHeader()
        {
            try
            {
                string[] colName = new string[] { "sku", "sku_dsc", "sku_qty", "CheckBox" };
                string[] colValue = new string[] { "SKU", "品名", "数量", "false" };
                int[] colWidth = new int[] { 19, 71, 0, 7 };
                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colValue.Length; i++)
                {
                    if (colName[i].Equals("CheckBox"))
                    {
                        row[i] = bCheckAll;
                    }
                    else
                    {
                        row[i] = colValue[i];
                    }
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
            DataGridCustomCheckBoxColumn CheckBoxCol = null;
            DataGridCustomTextBoxColumn TextBoxCol = null;
            DataTable dtResult = new DataTable();
            dtResult.TableName = dt.TableName;
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("CheckBox"))
                {
                    dtResult.Columns[i].DataType = typeof(Boolean);
                    CheckBoxCol = (DataGridCustomCheckBoxColumn)styles[i];
                }
                //if (styles[i].MappingName.Equals("sku_qty"))
                //{
                //    TextBoxCol = (DataGridCustomTextBoxColumn)styles[i];
                //}

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
                    if (name.Equals("CheckBox"))
                    {
                        rowNew[name] = bool.Parse(dt.Rows[i][name].ToString());
                    }
                    else if (name.Equals("sku"))
                    {
                        rowNew[name] = dt.Rows[i][name];
                        //string skunew= dt.Rows[i][name].ToString();
                        //rowNew[name] = skunew.Substring(3);
                        //MessageBox.Show(rowNew[name].ToString());
                    }
                    else
                    {
                        rowNew[name] = dt.Rows[i][name];
                    }
                }

                dtResult.Rows.Add(rowNew);
            }
            dgTable.DataSource = dtResult;
            if (CheckBoxCol != null && to > 0)
            {
                CheckBox Cbox = (CheckBox)CheckBoxCol.HostedControl;
                Cbox.CheckStateChanged += new EventHandler(Cbox_CheckStateChanged);
            }
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

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                string[] colName = new string[] { "sku", "sku_dsc", "sku_qty" };
                DataTable dt = ds.Tables["detail"];

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sku"] = dt.Rows[i]["sku"];
                    rowNew["sku_dsc"] = dt.Rows[i]["sku_dsc"];
                    rowNew["sku_qty"] = dt.Rows[i]["sku_qty"];
                    dtOK.Rows.Add(rowNew);
                }
            }
            catch (Exception)
            {
            }

            return dtOK;
        }

        #endregion

        #region 接口请求

        private void requestS01()
        {
            apiID = API_ID.S0001;
            string msg = "request=S00;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR01()
        {
            apiID = API_ID.R104301;
            string msg = "request=1043;usr=" + Config.User + ";op=01;groupId=" + GroupID.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR02()
        {
            apiID = API_ID.R104302;
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "1043", "02");
            string msg = "request=1043;usr=" + Config.User + ";op=02;groupId=" + GroupID.Text + ";sto=" + ShopNo + ";json=" + json;
            msg = DataGuid + msg;
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

                    case API_ID.S0001:
                        {
                            string file = Config.getApiFile("S00", "01");
                            from += "/S00/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOAD_S01;
                        }
                        break;
                    case API_ID.R104301:
                        {
                            string file = Config.getApiFile("1043", "01");
                            from += "/1043/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOAD104301;
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

        private void Cbox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            bool bCheck = box.CheckState == CheckState.Checked ? true : false;

            UpdateDsData();

            if (bCheckAll && !bCheck)
            {
                bCheckAll = false;
                DataTable dtHeader = (DataTable)dgHeader.DataSource;
                dtHeader.Rows[0]["CheckBox"] = false;
            }
        }

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
            checkData();
        }

        private void checkAll(bool bAll)
        {
            bCheckAll = bAll;

            DataTable dt = ds.Tables["detail"];
            if (dt.Rows.Count >= 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["CheckBox"] = bAll.ToString();
                }

                UpdatedgHeader();
                UpdatedgTable(false);
            }

        }

        private void checkData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = dt.Rows[dgTable.CurrentRowIndex]["sku_qty"].ToString();

                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        UpdatedgTable(false);
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 0)
                        {
                            string subtext = ok.Substring(0, 1);
                            if (UInt64.Parse(subtext) == 0)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdatedgTable(false);
                            }
                        }
                        else if (UInt64.Parse(ok) == 0)
                        {
                            if (ok.Length >= 2)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdatedgTable(false);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dtHeader = (DataTable)dgTable.DataSource;
                    if (dtHeader != null && dtHeader.Rows.Count > 0)
                    {
                        DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {

                            DataRow row = dtHeader.Rows[hitTest.Row];
                            string colName = dtHeader.Columns[hitTest.Column].ColumnName;

                            if (colName.Equals("CheckBox"))
                            {

                                if (bCheckAll)
                                {
                                    checkAll(false);
                                }
                                else
                                {
                                    checkAll(true);
                                }
                            }
                            else
                            {
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
                    string ColName = dtt.Columns[colIndex].ColumnName;
                    if (!ColName.Equals("CheckBox") && !ColName.Equals("sku_qty"))
                    {
                        int rowIndex = dgTable.CurrentCell.RowNumber;
                        dgTable.Select(rowIndex);
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

                            if (hitTest.Column == 3)
                            {
                                dt.Rows[hitTest.Row][hitTest.Column] = !(bool)dt.Rows[hitTest.Row][hitTest.Column];
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
                        for (int j = 2; j < 4; j++)
                        {
                            if (dtt.Columns[j].ColumnName.Equals("CheckBox"))
                            {
                                dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j].ToString();
                            }
                            else
                            {
                                dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j];
                            }
                        }
                    }
                    ds.Tables.Remove("detail");

                    ds.Tables.Add(dts);

                    Serialize(guid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        List<string> sku_Number = new List<string>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (SKUName.Text==""|| SKUNO.Text=="")
                    {
                        MessageBox.Show("请检查sku和品名");
                    }
                    else
                    {
                        if (sku_Number.Contains(SKUNO.Text))
                        {
                            MessageBox.Show("该sku已存在");
                        }
                        else
                        {
                            
                            DataRow newrow = ds.Tables["detail"].NewRow();
                            
                            ds.Tables["detail"].NewRow();
                            newrow["sku"] = SKUNO.Text;
                            sku_Number.Add(SKUNO.Text);
                            newrow["sku_dsc"] = SKUName.Text;
                            newrow["sku_qty"] = Sku_qty.Text;
                            newrow["CheckBox"] = bCheckAll.ToString();
                            ds.Tables["detail"].Rows.Add(newrow);
                            UpdatedgTable(false);
                            Serialize(guid);
                            //自动翻页至最后一页
                            int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                            if (pageIndex != pageCount)
                            {
                                UpdateDsData();
                                pageIndex = pageCount;
                                UpdatedgTable(false);
                                Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                            }

                            SKUNO.Text = "";
                            SKUName.Text = "";
                            //该值是无用数据
                            Sku_qty.Text = "0";
                            SKUNO.Focus();    
                     }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                SKUName.Text = "";

                //if (SKUNO.Text == "")
                //{
                //    MessageBox.Show("请输入SKU！");
                //    return;
                //}

                //if (!regText.IsMatch(SKUNO.Text))
                //{
                //    MessageBox.Show("输入SKU非法！");
                //    return;
                //}

                if (e.KeyValue == 13 && SKUNO.Text != "")
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    btnAdd.Focus();
                    requestS01();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (MessageBox.Show("确定删除选中行？", "",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        UpdateDsData();
                        DataTable dts = ds.Tables["detail"];
                        if (dts != null)
                        {
                            bool Checked = false;
                            if (dts.Rows.Count >= 1)
                            {
                                for (int i = dts.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (bool.Parse(dts.Rows[i]["CheckBox"].ToString()))
                                    {
                                        dts.Rows.RemoveAt(i);
                                        Checked = true;
                                    }
                                }
                                if (!Checked)
                                {
                                    MessageBox.Show("未选中任何行!");
                                }
                                else
                                {
                                    UpdatedgTable(false);
                                    Serialize(guid);
                                }
                            }
                            else
                            {
                                MessageBox.Show("表格内无数据!");
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

        private bool CheckSubmit()
        {
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (CheckSubmit())
                {
                    requestR02();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                sku_Number.Clear();
                //DialogResult = DialogResult.OK;
                if (btn02.Enabled)
                {
                    if (MessageBox.Show("是否保存当前数据？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Serialize(guid);
                        DialogResult = DialogResult.Abort;
                    }
                    else
                    {
                        File.Delete(Config.DirLocal + guid);
                    }
                    //if (MessageBox.Show("数据没有提交，确认退出当前页面？", "",
                    //MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    //{
                    //    File.Delete(Config.DirLocal + guid);
                    //    Close();
                    //}
                    //else
                    //{

                    //}
                }
                else
                {
                    File.Delete(Config.DirLocal + guid);
                    Close();
                }

            }
        }

        #endregion

        #region 翻页

        private void NexPage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    UpdateDsData();
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

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    UpdateDsData();
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
                    case API_ID.R104301:
                        if (result == ConnThread.RESULT_OK)
                        {
                            requestXML();
                        }
                        else
                        {
                            CreateDS();
                            Serialize(guid);
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
                    case API_ID.R104302:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功!");
                            btn02.Enabled = false;
                            SKUNO.Enabled = false;
                            btnAdd.Enabled = false;
                            btnDelete.Enabled = false;
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
                        if (true)
                        {
                            
                        }
                        break;
                    case API_ID.DOWNLOAD_S01:
                        if (result == ConnThread.RESULT_FILE)
                        {
                            LoadXML();
                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.DOWNLOAD104301:
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
                ShopNo = (string)param[0];
                GroupID.Text = (string)param[1];
                GroupName.Text = (string)param[2];
                if (!bool.Parse((string)param[3]))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                    {
                        this.Invoke(new InvokeDelegate(() =>
                        {
                            requestR01();

                        }));

                    }));
                }
                else
                {
                    CreateDS();
                    Serialize(guid);
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
                data[(int)DATA_INDEX.GroupID] = GroupID.Text;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.ShopNo] = ShopNo;
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
                data[(int)DATA_INDEX.DataGuid] = DataGuid;
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
                    GroupID.Text = sc.Data[(int)DATA_INDEX.GroupID];
                    SKUNO.Text=sc.Data[(int)DATA_INDEX.SKUNO];
                    SKUName.Text=sc.Data[(int)DATA_INDEX.SKUName];
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    DataGuid = sc.Data[(int)DATA_INDEX.DataGuid];
                    ds=sc.DS;

                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        if (ds.Tables["detail"].Rows.Count > 0)
                        {
                            UpdatedgTable(true);
                        }
                    }
                    else
                    {
                        MessageBox.Show("CreateDS");
                        CreateDS();
                    }

                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void GroupName_ParentChanged(object sender, EventArgs e)
        {

        }

        private void SKUNO_TextChanged(object sender, EventArgs e)
        {
            if (SKUNO.Text!=null)
            {
                SKUNO.Text = SKUNO.Text.ToString().Trim();
            }
        }

    }
}