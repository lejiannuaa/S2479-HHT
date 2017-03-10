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
using System.Runtime.InteropServices;
namespace Hola_Excel
{
    public partial class FormIn : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "A7B883E4-DD89-712E-465B-75578F707C10";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        //指示当前正在发送网络请求
        private bool busy = false;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BCFROM,
            BCTO,
            FROM,
            TO,
            OPUSR,
            TYPE,
            TABLEINDEX,
            PAGEINDEX,
            SORTFLAG,
            COLINDEX,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 6;

        private FormDownload formDownload = null;

        private string xmlFile = null;

        private SerializeClass sc = null;

        public DataSet ds = null;

        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
        private bool bCheckAll = false;
        private string colModify = "";
        //单号或箱号标识
        private bool bcOrDan = false;
        private int rowOld = -1;
        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            Z0301,
            Z0302,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        private int TaskBarHeight = 0;
        public FormIn()
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
                    dgTable.Height = dgTable.Height + 25;
                    TABLE_ROWMAX += 1;
                }
                btnReturn.Top = dstHeight - btnReturn.Height;
                btn01.Top = dstHeight - btn01.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                NextPage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                PrePage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                Page.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                cbOpUsr.Items.Add("");
                cbOpUsr.Items.Add(Config.User);
                cbType.SelectedIndex = 0;
                dtpFrom.Value = new System.DateTime(Config.Year, Config.Month, Config.Day, 0, 0, 0, 0);
                dtpTo.Value = new System.DateTime(Config.Year, 12, 31, 0, 0, 0, 0);
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
                            bCheckAll = false;
                            bNoData = false;
                        }
                    }

                    if (!bNoData)
                    {
                        pageIndex = 1;
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();
                        UpdateColumn(false);
                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                        ds.Dispose();
                        ds = null;
                        UpdateColumn(false);
                    }
                    File.Delete(xmlFile);
                }
                else
                {
                    pageIndex = 1;
                    ds.Dispose();
                    ds = null;
                    Page.Text = "1/1";
                    UpdateColumn(false);
                    MessageBox.Show("请求文件不存在，请重新请求!");
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

                if (bNoData)
                {
                    MessageBox.Show("无数据！");
                    tableIndex = -1;
                }
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
                if (bCustom && name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                }
                else
                {
                    style = new DataGridCustomTextBoxColumn();
                }
                style.Owner = dgTable;
                if (style is DataGridCustomTextBoxColumn)
                {
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
                if (ds == null)
                {
                    dgTable.DataSource = null;
                    dgHeader.DataSource = null;
                    dgTable.Controls.Clear();
                    dgHeader.Controls.Clear();
                }
                else
                {
                    string bcOdan = bcOrDan.Equals(false) ? "单号" : "箱号";
                    string[] colName = new string[] { bcOdan, "类型", "完成日期", "操作人", "CheckBox" };
                    string[] colValue = new string[] { bcOdan, "类型", "完成日期", "操作人", "false" };
                    int[] colWidth = new int[] { 30, 16, 24, 20, 7 };

                    if (!bDeserialize)
                    {
                        DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                        DataRowCollection rows = ds.Tables[tableIndex].Rows;
                        cols.Add("CheckBox");
                        for (int i = 0; i < rows.Count; i++)
                        {
                            rows[i]["CheckBox"] = false;
                        }
                    }

                    dgTable.TableStyles.Clear();
                    dgTable.Controls.Clear();
                    dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                    UpdateRow();

                    DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                    DataRow row = dtHeader.NewRow();
                    for (int i = 0; i < colName.Length; i++)
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
            try
            {
                DataTable dt = ds.Tables[tableIndex];
                string bcOdan = bcOrDan.Equals(false) ? "单号" : "箱号";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (key.Equals(dt.Rows[i][bcOdan].ToString()))
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
            DataTable dtOK = new DataTable();
            try
            {
                dtOK.TableName = "info";
                dtOK.Columns.Add("type");

                DataRow rowNew = dtOK.NewRow();
                rowNew["type"] = cbType.SelectedIndex.ToString();

                dtOK.Rows.Add(rowNew);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dtOK;
        }

        private DataTable getDTDetail()
        {

            DataTable detail = new DataTable();
            try
            {
                if (ds != null)
                {
                    DataTable dt = ds.Tables[tableIndex];
                    detail.TableName = "detail";
                    string bcOdan = bcOrDan.Equals(false) ? "单号" : "箱号";
                    detail.Columns.Add("OddNum");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow rowNew = detail.NewRow();

                        if (bool.Parse(dt.Rows[i]["CheckBox"].ToString()))
                        {
                            rowNew["OddNum"] = dt.Rows[i][bcOdan];

                            detail.Rows.Add(rowNew);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return detail;
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

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.Z0301;

            string op = "01";
            string from = dtpFrom.Text.Replace("-", "");
            string to = dtpTo.Text.Replace("-", "");
            string t = cbType.SelectedIndex.ToString();
            string opusr = cbOpUsr.SelectedIndex > 0 ? Config.User : "";
            string msg = "request=203;usr=" + Config.User + ";op=" + op.ToUpper() + ";from=" + from.ToUpper() + ";to=" + to.ToUpper()
                + ";bcfrom=" + tbBCDanFrom.Text.ToUpper() + ";bcto=" + tbBCDanTo.Text.ToUpper() + ";type=" + t.ToUpper() + ";opusr=" + opusr.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request02()
        {
            apiID = API_ID.Z0302;

            DataTable[] dtSubmit = new DataTable[] { getDTOK(),getDTDetail() };

            string op = "02";
            string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "203", op);
            string msg = "request=203;usr=" + Config.User + ";op=" + op.ToUpper() + ";json=" + json;
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
                    case API_ID.Z0301:
                        {
                            string file = Config.getApiFile("203", "01");
                            from += "/203/" + file;
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
                            UpdateRow();
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                        }
                        else
                        {
                            pageIndex++;
                            MessageBox.Show("已是首页!");
                        }
                        Serialize(guid);
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
                            UpdateRow();
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                        }
                        else
                        {
                            pageIndex--;
                            MessageBox.Show("已是最后一页!");
                        }
                        Serialize(guid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    case API_ID.Z0301:
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
                            showXML();
                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.Z0302:
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

                    default:
                        break;
                }
            }));
        }
        #endregion

        #region UI事件响应
        private void dgTable_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox box = (CheckBox)sender;
                bool bCheck = box.CheckState == CheckState.Checked ? true : false;
                DataTable dt = (DataTable)dgTable.DataSource;
                int rowIndex = dgTable.CurrentCell.RowNumber;

                colModify = "CheckBox";
                string bcOdan = bcOrDan.Equals(false) ? "单号" : "箱号";
                updateData(dt.Rows[rowIndex][bcOdan].ToString(), bCheck.ToString());

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

        private void checkAll(bool bAll)
        {
            bCheckAll = bAll;

            DataTable dt = ds.Tables[tableIndex];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CheckBox"] = bAll;
            }
            UpdateColumn(true);
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
                            if (dtTmp != null && dtTmp.Rows.Count >= 1)
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
                                Serialize(guid);
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
                DataTable dt = (DataTable)dgTable.DataSource;
                if (rowOld >= 0 && rowOld < (dgTable.DataSource as DataTable).Rows.Count)
                {
                    dgTable.UnSelect(rowOld);
                }
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                int rowIndex = dgTable.CurrentCell.RowNumber;
                rowOld = rowIndex;
                if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox"))
                {
                    dgTable.Select(rowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool checkValidation()
        {
            try
            {
                if (tbBCDanFrom.Text == "" || tbBCDanTo.Text == "")
                {
                    MessageBox.Show("请将单号填写完整!");
                    return false;
                }
             
                if (cbType.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择类型!");
                    return false;
                }

                if (cbOpUsr.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择操作人!");
                    return false;
                }
                
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show("起始时间不可大于截止时间!");
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

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null && tableIndex >= 0)
                {
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    rowOld = rowIndex;
                    if (!dt.Columns[colIndex].ColumnName.Equals("CheckBox"))
                    {
                        dgTable.Select(rowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (checkValidation())
                {
                    if (cbType.SelectedIndex == 0)
                    {
                        bcOrDan = false;
                    }
                    else
                    {
                        bcOrDan = true;
                    }
                    request01();
                }
            }
            
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (ds != null)
                {
                    DataTable dt = getDTDetail();
                    if (dt!=null && dt.Rows.Count >= 1)
                    {
                        request02();
                    }
                    else
                    {
                        MessageBox.Show("未选中任何行!");
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Clear(true);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
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
        }

        private bool Dtpfocus()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Focused)
                {
                    return false;
                }
            }

            if(this.Focused)
            {
                return false;
            }
            return true;
        }

        private void Clear(bool bReset)
        {
            try
            {
                tbBCDanFrom.Text = "";
                tbBCDanTo.Text = "";
                cbOpUsr.SelectedIndex = 0;
                if (bReset)
                {
                    cbType.SelectedIndex = 0;
                    dtpFrom.Value = new System.DateTime(Config.Year, Config.Month, Config.Day, 0, 0, 0, 0);
                    dtpTo.Value = new System.DateTime(Config.Year, 12, 31, 0, 0, 0, 0);
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
                ds = null;

                UpdateColumn(false);

                pageIndex = 1;
                Page.Text = "1/1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear(false);
        }

        private void tbBCDanFrom_LostFocus(object sender, EventArgs e)
        {
            if (tbBCDanFrom.Text != "")
            {
                if (tbBCDanTo.Text == "")
                {
                    tbBCDanTo.Text = tbBCDanFrom.Text;
                }
            }
        }

        private void tbBCDanTo_LostFocus(object sender, EventArgs e)
        {
            if (tbBCDanTo.Text != "")
            {
                if (tbBCDanFrom.Text == "")
                {
                    tbBCDanFrom.Text = tbBCDanTo.Text;
                }
            }

        }

        #endregion

        #region 实现ScanCallback接口
        public void scanCallback(string barCode)
        {
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
                data[(int)DATA_INDEX.BCFROM] = tbBCDanFrom.Text;
                data[(int)DATA_INDEX.BCTO] = tbBCDanTo.Text;
                data[(int)DATA_INDEX.FROM] = dtpFrom.Text;
                data[(int)DATA_INDEX.TO] = dtpTo.Text;
                data[(int)DATA_INDEX.OPUSR] = cbOpUsr.Text;
                data[(int)DATA_INDEX.TYPE] = cbType.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                sc.Data = data;
                sc.DS = ds;
                if (ds != null)
                {
                    DataTable dt = ds.Tables[tableIndex];
                    if (dt.DefaultView.Sort.IndexOf(" asc") > 0)
                    {
                        data[(int)DATA_INDEX.SORTFLAG] = "asc";
                    }
                    else
                    {
                        data[(int)DATA_INDEX.SORTFLAG] = "desc";
                    }
                }

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
                    tbBCDanFrom.Text = sc.Data[(int)DATA_INDEX.BCFROM];
                    tbBCDanTo.Text = sc.Data[(int)DATA_INDEX.BCTO];
                    dtpFrom.Value =DateTime.Parse(sc.Data[(int)DATA_INDEX.FROM]);
                    dtpTo.Value = DateTime.Parse(sc.Data[(int)DATA_INDEX.TO]);
                    cbOpUsr.Text = sc.Data[(int)DATA_INDEX.OPUSR];
                    cbType.Text = sc.Data[(int)DATA_INDEX.TYPE];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    ds = sc.DS;
                    if (ds != null&&tableIndex>=0)
                    {
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

 
       
    }
}