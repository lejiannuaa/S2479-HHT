using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using HolaCore;
using System.Runtime.InteropServices;
namespace Hola_Out
{
    public partial class FormPurchase2 : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "B0D539EC-DC05-107F-A358-94039F189438";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        //指示当前正在发送网络请求
        private bool busy = false;

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            FROM,
            TO,
            TOID,
            TONAME,
            NEED,
            COUNT,
            TABLEINDEX,
            PAGEINDEX,
            CHILD,
            COLINDEX,
            LASTBC,
            SORTFLAG,
            DATAMAX
        }
        #endregion

        private  int TABLE_ROWMAX = 6;
        private FormDownload formDownload = null;
        private FormScan formScan = null;
        private Form child = null;

        private string xmlFile = null;
        //发送数据标识唯一性
        private string DataGuid = "";
        private SerializeClass sc = null;
        //保留单号
        private string lastBC = null;
        public DataSet ds = null;
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
        private int ReSum = 0;
        private int TrSum = 0;
        private bool bRtv = true;
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        private int TaskBarHeight = 0;
        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            l0601,
            l0602,
            l0603,
            DOWNLOAD_01
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormPurchase2()
        {
            InitializeComponent();
          
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
            Serialize(guid);
        }

        private bool Load01(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                ds = new DataSet();
                ds.ReadXml(reader);

                QueryPerformanceCounter(ref Stop);
                string str1 = "loadXML ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str1, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    if (dt.TableName.Equals("info"))
                    {
                        tableIndex = i;
                        bNoData = false;
                    }
                    if (dt.TableName == "bcinfo")
                    {
                        bRtv = false;
                        labelTo.Text = dt.Rows[0][0].ToString().Equals("null") ? "" : dt.Rows[0][0].ToString() + ":";
                        tbToID.Text = dt.Rows[0][1].ToString().Equals("null") ? "" : dt.Rows[0][1].ToString();
                        tbToName.Text = dt.Rows[0][2].ToString().Equals("null") ? "" : dt.Rows[0][2].ToString();

                    }
                }
                if (!bRtv)
                {
                    if (!bNoData)
                    {
                        if (string.IsNullOrEmpty(lastBC) || !lastBC.Equals(btn01.Text))
                        {
                            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
                        }
                        pageIndex = 1;
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount <= 0)
                        {
                            pageCount = 1;
                        }
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                        UpdateColumn(false);
                        if (ds.Tables[tableIndex].Rows.Count > 0)
                        {
                            btn02.Enabled = true;
                            btn03.Enabled = false;
                        }
                        else
                        {
                            bNoData = true;
                            btn02.Enabled = false;
                            btn03.Enabled = false;
                        }
                    }
                    else
                    {
                        pageIndex = 1;
                        ClearBox();
                        CreateDS();
                        UpdateColumn(false);
                        btn02.Enabled = false;
                        btn03.Enabled = false;
                    }
                }
                else
                {
                    pageIndex = 1;
                    ClearBox();
                    ds = new DataSet();
                    CreateDS();
                    UpdateColumn(false);
                    btn02.Enabled = false;
                    btn03.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            lastBC = btn01.Text;
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

                            if (!bRtv && bNoData)
                            {
                                bRtv = true;
                                MessageBox.Show("无数据！");
                            }
                            else if (bRtv && bNoData)
                            {
                                btn01.Text = "";
                                MessageBox.Show("单号不存在!");
                            }
                            else
                            {
                                bRtv = true;
                            }
                        }
                        else
                        {
                            pageIndex = 1;
                            ClearBox();
                            ds = new DataSet();
                            CreateDS();
                            UpdateColumn(false);
                            btn02.Enabled = true;
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
                //DataGridCustomColumnBase style = new DataGridCustomTextBoxColumn();
                //style.Owner = dgTable;
                //style.ReadOnly = true;
                //style.MappingName = name[i];
                //style.Width = (int)(dstWidth * width[i] / 100);
                //style.AlternatingBackColor = SystemColors.ControlDark;
                //ts.GridColumnStyles.Add(style);
                DataGridCustomColumnBase style = new DataGridCustomTextBoxColumn();


                if (name[i].Equals("SKU"))
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
                string[] colName = new string[] { "ID", "SKU", "品名", "要求总数", "实退数量", "差异" };
                string[] colValue = new string[] { "ID", "SKU", "品名", "要求", "实退", "差异" };
                //int[] colWidth = new int[] { 9, 24, 23, 15, 15, 11 };
                int[] colWidth = new int[] { 9, 19, 28, 15, 15, 11 };

                DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                DataRowCollection rows = ds.Tables[tableIndex].Rows;
                if (!cols.Contains("ID"))
                {
                    cols.Add("ID", System.Type.GetType("System.UInt32"));
                }

                if (!cols.Contains("差异"))
                {
                    cols.Add("差异");
                }
                if (!bDeserialize)
                {
                    DataTable dt = ds.Tables[tableIndex];

                    if (dt.TableName.Equals("info"))
                    {
                        for (int i = 0; i < rows.Count; i++)
                        {
                            rows[i]["ID"] = i + 1;
                        }
                        for (int i = 0; i < rows.Count; i++)
                        {
                            string total = rows[i]["实退数量"].ToString();
                            string ok = rows[i]["要求总数"].ToString();
                            rows[i]["差异"] = (int.Parse(total) - int.Parse(ok)).ToString();
                        }

                        if (dt.Rows.Count >= 1)
                        {
                            ReSum = 0;
                            TrSum = 0;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                ReSum += int.Parse(dt.Rows[j]["要求总数"].ToString());
                                TrSum += int.Parse(dt.Rows[j]["实退数量"].ToString());
                            }
                        }
                        if (ReSum >= 0 && TrSum >= 0)
                        {
                            tbCountNeed.Text = ReSum.ToString();
                            tbCount.Text = TrSum.ToString();
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
                    row[i] = colValue[i];
                }
                dtHeader.Rows.Add(row);
                dgHeader.TableStyles.Clear();
                dgHeader.Controls.Clear();
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
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
            DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
            DataTable dtResult = new DataTable();
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
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

                    if (name.Equals("差异"))
                    {
                        string total = dt.Rows[i]["实退数量"].ToString();
                        string ok = dt.Rows[i]["要求总数"].ToString();
                        rowNew[name] = (int.Parse(total) - int.Parse(ok)).ToString();
                    }
                    else
                    {
                        rowNew[name] = dt.Rows[i][name];
                    }
                }

                dtResult.Rows.Add(rowNew);
            }

            dgTable.DataSource = dtResult;           
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

        //创建包含列空数据集
        private void CreateDS()
        {
            string[] colName = { "SKU", "品名", "要求总数", "实退数量"};
            DataTable dt = new DataTable();
            dt.TableName = "info";
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
            btn01.Text = "";
            tbCount.Text = "";
            tbCountNeed.Text = "";
            tbToID.Text = "";
            tbToName.Text = "";
            labelTo.Text = "调入地:";
        }

        #region 接口请求
        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.l0601:
                    {
                        string file = Config.getApiFile("106", "01");
                        from += "/106/" + file;
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
            string msg = "request=106;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            if (op.Equals("02"))
            {
                msg = DataGuid + msg;
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
            apiID = API_ID.l0601;

            request("01", btn01.Text);
        }

        private void request02()
        {
            apiID = API_ID.l0602;

            request("02", btn01.Text);
        }

        private void request03()
        {
            apiID = API_ID.l0603;

            request("03", btn01.Text);
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
                    case API_ID.l0601:
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
                            btn02.Enabled = false;
                            btn03.Enabled = false;
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.l0602:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ClearPage();
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
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.l0603:
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
                case API_ID.l0601:
                    if (barCode != "")
                    {
                        btn01.Text = barCode;
                        deleteAll();
                        request01();
                    }
                    else
                    {
                        MessageBox.Show("查询箱号不可为空!");
                        btn01.Text = "";
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
            ds = new DataSet();
            CreateDS();
            btn02.Enabled = false;
            btn03.Enabled = !btn02.Enabled;
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
                data[(int)DATA_INDEX.TO] = labelTo.Text;
                data[(int)DATA_INDEX.TOID] = tbToID.Text;
                data[(int)DATA_INDEX.TONAME] = tbToName.Text;
                data[(int)DATA_INDEX.NEED] = tbCountNeed.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.LASTBC] = lastBC;
                if (child == null)
                {
                    data[(int)DATA_INDEX.CHILD] = "";
                }
                else
                {
                    data[(int)DATA_INDEX.CHILD] = child.GetType().ToString();
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
                    labelTo.Text = sc.Data[(int)DATA_INDEX.TO];
                    tbToID.Text = sc.Data[(int)DATA_INDEX.TOID];
                    tbToName.Text = sc.Data[(int)DATA_INDEX.TONAME];
                    tbCountNeed.Text = sc.Data[(int)DATA_INDEX.NEED];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    lastBC = sc.Data[(int)DATA_INDEX.LASTBC];
                    ds = sc.DS;
                    DataTable dt = ds.Tables[tableIndex];
                    if (colIndex >= 0)
                    {
                        DataColumn col = dt.Columns[colIndex];
                        if (sc.Data[(int)DATA_INDEX.SORTFLAG] == "asc")
                        {
                            dt.DefaultView.Sort = col.ColumnName.ToString() + " asc";
                        }
                        else
                        {
                            dt.DefaultView.Sort = col.ColumnName.ToString() + " desc";
                        }
                    }
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    if (pageCount == 0)
                        Page.Text = "1/1";
                    else
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    UpdateColumn(true);
                    if(!string.IsNullOrEmpty(sc.Data[(int)DATA_INDEX.CHILD]))
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
                btn03.Enabled = true;
                btn02.Enabled = false;
            }
            catch (Exception)
            {
            }

        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (ds != null)
                    {
                        DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            DataTable dtTmp = (DataTable)(dgTable.DataSource);
                            if (dtTmp != null && dtTmp.Rows.Count > 0)
                            {
                                string colName = dtTmp.Columns[hitTest.Column].ColumnName;

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

                                    UpdateColumn(true);
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
            int rowIndex = dgTable.CurrentCell.RowNumber;
            dgTable.Select(rowIndex);
        }

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgTable.DataSource;
            if (dt != null && dt.Rows.Count >= 1)
            {
                int rowIndex = dgTable.CurrentCell.RowNumber;
                dgTable.Select(rowIndex);
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                contextMenu1.MenuItems.Clear();
                if (!busy)
                {
                    int x = Control.MousePosition.X;
                    int y = Control.MousePosition.Y - dgTable.Top - TaskBarHeight;

                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                       
                        if (btn02.Enabled)
                        {
                            contextMenu1.MenuItems.Add(this.menuModify);
                        }
                        if (dgTable.CurrentRowIndex != hitTest.Row)
                        {
                            dgTable.UnSelect(dgTable.CurrentRowIndex);
                            dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                        }
                        dgTable.Select(dgTable.CurrentRowIndex);
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
                DataRow row = null;
                child = new FormPurchase2Ex();
                if (dt.Rows.Count >= 1)
                {
                    row = dt.Rows[dgTable.CurrentRowIndex];
                }
                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }
                else
                {
                    Serialize(guid);
                    string ss = row["SKU"].ToString();
                    if (row != null)
                    {
                        ((ISerializable)child).init(new object[] { btn01.Text, tbToID.Text, row["要求总数"].ToString(), row["实退数量"].ToString(), row["SKU"].ToString() });
                    }
                }

                if (DialogResult.OK == child.ShowDialog())
                {
                    request01();
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

        private void menuModify_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                Serialize(guid);
                showChild(false);
                
            }
        }

        private void showScan()
        {
            if (!busy)
            {
                formScan = new FormScan();
                formScan.init(this, btn01.Text);
                formScan.ShowDialog();
                formScan.Dispose();
                formScan = null;
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
                    string ColReValue = dt.Rows[i]["实退数量"].ToString();
                    string ColNeedValue = dt.Rows[i]["要求总数"].ToString();
                    if (int.Parse(ColReValue) > int.Parse(ColNeedValue))
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
                            MessageBox.Show("请检查实退数量于第" + rowindex.ToString() + "行");

                        }
                        else
                        {
                            MessageBox.Show("请检查实退数量于第" + (rowindex % TABLE_ROWMAX).ToString() + "行");

                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                apiID = API_ID.l0601;
                showScan();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (btn01.Text != "")
                {
                    if (ds.Tables[tableIndex].Rows.Count > 0)
                    {
                        if (CheckSubmit())
                        {
                            apiID = API_ID.l0602;
                            request02();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请输入单号!");
                }
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            if(!busy)
            {
                if (string.IsNullOrEmpty(btn01.Text) && btn01.Text.Equals("null"))
                {
                    apiID = API_ID.l0603;
                    request03();
                }
                else
                {
                    MessageBox.Show("请检查单号!");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
       
    }
}