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
    public partial class FormInWave : Form, ConnCallback, ISerializable
    {
        private const string guid = "AD24ACEA-1C58-451e-935B-A3D6E41BC112";

        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        //指示当前正在发送网络请求
        private bool busy = false;
        private const string OGuid = "00000000000000000000000000000000";
        #region 序列化索引
        private enum DATA_INDEX
        {
            WAVE,
            TYPE,
            TOTAL,
            OK,
            READY,
            TABLEINDEX,
            PAGEINDEX,
            CHILD,
            FROMLOCATION,
            SORTFLAG,
            COLINDEX,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 7;
        private FormDownload formDownload = null;
        private Form child = null;

        private string xmlFile = null;

        private SerializeClass sc = null;

        //最近一次查询波次号
        private string oldWave = "";

        public DataSet ds = null;
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
        private int rowOld = -1;
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
            E0001= 1,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion
        private int TaskBarHeight = 0;

        #region Form类型ID
        private enum FORM_ID
        {
            NONE
        }

        private FORM_ID formID = FORM_ID.NONE;
        #endregion
        public FormInWave()
        {
            InitializeComponent();
            cbType.SelectedIndex = 0;
            cbState.SelectedIndex = 0;

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
                    Config.writeFile(str1, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("detail"))
                        {
                            tableIndex = i;
                            bNoData = false;
                        }
                        else if (ds.Tables[i].TableName.Equals("info"))
                        {
                            tbTotal.Text = ds.Tables[i].Rows[0]["总共箱数"].ToString();
                            tbOK.Text = ds.Tables[i].Rows[0]["已收箱数"].ToString();
                            tbReady.Text = ds.Tables[i].Rows[0]["未收箱数"].ToString();
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
                        pageIndex = 1;
                        Page.Text = "1/1";
                        ds.Dispose();
                        ds = null;
                        MessageBox.Show("无数据！");
                        UpdateColumn();
                    }
                    File.Delete(xmlFile);
                }
                else
                {
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

                Serialize(guid);
                QueryPerformanceCounter(ref Stop);
                string str1 = "Serialize ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str1, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);
            }
        }

        private DataGridTableStyle getTableStyle(string[] name, int[] width)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables[tableIndex];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;

            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = new DataGridCustomTextBoxColumn();
                style.Owner = dgTable;
                style.ReadOnly = true;
                style.MappingName = name[i];
                style.Width = (int)(dstWidth * width[i] / 100);
                style.AlternatingBackColor = SystemColors.ControlDark;
                ts.GridColumnStyles.Add(style);
            }

            return ts;
        }

        private void UpdateColumn()
        {
            try
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
                    string[] colName = new string[] { "箱号", "状态", };
                    int[] colWidth = new int[] { 70, 28 };

                    dgTable.TableStyles.Clear();
                    dgTable.Controls.Clear();
                    dgTable.TableStyles.Add(getTableStyle(colName, colWidth));
                    UpdateRow();

                    DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                    DataRow row = dtHeader.NewRow();
                    for (int i = 0; i < colName.Length; i++)
                    {
                        row[i] = colName[i];
                    }
                    dtHeader.Rows.Add(row);
                    dgHeader.TableStyles.Clear();
                    dgHeader.Controls.Clear();
                    dgHeader.TableStyles.Add(getTableStyle(colName, colWidth));
                    dgHeader.DataSource = dtHeader;
                }
                QueryPerformanceCounter(ref Stop);
                string str = "Show ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);
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
                    rowNew[j] = dt.Rows[i][j];
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

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.E0001;
            string op = "01";
            string type = cbType.Text;
            string msg = "request=300;usr=" + Config.User + ";op=" + op.ToUpper() + ";hhttype=" + type + ";hhtwav=" + tbWave.Text + ";hhtstat=" + cbState.SelectedIndex ;
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
                    case API_ID.E0001:
                        {
                            string file = Config.getApiFile("300", "01");
                            from += "/300/" + file;
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
                    }
                    Serialize(guid);
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
                    case API_ID.E0001:
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

                    default:
                        break;
                }
            }));
        }
        #endregion

        #region UI事件响应
        private void showChild(bool bDeserialize)
        {
            try
            {
                if (child != null)
                {
                    child.Dispose();
                    child = null;
                }

                //switch (formID)
                //{
                //    default:
                //        return;
                //}

                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }
                else
                {
                    Serialize(guid);
                }

                child.ShowDialog();
                Show();
                child.Dispose();

                child = null;
                formID = FORM_ID.NONE;
                Serialize(guid);
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
                                DataColumn col = dt.Columns[hitTest.Column];

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

                                UpdateColumn();
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

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (ds != null)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (dt != null && dt.Rows.Count >= 1)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbWave_TextChanged(object sender, EventArgs e)
        {
            if (!oldWave.Equals(tbWave.Text))
            {
                oldWave = tbWave.Text;
                tbTotal.Text = "";
                tbReady.Text = "";
                tbOK.Text = "";
            }
        }

        private bool checkValidation()
        {
            try
            {
                if (tbWave.Text == "")
                {
                    MessageBox.Show("请将波次号填全!");
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

        private void tbWave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!busy && e.KeyChar == (char)13)
            {
                if (checkValidation())
                {
                    request01();
                }
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (checkValidation())
                {
                    request01();
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

        private void Clear(bool SEL)
        {
            try
            {
                tbWave.Text = "";
                tbTotal.Text = "";
                tbOK.Text = "";
                tbReady.Text = "";

                cbType.SelectedIndex = 0;

                if (ds != null)
                {
                    ds.Dispose();
                }
                ds = null;

                UpdateColumn();

                pageIndex = 1;
                Page.Text = "1/1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                data[(int)DATA_INDEX.WAVE] = tbWave.Text;
                data[(int)DATA_INDEX.TYPE] = cbType.Text;
                data[(int)DATA_INDEX.TOTAL] = tbTotal.Text;
                data[(int)DATA_INDEX.OK] = tbOK.Text;
                data[(int)DATA_INDEX.READY] = tbReady.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.COLINDEX] = colIndex.ToString();
                data[(int)DATA_INDEX.CHILD] = formID.ToString();
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
                    tbWave.Text = sc.Data[(int)DATA_INDEX.WAVE];
                    cbType.Text = sc.Data[(int)DATA_INDEX.TYPE];
                    tbTotal.Text = sc.Data[(int)DATA_INDEX.TOTAL];
                    tbOK.Text = sc.Data[(int)DATA_INDEX.OK];
                    tbReady.Text = sc.Data[(int)DATA_INDEX.READY];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    colIndex = int.Parse(sc.Data[(int)DATA_INDEX.COLINDEX]);
                    ds = sc.DS;

                    if (ds != null && tableIndex >= 0)
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

                    UpdateColumn();

                    formID = (FORM_ID)Enum.Parse(typeof(FORM_ID), sc.Data[(int)DATA_INDEX.CHILD], true);
                    if (formID != FORM_ID.NONE)
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