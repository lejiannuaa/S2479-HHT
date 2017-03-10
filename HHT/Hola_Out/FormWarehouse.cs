using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Permissions;
using HolaCore;

namespace Hola_Out
{
    public partial class FormWarehouse : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "BCC631F7-42CE-FC92-E874-3655245CDB84";

        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        //指示当前正在发送网络请求
        private bool busy = false;

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            SKU,
            SKUNAME,
            COUNTAVAIL,
            COUNT,
            REASONINDEX,
            TABLEINDEX,
            PAGEINDEX,
            BNEW,
            COLINDEX,
            SORTFLAG,
            LASTSKU,
            DATAMAX
        }
        #endregion

        private const int TABLE_ROWMAX = 7;
        private string lastSKU = null;
        private FormDownload formDownload = null;
        private FormScan formScan = null;
        private string bc = "";

        private string xmlFile = null;

        private SerializeClass sc = null;

        public DataSet ds = null;

        private List<string> reason = new List<string>();
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
        private bool bCheckAll = false;
        private bool OSKU = false;
        private bool DSKU = false;
        private string colModify = "";
        private int tableInfo = 0;
        Regex reg = new Regex(@"^\d+$");
        private bool bBox = true;
        //DataGridEvenOddHandler handler = null;

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            l0301,
            l0302,
            l0303,
            l0304,
            l0201,
            DOWNLOAD_01,
            DOWNLOAD_02,
            DOWNLOAD_102
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormWarehouse()
        {
            InitializeComponent();

            doLayout();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;

            try
            {
                SuspendLayout();
                btn03.Top = dstHeight - btnReturn.Height;
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
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

                case API_ID.DOWNLOAD_102:
                    break;

                default:
                    return;
            }

            LoadData();
            Serialize();
        }

        private void addTable(DataTable dt, string name)
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

        private bool Load01(XmlNodeReader reader)
        {
            bool bNoData = true;
            ds = new DataSet();
            ds.ReadXml(reader);

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
                    pageIndex = 1;
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    if (pageCount <= 0)
                    {
                        pageCount = 1;
                    }
                    Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    UpdateColumn(false);
                    setButton(true);
                    btn03.Enabled = false;
                }
                else
                {
                    pageIndex = 1;
                    ds = new DataSet();
                    CreateDS();
                    setButton(false);
                    btn04.Enabled = false;
                    ClearBox();
                    UpdateColumn(false);
                }
            }
            else
            {
                pageIndex = 1;
                ds = new DataSet();
                CreateDS();
                setButton(false);
                btn04.Enabled = false; 
                ClearBox();
                UpdateColumn(false);
            }
            return bNoData;
        }

        private bool Load02(XmlNodeReader reader)
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
                        addTable(dt, "SKU");

                        tbSKUName.Text = dt.Rows[0]["品名"].ToString().Equals("null") ? "" : dt.Rows[0]["品名"].ToString();
                        tbCountAvail.Text = dt.Rows[0]["库存"].ToString().Equals("null") ? "" : dt.Rows[0]["库存"].ToString();
                        if (btn02.Text.Length > 9)
                        {
                            btn02.Text = dt.Rows[0]["HHTSKU"].ToString();
                        }

                        if (string.IsNullOrEmpty(lastSKU))
                        {
                            lastSKU = btn02.Text;
                            tbCount.Text = "1";
                        }
                        else if (lastSKU == btn02.Text)
                        {
                            if (reg.IsMatch(tbCount.Text))
                            {
                                tbCount.Text = (UInt64.Parse(tbCount.Text) + 1).ToString();
                            }
                        }
                        else
                        {
                            lastSKU = btn02.Text;
                            tbCount.Text = "1";
                        }
                        break;
                    }
                }

                if (bNoData)
                {
                    tbSKUName.Text = "";
                    tbCountAvail.Text = "";
                    lastSKU = "";
                    tbCount.Text = "";
                }

                dsSKU.Dispose();
            }
            catch (Exception ex)
            {
                tbSKUName.Text = "";
                tbCountAvail.Text = "";
                lastSKU = "";
                tbCount.Text = "";
                MessageBox.Show(ex.Message);
            }

            return bNoData;
        }

        private bool Load102(XmlNodeReader reader)
        {
            bool bNoData = false;

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

            return bNoData;
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;

            bool bNoData = false;
            XmlDocument doc = null;
            XmlNodeReader reader = null;
            try
            {
                if (File.Exists(xmlFile))
                {
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
                            setButton(false);
                            ClearBox();
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
                            tbSKUName.Text = "";
                            tbCountAvail.Text = "";
                            tbCount.Text = "";
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
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                if (reader != null)
                    reader.Close();
                File.Delete(xmlFile);
                GC.Collect();
                Serialize();
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
                if (bCustom && name[i].Equals("CheckBox"))
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
                string[] colName = new string[] { "ID", "HHTSKU", "品名", "门店申请数量", "退货描述", "CheckBox" };
                string[] colValue = new string[] { "ID", "SKU", "品名", "数量", "原因", "false" };
                int[] colWidth = new int[] { 10, 25, 20, 15, 20, 7 };

                DataTable dt = ds.Tables[tableIndex];
                DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                DataRowCollection rows = ds.Tables[tableIndex].Rows;
               
                if (!bDeserialize)
                {
                    if (!cols.Contains("ID"))
                    {
                        cols.Add("ID",System.Type.GetType("System.UInt32"));
                    }
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["ID"] = i+1;
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
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, true));
                dgHeader.DataSource = dtHeader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UpdateRow()
        {
            DataGridCustomCheckBoxColumn colCheckBox = null;
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
            }
            dtResult.TableName = dt.TableName;

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
        }

        private void updateData(string key, string value)
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
                        Serialize();
                    }
                    return;
                }
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
                
                rowNew["sku"] = AddZero(dt.Rows[i]["HHTSKU"].ToString());
                rowNew["HHTQTY"] = dt.Rows[i]["门店申请数量"];
                rowNew["reason"] = dt.Rows[i]["退货原因"];
                rowNew["vendorcode"] = "";
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
            btn03.Enabled = flag;
            btn04.Enabled = !btn03.Enabled;
        }
        //创建包含列空数据集
        private void CreateDS()
        {
            string[] colName = { "HHTSKU", "品名", "门店申请数量", "退货原因", "退货描述" ,"库存"};

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
        private void ClearBox()
        {
            btn02.Text = "";
            tbCount.Text = "";
            tbCountAvail.Text = "";
            tbSKUName.Text = "";
            if (cbReason.Items.Count >= 1)
            {
                cbReason.SelectedIndex = -1;
            }
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

        #region 接口请求
        private void requestXML()
        {
            string from = Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.l0301:
                    {
                        string file = Config.getApiFile("103", "01");
                        from += "/103/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_01;
                    }
                    break;

                case API_ID.l0302:
                    {
                        string file = Config.getApiFile("103", "02");
                        from += "/103/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_02;
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

            new ConnThread(this).Send(msg);
            wait();
        }

        private void request(string op, string bc)
        {
            string msg = "request=103;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();

            new ConnThread(this).Send(msg);
            wait();
        }

        private void request01()
        {
            apiID = API_ID.l0301;

            request("01", btn01.Text);
        }

        private void request02()
        {
            apiID = API_ID.l0302;

            request("02",btn02.Text);
        }

        private void request03()
        {
            apiID = API_ID.l0303;
            string op = "03";
            bc = btn01.Text;
            DataTable[] dtSubmit = new DataTable[] { getDTOK() };
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "103", op);
            string msg = "request=103;usr=" + Config.User + ";op=" + op + ";bc=" + bc + ";json=" + json;

            new ConnThread(this).Send(msg);
            wait();
        }

        private void request04()
        {
            apiID = API_ID.l0304;

            request("04", btn01.Text);
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

        public void requestCallback(string data, int result)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();

                switch (apiID)
                {
                    case API_ID.l0301:
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
                            MessageBox.Show(data);
                        }
                        break;
                    case API_ID.l0302:
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
                            if (apiID == API_ID.l0302)
                            {
                                lastSKU = "";
                            }
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.l0303:
                        if (result == ConnThread.RESULT_OK)
                        {
                            if (btn01.Enabled)
                            {
                                btn03.Enabled = false;
                                btn04.Enabled = true;
                            }
                            else
                            {
                                btnAdd.Enabled = false;
                                btnDelete.Enabled = false;
                                btn03.Enabled = false;
                                btn04.Enabled = true;

                            }
                            MessageBox.Show("请求成功!");
                            if (!btn01.Enabled)
                            {
                                btn01.Text = data;
                            }
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
                            setButton(true);
                            MessageBox.Show(data);
                        }
                        break;
                    case API_ID.l0304:
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
                case API_ID.l0301:
                    btn01.Text = barCode;
                    if (btn01.Enabled)
                    {
                        if (barCode != "")
                        {
                            deleteAll();
                            request01();
                        }
                        else
                        {
                            MessageBox.Show("查询箱号不可为空!");
                        }
                    }
                    Serialize();
                    break;

                case API_ID.l0302:
                    if (barCode != "")
                    {
                        barCode = AddZero(barCode);
                        btn02.Text = barCode;
                        request02();
                    }
                    else
                    {
                        MessageBox.Show("查询SKU不可为空!");
                    }
                    Serialize();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 实现ISerialize接口
        public void init(object[] param)
        {
            if (param != null && param.Length > 0)
            {
                if (bool.Parse(param[0].ToString()))
                {
                    if (ds == null)
                    {
                        ds = new DataSet();
                        CreateDS();
                        UpdateColumn(false);
                    }
                    setButton(false);
                    btn04.Enabled = false;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                }
                else
                {
                    btn01.Enabled = true;
                    setButton(false);
                    btn04.Enabled = false;
                    ds = new DataSet();
                    CreateDS();
                }
                Serialize();
            }
        }

        public void Serialize()
        {
            try
            {
                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
                data[(int)DATA_INDEX.BARCODE] = btn01.Text;
                data[(int)DATA_INDEX.SKU] = btn02.Text;
                data[(int)DATA_INDEX.SKUNAME] = tbSKUName.Text;
                data[(int)DATA_INDEX.COUNTAVAIL] = tbCountAvail.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.REASONINDEX] = cbReason.SelectedIndex.ToString();
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.LASTSKU] = lastSKU;
                if (btn01.Enabled)
                {
                    data[(int)DATA_INDEX.BNEW] = "True";
                }
                else
                {
                    data[(int)DATA_INDEX.BNEW] = "False";
                }
                sc.Data = data;

                DataTable dt = ds.Tables[tableIndex];
                if (dt.DefaultView.Sort.IndexOf(" asc") > 0)
                {
                    data[(int)DATA_INDEX.SORTFLAG] = "asc";
                }
                else
                {
                    data[(int)DATA_INDEX.SORTFLAG] = "desc";
                }
                sc.DS = ds;

                sc.Serialize(Config.DirLocal + guid);
            }
            catch (Exception)
            {
            }
        }

        public void Deserialize()
        {
            try
            {
                sc = SerializeClass.Deserialize(Config.DirLocal + guid);

                if (sc != null)
                {
                    btn01.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    btn02.Text = sc.Data[(int)DATA_INDEX.SKU];
                    tbSKUName.Text = sc.Data[(int)DATA_INDEX.SKUNAME];
                    tbCountAvail.Text = sc.Data[(int)DATA_INDEX.COUNTAVAIL];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    lastSKU = sc.Data[(int)DATA_INDEX.LASTSKU];
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
        private void dgTable_CheckStateChanged(object sender, EventArgs e)
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

        private void checkAll(bool bAll)
        {
            bCheckAll = bAll;
            if (tableIndex >= 0)
            {
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
        }

        private void refreshData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                DataTable DT = ds.Tables[tableIndex];
                string id = dt.Rows[dgTable.CurrentRowIndex]["ID"].ToString();
                string SKU = dt.Rows[dgTable.CurrentRowIndex]["HHTSKU"].ToString();
                string ok = dt.Rows[dgTable.CurrentRowIndex]["门店申请数量"].ToString();
                string total = "0";

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    if (DT.Rows[i]["ID"].ToString().Equals(id))
                    {
                        total = DT.Rows[i]["库存"].ToString();
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
                                    colModify = "门店申请数量";
                                    updateData(id, ok);
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
                                    colModify = "门店申请数量";
                                    updateData(id, ok);
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

        private int checkOnly()
        {
            int rowflag = 0;
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
                            string SKU = dt.Rows[i]["HHTSKU"].ToString();
                            string Reason = dt.Rows[i]["退货描述"].ToString().Trim();
                            if (btn02.Text == SKU)
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
                    MessageBox.Show(ex.ToString());
                }
            }
            return rowflag;
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
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
                        refreshData();
                    }
                }
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgTable.DataSource;
            int colIndex = dgTable.CurrentCell.ColumnNumber;
            if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox") && colIndex!=3)
            {
                int rowIndex = dgTable.CurrentCell.RowNumber;
                dgTable.Select(rowIndex);
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
                        if (hitTest.Row == rowOld && hitTest.Column == columnOld)
                        {
                        }
                        else
                        {
                            refreshData();
                        }
                    }
                }

            }
        }

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgTable.DataSource;
            if (dt != null && dt.Rows.Count>=1)
            {
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox"))
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
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
                if (string.IsNullOrEmpty(btn02.Text))
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
                    if (UInt64.Parse(Num) == 0)
                    {
                        MessageBox.Show("请输入正整数！");
                        return false;
                    }
                    else if (UInt64.Parse(Num) > 0)
                    {
                        string subtext = Num.Substring(0, 1);
                        if (UInt64.Parse(subtext) == 0)
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

                if (UInt64.Parse(tbCount.Text) > UInt64.Parse(tbCountAvail.Text))
                {
                    MessageBox.Show("已超出可用库存！");
                    return false;
                }

                if (ds != null)
                {
                    if (btn01.Enabled == true)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            if (ds.Tables[i].TableName.Equals("info"))
                            {
                                string state = ds.Tables[i].Rows[0][0].ToString();
                                if (state == "1O" || state == "1Y")
                                {
                                    MessageBox.Show("申请中或已成功不可提交!");
                                    return false;
                                }
                                else if (state == "0O" || state == "0N")
                                {
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show("箱状态未确定!");
                                    return false;
                                }
                            }
                        }

                    }
                }

                if (tableIndex >= 0)
                {
                    DataTable dt = ds.Tables[tableIndex];
                    if (dt.Rows.Count >= 1)
                    {
                        bool ChSKU = false;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string SKU = dt.Rows[i]["HHTSKU"].ToString();
                            string Reason = dt.Rows[i]["退货描述"].ToString().Trim();
                            if (btn02.Text == SKU)
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

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!busy && ds!=null)
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
                            Array Reason = reason.ToArray();
                            DataRow row = dt.NewRow();
                            row["HHTSKU"] = btn02.Text;
                            row["品名"] = tbSKUName.Text;
                            row["门店申请数量"] = tbCount.Text;
                            row["退货原因"] = Reason.GetValue(cbReason.SelectedIndex).ToString();
                            row["退货描述"] = cbReason.SelectedItem.ToString();
                            row["CheckBox"] = bCheckAll.ToString();
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
                    Serialize();
                }
            }
        }

        private void deleteData()
        {
            try
            {
                bool rowCheck = false;
                DataTable dt = ds.Tables[tableIndex];
                if (ds != null && dt.Rows.Count >= 1)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {

                        if (dt.Rows[i]["CheckBox"].ToString().Equals("True") ? true : false)
                        {
                            dt.Rows.RemoveAt(i);
                            rowCheck = true;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void deleteAll()
        {
            try
            {
                if (ds != null)
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
                MessageBox.Show(ex.ToString());
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
                apiID = API_ID.l0301;
                bc = btn01.Text;
                showScan();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                apiID = API_ID.l0302;
                bc = btn02.Text;
                showScan();
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (btn01.Enabled)
                    {
                        if (btn01.Text != "")
                        {
                            request03();
                        }
                        else
                        {
                            MessageBox.Show("箱号不能为空!");
                        }
                    }
                    else
                    {
                        if (ds.Tables[tableIndex].Rows.Count > 0)
                        {
                            request03();
                        }
                        else
                        {
                            MessageBox.Show("表格内无数据！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn04_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request04();
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
                if (btn03.Enabled)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (dt!=null && dt.Rows.Count >= 1)
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

        private void FormWarehouse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (!busy && formScan==null)
                {
                    btnAdd_Click(null, null);
                }
            }
        }
      
        #endregion

       
    }
}