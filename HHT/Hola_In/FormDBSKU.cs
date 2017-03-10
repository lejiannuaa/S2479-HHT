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
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using HolaCore;
using System.Threading;
using System.Runtime.InteropServices;

namespace Hola_In
{
    public partial class FormDBSKU : Form, ConnCallback, ScanCallback, ISerializable
    {
     
        private const string guid = "FB5E3220-071E-2F10-EA4D-1CF1A989EC96";

        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
      
        //短收明细表
        private DataTable dtDiff = null;
        //默认排序表
        private DataTable dtOrder = null;

        private Form child = null;
        FormDownload formDownload = null;
        private FormScan formScan = null;
        //指示当前正在发送网络请求
        private bool busy = false;

        private SerializeClass sc = null;
        private int TaskBarHeight = 0;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            STATE,
            SKU,
            COUNT,
            TABLEINDEX,
            PAGEINDEX,
            ROWINDEX,
            COLINDEX,
            SORTFLAG,
            ROWFLAG,
            CHILD,
            BTN00103,
            BTN00104,
            BTN00106,
            BTN02,
            BTN03,
            BTN04,
            BTNADD,
            DATAGUID,
            DataGuid03,
            DataGuid04,
            DATAGUID05,
            BREQUEST,
            DATAMAX
        }
        #endregion
        private DataSet DsReason=null;
        private DataSet ds = null;
        //发送数据标识唯一性
        private string DataGuid = "";
        private string DataGuid03 = "";
        private string DataGuid04 = "";
        private string xmlFile = null;
        private string DataGuid05 = "";
        private int TABLE_ROWMAX = 5;
        private int tableIndex =-1;
        public int pageIndex = 1;
        private int colIndex = -1;
        private int rowFlag = -1;
        private string lastSKU = null;
        Regex regi = new Regex("^[0-9]*[1-9][0-9]*$");
        Regex reg = new Regex(@"^\d+$");
        private bool bRequest = false;
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            O0201,
            O0202,
            O0203,
            O0204,
            O0205,
            O0103,
            O0104,
            O0106,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormDBSKU()
        {
            InitializeComponent();
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            DataGuid03 = Guid.NewGuid().ToString().Replace("-", "");
            DataGuid04 = Guid.NewGuid().ToString().Replace("-", "");
            DataGuid05 = Guid.NewGuid().ToString().Replace("-", "");

            doLayout();
            QueryPerformanceFrequency(ref freq);
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
                        }
                        else if (ds.Tables[i].TableName.Equals("info"))
                        {
                            string state = ds.Tables[i].Rows[0]["state"].ToString();
                            switch (state)
                            {
                                case "0":
                                    tbState.Text = "待收货";
                                    setButton(true, true, true,false,false, false);
                                    btnAdd.Enabled = true;
                                    break;

                                case "1":
                                    tbState.Text = "收货中";
                                    setButton(false, false, false,false,false, true);
                                    btnAdd.Enabled = false;
                                    break;

                                case "2":
                                    tbState.Text = "已收货";
                                    setButton(false, false, false,true,true, false);
                                    btnAdd.Enabled = false;
                                    break;

                                default:
                                    tbState.Text = state;
                                    setButton(false, false, false, false, false, false);
                                    btnAdd.Enabled = false;
                                    break;
                            }
                        }
                    }

                    if (!bNoData)
                    {
                        DataGuid = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid03 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid04 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid05 = Guid.NewGuid().ToString().Replace("-", "");
                        pageIndex = 1;
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();

                        UpdateColumn(true, true, false);
                        
                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                        MessageBox.Show("无数据！");
                        setButton(false, false, false, false, false, false);
                        btnAdd.Enabled = false;
                    }
                }
                else
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
                    MessageBox.Show("请求文件不存在,请重新请求!");
                    setButton(false, false, false, false, false, false);
                    btnAdd.Enabled = false;
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
                
                if (bCustom && name[i].Equals("HHTRQT"))
                {
                    style.Owner = dgTable;
                    if (tbState.Text.Equals("待收货"))
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
                else if (name[i].Equals("HHTSKU"))
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
                string[] colName = new string[] { "ID", "HHTSKU", "HHTDSC", "HHTPQT", "HHTRQT", "差异" }; 
                string[] colValue = new string[] { "ID", "SKU", "品名", "出货", "完好", "差异" };
                //int[] colWidth = new int[] { 10, 25, 25, 13, 12, 12 };
                int[] colWidth = new int[] { 10, 19, 31, 13, 12, 12 };

                //从XML首次加载需初始化列
                if (bInitColumn)
                {
                    DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                    DataRowCollection rows = ds.Tables[tableIndex].Rows;
                    cols.Add("ID", System.Type.GetType("System.UInt32"));
                    cols.Add("差异");
                    cols.Add("维护");
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["ID"] = i + 1;
                        rows[i]["差异"] = int.Parse(rows[i]["HHTPQT"].ToString())-int.Parse(rows[i]["HHTRQT"].ToString());
                        rows[i]["维护"] = "0";
                    }

                    if (dtDiff != null)
                    {
                        dtDiff.Dispose();
                    }
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
                for (int i = 0; i < colValue.Length; i++)
                {
                    row[i] = colValue[i];
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
                string total = dtResult.Rows[i]["HHTPQT"].ToString();
                string ok = dtResult.Rows[i]["HHTRQT"].ToString();
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
            //加skuname  chsl
            //dtOK.Columns.Add("skuname");
            //dtOK.Columns.Add("chsl");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rowNew = dtOK.NewRow();

                rowNew["SKU"] = dt.Rows[i]["HHTSKU"];
                rowNew["whsl"] = dt.Rows[i]["HHTRQT"];
                //加skuname  chsl
                //rowNew["skuname"] = dt.Rows[i]["HHTRQT"];
                //rowNew["chsl"] = dt.Rows[i]["HHTRQT"];
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

        //设置按钮可用状态
        private void setButton(bool b1, bool b2,bool b3,bool b4,bool b5, bool b6)
        {
            btn00103.Enabled = b1;
            btn00104.Enabled = b2;
            btn02.Enabled = b3;
            btn03.Enabled = b4;
            btn04.Enabled = b5;
            btn00106.Enabled = b6;
            //Camera.Enabled=

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
                    if (styles[i].MappingName.Equals("HHTRQT"))
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

        private bool CheckGridCellData(int row, string ss)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = ss;
                string total = dt.Rows[dgTable.CurrentRowIndex]["HHTPQT"].ToString();
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
                                updateData(id, "HHTRQT", ok, false);
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
                                updateData(id, "HHTRQT", ok, false);
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

        private void FormDBSKU_KeyDown(object sender, KeyEventArgs e)
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
                                if (styles[i].MappingName.Equals("HHTRQT"))
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
                                if (styles[i].MappingName.Equals("HHTRQT"))
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
                                if (SKUADD.Equals(dt.Rows[i]["HHTSKU"].ToString()))
                                {
                                    string total = dt.Rows[i]["HHTPQT"].ToString();
                                    string ok = dt.Rows[i]["HHTRQT"].ToString();
                                    if (UInt64.Parse(total) < UInt64.Parse(ok) + UInt64.Parse(tbCount.Text))
                                    {
                                        MessageBox.Show("完好数量不可大于出货数量！(2)");
                                        return;
                                    }
                                    else
                                    {
                                        bAdded = true;
                                        dt.Rows[i]["HHTRQT"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                        //刷新排序表
                                        for (int j = 0; j < dtOrder.Rows.Count; j++)
                                        {
                                            if (SKUADD.Equals(dtOrder.Rows[j]["HHTSKU"].ToString()))
                                            {
                                                dtOrder.Rows[j]["HHTRQT"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
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
                                    string total = dt.Rows[i]["HHTPQT"].ToString();
                                    string ok = dt.Rows[i]["HHTRQT"].ToString();
                                    if (UInt64.Parse(total) < UInt64.Parse(ok) + UInt64.Parse(tbCount.Text))
                                    {
                                        MessageBox.Show("完好数量不可大于出货数量！(3)");
                                        return;
                                    }
                                    else
                                    {
                                        bAdded = true;
                                        dt.Rows[i]["HHTRQT"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
                                        //刷新排序表
                                        for (int j = 0; j < dtOrder.Rows.Count; j++)
                                        {
                                            if (tbSKU.Text.Equals(dtOrder.Rows[j]["IPC"].ToString()))
                                            {
                                                dtOrder.Rows[j]["HHTRQT"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
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
                                        string total = dt.Rows[i]["HHTPQT"].ToString();
                                        string ok = dt.Rows[i]["HHTRQT"].ToString();
                                        if (UInt64.Parse(total) < UInt64.Parse(ok) + UInt64.Parse(tbCount.Text))
                                        {
                                            MessageBox.Show("完好数量不可大于出货数量！(4)");
                                            return;
                                        }
                                        else
                                        {
                                            bAdded = true;
                                            dt.Rows[i]["HHTRQT"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
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
                                                        dtOrder.Rows[k]["HHTRQT"] = (UInt64.Parse(ok) + UInt64.Parse(tbCount.Text)).ToString();
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
                string total = dt.Rows[lastRowIndex]["HHTPQT"].ToString();
                string ok = dt.Rows[lastRowIndex]["HHTRQT"].ToString();
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
                                updateData(id, "HHTRQT", ok, bReorder);
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
                                updateData(id, "HHTRQT", ok, bReorder);
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
                dt.Rows[lastRowIndex]["HHTRQT"] = int.Parse(dt.Rows[lastRowIndex]["HHTPQT"].ToString()) - int.Parse(dt.Rows[lastRowIndex]["差异"].ToString());
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

                tbSKU.Text = dt.Rows[dgTable.CurrentRowIndex]["HHTSKU"].ToString();
                tbCount.Text = dt.Rows[dgTable.CurrentRowIndex]["HHTRQT"].ToString();

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
                //加参数
                child = new FormReason(this.Text.ToString());
                string state = "";
                if (ds.Tables["info"] != null && ds.Tables["info"].Rows.Count>0)
                {
                    state = ds.Tables["info"].Rows[0]["state"].ToString();
                }
                if (!bDeserialize)
                {
                    string total = row["HHTPQT"].ToString();
                    string ok = row["HHTRQT"].ToString();
                    string diff = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                    ((FormReason)child).GetDtReason += new FormReason.getDtReason(getdtReason);
                    ((ISerializable)child).init(new object[] { btnBC.Text, row["HHTSKU"].ToString(), row["HHTDSC"].ToString(), diff, dtDiff, DsReason, bRequest.ToString(), state });
                }
                else
                {
                    string total = row["HHTPQT"].ToString();
                    string ok = row["HHTRQT"].ToString();
                    string diff = (UInt64.Parse(total) - UInt64.Parse(ok)).ToString();
                    ((ISerializable)child).init(new object[] { btnBC.Text, row["HHTSKU"].ToString(), row["HHTDSC"].ToString(), diff, dtDiff, state });
                    ((ISerializable)child).Deserialize(null);
                }

                if (DialogResult.OK == child.ShowDialog())
                {
                    try
                    {
                        string strOK = row["HHTRQT"].ToString();
                        string strTotal = row["HHTPQT"].ToString();
                        string strDiff = (UInt64.Parse(strTotal) - UInt64.Parse(strOK)).ToString();
                        updateData(row["ID"].ToString(), "维护", strDiff, false);


                        //MessageBox.Show(JSONClass.DataTableToString(new DataTable[]{dtDiff}));
						//原因数据
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

        private void menuDetail_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                showChild(false);
            }
        }

        private void tbCount_TextChanged(object sender, EventArgs e)
        {
            if(tbCount.Text.Length>=10)
            {
                MessageBox.Show("输入字符串过长!");
                tbCount.Text = tbCount.Text.Substring(0, 9);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                addCount();
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
                    tbSKU.Text = dt.Rows[dgTable.CurrentRowIndex]["HHTSKU"].ToString();
                    tbCount.Text = dt.Rows[dgTable.CurrentRowIndex]["HHTRQT"].ToString();

                    for (int i = 0; i < dtOrder.Rows.Count; i++)
                    {
                        string total = dtOrder.Rows[i]["HHTPQT"].ToString();
                        string ok = dtOrder.Rows[i]["HHTRQT"].ToString();
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
                if (!string.IsNullOrEmpty(btnBC.Text))
                {
                    request03();
                }
                else
                {
                    MessageBox.Show("请检查箱号!");
                }
            }

        }

        private void btn04_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(btnBC.Text))
                {
                    request04();
                }
                else
                {
                    MessageBox.Show("请检查箱号!");
                }
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

        //原FormDB事件

        private void btnBC_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (formScan == null)
                {
                    formScan = new FormScan();
                }
                formScan.init(this, btnBC.Text);
                FullscreenClass.ShowSIP(true);
                formScan.ShowDialog();
                FullscreenClass.ShowSIP(false);
                formScan.Dispose();
                formScan = null;
            }
        }

        private void btn00106_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(btnBC.Text))
                {
                    request00106();
                }
            }
        }

        private void btn00103_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(btnBC.Text))
                {
                    if (MessageBox.Show("无该箱，您确定吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        request00103();
                    }
                }
            }
        }

        private void btn00104_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(btnBC.Text))
                {
                    if (MessageBox.Show("整箱破损，您确定吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        request00104();
                    }
                }
            }
        }
          
     
        private string photoName1="";
        private string photoName2="";
        private string photoName3="";
        private bool isPO = false;
        //组合照片名
        private string CameraPhoto="";
        //退出照相页面取回照片名
        void getPhotoName(string photo1, string photo2, string photo3)
        {
            if (photo1!=null&&photo1!="")
            {
                string a = photo1.Substring(photo1.Length - 40, 40);
                photoName1 = a;
            }

            if (photo2 != null && photo2 != "")
            {
                string b = photo2.Substring(photo2.Length - 40, 40);
                photoName2 = b;
            }
            if (photo3 != null && photo3 != "")
            {
                string c = photo3.Substring(photo3.Length - 40, 40);
                photoName3 = c;
            }
   
            CameraPhoto=photoName1+","+photoName2+","+photoName3;
        }

        private void Camera_Click(object sender, EventArgs e)
        {
            FormCamera formCamera = new FormCamera(photoName1,photoName2,photoName3,isPO);
            //formCamera.MyPhotoEvent += new FormCamera.MyPhotoDelegate(getPhotoName);
            PageTitle pageTitle = new PageTitle(formCamera.getTitle);
            pageTitle(this);
            formCamera.ShowDialog();
            CameraPhoto = formCamera.photoName();
        }


        #endregion

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.O0201;
            ClearGrid();
            string op = "01";
            string bc = btnBC.Text.ToString();
            string msg = "request=002;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);

            wait();
        }

        private void request02()
        {
            apiID = API_ID.O0202;

            DataTable[] dtSubmit = new DataTable[] { getDTOK(), dtDiff };

            string op = "02";
            string bc = btnBC.Text;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "002", op);

            string msg = "request=002;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper() + ";photo=" + CameraPhoto + ";json=" + json;
            msg = DataGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request03()
        {  
            apiID = API_ID.O0203;
            string op = "03";
            string bc = btnBC.Text.ToString();
            string msg = "request=002;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);

            wait();
        }

        private void request04()
        {
            apiID = API_ID.O0204;
            string op = "04";
            string bc = btnBC.Text.ToString();
            string msg = "request=002;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);

            wait();
        }

        private void request05()
        {
            apiID = API_ID.O0205;
            string op = "05";
            string bc = btnBC.Text.ToString();
            string msg = "request=002;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = DataGuid05 + msg;
            new ConnThread(this).Send(msg);
            wait();

        }

        //原formDB的接口
        private void request00106()
        {
            apiID = API_ID.O0106;
            string op = "06";
            string bc = btnBC.Text;
            string msg = "request=001;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            
            wait();
        }

        private void request00103()
        {
            apiID = API_ID.O0103;
            string bc = btnBC.Text;
            string op = "03";
            string msg = "request=001;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = DataGuid03 + msg;
            new ConnThread(this).Send(msg);

            wait();
        }

        private void request00104()
        {
            apiID = API_ID.O0104;
            string bc = btnBC.Text;
            string op = "04";
            string msg = "request=001;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            msg = DataGuid04 + msg;
            new ConnThread(this).Send(msg);
       
            wait();
        }

        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.O0201:
                    {
                        string file = Config.getApiFile("002", "01");
                        from += "/002/" + file;
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
        #endregion

        #region 实现ISerialize接口
        public void init(object[] param)
        {
        }

        public void Serialize(string file)
        {
            try
            {
                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
                data[(int)DATA_INDEX.BARCODE] = btnBC.Text;
                data[(int)DATA_INDEX.STATE] = tbState.Text;
                data[(int)DATA_INDEX.SKU] = tbSKU.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.ROWFLAG] = rowFlag.ToString();
                data[(int)DATA_INDEX.BTNADD] = btnAdd.Enabled.ToString();
                data[(int)DATA_INDEX.BTN02] = btn02.Enabled.ToString();
                data[(int)DATA_INDEX.BTN03] = btn03.Enabled.ToString();
                data[(int)DATA_INDEX.BTN04] = btn04.Enabled.ToString();
                data[(int)DATA_INDEX.BTN00106] = btn00106.Enabled.ToString();
                data[(int)DATA_INDEX.BTN00103] = btn00103.Enabled.ToString();
                data[(int)DATA_INDEX.BTN00104] = btn00104.Enabled.ToString();
                data[(int)DATA_INDEX.DATAGUID] = DataGuid;
                data[(int)DATA_INDEX.DataGuid03] = DataGuid03;
                data[(int)DATA_INDEX.DataGuid04] = DataGuid04;
                data[(int)DATA_INDEX.DATAGUID05] = DataGuid05;
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
                    tbState.Text = sc.Data[(int)DATA_INDEX.STATE];
                    tbSKU.Text = sc.Data[(int)DATA_INDEX.SKU];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    rowFlag = int.Parse(sc.Data[(int)DATA_INDEX.ROWFLAG]);
                    btnAdd.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNADD]);
                    btn02.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN02]);
                    btn03.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN03]);
                    btn04.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN04]);
                    btn00106.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN00106]);
                    btn00103.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN00103]);
                    btn00104.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN00104]);
                    DataGuid = sc.Data[(int)DATA_INDEX.DATAGUID];
                    DataGuid03 = sc.Data[(int)DATA_INDEX.DataGuid03];
                    DataGuid04 = sc.Data[(int)DATA_INDEX.DataGuid04];
                    DataGuid05 = sc.Data[(int)DATA_INDEX.DATAGUID05];
                    bRequest = bool.Parse(sc.Data[(int)DATA_INDEX.BREQUEST]);
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
                    case API_ID.O0201:
                        if (result == ConnThread.RESULT_OK)
                        {
                            requestXML();
                        }
                        else if (result == ConnThread.RESULT_WARNING)
                        {
                            MessageBox.Show(data + "箱号:" + btnBC.Text);
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
                            MessageBox.Show(data+"箱号:"+btnBC.Text);
                            setButton(false, false, false, false, false, false);
                            btnAdd.Enabled = false;
                        }
                        break;

                    case API_ID.O0202:
                        if (result == ConnThread.RESULT_OK)
                        {
                            tbState.Text = "已收货";
                            setButton(false, false, false, true, true, false);
                            if (ds.Tables["info"] != null && ds.Tables["info"].Rows.Count > 0)
                            {
                                ds.Tables["info"].Rows[0]["state"] = "2";
                            }
                            btnAdd.Enabled = false;
                            LockGrid();
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
                            Serialize(guid);
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.O0203:
                    case API_ID.O0204:
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

                    case API_ID.O0205:
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
                                if (DialogResult.OK == MessageBox.Show(data))
                                {
                                    ;
                                }
                            }
                            close();
                        }
                        break;
              
                    case API_ID.O0103:
                    case API_ID.O0104:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                           
                         
                            tbState.Text = "已收货";
                            setButton(false, false, false, true, true, false);
                            btnAdd.Enabled = false;
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

                    case API_ID.O0106:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                            tbState.Text = "待收货";
                            setButton(true, true, true, false, false, false);
                            btnAdd.Enabled = true;
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

        #region 实现ScanCallback接口
        public void scanCallback(string barCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barCode))
                {
                    this.btnBC.Text = barCode;
                    request01();
                }
                else
                {
                    MessageBox.Show("箱号不可为空!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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