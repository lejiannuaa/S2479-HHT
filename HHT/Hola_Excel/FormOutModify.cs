using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using HolaCore;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;
namespace Hola_Excel
{
    public partial class FormOutModify : Form,ISerializable,ConnCallback
    {

        private const string guid = "C9E197C3-ED49-494b-8353-20BAAD3FB65C";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private const string OGuid = "00000000000000000000000000000000";
        //指示当前正在发送网络请求
        private bool busy = false;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            FROM,
            NEED,
            COUNT,
            TABLEINDEX,
            PAGEINDEX,
            DATAGUID,
            STATE,
            DATAMAX
        }
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            Z0401,
            Z0402,
            Z0403,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        private DataSet ds = null;
        //箱状态
        private string state = "";
        private int TABLE_ROWMAX = 8;
        private string xmlFile = null;

        private SerializeClass sc = null;
        private string SKUModify = null;
        private int pageIndex = 1;
        private int tableIndex = -1;
        //发送数据标识唯一性
        private string DataGuid = "";
        Regex reg = new Regex(@"^\d+$");
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        private int TaskBarHeight = 0;
        public FormOutModify()
        {
            InitializeComponent();
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            doLayout();
            QueryPerformanceFrequency(ref freq);
            DialogResult = DialogResult.Abort;
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
                    dgTable.Height = dgTable.Height + 25;
                    TABLE_ROWMAX += 1;
                }
                btn02.Top = dstHeight - btn02.Height;
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
            try
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                ds = new DataSet();
                string[] colName = { "sku", "箱号", "实退数量" };
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
            catch (Exception)
            {
            }
        }


        #region XML数据加载

        private void showXML()
        {
            LoadData();
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
                        if (ds.Tables[i].TableName == "info")
                        {
                            tableIndex = i;
                            bNoData = false;
                            break;
                        }
                    }

                    if (!bNoData)
                    {
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount <= 0)
                        {
                            pageCount = 1;
                        }
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                        UpdateColumn(false);
                        Serialize(guid);
                        btn02.Enabled = true;
                    }
                    else
                    {
                        CreateDS();
                        btn02.Enabled = false;
                        UpdateColumn(false);
                    }
                }
                else
                {
                    CreateDS();
                    btn02.Enabled = false;
                    UpdateColumn(false);
                    MessageBox.Show("请求文件不存在，请重新请求!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);
                if (reader != null)
                    reader.Close();

                if (File.Exists(xmlFile))
                {
                    if (bNoData)
                    {
                        btn02.Enabled = false;
                        MessageBox.Show("无数据！");
                    }
                }
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
                DataGridCustomColumnBase style = new DataGridCustomTextBoxColumn();
                //DataGridColumnStyle style1 = new DataGridTextBoxColumn();
                if (name[i].Equals("sku")&&bCustom)
                {
                    style.Owner = dgTable;
                    style.ReadOnly = true;
                    style.Alignment = HorizontalAlignment.Right;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    style.AlternatingBackColor = SystemColors.ControlDark;
                    ts.GridColumnStyles.Add(style);
                }
                else
                {
                    style.Owner = dgTable;
                    if (bCustom)
                    {
                        if (name[i].Equals("实退数量") && state.Equals("3Y"))
                        {
                            style.ReadOnly = false;
                        }
                        else
                        {
                            style.ReadOnly = true;
                        }
                    }
                    else
                    {
                        style.ReadOnly = true;
                    }
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    style.AlternatingBackColor = SystemColors.ControlDark;
                    ts.GridColumnStyles.Add(style);
                }
                
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

                string[] colName = new string[] { "sku", "箱号", "实退数量" };
                string[] colValue = new string[] { "sku", "箱号", "实退数量" };
                //int[] colWidth = new int[] { 30, 48, 20 };
                int[] colWidth = new int[] { 19, 59, 20 };
                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colName.Length; i++)
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
                    rowNew[name] = dt.Rows[i][name];
                }
                dtResult.Rows.Add(rowNew);
            }

            dgTable.DataSource = dtResult;
        }

        private void updateData(string key, string value)
        {
            try
            {
                DataTable dt = ds.Tables[tableIndex];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (key.Equals(dt.Rows[i]["箱号"].ToString()))
                    {
                        dt.Rows[i]["实退数量"] = value;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private bool updateCount()
        {
            try
            {
                DataTable dtReason = (DataTable)dgTable.DataSource;
                if (dtReason != null && dtReason.Rows.Count > 0)
                {
                    int nCount = 0;
                    bool BUpdate = false;
                    for (int i = 0; i < dtReason.Rows.Count; i++)
                    {
                        string count = dtReason.Rows[i]["实退数量"].ToString();
                        if (!reg.IsMatch(count))
                        {
                            MessageBox.Show("请输入正整数！");
                            return false;
                        }
                        else
                        {

                            if (int.Parse(count) == 0)
                            {
                                if (count.Length == 1)
                                {
                                    nCount += int.Parse(count);
                                    BUpdate = true;
                                    updateData(dtReason.Rows[i]["箱号"].ToString(), count);
                                }
                                else
                                {
                                    MessageBox.Show("请输入正整数！");
                                    return false;
                                }
                            }

                            if (int.Parse(count) > 0)
                            {
                                string subtext = count.Substring(0, 1);
                                if (int.Parse(subtext) == 0)
                                {
                                    MessageBox.Show("请输入正整数！");
                                    return false;
                                }
                                else
                                {
                                    nCount += int.Parse(count);
                                    BUpdate = true;
                                    updateData(dtReason.Rows[i]["箱号"].ToString(), count);
                                }
                            }
                        }

                    }
                    if (BUpdate)
                    {
                        tbCount.Text = nCount.ToString();
                    }

                    UpdateColumn(true);
                    Serialize(guid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private DataTable getDTOK()
        {
            string[] colName = { "sku", "箱号", "实退数量" };
            DataTable dt = ds.Tables[tableIndex];
            DataTable dtOK = new DataTable();
            dtOK.TableName = "info";
            for (int i = 0; i < colName.Length; i++)
            {
                dtOK.Columns.Add(colName[i]);
            }

            dataReal = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rowNew = dtOK.NewRow();

                rowNew["sku"] = dt.Rows[i]["sku"];
                rowNew["箱号"] = dt.Rows[i]["箱号"];
                rowNew["实退数量"] = dt.Rows[i]["实退数量"];
                dataReal += int.Parse(dt.Rows[i]["实退数量"].ToString());
                dtOK.Rows.Add(rowNew);
            }

            return dtOK;
        }

        #endregion

        #region UI响应

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (!busy)
            {
                DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);
                if (hitTest.Type == DataGrid.HitTestType.Cell)
                {
                    if (ds.Tables[tableIndex].Rows.Count >= 1)
                    {
                        updateCount();
                    }
                }
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            if (!busy)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                if (!dt.Columns[colIndex].ColumnName.Equals("实退数量"))
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
                }
            }
        }

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);

                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        int rowOld = dgTable.CurrentCell.RowNumber;
                        int columnOld = dgTable.CurrentCell.ColumnNumber;
                        if (columnOld == 2)
                        {
                            if (hitTest.Row == rowOld && hitTest.Column == columnOld)
                            {
                            }
                            else
                            {
                                updateCount();
                            }
                        }
                    }
                    else
                    {
                        updateCount();
                    }
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
                UpdateColumn(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                try
                {
                    if (!string.IsNullOrEmpty(tbBC.Text) && !tbBC.Text.Equals("null"))
                    {
                        if (state.Equals("3Y"))
                        {
                            if (updateCount())
                            {

                                if (int.Parse(tbCountNeed.Text) < int.Parse(tbCount.Text))
                                {
                                    MessageBox.Show("请检查实退数量！");
                                }
                                else
                                {
                                    request02();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("此单据目前不可提交！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("请检查单据!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
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

        #region 接口请求

        private void request01()
        {
            apiID = API_ID.Z0401;

            string op = "01";

            string msg = "request=204;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBC.Text.ToUpper() + ";sku=" + SKUModify.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);

            wait();
        }

        private void request02()
        {
            apiID = API_ID.Z0402;

            string op = "02";
            DataTable[] dtSubmit = new DataTable[] { getDTOK() };
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "204", op);
            string msg = "request=204;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBC.Text.ToUpper() + ";json=" + json;
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
                    case API_ID.Z0401:
                        {
                            string file = Config.getApiFile("204", "01");
                            from += "/204/" + file;
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
                    case API_ID.Z0401:
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
                            CreateDS();
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.Z0402:
                        if (result == ConnThread.RESULT_OK)
                        {
                            btn02.Enabled = false;
                            deleteAll();
                            MessageBox.Show("请求成功！");
                            DialogResult = DialogResult.OK;
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
                            btn02.Enabled = true;
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.Z0403:
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

        private int dataReal = 0;
        public string ModifiedData(string name)
        {
            if ("实际出货量".Equals(name))
            {
                return dataReal.ToString();
            }

            return "";
        }
        #endregion

        #region 实现ISerialize接口
        public void init(object[] param)
        {
            tbBC.Text = (string)param[0];
            tbFrom.Text = (string)param[1];
            tbCountNeed.Text = (string)param[2];
            tbCount.Text = (string)param[3];
            SKUModify = (string)param[4];
            state = (string)param[5];
            ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
            {
                this.Invoke(new InvokeDelegate(() =>
                {
                    if (ds == null)
                    {
                        request01();
                    }
                }));
            }));
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
                data[(int)DATA_INDEX.BARCODE] = tbBC.Text;
                data[(int)DATA_INDEX.FROM] = tbFrom.Text;
                data[(int)DATA_INDEX.NEED] = tbCountNeed.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.DATAGUID] = DataGuid;
                data[(int)DATA_INDEX.STATE] = state;
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
                    tbBC.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    tbFrom.Text = sc.Data[(int)DATA_INDEX.FROM];
                    tbCountNeed.Text = sc.Data[(int)DATA_INDEX.NEED];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]); ;
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    DataGuid = sc.Data[(int)DATA_INDEX.DATAGUID];
                    state = sc.Data[(int)DATA_INDEX.STATE];
                    ds = sc.DS;
                    if (ds != null)
                    {
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                        UpdateColumn(true);
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