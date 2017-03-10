using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using System.IO;
using HolaCore;
namespace Hola_Excel
{
    public partial class FormOutDetailF : Form, ConnCallback, ISerializable
    {
        private const string guid = "616E4F4B-B9E6-4e7c-80EF-42402BDF6918";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private const string OGuid = "00000000000000000000000000000000";
        //指示当前正在发送网络请求
        private bool busy = false;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            MANUID,
            MANUNAME,
            TABLEINDEX,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 6;
        private FormDownload formDownload = null;
        private SerializeClass sc = null;
        private string xmlFile = null;
        public DataSet ds = null;
        private int pageIndex = 1;
        private int tableIndex = -1;
        private int colIndex = -1;
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
            Z0501,
            DOWNLOAD_XML
        }
        private API_ID apiID = API_ID.NONE;
        #endregion
        public FormOutDetailF()
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
                        DataTable dt = ds.Tables[i];
                        if (dt.TableName.Equals("info"))
                        {
                            tbDescID.Text = dt.Rows[0]["tlcid"].ToString();
                            tbDescName.Text = dt.Rows[0]["tlcnm"].ToString();
                        }
                        else if (dt.TableName.Equals("detail"))
                        {
                            tableIndex = i;
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
                        UpdateColumn();

                    }
                    else
                    {

                        pageIndex = 1;
                        Page.Text = "1/1";
                        ds.Dispose();
                        ds = null;
                        UpdateColumn();
                    }
                
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
                DataGridColumnStyle style1 = new DataGridTextBoxColumn();
                style1.MappingName = name[i];
                style1.Width = (int)(dstWidth * width[i] / 100);
                ts.GridColumnStyles.Add(style1);
                
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
                string[] colName = new string[] { "ID", "SKU", "reason",  };
                string[] colValue = new string[] { "ID","SKU", "退货原因" };
                int[] colWidth = new int[] { 10, 44, 44 };

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
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth));
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
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth));
                dgHeader.DataSource = dtHeader;
            }
            QueryPerformanceCounter(ref Stop);
            string str = "Show ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
            Config.writeFile(str, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);
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
            apiID = API_ID.Z0501;

            string op = "01";
            string msg = "request=205;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + tbBCBox.Text.ToUpper();
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
                    case API_ID.Z0501:
                        {
                            string file = Config.getApiFile("205", "01");
                            from += "/205/" + file;
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
            }
        }

        private void NextPage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                if (tableIndex >= 0 && ds.Tables[tableIndex].Rows.Count > 0)
                {
                   
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
                    case API_ID.Z0501:
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

        #region 实现ISerialize接口

        public void init(object[] param)
        {
            try
            {
                tbBCBox.Text = (string)param[0];
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
                data[(int)DATA_INDEX.BARCODE] = tbBCBox.Text;
                data[(int)DATA_INDEX.MANUID] = tbDescID.Text;
                data[(int)DATA_INDEX.MANUNAME] = tbDescName.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
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
                    tbBCBox.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    tbDescID.Text = sc.Data[(int)DATA_INDEX.MANUID];
                    tbDescName.Text = sc.Data[(int)DATA_INDEX.MANUNAME];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    ds = sc.DS;
                    if (ds != null)
                    {
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();

                        UpdateColumn();
                    }
                }
            }
            catch (Exception)
            {
            }
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
                            UpdateColumn();
                        }
                    }
                }
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            if (!busy)
            {
                int rowIndex = dgTable.CurrentCell.RowNumber;
                dgTable.Select(rowIndex);
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
    }
}