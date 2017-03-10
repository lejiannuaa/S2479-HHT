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
using System.Text.RegularExpressions;
using HolaCore;
using System.Runtime.InteropServices;

namespace Hola_Excel
{
    public partial class FormOutDetail : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "AFC8BB1E-DE49-5786-E6CE-424052E00B8F";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        //指示当前正在发送网络请求
        private bool busy = false;

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            TYPE,
            BCBOX,
            FROM,
            TO,
            TRANSPORT,
            DESC,
            TABLEINDEX,
            PAGEINDEX,
            SORTFLAG,
            COLINDEX,
            FROMLOCATION,
            CHILD,
            DATAGUID,
            BTN02,
            BTN03,
            DATAMAX
        }
        #endregion
        private Form child = null;
        private int TABLE_ROWMAX = 6;
        private FormDownload formDownload = null;

        private string xmlFile = null;
        //发送数据标识唯一性
        private string DataGuid = "";
        private int rowFlag = -1;
        private SerializeClass sc = null;

        public DataSet ds = null;
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
        private int rowold = -1;
        private string FromLocation = null;
        Regex reg = new Regex(@"^\d+$");
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
            Z0201,
            Z0202,
            Z0203,
            Z0204,
            DOWNLOAD_XML
        }
        private API_ID apiID = API_ID.NONE;
        #endregion
        private int TaskBarHeight = 0;
        //是否已维护过至少一笔出货数量
        private bool bModified = false;
        public FormOutDetail()
        {
            InitializeComponent();

            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
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
                btn02.Top = dstHeight - btn02.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                NextPage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                PrePage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                Page.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                cbTransport.SelectedIndex = 0;
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
                        DataTable dt = ds.Tables[i];
                        if (dt.TableName.Equals("info"))
                        {
                            tableIndex = i;
                            bNoData = false;
                        }
                        else if (dt.TableName.Equals("bcinfo"))
                        {
                            tbFrom.Text = dt.Rows[0][0].ToString().Equals("null") ? "" : dt.Rows[0][0].ToString();
                            tbTo.Text = dt.Rows[0][1].ToString().Equals("null") ? "" : dt.Rows[0][1].ToString();
                        }
                        else if (dt.TableName.Equals("boxinfo"))
                        {
                            tbBCBox.Text = "共" + dt.Rows.Count.ToString() + "箱";
                            if (dt.Rows[0]["state"].Equals("4Y"))
                            {
                                btn03.Enabled = true;
                            }
                            else
                            {
                                btn03.Enabled = false;
                            }
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
                        UpdateColumn();
                  
                    }
                    else
                    {
                        btn02.Enabled = false;
                        btn03.Enabled = false;
                        pageIndex = 1;
                        Page.Text = "1/1";
                        ds.Dispose();
                        ds = null;
                        UpdateColumn();
                    }
                   
                }
                else
                {
                    btn02.Enabled = false;
                    btn03.Enabled = false;
                    pageIndex = 1;
                    Page.Text = "1/1";
                    ds.Dispose();
                    ds = null;
                    UpdateColumn();
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
                File.Delete(xmlFile);
                if (bNoData)
                {
                    MessageBox.Show("无数据！");
                }
                Serialize(guid);
                QueryPerformanceCounter(ref Stop);
                string str1 = "Serialize ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str1, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
            }
        }

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool header)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables[tableIndex];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;
            
            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = new DataGridCustomTextBoxColumn();
                DataGridColumnStyle style1 = new DataGridTextBoxColumn();
                if (name[i].Equals("实际出货量"))
                {
                    style.Owner = dgTable;
                    style.ReadOnly = true;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    style.AlternatingBackColor = SystemColors.ControlDark;
                    ts.GridColumnStyles.Add(style);
                }
                else if (name[i].Equals("SKU"))
                {
                    if (header)
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
                        style1.MappingName = name[i];
                        style1.Width = (int)(dstWidth * width[i] / 100);
                        ts.GridColumnStyles.Add(style1);
                    }

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

        private void UpdateColumn()
        {
            Start = 0;
            Stop = 0;
            QueryPerformanceCounter(ref Start);
            if (ds == null)
            {
                dgHeader.DataSource = null;
                dgTable.DataSource = null;
                dgHeader.Controls.Clear();
                dgTable.Controls.Clear();
            }
            else
            {
                string[] colName = new string[] {"ID", "SKU", "品名", "预计出货量", "实际出货量", "操作人" };
                string[] colValue = new string[] {"ID", "SKU", "品名", "预计", "实际", "操作人" };
                //int[] colWidth = new int[] { 10, 24, 24, 11, 11, 17 };
                int[] colWidth = new int[] { 10, 19, 29, 11, 11, 17 };

                DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                DataRowCollection rows = ds.Tables[tableIndex].Rows;
                if (!cols.Contains("ID"))
                {
                    cols.Add("ID", System.Type.GetType("System.UInt32"));
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["ID"] = i + 1;
                    }
                } 
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
            }
            QueryPerformanceCounter(ref Stop);
            string str = "Show ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
            Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
        }

        private void UpdateRow()
        {
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
            DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
            DataGridCustomTextBoxColumn colTextBox = null;
            DataTable dtResult = new DataTable();
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("实际出货量"))
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
                    rowNew[name] = dt.Rows[i][name];
                }

                dtResult.Rows.Add(rowNew);
            }

            dgTable.DataSource = dtResult;
            if (colTextBox != null && to > 0)
            {
                TextBox Tbox = (TextBox)colTextBox.HostedControl;
                Tbox.LostFocus += new EventHandler(Tbox_LostFocus);
            }
        }

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                dtOK.TableName = "info";
                dtOK.Columns.Add("TranWay");
                dtOK.Columns.Add("Remark");

                DataRow rowNew = dtOK.NewRow();
                rowNew["TranWay"] = cbTransport.SelectedIndex.ToString();
                rowNew["Remark"] = tbDesc.Text;

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
            DataTable dt = ds.Tables[tableIndex];
            DataTable dtDetail = new DataTable();
            try
            {
                if (dt != null)
                {
                    dtDetail.TableName = "detail";
                    dtDetail.Columns.Add("SKU");
                    dtDetail.Columns.Add("Realoutputs");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow rowNew = dtDetail.NewRow();
                        rowNew["SKU"] = dt.Rows[i]["SKU"];
                        rowNew["Realoutputs"] = dt.Rows[i]["实际出货量"];
                        dtDetail.Rows.Add(rowNew);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dt.Dispose();
            return dtDetail;

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
                        dt.Rows[i]["实际出货量"] = value;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private bool refreshData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string rCount = dt.Rows[i]["实际出货量"].ToString();
                        string eCount = dt.Rows[i]["预计出货量"].ToString();
                        if (rCount.Length <= 9)
                        {
                            if (!reg.IsMatch(rCount))
                            {
                                MessageBox.Show("请输入非负整数!");
                                UpdateColumn();
                                return false;
                            }
                            else
                            {
                                if (UInt64.Parse(rCount) > 0)
                                {
                                    string subtext = rCount.Substring(0, 1);
                                    if (UInt64.Parse(subtext) == 0)
                                    {
                                        MessageBox.Show("请输入非负整数！");
                                        UpdateColumn();
                                        return false;
                                    }
                                    else if (UInt64.Parse(rCount) > UInt64.Parse(eCount))
                                    {
                                        MessageBox.Show("实际出货不可大于预计出货!");
                                        UpdateColumn();
                                        return false;
                                    }
                                    else
                                    {
                                        updateData(dt.Rows[i]["ID"].ToString(), rCount);
                                    }
                                }
                                else if (UInt64.Parse(rCount) == 0)
                                {
                                    if (rCount.Length >= 2)
                                    {
                                        MessageBox.Show("请输入非负整数！");
                                        UpdateColumn();
                                        return false;
                                    }
                                    else
                                    {
                                        updateData(dt.Rows[i]["ID"].ToString(), rCount);
                                    }
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("输入字符过长!");
                            UpdateColumn();
                            return false;
                        }
                    }
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
            apiID = API_ID.Z0201;

            string op = "01";
            string msg = "request=202;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBC.Text.ToUpper() + ";boxbc=" + tbBCBox.Text.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request02()
        {
            try
            {
                apiID = API_ID.Z0202;

                DataTable[] dtSubmit = new DataTable[] { getDTOK(), getDTDetail() };
                DataTable dt = ds.Tables["boxinfo"];
                string state = dt.Rows[0]["state"].ToString();

                string op = "02";
                string json = Config.addJSONConfig(JSONClass.DataTableToString(dtSubmit), "202", op);
                string msg = "request=202;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBC.Text.ToUpper() + ";boxbc=" + ";state="+ state +";HHTESY="+ FromLocation +";json=" + json;
                msg = DataGuid + msg;
                new ConnThread(this).Send(msg);
                wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void request03()
        {
            apiID = API_ID.Z0203;

            string op = "03";
            string msg = "request=202;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBC.Text.ToUpper() + ";type=" + tbType.Text.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request04()
        {
            apiID = API_ID.Z0204;

            string op = "04";
            string msg = "request=202;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBC.Text.ToUpper() + ";type=" + tbType.Text.ToUpper();
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
                    case API_ID.Z0201:
                        {
                            string file = Config.getApiFile("202", "01");
                            from += "/202/" + file;
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
            if (!busy && ds != null)
            {
                if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                {
                    refreshData();
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdateColumn();
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex++;
                        MessageBox.Show("已是首页!");
                    }
                }
                Serialize(guid);
            }
        }

        private void NextPage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                {
                    refreshData();
                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {
                        UpdateColumn();
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex--;
                        MessageBox.Show("已是最后一页!");
                    }
                }
                Serialize(guid);
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
                    case API_ID.Z0201:
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

                    case API_ID.Z0202:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                            btn02.Enabled = false;
                            btn03.Enabled = true;
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
                            btn03.Enabled = false;
                            MessageBox.Show(data);
                        }
                        break;

                    case API_ID.Z0203:
                    case API_ID.Z0204:
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

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
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

                            refreshData();
                            Serialize(guid);
                        }
                    }
                }
            }
        }

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
            refreshData();
        }

        private void tbDesc_GotFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(true);
            }
        }

        private void tbDesc_LostFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(false);
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (rowold >= 0 && rowold < (dgTable.DataSource as DataTable).Rows.Count)
                {
                    dgTable.UnSelect(rowold);
                }
                DataTable dt = (DataTable)dgTable.DataSource;
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                int rowIndex = dgTable.CurrentCell.RowNumber;
                rowold = rowIndex;

                if (!dt.Columns[colIndex].ColumnName.Equals("实际出货量"))
                {
                    dgTable.Select(rowIndex);
                }
            }
        }

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (!busy)
            {
                if (ds != null)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;

                    if (dt != null && dt.Rows.Count >= 1)
                    {
                        rowold = dgTable.CurrentCell.RowNumber;
                        int columnOld = dgTable.CurrentCell.ColumnNumber;
                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            if (hitTest.Row == rowold && hitTest.Column == columnOld)
                            {
                                rowold = hitTest.Row;
                            }
                        }
                    }
                }
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if (busy)
                return;

            contextMenu1.MenuItems.Clear();

            if ((sender as ContextMenu).SourceControl is Button)
            {
                contextMenu1.MenuItems.Add(menuPrintMark);
                contextMenu1.MenuItems.Add(menuPrintTable);
                return;
            }

            if (tableIndex >= 0 && ds!=null)
            {
                DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
                DataTable DT = (DataTable)dgTable.DataSource;
                dgTable.Focus();

                int x = Control.MousePosition.X;
                int y = Control.MousePosition.Y - dgTable.Top-TaskBarHeight;
                int rowOld = dgTable.CurrentCell.RowNumber;
                int columnOld = dgTable.CurrentCell.ColumnNumber;

                DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);

                if (hitTest.Type == DataGrid.HitTestType.Cell)
                {
                    rowFlag = hitTest.Row;
                    contextMenu1.MenuItems.Add(menuDetail);
                    menuDetail.Enabled = true;

                    if (hitTest.Row == rowOld && hitTest.Column == columnOld)
                    {

                    }
                    else
                    {
                        if (columnOld == 4)
                        {
                            refreshData();
                        }
                        dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                        if (hitTest.Column == 4)
                        {
                            dgTable.Select(hitTest.Row);
                        }
                    }
                }
            }
        }

        private void showChild(bool bDeserialize)
        {

            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;

                if (child != null)
                {
                    child.Dispose();
                    child = null;
                }
                child = new FormOutModify();
                if (!bDeserialize)
                {
                    Serialize(guid);
                    string state= ds.Tables["boxinfo"].Rows[0]["state"].ToString();
                     
                    ((ISerializable)child).init(new object[] { tbBC.Text,tbFrom.Text, dt.Rows[rowFlag]["预计出货量"].ToString(),dt.Rows[rowFlag]["实际出货量"].ToString(),dt.Rows[rowFlag]["SKU"].ToString(),state });
                }
                else
                {
                    ((ISerializable)child).Deserialize(null);
                }

                if (DialogResult.OK == child.ShowDialog())
                {
                    bModified = true;
                    dt.Rows[rowFlag]["实际出货量"] = ((FormOutModify)child).ModifiedData("实际出货量");
                    //request01();
                }
                Show();
                child.Dispose();

                child = null;
                Serialize(guid);
            }
            catch (Exception)
            {
            }
        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                showChild(false);
            }
        }

        private void menuPrintMark_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request03();
            }
        }

        private void menuPrintTable_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request04();
            }
        }

        private bool CheckSubmit()
        {
            try
            {
                bool NumCheck = false;
                if (ds != null)
                {
                    DataTable dt = ds.Tables["boxinfo"];
                    DataTable dtt = ds.Tables["info"];
                    if (dt.Rows.Count > 0)
                    {
                        if (!dt.Rows[0]["state"].ToString().Equals("3Y"))
                        {
                            MessageBox.Show("此单据目前不可提交！");
                            return false;
                        }
                    }

                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        string num = dtt.Rows[i]["实际出货量"].ToString();
                        if (num.Equals("0"))
                        {
                            ;
                        }
                        else
                        {
                            NumCheck = true;
                            break;
                        }
                    }
                }
                else
                {
                    return false;
                }

                if (!NumCheck)
                {
                    if (MessageBox.Show("未填写实物出货数量，是否提交?", "",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                    {
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

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(tbBC.Text) && !tbBC.Text.Equals("null"))
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        
                        if (refreshData() && CheckSubmit())
                        {
                            if (!bModified && MessageBox.Show("没有维护实物出货数量，是否确认提交？", "",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                            {
                                return;
                            }
                            
                            request02();
                        }
                    }
                    else
                    {
                        MessageBox.Show("表格内无数据!");
                    }
                }
                else
                {
                    MessageBox.Show("请输入单据号码!");
                }
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (!string.IsNullOrEmpty(tbBC.Text) && !tbBC.Text.Equals("null"))
                {
                    Control btn = (Control)sender;
                    contextMenu1.Show(btn, new Point(0, btn.Height));
                }
                else
                {
                    MessageBox.Show("请检查单据号码!");
                }
            }
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

        #endregion

        #region 实现ScanCallback接口
        public void scanCallback(string barCode)
        {
        }
        #endregion

        #region 实现ISerialize接口

        public void init(object[] param)
        {
            try
            {
                tbBC.Text = (string)param[0];
                tbBCBox.Text = (string)param[1];
                tbType.Text = (string)param[2];
                FromLocation = (string)param[3];
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
                Serialize(guid);
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

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
                data[(int)DATA_INDEX.BARCODE] = tbBC.Text;
                data[(int)DATA_INDEX.TYPE] = tbType.Text;
                data[(int)DATA_INDEX.FROM] = tbFrom.Text;
                data[(int)DATA_INDEX.TO] = tbTo.Text;
                data[(int)DATA_INDEX.BCBOX] = tbBCBox.Text;
                data[(int)DATA_INDEX.TRANSPORT] = cbTransport.Text;
                data[(int)DATA_INDEX.DESC] = tbDesc.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.FROMLOCATION] = FromLocation;
                data[(int)DATA_INDEX.DATAGUID] = DataGuid;
                data[(int)DATA_INDEX.BTN02] = btn02.Enabled.ToString();
                data[(int)DATA_INDEX.BTN03] = btn03.Enabled.ToString();
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
                    tbBC.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    tbType.Text = sc.Data[(int)DATA_INDEX.TYPE];
                    tbFrom.Text = sc.Data[(int)DATA_INDEX.FROM];
                    tbTo.Text = sc.Data[(int)DATA_INDEX.TO];
                    tbBCBox.Text = sc.Data[(int)DATA_INDEX.BCBOX];
                    cbTransport.Text = sc.Data[(int)DATA_INDEX.TRANSPORT];
                    tbDesc.Text = sc.Data[(int)DATA_INDEX.DESC];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    FromLocation = sc.Data[(int)DATA_INDEX.FROMLOCATION];
                    DataGuid = sc.Data[(int)DATA_INDEX.DATAGUID];
                    btn02.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN02]);
                    btn03.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN03]);
                   
                    ds = sc.DS;
                    if (ds != null)
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
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();

                        UpdateColumn();
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

    }
}