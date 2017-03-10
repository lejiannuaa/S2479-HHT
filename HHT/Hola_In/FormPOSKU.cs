using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using HolaCore;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Hola_In;



namespace Hola_In
{
    public partial class FormPOSKU : Form, ConnCallback, ISerializable, ScanCallback
    {
        //临时存储
        private const string guid = "FCBA2759-CC36-B3E0-402F-09E7775421FD";
        //手动存储
        private const string guid2 = "BA1C6ED3-C098-4f0A-A92C-B7DFCAAF86EF";
        private const string OGuid = "00000000000000000000000000000000";
        private SerializeClass sc = null;

        //指示当前正在发送网络请求
        private bool busy = false;

        private bool bCheckAll = false;
        //短收明细表
        DataTable dtDiff = null;
        //默认排序表
        private DataTable dtOrder = null;

        private Form child = null;
        private FormScan formScan = null;
        private FormDownload formDownload = null;
        private int TaskBarHeight = 0;
        private string xmlFile = null;

        private delegate void InvokeDelegate();

        private DataSet ds = null;
        private int TABLE_ROWMAX = 5;
        //发送数据标识唯一性
        private string DataGuid05 = "";
        private string DataGuid02 = "";
        private string DataGuid03 = "";
        private int tableIndex = -1;
        private int pageIndex = 1;
        private int colIndex = -1;
        private int rowFlag = -1;
        private string lastSKU = null;
        Regex regi = new Regex("^[0-9]*[1-9][0-9]*$");
        Regex reg = new Regex(@"^\d+$");
        private string DataGuid = "";
        private bool bRequest=false;
        private DataSet DsReason = null;
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            SKU,
            COUNT,
            STATE,
            TABLEINDEX,
            PAGENDEX,
            ROWINDEX,
            COLINDEX,
            ID,
            NAME,
            TYPE,
            SORTFLAG,
            ROWFLAG,
            CHILD,
            BTN02,
            BTN03,
            BTN04,
            BTNADD,
            BTNSELECT,
            BTNSELECTTXT,
            DATAGUID,
            DATAGUID02,
            DATAGUID03,
            DATAGUID05,
            BREQUEST,
            DATAMAX
        }
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            O0501,
            O0502,
            O0503,
            O0504,
            O0505,
            O0402,
            O0403,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormPOSKU()
        {
            InitializeComponent();
            DataGuid05 = Guid.NewGuid().ToString().Replace("-", "");
            doLayout();

            NextPage.Focus();
            QueryPerformanceFrequency(ref freq);

            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"\Program Files\hhtiii\00501.xml");
            //LoadData();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
           
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                if (dstWidth > 240)
                {
                    dgTable.Height = dgTable.Height + 25;
                    TABLE_ROWMAX += 1;
                }
                btnReturn.Top = dstHeight - btnReturn.Height;
                btn02.Top = dstHeight - btn02.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                NextPage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                PrePage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                Page.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;

                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        #region XML数据加载
        private void showXML()
        {
            LoadData();
            Serialize(guid);
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;


            //xmlFile = @"\Program Files\hhtiii\00501.xml";
            try
            {
                if (File.Exists(xmlFile))
                {
                    Start = 0;
                    Stop = 0;
                    QueryPerformanceCounter(ref Start);

                    doc = new XmlDocument();
                    doc.Load(xmlFile);
                    reader = new XmlNodeReader(doc);
                    ds = new DataSet();
                    ds.ReadXml(reader);

                    QueryPerformanceCounter(ref Stop);
                    string str1 = "loadXML ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                    Config.writeFile(str1, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("detail"))
                        {
                            tableIndex = i;
                            bNoData = false;
                            tbManufacturerID.Text = ds.Tables[i].Rows[0]["厂商"].ToString();
                            tbManufacturerName.Text = ds.Tables[i].Rows[0]["厂商描述"].ToString();
                            tbManufacturerType.Text = ds.Tables[i].Rows[0]["厂商类型"].ToString();
                            //判断照相按钮
                            //if (tbManufacturerType.Text=="代中转")
                            //{
                            //    Camera.Enabled = true;
                            //}
                            //else
                            //{
                            //    Camera.Enabled = false;
                            //}

                        }
                        else if (ds.Tables[i].TableName.Equals("info"))
                        {
                            string state = ds.Tables[i].Rows[0]["STATE"].ToString();
                            switch (UInt64.Parse(state))
                            {
                                case 0: //删除
                                    setButton(false, false, false, false, false);
                                    tbState.Text = "删除";
                                    break;

                                case 1: //待申请
                                    btnSelect.Text = "申请收货";
                                    setButton(false, false, false, false, true);
                                    tbState.Text = "可申请";
                                    break;

                                case 2: //申请中
                                    btnSelect.Enabled = false;
                                    break;

                                case 3: //可收货
                                    btnSelect.Text = "解除收货";
                                    setButton(true, true, false, false, true);
                                    tbState.Text = "可收货";
                                    break;

                                case 4: //收货中
                                    btnSelect.Text = "解除收货";
                                    setButton(false, false, false, false, true);
                                    tbState.Text = "收货中";
                                    break;

                                case 5: //已收货
                                    tbState.Text = "已收货";
                                    setButton(false, false, false, false, false);
                                    break;

                                default:
                                    setButton(false, false, false, false, false);
                                    break;
                            }
                        }
                    }
                    if (!bNoData)
                    {
                        DataGuid = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid02 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid03 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid05 = Guid.NewGuid().ToString().Replace("-", "");
                        pageIndex = 1;
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();
                        
                        UpdateColumn(true, true, false);
                    }
                }
                else
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
                    MessageBox.Show("请求文件不存在,请重新请求!");
                    setButton(false, false, false, false, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);
                if (reader != null)
                {
                    reader.Close();
                }

                if (bNoData)
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
                    MessageBox.Show("无数据！");
                    setButton(false, false, false, false, false);
                }
                File.Delete(xmlFile);
                GC.Collect();
                Serialize(guid);
                QueryPerformanceCounter(ref Stop);
                string str1 = "Serialize ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str1, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
            }
        }

        private void ClearTableStyle(GridTableStylesCollection ts)
        {
            foreach (DataGridTableStyle tstyle in ts)
            {
                foreach (DataGridTextBoxColumn cstyle in tstyle.GridColumnStyles)
                {
                    try
                    {
                        (cstyle as DataGridCustomColumnBase).Clear();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool bCustom)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables[tableIndex];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;

            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = new DataGridCustomTextBoxColumn();
                DataGridColumnStyle style1 = new DataGridTextBoxColumn();
                

                if (bCustom && name[i].Equals("完好"))
                {
                    style.Owner = dgTable;
                    if (tbState.Text.Equals("可收货"))
                    {
                        style.ReadOnly = false;
                    }
                    else
                    {
                        style.ReadOnly = true;
                    }
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    
                    ts.GridColumnStyles.Add(style);
                }
                else if (name[i].Equals("SKU"))
                {
                    if (bCustom)
                    {
                        style = new DataGridCustomTextBoxColumn();

                        style.Owner = dgTable;

                        style.Alignment = HorizontalAlignment.Right;
                        style.ReadOnly = true;
                    }
                    else
                    {
                        style = new DataGridCustomTextBoxColumn();
                        style.Owner = dgTable;
                        style.ReadOnly = true;
                    }
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

            return ts;
        }

        //初始化源表、排序表
        private void UpdateColumn(bool bInitColumn, bool bReorder, bool bTurnPage)
        {
            try
            {
                //不排序、只刷新
                if (!bReorder)
                {
                    UpdateRow(bReorder, bTurnPage);
                    return;
                }

                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);
                
                string[] colName = new string[] { "ID", "SKU", "品名", "出货数量", "完好", "差异" };//
                string[] colValue = new string[] { "ID", "SKU", "品名", "出货", "完好", "差异" };      //
                //int[] colWidth = new int[] { 10, 25, 25, 13, 13, 11 };                                       
                int[] colWidth = new int[] { 10, 19, 31, 13, 13, 11 };

                //从XML首次加载需初始化列
                if (bInitColumn)
                {
                    DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                    DataRowCollection rows = ds.Tables[tableIndex].Rows;
                    cols.Add("ID", System.Type.GetType("System.UInt32"));
                    cols.Add("完好");
                    cols.Add("差异");
                    cols.Add("维护");
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["ID"] = i + 1;
                        rows[i]["完好"] = "0";
                        rows[i]["差异"] = rows[i]["出货数量"];
                        rows[i]["维护"] = "0";
                    }

                    if (dtDiff != null)
                    {
                        dtDiff.Dispose();
                    }
					//加CheckBox
                    if (!cols.Contains("CheckBox"))            //
                    {
                        cols.Add("CheckBox");
                        for (int i = 0; i < rows.Count; i++)
                        {
                            rows[i]["CheckBox"] = "False";
                        }
                    }
                                                               //

                    dtDiff = new DataTable();
                    dtDiff.TableName = "diff";
                    dtDiff.Columns.Add("SKU");
                    dtDiff.Columns.Add("reason");
                    dtDiff.Columns.Add("count");
                    dtDiff.Columns.Add("photo");
                    ds.Tables.Add(dtDiff);
                }

                dtOrder = ds.Tables[tableIndex].DefaultView.ToTable();

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow(bReorder, bTurnPage);

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colName.Length; i++)
                {
                    row[i] = colValue[i];
                }
				//加CheckBox
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

                QueryPerformanceCounter(ref Stop);
                string str = "Show ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //从排序表读取当前页数据
        private void UpdateRow(bool bReorder, bool bTurnPage)
        {
            DataTable dtResult = null;

            //如果需要重新排序则重新读取
            if (bReorder || bTurnPage)
            {
                dtResult = new DataTable();
                dtResult.TableName = dtOrder.TableName;
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                for (int i = 0; i < styles.Count; i++)
                {
                    dtResult.Columns.Add(styles[i].MappingName);
                }

                int from = (pageIndex - 1) * TABLE_ROWMAX;
                int to = from + TABLE_ROWMAX;
                if (to > dtOrder.Rows.Count)
                {
                    to = dtOrder.Rows.Count;
                }

                for (int i = from; i < to; i++)
                {
                    DataRow rowNew = dtResult.NewRow();
                    for (int j = 0; j < styles.Count; j++)
                    {
                        string name = styles[j].MappingName;
                        rowNew[name] = dtOrder.Rows[i][name];
                    }

                    dtResult.Rows.Add(rowNew);
                }

                dgTable.DataSource = dtResult;
            }
            else
            {
                dtResult = (DataTable)dgTable.DataSource;
            }

            //计算差异列
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                string total = dtResult.Rows[i]["出货数量"].ToString();
                string ok = dtResult.Rows[i]["完好"].ToString();
                string diff = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                dtResult.Rows[i]["差异"] = diff;
            }
        }

        //刷新列表保存到ds
        private void updateData(string key, string col, string value, bool bReorder)
        {
            try
            {
                //刷新排序表
                for (int i = 0; i < dtOrder.Rows.Count; i++)
                {
                    if (key.Equals(dtOrder.Rows[i]["ID"].ToString()))
                    {
                        dtOrder.Rows[i][col] = value;
                    }
                }

                //刷新源表
                DataTable dt = ds.Tables[tableIndex];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (key.Equals(dt.Rows[i]["ID"].ToString()))
                    {
                        dt.Rows[i][col] = value;
                        UpdateColumn(false, bReorder, false);
                        Serialize(guid);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable getDTOK()
        {
            DataTable dt = ds.Tables[tableIndex];
            DataTable dtOK = new DataTable();
            dtOK.TableName = "info";
            dtOK.Columns.Add("SKU");
            dtOK.Columns.Add("whsl");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rowNew = dtOK.NewRow();

                rowNew["SKU"] = dt.Rows[i]["SKU"];
                rowNew["whsl"] = dt.Rows[i]["完好"];

                //加skuname chsl
                
                dtOK.Rows.Add(rowNew);
            }

            return dtOK;
        }

        private void getdtReason(DataSet dt)
        {
            DsReason = dt;
        }

        #endregion

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
            if (SKU.Length < 9)
            {
                StringBuilder addString = new StringBuilder();
                int addStringNo = 9 - SKU.Length;
                addString.Append('0', addStringNo);
                SKU = addString.ToString() + SKU;
            }
            return SKU;
        }

        //设置按钮可用状态
        private void setButton(bool bAdd, bool b2, bool b3, bool b4, bool bSele)
        {
            btnAdd.Enabled = bAdd;
            btn02.Enabled = b2;
            btn03.Enabled = b3;
            btn04.Enabled = b4;
            btnSelect.Enabled = bSele;
        }

        #region 翻页
        private void PrePage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                    {
                        lastRowIndex = dgTable.CurrentRowIndex;
                        refreshData(false);
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);

                        pageIndex--;
                        if (pageIndex >= 1)
                        {
                            lastRowIndex = -1;
                            ClearTableStyle(dgTable.TableStyles);
                            UpdateColumn(false, false, true);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NextPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                    {
                        lastRowIndex = dgTable.CurrentRowIndex;
                        refreshData(false);
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);

                        pageIndex++;
                        if (pageIndex <= pageCount)
                        {
                            lastRowIndex = -1;
                            ClearTableStyle(dgTable.TableStyles);
                            UpdateColumn(false, false, true);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region UI事件响应

        private void LockGrid()
        {
            try
            {
                int styleCount = dgTable.TableStyles[0].GridColumnStyles.Count;
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                DataGridCustomColumnBase style;
                for (int i = 0; i < styleCount; i++)
                {
                    if (styles[i].MappingName.Equals("完好"))
                    {
                        style = (DataGridCustomColumnBase)styles[i];
                        style.ReadOnly = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void UnLockGrid()
        {
            try
            {
                int styleCount = dgTable.TableStyles[0].GridColumnStyles.Count;
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                DataGridCustomColumnBase style;
                for (int i = 0; i < styleCount; i++)
                {
                    if (styles[i].MappingName.Equals("完好"))
                    {
                        style = (DataGridCustomColumnBase)styles[i];
                        style.ReadOnly = false;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tbSKU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!busy && e.KeyChar == (char)13)
            {
                if (lastSKU == null)
                {
                    tbCount.Focus();
                }
                else
                {
                    try
                    {
                        if (lastSKU.Equals(tbSKU.Text))
                        {
                            tbCount.Text = (UInt64.Parse(tbCount.Text) + 1).ToString();
                        }
                        else
                        {
                            tbCount.Text = "1";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        tbCount.Text = "1";
                    }
                    finally
                    {
                        lastSKU = null;
                    }
                }
            }
        }

        private void tbSKU_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == 0)
            {
                lastSKU = tbSKU.Text;
                tbSKU.Text = "";
            }
        }

        private void FormPOSKU_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dtt = (DataTable)dgTable.DataSource;
                    if (dtt != null && dtt.Rows.Count > 0)
                    {
                        if ((e.KeyCode == System.Windows.Forms.Keys.Up))
                        {
                            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                            for (int i = 0; i < styles.Count; i++)
                            {
                                if (styles[i].MappingName.Equals("完好"))
                                {
                                    DataGridCustomColumnBase col = (DataGridCustomColumnBase)styles[i];
                                    if (((TextBox)col.HostedControl).Focused)
                                    {
                                        int row = dgTable.CurrentCell.RowNumber;

                                        if (CheckGridCellData(row, ((TextBox)col.HostedControl).Text))
                                        {
                                            if (--row >= 0)
                                            {
                                                dgTable.CurrentCell = new DataGridCell(row, 4);
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        if ((e.KeyCode == System.Windows.Forms.Keys.Down))
                        {
                            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                            for (int i = 0; i < styles.Count; i++)
                            {
                                if (styles[i].MappingName.Equals("完好"))
                                {
                                    DataGridCustomColumnBase col = (DataGridCustomColumnBase)styles[i];
                                    if (((TextBox)col.HostedControl).Focused)
                                    {
                                        int row = dgTable.CurrentCell.RowNumber;
                                        if (CheckGridCellData(row, ((TextBox)col.HostedControl).Text))
                                        {
                                            DataTable dt = (DataTable)dgTable.DataSource;
                                            int rowCount = dt.Rows.Count - 1;
                                            if (++row <= rowCount)
                                            {
                                                dgTable.CurrentCell = new DataGridCell(row, 4);
                                            }
                                        }
                                    }
                                    break;
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

        private bool CheckGridCellData(int row,string ss)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = ss;
                string total = dt.Rows[dgTable.CurrentRowIndex]["出货数量"].ToString();
                string id = dt.Rows[row]["ID"].ToString();
                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        return false;
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 0)
                        {
                            string subtext = ok.Substring(0, 1);
                            if (UInt64.Parse(subtext) == 0)
                            {
                                MessageBox.Show("请输入非负整数！");
                                return false;
                            }
                            else if (UInt64.Parse(total) < UInt64.Parse(ok))
                            {
                                MessageBox.Show("完好数量不可大于出货数量！(1)");
                                return false;
                            }
                            else
                            {
                                updateData(id, "完好", ok, false);
                                dt.Rows[dgTable.CurrentRowIndex]["差异"] = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                                return true;
                            }
                        }
                        else if (UInt64.Parse(ok) == 0)
                        {
                            if (ok.Length >= 2)
                            {
                                MessageBox.Show("请输入非负整数！");
                                return false;
                            }
                            else
                            {
                                updateData(id, "完好", ok, false);
                                dt.Rows[dgTable.CurrentRowIndex]["差异"] = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void addCount()
        {
            try
            {
                if (tableIndex < 0)
                {
                    MessageBox.Show("无数据！");
                }
                bool bAdded = false;
                bool BNum = true;
                if (regi.IsMatch(tbCount.Text))
                {
                    string OK = tbCount.Text;
                    if (UInt64.Parse(OK) > 0)
                    {
                        string subtext = OK.Substring(0, 1);
                        if (UInt64.Parse(subtext) == 0)
                        {
                            BNum = false;
                            MessageBox.Show("请输入正整数！");
                        }
                    }
                    if (BNum)
                    {
                        DataTable dt = ds.Tables[tableIndex];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (tbSKU.Text.Length < 8 || tbSKU.Text.Length == 9)
                            {
                                string SKUADD = AddZero(tbSKU.Text);
                                if (SKUADD.Equals(dt.Rows[i]["SKU"].ToString()))
                                {
                                    string total = dt.Rows[i]["出货数量"].ToString();
                                    string ok = dt.Rows[i]["完好"].ToString();
                                    if (UInt64.Parse(total) < UInt64.Parse(ok) + UInt64.Parse(tbCount.Text))
                                    {
                                        MessageBox.Show("完好数量不可大于出货数量！(2)");
                                        return;
                                    }
                                    else
                                    {
                                        bAdded = true;
                                        dt.Rows[i]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                        //刷新排序表
                                        for (int j = 0; j < dtOrder.Rows.Count; j++)
                                        {
                                            if (SKUADD.Equals(dtOrder.Rows[j]["SKU"].ToString()))
                                            {
                                                dtOrder.Rows[j]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                                break;
                                            }
                                        }
                                        //刷新显示表
                                        DataTable dtView = (DataTable)dgTable.DataSource;
                                        for (int j = 0; j < dtView.Rows.Count; j++)
                                        {
                                            if (SKUADD.Equals(dtView.Rows[j]["SKU"].ToString()))
                                            {
                                                dtView.Rows[j]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            else if (tbSKU.Text.Length == 8)
                            {
                                if (tbSKU.Text.Equals(dt.Rows[i]["IPC"].ToString()))
                                {
                                    string total = dt.Rows[i]["出货数量"].ToString();
                                    string ok = dt.Rows[i]["完好"].ToString();
                                    if (UInt64.Parse(total) < UInt64.Parse(ok) + UInt64.Parse(tbCount.Text))
                                    {
                                        MessageBox.Show("完好数量不可大于出货数量！(3)");
                                        return;
                                    }
                                    else
                                    {
                                        bAdded = true;
                                        dt.Rows[i]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                        //刷新排序表
                                        for (int j = 0; j < dtOrder.Rows.Count; j++)
                                        {
                                            if (tbSKU.Text.Equals(dtOrder.Rows[j]["IPC"].ToString()))
                                            {
                                                dtOrder.Rows[j]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                                break;
                                            }
                                        }
                                        //刷新显示表
                                        DataTable dtView = (DataTable)dgTable.DataSource;
                                        //for (int j = 0; j < dtView.Rows.Count; j++)
                                        //{
                                        //    if (tbSKU.Text.Equals(dtView.Rows[j]["IPC"].ToString()))
                                        //    {
                                        //        dtView.Rows[j]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                        //        break;
                                        //    }
                                        //}
                                        for (int j = 0; j < dtView.Rows.Count; j++)
                                        {
                                            if (tbSKU.Text.Equals(dtOrder.Rows[j]["IPC"].ToString()))
                                            {
                                                dtView.Rows[j]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                bool BUPC = false;
                                string UPC = dt.Rows[i]["UPC"].ToString();
                                string[] upc = UPC.Split(',');
                                for (int j = 0; j < upc.Length; j++)
                                {
                                    string PpSKU = (string)upc.GetValue(j);
                                    if (tbSKU.Text.Equals(PpSKU))
                                    {
                                        BUPC = true;
                                        string total = dt.Rows[i]["出货数量"].ToString();
                                        string ok = dt.Rows[i]["完好"].ToString();
                                        if (UInt64.Parse(total) < UInt64.Parse(ok) + UInt64.Parse(tbCount.Text))
                                        {
                                            MessageBox.Show("完好数量不可大于出货数量！(4)");
                                            return;
                                        }
                                        else
                                        {
                                            bAdded = true;
                                            dt.Rows[i]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                            //刷新排序表
                                            for (int k = 0; k < dtOrder.Rows.Count; k++)
                                            {
                                                BUPC = false;
                                                UPC = dtOrder.Rows[k]["UPC"].ToString();
                                                upc = UPC.Split(',');
                                                for (int l = 0; l < upc.Length; l++)
                                                {
                                                    PpSKU = (string)upc.GetValue(l);
                                                    if (tbSKU.Text.Equals(PpSKU))
                                                    {
                                                        dtOrder.Rows[k]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                                        break;
                                                    }
                                                }
                                                if (BUPC)
                                                {
                                                    break;
                                                }
                                            }
                                            //刷新显示表
                                            DataTable dtView = (DataTable)dgTable.DataSource;
                                            for (int k = 0; k < dtView.Rows.Count; k++)
                                            {
                                                BUPC = false;
                                                UPC = dtOrder.Rows[k]["UPC"].ToString();
                                                upc = UPC.Split(',');
                                                for (int l = 0; l < upc.Length; l++)
                                                {
                                                    PpSKU = (string)upc.GetValue(l);
                                                    if (tbSKU.Text.Equals(PpSKU))
                                                    {
                                                        dtView.Rows[k]["完好"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                                        break;
                                                    }
                                                }
                                                if (BUPC)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                if (BUPC)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (reg.IsMatch(tbCount.Text))
                {
                    string ok = tbCount.Text;
                    if (UInt64.Parse(ok) == 0)
                    {
                        if (ok.Length >= 2)
                        {
                            BNum = false;
                            MessageBox.Show("请输入正整数！");
                        }
                        else
                        {
                            BNum = false;
                            MessageBox.Show("请输入正整数！");
                        }
                    }
                }
                else
                {
                    BNum = false;
                    MessageBox.Show("请输入正整数！");
                }
                if (BNum)
                {
                    if (bAdded)
                    {
                        tbSKU.Text = "";
                        tbCount.Text = "";
                        tbSKU.Focus();

                        ClearTableStyle(dgTable.TableStyles);
                        UpdateColumn(false, false, true);
                    }
                    else
                    {
                        MessageBox.Show("您输入的" + tbSKU.Text + "不存在于此箱中!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!busy && e.KeyChar == (char)13)
            {
                addCount();
            }
        }

        private bool refreshData(bool bReorder)
        {
            bool bRestore = false;
            if (lastRowIndex < 0)
            {
                UpdateColumn(false, bReorder, false);
                return bRestore;
            }

            DataTable dt = (DataTable)dgTable.DataSource;
            try
            {
                string id = dt.Rows[lastRowIndex]["ID"].ToString();
                string total = dt.Rows[lastRowIndex]["出货数量"].ToString();
                string ok = dt.Rows[lastRowIndex]["完好"].ToString();
                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        bRestore = true;
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 0)
                        {
                            string subtext = ok.Substring(0, 1);
                            if (UInt64.Parse(subtext) == 0)
                            {
                                MessageBox.Show("请输入非负整数！");
                                bRestore = true;
                            }
                            else if (UInt64.Parse(total) < UInt64.Parse(ok))
                            {
                                MessageBox.Show("完好数量不可大于出货数量！(5)");
                                bRestore = true;
                            }
                            else
                            {
                                updateData(id, "完好", ok, bReorder);
                            }
                        }
                        else if (UInt64.Parse(ok) == 0)
                        {
                            if (ok.Length >= 2)
                            {
                                MessageBox.Show("请输入非负整数！");
                                bRestore = true;
                            }
                            else
                            {
                                updateData(id, "完好", ok, bReorder);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
                    bRestore = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (bRestore)
            {
                dt.Rows[lastRowIndex]["完好"] = int.Parse(dt.Rows[lastRowIndex]["出货数量"].ToString()) - int.Parse(dt.Rows[lastRowIndex]["差异"].ToString());
            }

            return bRestore;
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy && tableIndex >= 0)
                {
                    DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        DataTable dtHeader = (DataTable)dgHeader.DataSource;
                        DataColumn col = dtHeader.Columns[hitTest.Column];
                        DataTable dt = ds.Tables[tableIndex];
                        if (dt != null && dt.Rows.Count >= 1)
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

                            refreshData(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int lastRowIndex = -1;
        private string lastText = "";
        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox tb = dgTable.Controls[0] as TextBox;
                if (tb != null)
                {
                    lastText = tb.Text;
                    if (lastRowIndex == -1 && dgTable.Controls.Count == 1)
                    {
                        tb.LostFocus += new EventHandler(tb_LostFocus);
                    }
                }

                DataTable dt = (DataTable)dgTable.DataSource;

                if (lastRowIndex >= 0 && lastRowIndex != dgTable.CurrentRowIndex)
                {
                    dgTable.UnSelect(lastRowIndex);
                }

                if (!dt.Columns[dgTable.CurrentCell.ColumnNumber].ColumnName.Equals("HHTRQT"))
                {
                    dgTable.Select(dgTable.CurrentRowIndex);
                }

                tbSKU.Text = dt.Rows[dgTable.CurrentRowIndex]["SKU"].ToString();
                tbCount.Text = dt.Rows[dgTable.CurrentRowIndex]["完好"].ToString();
                //tbManufacturerID.Text = dt.Rows[dgTable.CurrentRowIndex]["厂商"].ToString();
                //tbManufacturerName.Text = dt.Rows[dgTable.CurrentRowIndex]["厂商描述"].ToString();
                //tbManufacturerType.Text = dt.Rows[dgTable.CurrentRowIndex]["厂商类型"].ToString();

                if (lastRowIndex >= 0)
                {
                    refreshData(false);
                }
                lastRowIndex = dgTable.CurrentRowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private System.Windows.Forms.Timer t = null;
        void tb_LostFocus(object sender, EventArgs e)
        {
            if (t == null)
            {
                t = new System.Windows.Forms.Timer();
                t.Tick += new EventHandler(tb_Restore);
                t.Interval = 100;
            }
            t.Enabled = true;
        }

        public void tb_Restore(Object myObject, EventArgs myEventArgs)
        {
            if (t.Enabled)
            {
                t.Enabled = false;
                if (refreshData(false))
                {
                    (dgTable.Controls[0] as TextBox).Text = lastText;
                }
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                contextMenu1.MenuItems.Clear();

                if (!busy && tableIndex >= 0)
                {
                    int x = Control.MousePosition.X;
                    int y = Control.MousePosition.Y - dgTable.Top - TaskBarHeight;

                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);

                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        contextMenu1.MenuItems.Add(this.menuDetail);

                        rowFlag = hitTest.Row;
                        dgTable.Select(rowFlag);

                        dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showChild(bool bDeserialize)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                DataRow row = dt.Rows[rowFlag];
                child = new FormReason(this.Text.ToString());
                if (!bDeserialize)
                {
                    string total = row["出货数量"].ToString();
                    string ok = row["完好"].ToString();
                    string diff = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                    ((FormReason)child).GetDtReason += new FormReason.getDtReason(getdtReason); 
                    ((ISerializable)child).init(new object[] { btnBC.Text, row["SKU"].ToString(), row["品名"].ToString(), diff, dtDiff, DsReason, bRequest.ToString(),"0" });
                }
                else
                {
                    string total = row["出货数量"].ToString();
                    string ok = row["完好"].ToString();
                    string diff = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                    ((ISerializable)child).init(new object[] { btnBC.Text, row["SKU"].ToString(), row["品名"].ToString(), diff, dtDiff,"0" });
                    ((ISerializable)child).Deserialize(null);
                }

                if (DialogResult.OK == child.ShowDialog())
                {
                    try
                    {
                        string strOK = row["完好"].ToString();
                        string strTotal = row["出货数量"].ToString();
                        string strDiff = (UInt64.Parse(strTotal) - UInt64.Parse(strOK)).ToString();
                        updateData(row["ID"].ToString(), "维护", strDiff, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (!bRequest)
                {
                    bRequest = true;
                }
                Show();
                child.Dispose();

                child = null;
                Serialize(guid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearGrid()
        {
            tableIndex = -1;
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
            }
            dgHeader.Controls.Clear();
            dgHeader.DataSource = null;
            dgTable.Controls.Clear();
            dgTable.DataSource = null;
        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                showChild(false);
                //this.Text.ToString();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                try
                {
                    if (tableIndex < 0)
                    {
                        MessageBox.Show("无数据");
                        return;
                    }

                    lastRowIndex = dgTable.CurrentRowIndex;
                    refreshData(false);
                    DataTable dt = (DataTable)dgTable.DataSource;
                    tbSKU.Text = dt.Rows[dgTable.CurrentRowIndex]["SKU"].ToString();
                    tbCount.Text = dt.Rows[dgTable.CurrentRowIndex]["完好"].ToString();

                    for (int i = 0; i < dtOrder.Rows.Count; i++)
                    {
                        string total = dtOrder.Rows[i]["出货数量"].ToString();
                        string ok = dtOrder.Rows[i]["完好"].ToString();
                        string maintain = dtOrder.Rows[i]["维护"].ToString();

                        if (UInt64.Parse(total) != UInt64.Parse(ok) + UInt64.Parse(maintain))
                        {
                            pageIndex = (int)Math.Ceiling((i + 1) / (double)TABLE_ROWMAX);
                            MessageBox.Show("请先维护差异：行" + (i % TABLE_ROWMAX + 1).ToString());
                            int pageCount = (int)Math.Ceiling(dtOrder.Rows.Count / (double)TABLE_ROWMAX);
                            if (pageCount == 0)
                                Page.Text = "1/1";
                            else
                                Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();

                            UpdateColumn(false, false, false);
                            return;
                        }
                    }

                    btn02.Enabled = false;
                    request02();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(btnBC.Text) && !btnBC.Text.Equals("null"))
                {
                    request03();
                }
                else
                {
                    MessageBox.Show("请检查单号!");
                }
            }
        }

        private void btn04_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(btnBC.Text) && !btnBC.Text.Equals("null"))
                {
                    request04();
                }
                else
                {
                    MessageBox.Show("请检查单号!");
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                addCount();
            }
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            if (ds == null)
            {
                MessageBox.Show("无数据！");
                return;
            }

            if (File.Exists(Config.DirLocal + guid2))
            {
                if (MessageBox.Show("检测到已有数据，是否覆盖？", "", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }
              
            Serialize(guid2);
            MessageBox.Show("暂存成功！");
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (ds != null)
            {
                if (MessageBox.Show("是否覆盖当前数据？", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            if (File.Exists(Config.DirLocal + guid2))
            {
                Deserialize(guid2);
            }
            else
            {
                MessageBox.Show("无数据！");
            }
        }

        private void close()
        {
            try
            {
                File.Delete(Config.DirLocal + guid);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbCount_TextChanged(object sender, EventArgs e)
        {
            if (tbCount.Text.Length >= 10)
            {
                MessageBox.Show("输入字符串过长!");
                tbCount.Text = tbCount.Text.Substring(0, 9);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                try
                {
                    if (!btn02.Enabled)
                    {
                        close();
                    }
                    else
                    {
                        if (DialogResult.Yes == MessageBox.Show("是否放弃当前操作？", "", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            request05();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //原fromPO事件

        private void btnBC_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                string bc = btnBC.Text;
                apiID = API_ID.O0501;
                formScan = new FormScan();
                formScan.init(this, bc);
                formScan.ShowDialog();
                formScan.Dispose();
                formScan = null;
                bc = "";
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (btnSelect.Text.Equals("申请收货"))
                {
                    request00402();
                }
                else if (btnSelect.Text.Equals("解除收货"))
                {
                    request00403();
                }
            }
        }

        private string photoName1="";
        private string photoName2="";
        private string photoName3="";
        //组合照片名
        private string CameraPhoto;
        //退出照相页面取回照片名
        void getPhotoName(string photo1, string photo2, string photo3)
        {
            photoName1 = photo1;
            photoName2 = photo2;
            photoName3 = photo3;
            CameraPhoto = photoName1 + "," + photoName2 + "," + photoName3;
        }

        private bool isPO = false;

        private void Camera_Click_1(object sender, EventArgs e)
        {
            FormCamera formCamera = new FormCamera(photoName1, photoName2, photoName3, isPO);
            //formCamera.MyPhotoEvent += new FormCamera.MyPhotoDelegate(getPhotoName);
            PageTitle pageTitle = new PageTitle(formCamera.getTitle);
            pageTitle(this);
            formCamera.ShowDialog();
            CameraPhoto = formCamera.photoName();
        }


        #endregion

        #region 实现ConnCallback接口
        public void progressCallback(int total, int progress)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                if (formDownload != null)
                {
                    formDownload.setProgress(total, progress);
                }
            }));
        }

        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.O0501:
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
                            setButton(false, false, false, false, false);
                        }
                        break;

                    case API_ID.O0502:
                        if (result == ConnThread.RESULT_OK)
                        {
                            tbState.Text = "已收货";
                            ds.Tables["info"].Rows[0]["STATE"] = "5";
                            setButton(false, false, true, true, false);
                            Serialize(guid);
                            MessageBox.Show("请求成功！");
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
                            setButton(false, true, false, false, false);
                            Serialize(guid);
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.O0503:
                    case API_ID.O0504:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
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

                    case API_ID.O0505:
                        {
                            if (result == ConnThread.RESULT_OK)
                            {
                                ;
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

                            close();
                        }
                        break;

                    case API_ID.O0402:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                            btnSelect.Text = "解除收货";
                            ds.Tables["info"].Rows[0]["STATE"] = "3";
                            tbState.Text = "可收货";
                            setButton(true, true, false, false, true);
                            UnLockGrid();
                            Serialize(guid);
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

                    case API_ID.O0403:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                            btnSelect.Text = "申请收货";
                            ds.Tables["info"].Rows[0]["STATE"] = "1";
                            tbState.Text = "可申请";
                            setButton(false, false, false, false, true);
                            LockGrid();
                            Serialize(guid);
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
                            showXML();
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

        #region 接口请求

        private void request(string op, string bc)
        {
            string msg = "request=005;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            if (op.Equals("05"))
            {
                msg = DataGuid05 + msg;
            }
            else
            {
                msg = OGuid + msg;
            }
            
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request01()
        {
            apiID = API_ID.O0501;
            ClearGrid();
            request("01", btnBC.Text);
        }

        private void request02()
        {
            apiID = API_ID.O0502;
            DataTable[] dtSubmit = new DataTable[] { getDTOK(), dtDiff };
            string op = "02";
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "005", op);
            string msg = "request=005;usr=" + Config.User + ";op=" + op + ";bc=" + btnBC.Text + ";photo=" + CameraPhoto + ";json=" + json;
            msg = DataGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request03()
        {
            apiID = API_ID.O0503;

            request("03", btnBC.Text);
        }

        private void request04()
        {
            apiID = API_ID.O0504;

            request("04", btnBC.Text);
        }

        private void request05()
        {
            apiID = API_ID.O0505;

            request("05", btnBC.Text);
        }

        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.O0501:
                    {
                        string file = Config.getApiFile("005", "01");
                        from += "/005/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_XML;
                    }
                    break;

                default:
                    return;
            }
            new ConnThread(this).Download(from, to, false);
            wait();
        }

        //原formPO的接口
        private void request00402()
        {
            apiID = API_ID.O0402;
            string bc = btnBC.Text;
            string op = "02";
            string msg = "request=004;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = DataGuid02 + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request00403()
        {
            apiID = API_ID.O0403;
            string bc = btnBC.Text;
            string op = "03";
            string msg = "request=004;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = DataGuid03 + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        #endregion

        #region 实现ScanCallback接口
        public void scanCallback(string barCode)
        {
            switch (apiID)
            {
                case API_ID.O0501:
                    btnBC.Text = barCode;
                    if (!string.IsNullOrEmpty(barCode))
                    {
                        request01();
                    }
                    else
                    {
                        MessageBox.Show("查询单号不可为空!");
                    }
                    break;               
                default:
                    return;
            }
        }
        #endregion

        #region 实现ISerialize接口
        public void init(object[] param)
        {
        }

        public void Serialize(string file)
        {
            try
            {
                if (ds == null)
                {
                    return;
                }

                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
                data[(int)DATA_INDEX.BARCODE] = btnBC.Text;
                data[(int)DATA_INDEX.SKU] = tbSKU.Text;
                data[(int)DATA_INDEX.STATE] = tbState.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGENDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.ID] = tbManufacturerID.Text;
                data[(int)DATA_INDEX.NAME] = tbManufacturerName.Text;
                data[(int)DATA_INDEX.TYPE] = tbManufacturerType.Text;
                data[(int)DATA_INDEX.ROWFLAG] = rowFlag.ToString();
                data[(int)DATA_INDEX.DATAGUID] = DataGuid;
                data[(int)DATA_INDEX.DATAGUID02] = DataGuid02;
                data[(int)DATA_INDEX.DATAGUID03] = DataGuid03;
                data[(int)DATA_INDEX.DATAGUID05] = DataGuid05;
                data[(int)DATA_INDEX.BTNADD] = btnAdd.Enabled.ToString();
                data[(int)DATA_INDEX.BTN02] = btn02.Enabled.ToString();
                data[(int)DATA_INDEX.BTN03] = btn03.Enabled.ToString();
                data[(int)DATA_INDEX.BTN04] = btn04.Enabled.ToString();
                data[(int)DATA_INDEX.BTNSELECT] = btnSelect.Enabled.ToString();
                data[(int)DATA_INDEX.BTNSELECTTXT] = btnSelect.Text;
                data[(int)DATA_INDEX.BREQUEST] = bRequest.ToString();
                if (ds.Tables[tableIndex].DefaultView.Sort.IndexOf(" asc") > 0)
                {
                    data[(int)DATA_INDEX.SORTFLAG] = "asc";
                }
                else
                {
                    data[(int)DATA_INDEX.SORTFLAG] = "desc";
                }

                if (child == null)
                {
                    data[(int)DATA_INDEX.CHILD] = "";
                }
                else
                {
                    data[(int)DATA_INDEX.CHILD] = child.GetType().ToString();
                }

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
                    btnBC.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    tbSKU.Text = sc.Data[(int)DATA_INDEX.SKU];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tbState.Text = sc.Data[(int)DATA_INDEX.STATE];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGENDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    tbManufacturerID.Text = sc.Data[(int)DATA_INDEX.ID];
                    tbManufacturerName.Text = sc.Data[(int)DATA_INDEX.NAME];
                    tbManufacturerType.Text = sc.Data[(int)DATA_INDEX.TYPE];
                    rowFlag = int.Parse(sc.Data[(int)DATA_INDEX.ROWFLAG]);
                    btnAdd.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNADD]);
                    btn02.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN02]);
                    btn03.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN03]);
                    btn04.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN04]);
                    btnSelect.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNSELECT]);
                    btnSelect.Text = sc.Data[(int)DATA_INDEX.BTNSELECTTXT];
                    DataGuid=sc.Data[(int)DATA_INDEX.DATAGUID];
                    DataGuid02 = sc.Data[(int)DATA_INDEX.DATAGUID02];
                    DataGuid03 = sc.Data[(int)DATA_INDEX.DATAGUID03];
                    DataGuid05 = sc.Data[(int)DATA_INDEX.DATAGUID05];
                    bRequest=bool.Parse(sc.Data[(int)DATA_INDEX.BREQUEST]);
                    ds = sc.DS;
                    if (ds != null)
                    {
                        dtDiff = ds.Tables[ds.Tables.Count - 1];

                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();

                        UpdateColumn(false, true, false);

                        if (tableIndex >= 0)
                        {
                            if (colIndex >= 0)
                            {
                                DataTable dtHeader = (DataTable)dgHeader.DataSource;
                                DataColumn col = dtHeader.Columns[colIndex];
                                DataTable dt = ds.Tables[tableIndex];
                                if (sc.Data[(int)DATA_INDEX.SORTFLAG] == "asc")
                                {
                                    dt.DefaultView.Sort = col.ColumnName.ToString() + " asc";
                                }
                                else
                                {
                                    dt.DefaultView.Sort = col.ColumnName.ToString() + " desc";
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sc.Data[(int)DATA_INDEX.CHILD]))
                    {
                        showChild(true);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region TextBox获取焦点后启动定时器实现全选

        private System.Windows.Forms.Timer tSKU = null;
        private void tbSKU_GotFocus(object sender, EventArgs e)
        {
            if (tSKU == null)
            {
                tSKU = new System.Windows.Forms.Timer();
                tSKU.Tick += new EventHandler(tbSKU_SelectAll);
                tSKU.Interval = 100;
            }
            tSKU.Enabled = true;
        }

        public void tbSKU_SelectAll(Object myObject, EventArgs myEventArgs)
        {
            if (tSKU.Enabled)
            {
                tSKU.Enabled = false;
                tbSKU.SelectAll();
            }
        }

        private void tbSKU_LostFocus(object sender, EventArgs e)
        {
            tSKU.Enabled = false;
            tbSKU.Select(0, 0);
        }

        private System.Windows.Forms.Timer tCount = null;
        private void tbCount_GotFocus(object sender, EventArgs e)
        {
            if (tCount == null)
            {
                tCount = new System.Windows.Forms.Timer();
                tCount.Tick += new EventHandler(tbCount_SelectAll);
                tCount.Interval = 100;
            }
            tCount.Enabled = true;
        }

        public void tbCount_SelectAll(Object myObject, EventArgs myEventArgs)
        {
            if (tCount.Enabled)
            {
                tCount.Enabled = false;
                tbCount.SelectAll();
            }
        }

        private void tbCount_LostFocus(object sender, EventArgs e)
        {
            tCount.Enabled = false;
            tbCount.Select(0, 0);
        }

        #endregion

        #region delegates 传参

        public delegate void PageTitle(Form form);

        #endregion
    }    
}