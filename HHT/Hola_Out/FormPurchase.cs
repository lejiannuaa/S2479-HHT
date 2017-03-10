using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using HolaCore;
using System.Collections.Generic;

namespace Hola_Out
{
    public partial class FormPurchase : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "F7EBB354-8C3C-8110-8A09-AE1C90101F7C";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        //指示当前正在发送网络请求
        private bool busy = false;

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            RTV,
            SKU,
            SKUNAME,
            COUNTAVAIL,
            COUNT,
            TABLEINDEX,
            PAGEINDEX,
            COLINDEX,
            SORTFLAG,
            LASTSKU,
            BTNADD,
            BTNDELE,
            BTN02,
            BTN03,
            DATAGUID,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 5;

        private FormDownload formDownload = null;
        private FormScan formScan = null;
        private string bc = "";

        private string xmlFile = null;
        //保留上次请求的SKU
        private string lastSKU = "";
        private SerializeClass sc = null;
        private bool OSKU = false;
        private bool DSKU = false;
        public DataSet ds = null;
        private List<string> reason = new List<string>();
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
        private bool bCheckAll = false;
        //发送数据标识唯一性
        private string DataGuid = "";
        private string colModify = "";

        Regex reg = new Regex(@"^\d+$");
        private int TaskBarHeight = 0;


        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            l0501,
            l0502,
            l0503,
            l0201,
            l0504,
            DOWNLOAD_01,
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormPurchase()
        {
            InitializeComponent();
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            doLayout();
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
                if (dstWidth > 240)
                {
                    dgTable.Height = dgTable.Height + 50;
                    TABLE_ROWMAX = 7;
                }
                btn02.Top = dstHeight - btnReturn.Height;
                btnReturn.Top = dstHeight - btnReturn.Height;
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
            switch (apiID)
            {
                case API_ID.DOWNLOAD_01:
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
                        if (btn01.Text.Length > 9 || btn01.Text.Length == 8)
                        {
                            btn01.Text = dt.Rows[0]["HHTSKU"].ToString();
                        }

                        if (string.IsNullOrEmpty(lastSKU))
                        {
                            lastSKU = btn01.Text;
                            tbCount.Text = "1";
                        }
                        else if (lastSKU == btn01.Text)
                        {
                            if (reg.IsMatch(tbCount.Text))
                            {
                                tbCount.Text = (UInt64.Parse(tbCount.Text) + 1).ToString();
                            }
                        }
                        else
                        {
                            lastSKU = btn01.Text;
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
                    lastSKU = "";
                }
                dsSKU.Dispose();
            }
            catch (Exception ex)
            {
                tbSKUName.Text = "";
                tbCountAvail.Text = "";
                tbCount.Text = "";
                lastSKU = "";
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
                            if (bNoData)
                            {
                                MessageBox.Show("无数据！");
                            }
                        }
                        else
                        {
                            tbSKUName.Text = "";
                            tbCountAvail.Text = "";
                            lastSKU = "";
                            tbCount.Text = "";
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

                if (reader != null)
                    reader.Close();

                doc = null;
                File.Delete(xmlFile);
                GC.Collect();
                Serialize(guid);

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
                string[] colName = new string[] { "ID", "SKU", "品名", "门店申请数量", "CheckBox" };
                string[] colValue = new string[] { "ID", "SKU", "品名", "数量", "false" };
                //int[] colWidth = new int[] { 10, 35, 30, 15, 7 };
                int[] colWidth = new int[] { 10, 19, 46, 15, 7 };

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

                if (!bDeserialize)
                {

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
            int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
            if (pageCount <= 0)
            {
                pageCount = 1;
            }
            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
            if (colCheckBox != null && to > 0)
            {
                CheckBox box = (CheckBox)colCheckBox.HostedControl;
                box.CheckStateChanged += new EventHandler(dgTable_CheckStateChanged);
            }
            if (colTextBox != null && to > 0)
            {
                TextBox Tbox = (TextBox)colTextBox.HostedControl;
                Tbox.LostFocus += new EventHandler(Tbox_LostFocus);
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
            string[] colName = { "sku", "HHTQTY", "RTV" };
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

                rowNew["sku"] = dt.Rows[i]["SKU"];
                rowNew["HHTQTY"] = dt.Rows[i]["门店申请数量"];
                rowNew["RTV"] = tbRTV.Text;
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
            btn02.Enabled = flag;
            btn03.Enabled = !btn02.Enabled;
        }
        //清空界面所有输入框
        private void ClearBox()
        {
            btn01.Text = "";
            tbCount.Text = "";
            tbCountAvail.Text = "";
            tbRTV.Text = "";
            tbSKUName.Text = "";
        }

        private void CreateDS()
        {
            try
            {
                string[] colName = { "SKU", "门店申请数量", "品名", "RTV", "库存", "CheckBox" };

                DataTable dt = new DataTable();
                dt.TableName = "detail";
                for (int i = 0; i < colName.Length; i++)
                {
                    dt.Columns.Add(colName[i]);
                }

                tableIndex = 0;
                ds = new DataSet();
                ds.Tables.Add(dt);
            }
            catch (Exception)
            {
            }
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

        #region 接口请求
        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.l0501:
                    {
                        string file = Config.getApiFile("105", "01");
                        from += "/105/" + file;
                        to += file;
                        xmlFile = to;
                        apiID = API_ID.DOWNLOAD_01;
                    }
                    break;

                default:
                    return;
            }

            new ConnThread(this).Download(from, to, false);

            wait();
        }

        private void request(string op, string bc)
        {
            string msg = "";
            if (op == "04")
            {
                msg = "request=104;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            }
            else
            {
                msg = "request=105;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            }
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request01()
        {
            apiID = API_ID.l0501;
            
            string op = "01";
            string msg = "request=105;usr=" + Config.User + ";op=" + op + ";bc=" + tbRTV.Text.ToUpper() + ";sku="+ btn01.Text.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
            

            //request("01", btn01.Text);
        }

        private void request02()
        {
            apiID = API_ID.l0502;

            string op = "02";
            DataTable[] dtSubmit = new DataTable[] { getDTOK() };
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "105", op);
            string msg = "request=105;usr=" + Config.User + ";op=" + op + ";bc=" + bc + ";json=" + json;
            msg = DataGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request03()
        {
            apiID = API_ID.l0503;

            request("04", tbBox.Text);
        }

        private void request04()
        {
            apiID = API_ID.l0504;
            string op = "04";
            string msg = "request=105;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbRTV.Text.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
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
                    case API_ID.l0501:
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
                            if (apiID == API_ID.l0501)
                            {
                                lastSKU = "";
                            }
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.l0502:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ClearPage();
                            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
                            tbBox.Text = data;
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

                    case API_ID.l0503:
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

                    case API_ID.l0504:
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
                        break;

                    case API_ID.DOWNLOAD_01:
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
                case API_ID.l0501:
                    if (barCode != "")
                    {
                        barCode = AddZero(barCode);
                        btn01.Text = barCode;
                        request01();
                    }
                    else
                    {
                        MessageBox.Show("查询SKU不可为空！");
                    }
                    Serialize(guid);
                    break;
                default:
                    return;
            }
        }
        #endregion

        #region 实现ISerialize接口
        public void init(object[] param)
        {
            if (param != null && param.Length > 0)
            {
                if (ds == null)
                {

                    CreateDS();
                    UpdateColumn(false);
                }
                int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                if (pageCount <= 0)
                {
                    pageCount = 1;
                }
                Page.Text = "1/" + pageCount.ToString();
            }
            setButton(false);
            btn03.Enabled = false;
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
                data[(int)DATA_INDEX.SKU] = btn03.Text;
                data[(int)DATA_INDEX.RTV] = tbRTV.Text;
                data[(int)DATA_INDEX.SKUNAME] = tbSKUName.Text;
                data[(int)DATA_INDEX.COUNTAVAIL] = tbCountAvail.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.LASTSKU] = lastSKU;
                data[(int)DATA_INDEX.BTNADD] = btnAdd.Enabled.ToString();
                data[(int)DATA_INDEX.BTNDELE] = btnDelete.Enabled.ToString();
                data[(int)DATA_INDEX.BTN02] = btn02.Enabled.ToString();
                data[(int)DATA_INDEX.BTN03] = btn03.Enabled.ToString();
                data[(int)DATA_INDEX.DATAGUID] = DataGuid;
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
                    btn01.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    btn03.Text = sc.Data[(int)DATA_INDEX.SKU];
                    tbRTV.Text = sc.Data[(int)DATA_INDEX.RTV];
                    tbSKUName.Text = sc.Data[(int)DATA_INDEX.SKUNAME];
                    tbCountAvail.Text = sc.Data[(int)DATA_INDEX.COUNTAVAIL];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    lastSKU = sc.Data[(int)DATA_INDEX.LASTSKU];
                    btn02.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN02]);
                    btn03.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN03]);
                    btnAdd.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNADD]);
                    btnDelete.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNDELE]);
                    DataGuid = sc.Data[(int)DATA_INDEX.DATAGUID];
                    ds = sc.DS;

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

                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    if (pageCount == 0)
                        Page.Text = "1/1";
                    else
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    UpdateColumn(true);
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region UI事件响应

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
                btn03.Enabled = false;
                btn02.Enabled = true;
            }
            catch (Exception)
            {
            }

        }

        private void ClearPage()
        {
            try
            {
                ClearBox();
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btn03.Enabled = true;
                btn02.Enabled = false;
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
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox") && !dt.Columns[colIndex].ColumnName.Equals("门店申请数量"))
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
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
                string SKU = dt.Rows[dgTable.CurrentRowIndex]["SKU"].ToString();
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
                        int row = dgTable.CurrentCell.RowNumber;
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
                if (string.IsNullOrEmpty(btn03.Text))
                {
                    MessageBox.Show("请输入SKU！");
                    return false;
                }

                if (string.IsNullOrEmpty(tbRTV.Text))
                {
                    MessageBox.Show("请输入RTV单号!");
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

                if (int.Parse(tbCount.Text) > int.Parse(tbCountAvail.Text))
                {
                    MessageBox.Show("已超出可用库存！");
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
            int rowflag = 0;
            int Sum = 0;
            DataTable dt = ds.Tables[tableIndex];
            try
            {
                if (ds.Tables[tableIndex].Rows.Count >= 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string SKU = dt.Rows[i]["SKU"].ToString();
                        if (btn01.Text == SKU)
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
                                row["SKU"] = btn01.Text;
                                row["品名"] = tbSKUName.Text;
                                row["门店申请数量"] = tbCount.Text;
                                row["库存"] = tbCountAvail.Text;
                                row["RTV"] = tbRTV.Text;
                                row["CheckBox"] = bCheckAll.ToString();
                                dt.Rows.Add(row);

                                int nCount = ds.Tables[tableIndex].Rows.Count;
                                pageIndex = (int)Math.Ceiling(nCount / (double)TABLE_ROWMAX);
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

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                apiID = API_ID.l0501;
                bc = btn01.Text;
                showScan();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (ds.Tables[tableIndex].Rows.Count > 0)
                    {
                        bc = tbRTV.Text;
                        if (bc != "")
                        {
                            if (CheckSubmit())
                            {
                                apiID = API_ID.l0502;
                                request02();
                            }
                        }
                        else
                        {
                            MessageBox.Show("请输入单号!");
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
                MessageBox.Show(ex.ToString());
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

        private void btn03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (tbBox.Text != "")
                {
                    request03();
                }
                else
                {
                    MessageBox.Show("打印箱号不可为空!");
                }
            }
        }

        private void tbRTV_LostFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (tbRTV.Text != "")
                {
                    request04();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (btn02.Enabled)
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

        private void Clear()
        {
            tbBox.Text = "";
            tbCount.Text = "";
            tbCountAvail.Text = "";
            tbRTV.Text = "";
            tbSKUName.Text = "";
            btn01.Text = "";
            deleteAll();
            btn02.Enabled = false;
            btn03.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                Clear();
            }

        }

        #endregion

    }
}