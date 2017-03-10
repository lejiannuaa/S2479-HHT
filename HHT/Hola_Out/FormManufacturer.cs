using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using HolaCore;
using System.Runtime.InteropServices;
namespace Hola_Out
{
    public partial class FormManufacturer : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "E13A0757-DCB1-E799-222F-CAA055D3D60D";

        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        //指示当前正在发送网络请求
        private bool busy = false;
        private bool bNew = false;
        private const string OGuid = "00000000000000000000000000000000";

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            PRICEAVAIL,
            MANUFACTURERID,
            MANUFACTURERNAME,
            SKU,
            SKUNAME,
            COUNTAVAIL,
            PRICE,
            COUNT,
            REASONINDEX,
            TABLEINDEX,
            TABLEINFO,
            PAGEINDEX,
            NPRICESKU,
            BNEW,
            SORTFLAG,
            COLINDEX,
            LASTSKU,
            LASTBOX,
            BTNADD,
            BTNDELE,
            BTN04,
            BTN05,
            DATAGUID,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 6;
        //上次请求过的SKU
        private string lastSKU = "";
        private FormDownload formDownload = null;
        private FormScan formScan = null;
        private string bc = "";
        private string xmlFile = null;
        //上次请求过的厂商
        private string oldManu = null;
        private SerializeClass sc = null;

        public DataSet ds = null;
        //SKU的价格
        private decimal nPriceSKU =0.0M;
        private bool OSKU = false;
        private bool DSKU = false;
        List<string> reason = new List<string>();
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int tableInfo = 0;
        private int colIndex = -1;
        private bool bCheckAll = false;
        private string colModify = "";
        private bool bBox = true;
        Regex reg = new Regex(@"^\d+$");
        Regex regF=new Regex("^[-+]?[0-9]*\\.?[0-9]+$");
        private string LastBox = "";
        //发送数据标识唯一性
        private string DataGuid = "";
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
            l0201,
            l0101,
            l0102,
            l0103,
            l0104,
            l0105,
            DOWNLOAD_01,
            DOWNLOAD_02,
            DOWNLOAD_03,
            DOWNLOAD_102,
        }

        private API_ID apiID = API_ID.NONE;
        #endregion
        private int TaskBarHeight = 0;
        public FormManufacturer()
        {
            InitializeComponent();
            DataGuid = Guid.NewGuid().ToString().Replace("-","");
            doLayout();
            QueryPerformanceFrequency(ref freq);
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                if (dstWidth >= 320)
                {
                    dgTable.Height = 150;
                    TABLE_ROWMAX = 6;
                }
                else
                {
                    dgTable.Height = 60;
                    TABLE_ROWMAX = 3;
                }
                btnReturn.Top = dstHeight - btnReturn.Height;
                btn04.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                NextPage.Top = dgTable.Bottom + (pbBar.Top-dgTable.Bottom-NextPage.Height)/2;
                PrePage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                Page.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        #region XML数据加载
        private void showXML()
        {
            switch (apiID)
            {
                case API_ID.DOWNLOAD_01:
                    break;

                case API_ID.DOWNLOAD_02:
                    break;

                case API_ID.DOWNLOAD_03:
                    break;

                case API_ID.DOWNLOAD_102:
                    break;

                default:
                    return;
            }

            LoadData();
        }

        private void addTable(DataTable dt, string name)
        {
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals(name))
                        {
                            ds.Tables.RemoveAt(i);
                            break;
                        }
                    }
                }


                DataTable dtNew = dt.Clone();
                dtNew.TableName = name;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dtNew.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row[j] = dt.Rows[i][j];
                    }
                    dtNew.Rows.Add(row);
                }
                if (ds != null)
                {
                    ds.Tables.Add(dtNew);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool Load01(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {
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
                    if (ds.Tables[i].TableName.Equals("info"))
                    {
                        tableInfo = i;
                        bBox = false;
                    }
                }

                if (!bBox)
                {

                    if (!bNoData)
                    {
                        DataRowCollection rows = ds.Tables[tableIndex].Rows;
                        if (ds.Tables[tableIndex].Rows.Count > 0)
                        {
                            bCheckAll = false;
                            if (string.IsNullOrEmpty(LastBox) || !LastBox.Equals(btn01.Text))
                            {
                                DataGuid = Guid.NewGuid().ToString().Replace("-", "");
                            }
                            btn02.Text = rows[0]["厂商代码"].ToString().Equals("null") ? "" : rows[0]["厂商代码"].ToString();
                            tbManufacturerName.Text = rows[0]["厂商名称"].ToString().Equals("null") ? "" : rows[0]["厂商名称"].ToString();
                            tbPriceAvail.Text = rows[0]["可退金额"].ToString().Equals("null") ? "" : rows[0]["可退金额"].ToString();
                            tbPrice.Text = rows[0]["已退金额"].ToString().Equals("null") ? "" : rows[0]["已退金额"].ToString();
                            pageIndex = 1;
                            int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                            if (pageCount <= 0)
                            {
                                pageCount = 1;
                            }
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                            UpdateColumn(false);

                            setButton(true);
                        }
                        else
                        {
                            bNoData = true;
                            setButton(false);
                            btn05.Enabled = false;
                        }
                    }
                    else
                    {
                        pageIndex = 1;
                        ds = new DataSet();
                        ClearBox(true);
                        CreateDS();
                        setButton(false);
                        btn05.Enabled = false;
                        UpdateColumn(false);
                    }
                }
                else
                {
                    pageIndex = 1;
                    ds = new DataSet();
                    ClearBox(true);
                    CreateDS();
                    setButton(false);
                    btn05.Enabled = false;
                    UpdateColumn(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LastBox = btn01.Text;
            return bNoData;
        }

        private bool Load02(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {
                DataSet dsManufacturer = new DataSet();
                dsManufacturer.ReadXml(reader);

                for (int i = 0; i < dsManufacturer.Tables.Count; i++)
                {
                    DataTable dt = dsManufacturer.Tables[i];
                    if (dt.TableName == "info")
                    {
                        bNoData = false;

                        tbManufacturerName.Text = dt.Rows[0][0].ToString().Equals("null") ? "" : dt.Rows[0][0].ToString();
                        tbPriceAvail.Text = dt.Rows[0][2].ToString().Equals("null") ? "" : dt.Rows[0][1].ToString();
                        break;
                    }
                }
                if (bNoData)
                {
                    ds = new DataSet();
                    ClearBox(true);
                    CreateDS();
                    deleteAll();
                }
                else
                {
                    if (!btn01.Enabled)
                    {
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = true;
                        btn04.Enabled = false;
                        btn05.Enabled = false;
                    }
                }

                dsManufacturer.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return bNoData;
        }

        private bool Load03(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {
                DataSet dsSKU = new DataSet();
                dsSKU.ReadXml(reader);

                for (int i = 0; i < dsSKU.Tables.Count; i++)
                {
                    DataTable dt = dsSKU.Tables[i];
                    if (dt.TableName == "info")
                    {
                        bNoData = false;

                        tbSKUName.Text = dt.Rows[0]["品名"].ToString().Equals("null") ? "" : dt.Rows[0]["品名"].ToString();
                        tbCountAvail.Text = dt.Rows[0]["库存"].ToString().Equals("null") ? "" : dt.Rows[0]["库存"].ToString();

                        if (regF.IsMatch(dt.Rows[0]["成本"].ToString()))
                        {
                            nPriceSKU = decimal.Parse(dt.Rows[0]["成本"].ToString());
                        }
                        else
                        {
                            nPriceSKU = 0.0M;
                        }

                        if (btn03.Text.Length > 9 || btn03.Text.Length == 8)
                        {
                            btn03.Text = dt.Rows[0]["HHTSKU"].ToString();
                        }
                        if (string.IsNullOrEmpty(lastSKU))
                        {
                            lastSKU = btn03.Text;
                            tbCount.Text = "1";
                        }
                        else if (lastSKU == btn03.Text)
                        {
                            if (reg.IsMatch(tbCount.Text))
                            {
                                tbCount.Text = (UInt64.Parse(tbCount.Text) + 1).ToString();
                            }
                        }
                        else
                        {
                            lastSKU = btn03.Text;
                            tbCount.Text = "1";
                        }
                        break;
                    }
                }
                if (bNoData)
                {
                    tbSKUName.Text = "";
                    tbCountAvail.Text = "";
                    tbCount.Text = "";
                    nPriceSKU = 0.0M;
                    lastSKU = "";
                }

                dsSKU.Dispose();

            }
            catch (Exception ex)
            {
                tbSKUName.Text = "";
                tbCountAvail.Text = "";
                tbCount.Text = "";
                nPriceSKU = 0.0M;
                lastSKU = "";
                MessageBox.Show(ex.Message);
            }
            return bNoData;
        }

        private bool Load102(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {

                DataSet dsReason = new DataSet();
                dsReason.ReadXml(reader);

                for (int i = 0; i < dsReason.Tables.Count; i++)
                {
                    DataTable dt = dsReason.Tables[i];
                    if (dt.TableName == "info" && dt.Rows.Count > 0)
                    {
                        bNoData = false;
                        addTable(dt, "Reason");
                        reason.Clear();
                        cbReason.Items.Clear();
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            reason.Add(dt.Rows[j][0].ToString());
                            cbReason.Items.Add(dt.Rows[j][1].ToString());
                        }
                        cbReason.Invalidate();
                        cbReason.Enabled = true;
                        btnReason.Visible = false;
                        cbReason.Focus();
                        break;
                    }
                }

                dsReason.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return bNoData;
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
                }
                switch (apiID)
                {
                    case API_ID.DOWNLOAD_01:
                        if (File.Exists(xmlFile))
                        {
                            bNoData = Load01(reader);

                            if (!bBox && bNoData)
                            {
                                bBox = true;
                                MessageBox.Show("无数据！");
                            }
                            else if (bBox && bNoData)
                            {
                                btn01.Text = "";
                                MessageBox.Show("箱号不存在!");
                            }
                            else
                            {
                                bBox = true;
                            }
                        }
                        else
                        {
                            pageIndex = 1;
                            ClearBox(true);
                            ds = new DataSet();
                            CreateDS();
                            UpdateColumn(false);
                            MessageBox.Show("请求文件不存在，请重新请求!");
                        }
                        break;

                    case API_ID.DOWNLOAD_02:
                        if (File.Exists(xmlFile))
                        {
                            bNoData = Load02(reader);
                            if (bNoData)
                            {
                                MessageBox.Show("无数据！");
                            }
                        }
                        else
                        {
                            tbManufacturerName.Text = "";
                            tbPriceAvail.Text = "";
                            MessageBox.Show("请求文件不存在，请重新请求!");
                        }
                        break;

                    case API_ID.DOWNLOAD_03:
                        if (File.Exists(xmlFile))
                        {
                            bNoData = Load03(reader);
                            if (bNoData)
                            {
                                MessageBox.Show("无数据！");
                            }
                        }
                        else
                        {
                            tbSKUName.Text = "";
                            tbCountAvail.Text = "";
                            nPriceSKU = 0.0M;
                            lastSKU = "";
                            MessageBox.Show("请求文件不存在，请重新请求!");
                        }
                        break;

                    case API_ID.DOWNLOAD_102:
                        if (File.Exists(xmlFile))
                        {
                            bNoData = Load102(reader);
                            if (bNoData)
                            {
                                MessageBox.Show("无数据！");
                            }
                        }
                        else
                        {
                            MessageBox.Show("请求文件不存在，请重新请求!");
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
                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);
                if (reader != null)
                    reader.Close();
                doc = null;
                File.Delete(xmlFile);
                GC.Collect();
                Serialize(guid);
                QueryPerformanceCounter(ref Stop);
                string str1 = "Serialize ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str1, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
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
                DataGridCustomColumnBase style = null;
                if (name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                }
                else if (name[i].Equals("门店申请数量"))
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
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
                }
                else
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = true;
                }
                style.MappingName = name[i];
                style.Width = (int)(dstWidth * width[i] / 100);
                style.AlternatingBackColor = SystemColors.ControlDark;
                ts.GridColumnStyles.Add(style);
            }

            return ts;
        }

        private void UpdateColumn(bool bDeserialize)
        {
            try
            {
                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);

                string[] colName = new string[] { "ID", "SKU", "品名", "门店申请数量", "退货描述", "CheckBox" };
                string[] colValue = new string[] { "ID", "SKU", "品名", "数量", "原因", "false" };
                //int[] colWidth = new int[] { 10, 25, 20, 15, 20, 7 };
                int[] colWidth = new int[] { 10, 19, 26, 15, 20, 7 };
                if (!bDeserialize)
                {
                    DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                    DataRowCollection rows = ds.Tables[tableIndex].Rows;
                    if (!cols.Contains("ID"))
                    {
                        cols.Add("ID", System.Type.GetType("System.UInt32"));
                    }
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["ID"] = i + 1;
                    }
                    if (!cols.Contains("CheckBox"))
                    {
                        cols.Add("CheckBox");
                        for (int i = 0; i < rows.Count; i++)
                        {
                            rows[i]["CheckBox"] = "False";
                        }
                    }
                }

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();

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
                //dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, true));
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

        private void UpdateRow()
        {
            DataGridCustomCheckBoxColumn colCheckBox = null;
            DataGridCustomTextBoxColumn colTextBox = null;
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
            DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
            DataTable dtResult = new DataTable();
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("CheckBox"))
                {
                    colCheckBox = (DataGridCustomCheckBoxColumn)styles[i];
                    dtResult.Columns[i].DataType = typeof(Boolean);
                }
                if (styles[i].MappingName.Equals("门店申请数量"))
                {
                    colTextBox = (DataGridCustomTextBoxColumn)styles[i];
                }
            }
            dtResult.TableName = dt.TableName;

            int from = (pageIndex - 1) * TABLE_ROWMAX;
            int to = from + TABLE_ROWMAX;
            if (to > dt.Rows.Count)
            {
                to = dt.Rows.Count;
            }

            Array Reason = reason.ToArray();
            for (int i = from; i < to; i++)
            {
                DataRow rowNew = dtResult.NewRow();

                for (int j = 0; j < styles.Count; j++)
                {
                    string name = styles[j].MappingName;
                    if (name.Equals("CheckBox"))
                    {
                        rowNew[name] = dt.Rows[i][name].Equals("True") ? true : false;
                    }
                    else
                    {
                        rowNew[name] = dt.Rows[i][name];
                    }
                }

                dtResult.Rows.Add(rowNew);
            }

            dgTable.DataSource = dtResult;

            if (colCheckBox != null && to > 0)
            {
                CheckBox box = (CheckBox)colCheckBox.HostedControl;
                box.CheckStateChanged += new EventHandler(dgTable_CheckStateChanged);
            }
            if (colTextBox != null && to > 0)
            {
                TextBox Tbox = (TextBox)colTextBox.HostedControl;
                Tbox.LostFocus+=new EventHandler(Tbox_LostFocus);
            }
        }

        private void updateData(string key, string value)
        {
            try
            {
                DataTable dt = ds.Tables[tableIndex];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (key.Equals(dt.Rows[i]["ID"].ToString()))
                    {
                        if (!value.Equals(dt.Rows[i][colModify]))
                        {
                            if (colModify.Equals("CheckBox"))
                            {
                                dt.Rows[i][colModify] = bool.Parse(value);
                            }
                            else
                            {
                                dt.Rows[i][colModify] = value;
                            }
                            UpdateColumn(true);
                            Serialize(guid);
                        }
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
            string[] colName = { "sku", "HHTQTY", "reason", "vendorcode" };
            DataTable dt = ds.Tables[tableIndex];
            DataTable dtOK = new DataTable();
            dtOK.TableName = "info";
            for (int i = 0; i < colName.Length; i++)
            {
                dtOK.Columns.Add(colName[i]);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rowNew = dtOK.NewRow();

                rowNew["sku"] = AddZero(dt.Rows[i]["SKU"].ToString());
                rowNew["HHTQTY"] = dt.Rows[i]["门店申请数量"];
                rowNew["reason"] = dt.Rows[i]["退货原因"];
                rowNew["vendorcode"] = dt.Rows[i]["厂商代码"];
                dtOK.Rows.Add(rowNew);
            }

            return dtOK;
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
        private void setButton(bool flag)
        {
            btnAdd.Enabled = flag;
            btnDelete.Enabled = flag;
            btn04.Enabled = flag;
            btn05.Enabled = !btn04.Enabled;
        }
        //创建包含列空数据集
        private void CreateDS()
        {
            string[] colName = { "SKU", "厂商代码", "厂商类型", "门店申请数量", "退货原因", "退货描述", "品名", "已退金额", "可退金额", "成本", "库存","CheckBox" };
            DataTable dt = new DataTable();
            dt.TableName = "detail";
            for (int i = 0; i < colName.Length; i++)
            {
                dt.Columns.Add(colName[i]);
            }
            tableIndex = 0;
            ds.Tables.Add(dt);
            dt.Dispose();
        }
        //清空界面所有输入框
        private void ClearBox(bool bClear)
        {
            tbCount.Text = "";
            tbCountAvail.Text = "";
            tbManufacturerName.Text = "";
            tbPrice.Text = "";
            tbPriceAvail.Text = "";
            tbSKUName.Text = "";
            if (bClear)
            {
                btn01.Text = "";
            }
            btn02.Text = "";
            btn03.Text = "";
            if (cbReason.Items.Count >= 1)
            {
                cbReason.SelectedIndex = -1;
            }
        }
        //SKU补零
        private string AddZero(string OLstring)
        {
            string SKU = OLstring;
            if (SKU.Length <8)
            {
                StringBuilder addString = new StringBuilder();
                int addStringNo = 9 - SKU.Length;
                addString.Append('0', addStringNo);
                SKU = addString.ToString() + SKU;
            }
            return SKU;
        }

        #region 接口请求
        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.l0101:
                    {
                        string file = Config.getApiFile("101", "01");
                        from += "/101/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_01;
                    }
                    break;

                case API_ID.l0102:
                    {
                        string file = Config.getApiFile("101", "02");
                        from += "/101/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_02;
                    }
                    break;

                case API_ID.l0103:
                    {
                        string file = Config.getApiFile("101", "03");
                        from += "/101/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_03;
                    }
                    break;

                case API_ID.l0201:
                    {
                        string file = Config.getApiFile("102", "01");
                        from += "/102/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_102;
                    }
                    break;

                default:
                    return;
            }

            new ConnThread(this).Download(from, to, false);

            wait();
        }

        private void request102()
        {
            apiID = API_ID.l0201;

            string msg = "request=102;usr=" + Config.User + ";op=01;vnd=";
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request(string op, string bc)
        {
            string msg="";
            if (op.Equals("03"))
            {
                string manu = btn02.Text.ToUpper();
                StringBuilder addString = new StringBuilder();
                int addStringNo = 6 -  manu.Length;
                addString.Append('0', addStringNo);
                manu = addString.ToString() + manu;
                msg = "request=101" + ";usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper()+";vnd="+manu;
            }
            else if (op.Equals("02"))
            {
                StringBuilder addString = new StringBuilder();
                int addStringNo = 6 - bc.Length;
                addString.Append('0', addStringNo);
                bc = addString.ToString() + bc;
                msg = "request=101" + ";usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            }
            else
            {
                msg = "request=101" + ";usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            }
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request01()
        {
            apiID = API_ID.l0101;

            request("01", btn01.Text);
        }

        private void request02()
        {
            apiID = API_ID.l0102;

            request("02", btn02.Text);
        }

        private void request03()
        {
            apiID = API_ID.l0103;

            request("03", btn03.Text);
        }

        private void request04()
        {
            apiID = API_ID.l0104;

            string op = "04";
            string bc = "";
            if (btn01.Enabled)
            {
                bc = btn01.Text;
            }
            DataTable[] dtSubmit = new DataTable[] { getDTOK() };
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "101", op);
           
            string msg = "request=101;usr=" + Config.User + ";op=" + op + ";bc=" + bc + ";json=" + json;
            msg = DataGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request05()
        {
            apiID = API_ID.l0105;

            request("05", btn01.Text);
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
                    case API_ID.l0101:
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
                            setButton(false);
                            btn05.Enabled = false;
                            MessageBox.Show(data);
                        }
                        break;
                    case API_ID.l0102:
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
                            ClearBox(true);
                            deleteAll();
                        }
                        break;
                    case API_ID.l0103:
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
                            lastSKU = "";
                        }
                        break;
                    case API_ID.l0201:
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

                    case API_ID.l0104:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ClearPage();
                            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
                            if (!btn01.Enabled)
                            {
                                btn01.Text = data;
                            }
                            Serialize(guid);
                            MessageBox.Show("请求成功!");

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
                            LockPage();
                            Serialize(guid);
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.l0105:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功!");
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
                    case API_ID.DOWNLOAD_03:
                        if (result == ConnThread.RESULT_FILE)
                        {
                            Serialize(guid);
                            showXML();
                        }
                        else
                        {
                            MessageBox.Show(data);
                            tbCount.Text = "";
                            tbSKUName.Text = "";
                            tbCountAvail.Text = "";
                        }
                        break;
                    case API_ID.DOWNLOAD_01:
                    case API_ID.DOWNLOAD_02:
                    case API_ID.DOWNLOAD_102:
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
            switch (apiID)
            {
                case API_ID.l0101:
                    btn01.Text = barCode;
                    if (btn01.Enabled)
                    {
                        if (barCode != "")
                        {
                            ClearBox(false);
                            deleteAll();
                            request01();
                        }
                        else
                        {
                            MessageBox.Show("查询箱号不可为空!");
                            btn01.Text = "";
                        }
                    }
                    Serialize(guid);
                    break;

                case API_ID.l0102:
                    if (barCode != "")
                    {
                        if (barCode.Length <= 6)
                        {
                            if (oldManu != barCode && oldManu!=null)
                            {
                                ClearBox(true);
                                deleteAll();
                            }
                            oldManu = barCode;
                            btn02.Text = barCode;
                            request02();
                        }
                        else
                        {
                            MessageBox.Show("厂商不可超过六位!");
                            btn02.Text = "";
                        }

                    }
                    else
                    {
                        MessageBox.Show("查询厂商不可为空!");
                        btn02.Text = "";
                    }
                    Serialize(guid);
                    break;

                case API_ID.l0103:
                    if (btn02.Text != "")
                    {
                        if (barCode != "")
                        {
                            barCode = AddZero(barCode);
                            btn03.Text = barCode;
                            request03(); 
                        }
                        else
                        {
                            MessageBox.Show("查询SKU不可为空!");
                            btn03.Text = "";
                        }
                        Serialize(guid);
                    }
                    else
                    {
                        MessageBox.Show("厂商不可为空!");
                        btn03.Text = "";
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
            bNew=bool.Parse(param[0].ToString());
            if (param != null && param.Length > 0)
            {
                if (bNew)
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        CreateDS();
                        UpdateColumn(false);
                    }
                    setButton(false);
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    btn05.Enabled = false;
                }
                else
                {
                    ds = new DataSet();
                    CreateDS();
                    btn01.Enabled = true;
                    setButton(false);
                    btn05.Enabled = false;
                }
             
                Serialize(guid);
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

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
                data[(int)DATA_INDEX.BARCODE] = btn01.Text;
                data[(int)DATA_INDEX.PRICEAVAIL] = tbPriceAvail.Text;
                data[(int)DATA_INDEX.MANUFACTURERID] = btn02.Text;
                data[(int)DATA_INDEX.MANUFACTURERNAME] = tbManufacturerName.Text;
                data[(int)DATA_INDEX.SKU] = btn03.Text;
                data[(int)DATA_INDEX.SKUNAME] = tbSKUName.Text;
                data[(int)DATA_INDEX.COUNTAVAIL] = tbCountAvail.Text;
                data[(int)DATA_INDEX.PRICE] = tbPrice.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.REASONINDEX] = cbReason.SelectedIndex.ToString();
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.TABLEINFO] = tableInfo.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.NPRICESKU] = nPriceSKU.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.LASTSKU] = lastSKU;
                data[(int)DATA_INDEX.LASTBOX] = LastBox;
                data[(int)DATA_INDEX.BTNADD] = btnAdd.Enabled.ToString();
                data[(int)DATA_INDEX.BTNDELE] = btnDelete.Enabled.ToString();
                data[(int)DATA_INDEX.BTN04] = btn04.Enabled.ToString();
                data[(int)DATA_INDEX.BTN05] = btn05.Enabled.ToString();
                data[(int)DATA_INDEX.DATAGUID] = DataGuid;

                if (btn01.Enabled)
                {
                    data[(int)DATA_INDEX.BNEW] = "True";
                }
                else
                {
                    data[(int)DATA_INDEX.BNEW] = "False";
                }

                DataTable dt = ds.Tables[tableIndex];
                if (dt.DefaultView.Sort.IndexOf(" asc") > 0)
                {
                    data[(int)DATA_INDEX.SORTFLAG] = "asc";
                }
                else
                {
                    data[(int)DATA_INDEX.SORTFLAG] = "desc";
                }
                sc.Data = data;
       
                sc.DS = ds;
               
                sc.Serialize(Config.DirLocal + file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    btn01.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    tbPriceAvail.Text = sc.Data[(int)DATA_INDEX.PRICEAVAIL];
                    btn02.Text = sc.Data[(int)DATA_INDEX.MANUFACTURERID];
                    tbManufacturerName.Text = sc.Data[(int)DATA_INDEX.MANUFACTURERNAME];
                    btn03.Text = sc.Data[(int)DATA_INDEX.SKU];
                    tbSKUName.Text = sc.Data[(int)DATA_INDEX.SKUNAME];
                    tbCountAvail.Text = sc.Data[(int)DATA_INDEX.COUNTAVAIL];
                    tbPrice.Text = sc.Data[(int)DATA_INDEX.PRICE];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    tableInfo = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINFO]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    nPriceSKU = decimal.Parse(sc.Data[(int)DATA_INDEX.NPRICESKU]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    lastSKU = sc.Data[(int)DATA_INDEX.LASTSKU];
                    LastBox = sc.Data[(int)DATA_INDEX.LASTBOX];
                    btn04.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN04]);
                    btn05.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN05]);
                    btnAdd.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNADD]);
                    btnDelete.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNDELE]);
                    DataGuid=sc.Data[(int)DATA_INDEX.DATAGUID];
                    ds = sc.DS;
                  
                    if (bool.Parse(sc.Data[(int)DATA_INDEX.BNEW]))
                    {
                        btn01.Enabled = true;
                    }
                    else
                    {
                        btn01.Enabled = false; 
                    }

                    DataTable DT = ds.Tables[tableIndex];
                    if (colIndex >= 0)
                    {
                        DataColumn col = DT.Columns[colIndex];
                        if (sc.Data[(int)DATA_INDEX.SORTFLAG] == "asc")
                        {
                            DT.DefaultView.Sort = col.ColumnName.ToString() + " asc";
                        }
                        else
                        {
                            DT.DefaultView.Sort = col.ColumnName.ToString() + " desc";
                        }
                    }
                   
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                         if (dt.TableName == "Reason")
                        {
                           
                            btnReason.Visible = false;
                            cbReason.Enabled = true;
                            reason.Clear();
                            cbReason.Items.Clear();
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                reason.Add(dt.Rows[j][0].ToString());
                                cbReason.Items.Add(dt.Rows[j][1].ToString());
                            }
                            cbReason.SelectedIndex = int.Parse(sc.Data[(int)DATA_INDEX.REASONINDEX]); 
                        }
                    }
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    if (pageCount == 0)
                        Page.Text = "1/1";
                    else
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    UpdateColumn(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 翻页
        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    refreshData();
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdateColumn(true);
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

        private void NextPage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    refreshData();
                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {
                        UpdateColumn(true);
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

        #region UI事件响应
        //表格不可编辑
        private void LockPage()
        {
            try
            {
                int styleCount = dgTable.TableStyles[0].GridColumnStyles.Count;
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                DataGridCustomColumnBase style;
                for (int i = 0; i < styleCount; i++)
                {
                    if (styles[i].MappingName.Equals("门店申请数量"))
                    {
                        style = (DataGridCustomColumnBase)styles[i];
                        style.ReadOnly = true;
                    }
                }
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
                btn05.Enabled = false;
                btn04.Enabled = true;
            }
            catch (Exception)
            {
            }
           
        }
        //表格可编辑
        private void ClearPage()
        {
            try
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;

                btn05.Enabled = true;
                btn04.Enabled = false;
            }
            catch (Exception)
            {
            }
           
        }

        private void dgTable_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox box = (CheckBox)sender;
                bool bCheck = box.CheckState == CheckState.Checked ? true : false;
                DataTable dt = (DataTable)dgTable.DataSource;
                int rowIndex = dgTable.CurrentCell.RowNumber;

                colModify = "CheckBox";
                updateData(dt.Rows[rowIndex]["ID"].ToString(), bCheck.ToString());

                if (bCheckAll && !bCheck)
                {
                    bCheckAll = false;
                    DataTable dtHeader = (DataTable)dgHeader.DataSource;
                    dtHeader.Rows[0]["CheckBox"] = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
                refreshData();
        }

        private void checkAll(bool bAll)
        {
            try
            {
                bCheckAll = bAll;

                DataTable dt = ds.Tables[tableIndex];
                if (dt.Rows.Count >= 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["CheckBox"] = bAll.ToString();
                    }

                    UpdateColumn(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void refreshData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                DataTable DT = ds.Tables[tableIndex];
                string id = dt.Rows[dgTable.CurrentRowIndex]["ID"].ToString();
                string SKU=dt.Rows[dgTable.CurrentRowIndex]["SKU"].ToString();
                string ok = dt.Rows[dgTable.CurrentRowIndex]["门店申请数量"].ToString();
                string total ="0";
               
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    if (DT.Rows[i]["ID"].ToString().Equals(id))
                    {
                        total = DT.Rows[i]["库存"].ToString();
                        if (!reg.IsMatch(total))
                        {
                            total = "0";
                        }
                        break;
                    }
                }

                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        UpdateColumn(true);
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 0)
                        {
                            string subtext = ok.Substring(0, 1);
                            if (UInt64.Parse(subtext) == 0)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdateColumn(true);
                            }
                            else if (UInt64.Parse(total) < UInt64.Parse(ok))
                            {
                                MessageBox.Show("该SKU已超出可用库存！");
                                UpdateColumn(true);
                            }
                            else
                            {
                                if (CheckSKUPriceAvail(id, ok))
                                {
                                    colModify = "门店申请数量";
                                    updateData(id, ok);
                                }
                            }
                        }
                        else if (UInt64.Parse(ok) == 0)
                        {
                            if (ok.Length >= 2)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdateColumn(true);
                            }
                            else
                            {
                                if (CheckSKUPriceAvail(id, ok))
                                {
                                    colModify = "门店申请数量";
                                    updateData(id, ok);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
                    UpdateColumn(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private bool CheckSKUPriceAvail(string id,string value)
        {
            try
            {
                DataTable dt = ds.Tables[tableIndex];

                decimal priceAvail = decimal.Parse(tbPriceAvail.Text);
                decimal priceReal = 0.0M;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ID"].ToString().Equals(id))
                    {
                        priceReal += decimal.Parse(dt.Rows[i]["成本"].ToString()) * int.Parse(value);
                    }
                    else
                    {
                        priceReal += decimal.Parse(dt.Rows[i]["成本"].ToString()) * int.Parse(dt.Rows[i]["门店申请数量"].ToString());
                    }
                }

                if (priceReal > priceAvail)
                {
                    MessageBox.Show("已超出厂商可退金额！");
                    return false;
                }
                else
                {
                    tbPrice.Text = priceReal.ToString();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        DataTable dtTmp = (DataTable)(dgTable.DataSource);
                        if (dtTmp != null && dtTmp.Rows.Count > 0)
                        {
                            string colName = dtTmp.Columns[hitTest.Column].ColumnName;

                            if (colName.Equals("CheckBox"))
                            {
                                DataTable dtHeader = (DataTable)dgHeader.DataSource;
                                DataRow row = dtHeader.Rows[hitTest.Row];
                                if (bool.Parse(row["CheckBox"].ToString()))
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
                                DataTable dt = ds.Tables[tableIndex];
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
                                }
                            }
                            UpdateColumn(true);
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
            if (!busy)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null)
                {
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox") && colIndex != 3)
                    {
                        int rowIndex = dgTable.CurrentCell.RowNumber;
                        dgTable.Select(rowIndex);
                    }
                }
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
                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);
                        int row=dgTable.CurrentCell.RowNumber;
                        if (row != hitTest.Row)
                        {
                            dgTable.UnSelect(row);
                        }
     
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null && dt.Rows.Count >= 1)
                {
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox"))
                    {
                        int rowIndex = dgTable.CurrentCell.RowNumber;
                        dgTable.Select(rowIndex);
                    }
                }
            }
        }

        private bool checkValidation()
        {
            try
            {
                if (btn01.Enabled)
                {
                    if (string.IsNullOrEmpty(btn01.Text))
                    {
                        MessageBox.Show("请输入箱号!");
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(tbManufacturerName.Text))
                {
                    MessageBox.Show("无此厂商！");
                    return false;
                }

                if (string.IsNullOrEmpty(btn03.Text))
                {
                    MessageBox.Show("请输入SKU！");
                    return false;
                }

                if (string.IsNullOrEmpty(tbSKUName.Text))
                {
                    MessageBox.Show("无此SKU！");
                    return false;
                }

                if (reg.IsMatch(tbCount.Text))
                {
                    string Num = tbCount.Text;
                    if (int.Parse(Num) == 0)
                    {
                        MessageBox.Show("请输入正整数！");
                        return false;
                    }
                    else if (int.Parse(Num) > 0)
                    {
                        string subtext = Num.Substring(0, 1);
                        if (int.Parse(subtext) == 0)
                        {
                            MessageBox.Show("请输入正整数！");
                            return false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请输入正整数！");
                    return false;
                }

                if (cbReason.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择退货原因！");
                    return false;
                }

                if (int.Parse(tbCount.Text) > int.Parse(tbCountAvail.Text))
                {
                    MessageBox.Show("已超出可用库存！");
                    return false;
                }
                if (tableIndex >=0)
                {
                    DataTable dt = ds.Tables[tableIndex];
                    if (dt.Rows.Count >= 1)
                    {
                        bool ChSKU = false;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string SKU = dt.Rows[i]["SKU"].ToString();
                            string Reason = dt.Rows[i]["退货描述"].ToString().Trim();
                            if (btn03.Text == SKU)
                            {
                                if (cbReason.SelectedItem.ToString().Trim() != Reason)
                                {
                                    ChSKU = true;
                                    break;
                                }
                            }
                        }
                        if (ChSKU)
                        {
                            MessageBox.Show("同一SKU只能选择一个退货原因!");
                            return false;
                        }

                    }
                }

                decimal PriceSum=MoneySum();

                if (PriceSum > decimal.Parse(tbPriceAvail.Text))
                {
                    MessageBox.Show("已超出可退金额！");
                    return false;
                }
                else
                {
                    tbPrice.Text = PriceSum.ToString();
                }

                if (ds.Tables[tableIndex].Rows.Count+1 > 60)
                {
                    MessageBox.Show("您添加的商品数过多！");
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
        
        private int checkOnly()
        {
           
            int rowflag=0;
            if (tableIndex >= 0)
            {
                int Sum = 0;
                DataTable dt = ds.Tables[tableIndex];
                try
                {
                    if (dt.Rows.Count >= 1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string SKU = dt.Rows[i]["SKU"].ToString();
                            string Reason = dt.Rows[i]["退货描述"].ToString().Trim();
                            if (btn03.Text == SKU)
                            {
                                if (cbReason.SelectedItem.ToString().Trim() == Reason)
                                {
                                    string cb = dt.Rows[i]["门店申请数量"].ToString().Trim();
                                    if (reg.IsMatch(cb))
                                    {
                                        Sum += int.Parse(cb);
                                        OSKU = true;
                                        rowflag = i;
                                        break;
                                    }
                                }

                            }
                        }

                        Sum += int.Parse(tbCount.Text);
                        if (reg.IsMatch(tbCountAvail.Text))
                        {
                            if (Sum > int.Parse(tbCountAvail.Text))
                            {
                                MessageBox.Show("该SKU数量已超出可用库存!");
                                DSKU = false;
                                OSKU = false;
                            }
                            else
                            {
                                DSKU = true;
                            }
                        }
                    }
                    else
                    {
                        DSKU = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return rowflag;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (checkValidation())
                    {

                        DataTable dt = ds.Tables[tableIndex];
                        int getId = checkOnly();
                        if (DSKU)
                        {
                            if (OSKU)
                            {
                                OSKU = false;
                                DSKU = false;
                                int newNum = int.Parse(dt.Rows[getId]["门店申请数量"].ToString()) + int.Parse(tbCount.Text);
                                dt.Rows[getId]["门店申请数量"] = newNum.ToString();
                            }
                            else
                            {
                                DSKU = false;
                                DataRow row = dt.NewRow();
                                Array Reason = reason.ToArray();
                                row["SKU"] = btn03.Text;
                                row["品名"] = tbSKUName.Text;
                                row["门店申请数量"] = tbCount.Text;
                                row["退货原因"] = Reason.GetValue(cbReason.SelectedIndex).ToString();
                                row["退货描述"] = cbReason.SelectedItem.ToString();
                                row["成本"] = nPriceSKU.ToString();
                                row["CheckBox"] = bCheckAll.ToString();
                                row["厂商代码"] = btn02.Text;
                                row["库存"] = tbCountAvail.Text;
                                dt.Rows.Add(row);
                                int nCount = ds.Tables[tableIndex].Rows.Count;
                                if (nCount == 0)
                                {
                                    pageIndex = 1;
                                }
                                else
                                {
                                    pageIndex = (int)Math.Ceiling(nCount / (double)TABLE_ROWMAX);
                                }
                                Page.Text = pageIndex.ToString() + "/" + pageIndex.ToString();
                            }
                            setButton(true);
                        }

                        UpdateColumn(false);
                        Serialize(guid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         }
  
        private decimal MoneySum()
        { 
            decimal moneySum = 0.0M;
            try
            {
                if (tableIndex >= 0)
                {
                    DataTable dt = ds.Tables[tableIndex];

                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (btn02.Text.Equals(dt.Rows[i]["厂商代码"].ToString()))
                            {
                                decimal Price = decimal.Parse(dt.Rows[i]["成本"].ToString());
                                int Num = int.Parse(dt.Rows[i]["门店申请数量"].ToString());
                                moneySum += Num * Price;
                            }
                        }
                    }
                    moneySum += nPriceSKU * int.Parse(tbCount.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return moneySum;
        }

        private void deleteData()
        {
            try
            {
                if (ds != null && tableIndex >= 0)
                {
                    bool rowCheck = false;
                    DataTable dt = ds.Tables[tableIndex];
                    decimal DeSum = 0.0M;
                    if (dt.Rows.Count >= 1)
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            string cb = "";
                            int Num = 0;
                            if (dt.Rows[i]["CheckBox"].ToString().Equals("True") ? true : false)
                            {
                                cb = dt.Rows[i]["成本"].ToString();
                                Num = int.Parse(dt.Rows[i]["门店申请数量"].ToString());
                                dt.Rows.RemoveAt(i);
                                DeSum += decimal.Parse(cb) * Num;
                                rowCheck = true;
                            }
                        }

                        if (regF.IsMatch(tbPrice.Text))
                        {
                            if (DeSum > 0.0M)
                            {
                                tbPrice.Text = (decimal.Parse(tbPrice.Text) - DeSum).ToString();
                            }
                        }

                        if (!rowCheck)
                        {
                            MessageBox.Show("未选中任何行!");
                        }
                        else
                        {
                            pageIndex = 1;
                            UpdateColumn(false);
                        }
                    }
                    else
                    {
                        MessageBox.Show("表格内无数据!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (MessageBox.Show("确认删除所选行吗？", "", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    deleteData();
                }
            }
        }

        private void showScan()
        {
            if (!busy)
            {
                formScan = new FormScan();
                formScan.init(this, bc);
                formScan.ShowDialog();
                formScan.Dispose();
                formScan = null;
                bc = "";
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

        private void btnReason_Click(object sender, EventArgs e)
        {
            if (!busy && cbReason.Items.Count == 0)
            {
                request102();
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                apiID = API_ID.l0101;
                bc = btn01.Text;
                showScan();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                apiID = API_ID.l0102;
                bc = btn02.Text;
                showScan();
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                apiID = API_ID.l0103;
                bc = btn03.Text;
                showScan();
            }
        }

        private void btn04_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (ds.Tables[tableIndex].Rows.Count > 0)
                    {

                        if (btn01.Enabled)
                        {
                            if (btn01.Text != "")
                            {
                                if (CheckSubmit())
                                {
                                    request04();
                                }
                            }
                            else
                            {
                                MessageBox.Show("箱号不能为空!");
                            }
                        }
                        else
                        {
                            if (CheckSubmit())
                            {
                                request04();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("表格内无数据！");
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
                DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string num = dt.Rows[i]["门店申请数量"].ToString();
                    if (UInt64.Parse(num) <= 0)
                    {
                        int rowindex = i + 1;
                        pageIndex = (int)Math.Ceiling(rowindex / (double)TABLE_ROWMAX);
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();

                        UpdateColumn(true);
                        if (rowindex <= TABLE_ROWMAX)
                        {
                            MessageBox.Show("第" + rowindex.ToString() + "行退货数量为0,请重新填写!");
                        }
                        else
                        {
                            MessageBox.Show("第" + (rowindex % TABLE_ROWMAX).ToString() + "行退货数量为0,请重新填写!");
                        }
                        
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void deleteAll()
        {
            try
            {
                if (ds != null && tableIndex>=0)
                {
                    DataTable dt = ds.Tables[tableIndex];
                    if (dt != null && dt.Rows.Count >= 1)
                    {
                        int rowNum = dt.Rows.Count;
                        for (int i = 0; i < rowNum; i++)
                        {
                            dt.Rows[0].Delete();
                        }
                    }
                    pageIndex = 1;
                    UpdateColumn(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn05_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request05();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (btn04.Enabled)
                    {
                        DataTable dt = (DataTable)dgTable.DataSource;
                        if (dt != null && dt.Rows.Count >= 1)
                        {
                            if (DialogResult.Yes == MessageBox.Show("是否放弃当前操作？", "", MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                File.Delete(Config.DirLocal + guid);
                                Close();
                            }
                        }
                        else
                        {
                            File.Delete(Config.DirLocal + guid);
                            Close();
                        }

                    }
                    else
                    {
                        File.Delete(Config.DirLocal + guid);
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion 

    }
}