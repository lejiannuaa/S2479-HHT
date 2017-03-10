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
    public partial class FormPrintInquiry : Form, ConnCallback, ISerializable
    {
        private const string guid = "C27149E4-E4DF-E248-4FB6-C0A75E70F52E";


        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        //指示当前正在发送网络请求
        private bool busy = false;
        private const string OGuid = "00000000000000000000000000000000";
        #region 序列化索引
        private enum DATA_INDEX
        {
            BCFROM,
            BCTO,
            BOXFROM,
            BOXTO,
            FROM,
            TO,
            STATE,
            TYPE,
            TABLEINDEX,
            PAGEINDEX,
            CHILD,
            FROMLOCATION,
            SORTFLAG,
            COLINDEX,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 5;
        private FormDownload formDownload = null;
        private Form child = null;
        private int rowFlag = -1;

        private string xmlFile = null;

        private SerializeClass sc = null;

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
            Z0101,
            Z0102,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion
        private int TaskBarHeight = 0;

        #region Form类型ID
        private enum FORM_ID
        {
            NONE,
            Detail,
            DetailF
        }

        private FORM_ID formID = FORM_ID.NONE;
        #endregion
        public FormPrintInquiry()
        {
            InitializeComponent();

            //cbType.SelectedIndex = 0;
            //cbFromLocation.SelectedIndex = 0;

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
                    Config.writeFile(str1, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("detail"))
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

                if (bNoData)
                {
                    MessageBox.Show("无数据！");
                    tableIndex = -1;
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
                    string[] colName = new string[] { "单号", "箱号", "类型", "状态", };
                    int[] colWidth = new int[] { 20, 50, 15, 12 };

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
                    string name = styles[j].MappingName;
                    if (name.Equals("状态"))
                    {
                        rowNew[name] = dt.Rows[i][name].ToString().Equals("Y") ? "成功" : "失败";
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

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.Z0101;
            string op = "01";
            string from = dtpFrom.Text.Replace("-", "");
            string to = dtpTo.Text.Replace("-", "");
            //string t = cbType.SelectedIndex.ToString();
            //string state = "";
            //if (cbState.SelectedIndex == 1)
            //{
            //    state = "y";
            //}
            //else if (cbState.SelectedIndex == 2)
            //{
            //    state = "n";
            //}
            //string frml = cbFromLocation.SelectedIndex.ToString();
            //string msg = "request=201;usr=" + Config.User + ";op=" + op.ToUpper() + ";from=" + from.ToUpper() + ";to=" + to.ToUpper()
            //    + ";bcfrom=" + tbBCDanFrom.Text.ToUpper() + ";bcto=" + tbBCDanTo.Text.ToUpper()
            //    + ";boxbcfrom=" + tbBCBoxFrom.Text.ToUpper() + ";boxbcto=" + tbBCBoxTo.Text.ToUpper() + ";type=" + t.ToUpper() + ";state=" + state.ToUpper()+";frml="+frml.ToUpper();
            //msg = OGuid + msg;
            string msg = "request=201;usr=" + Config.User + ";op=" + op.ToUpper() + ";from=" + from.ToUpper() + ";to=" + to.ToUpper()
              + ";bcfrom=" + ";bcto="
              + ";boxbcfrom=" + tbBCBoxFrom.Text.ToUpper() + ";boxbcto=" + tbBCBoxTo.Text.ToUpper() + ";type=" + ";state=" + ";frml=";
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
                    case API_ID.Z0101:
                        {
                            string file = Config.getApiFile("201", "01");
                            from += "/201/" + file;
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
                    case API_ID.Z0101:
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
                if (rowOld >= 0)
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

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                contextMenu1.MenuItems.Clear();
                if (!busy)
                {

                    if (ds == null)
                    {
                        ;
                    }
                    else
                    {
                        DataTable dt = (DataTable)dgTable.DataSource;

                        int x = Control.MousePosition.X;
                        int y = Control.MousePosition.Y - dgTable.Top - TaskBarHeight;

                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            if (dgTable.CurrentRowIndex != hitTest.Row)
                            {
                                dgTable.UnSelect(dgTable.CurrentRowIndex);
                                dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                            }
                            dgTable.Select(dgTable.CurrentRowIndex);
                            //contextMenu1.MenuItems.Add(menuDetail);

                            if (dt.Rows[dgTable.CurrentRowIndex]["状态"].ToString() == "成功")
                            {
                                contextMenu1.MenuItems.Add(menuCamera);
                            }
                            else
                            {
                            }


                            //加

                            rowFlag = hitTest.Row;
                            dgTable.Select(rowFlag);


                            dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);

                            if (dt.Rows[dgTable.CurrentRowIndex]["状态"].ToString().Equals("成功"))
                            {
                                formID = FORM_ID.Detail;
                            }
                            else
                            {
                                formID = FORM_ID.DetailF;
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

        private void showChild(bool bDeserialize)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                DataRow row = dt.Rows[dgTable.CurrentRowIndex];
                Serialize(guid);
                switch (formID)
                {
                    case FORM_ID.Detail:
                        child = new FormOutDetail();
                        if (bDeserialize)
                        {
                            ((ISerializable)child).Deserialize(null);
                        }
                        else
                        {
                            //((ISerializable)child).init(new object[] { row["单号"].ToString(), row["箱号"].ToString(), row["类型"].ToString(), cbFromLocation.Text });
                            ((ISerializable)child).init(new object[] { row["单号"].ToString(), row["箱号"].ToString(), row["类型"].ToString(), });
                        }
                        break;

                    case FORM_ID.DetailF:
                        child = new FormOutDetailF();
                        if (bDeserialize)
                        {
                            ((ISerializable)child).Deserialize(null);
                        }
                        else
                        {
                            ((ISerializable)child).init(new object[] { row["箱号"].ToString() });
                        }
                        break;

                    default:
                        return;
                }



                if (DialogResult.OK == child.ShowDialog())
                {
                    ;
                }
                Show();
                child.Dispose();

                child = null;
                formID = FORM_ID.NONE;
                Serialize(guid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void menuDetail_Click(object sender, EventArgs e)
        //{
        //    if (!busy)
        //    {
        //        showChild(false);
        //    }
        //}

        private bool checkValidation()
        {
            try
            {
                //if (tbBCDanFrom.Text == "")
                //{
                //    if (tbBCDanTo.Text != "")
                //    {
                //        MessageBox.Show("请将单号截止填全!");
                //        return false;
                //    }
                //}
                //else
                //{
                //    if (tbBCDanTo.Text == "")
                //    {
                //        MessageBox.Show("请将单号截止填全!");
                //        return false;
                //    }
                //}

                if (dtpFrom.Text == "")
                {
                    if (dtpTo.Text != "")
                    {
                        MessageBox.Show("请将截止日期填全!");
                        return false;
                    }
                }
                else
                {
                    if (dtpTo.Text == "")
                    {
                        MessageBox.Show("请将截止日期填全!");
                        return false;
                    }
                }

                if (tbBCBoxFrom.Text == "")
                {
                    if (tbBCBoxTo.Text != "")
                    {
                        MessageBox.Show("请将截止箱号填全!");
                        return false;
                    }
                }
                else
                {
                    if (tbBCBoxTo.Text == "")
                    {
                        MessageBox.Show("请将截止箱号填全!");
                        return false;
                    }
                }

                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show("起始时间不可大于截止时间!");
                    return false;
                }

                //if (cbType.SelectedIndex < 0)
                //{
                //    MessageBox.Show("请选择单据类型!");
                //    return false;
                //}
                //if (cbFromLocation.SelectedIndex < 0)
                //{
                //    MessageBox.Show("请选择发起方!");
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
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
                    if (DialogResult.Yes == MessageBox.Show("是否放弃当前操作？", "", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        File.Delete(Config.DirLocal + guid);
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Clear(bool bReset)
        {
            try
            {
                //tbBCDanFrom.Text = "";
                //tbBCDanTo.Text = "";
                tbBCBoxFrom.Text = "";
                tbBCBoxTo.Text = "";

                //cbState.SelectedIndex = -1;

                if (bReset)
                {
                    dtpFrom.Value = new System.DateTime(Config.Year, Config.Month, Config.Day, 0, 0, 0, 0);
                    dtpTo.Value = new System.DateTime(Config.Year, 12, 31, 0, 0, 0, 0);
                }

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

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Clear(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbFromLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Clear(false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbBCDanFrom_LostFocus(object sender, EventArgs e)
        {
            //if (tbBCDanFrom.Text != "")
            //{
            //    if (tbBCDanTo.Text == "")
            //    {
            //        tbBCDanTo.Text = tbBCDanFrom.Text;
            //    }
            //}
        }

        private void tbBCDanTo_LostFocus(object sender, EventArgs e)
        {
            //if (tbBCDanTo.Text != "")
            //{
            //    if (tbBCDanFrom.Text == "")
            //    {
            //        tbBCDanFrom.Text = tbBCDanTo.Text;
            //    }
            //}
        }

        private void tbBCBoxFrom_LostFocus(object sender, EventArgs e)
        {
            if (tbBCBoxFrom.Text != "")
            {
                if (tbBCBoxTo.Text == "")
                {
                    tbBCBoxTo.Text = tbBCBoxFrom.Text;
                }
            }
        }

        private void tbBCBoxTo_LostFocus(object sender, EventArgs e)
        {
            if (tbBCBoxTo.Text != "")
            {
                if (tbBCBoxFrom.Text == "")
                {
                    tbBCBoxFrom.Text = tbBCBoxTo.Text;
                }
            }
        }

        #region delegates 传参

        public delegate void PageTitle(Form form);

        #endregion

        private string photoName1 = "";
        private string photoName2 = "";
        private string photoName3 = "";
        //组合照片名
        private string CameraPhoto;

        private void getPhotoName(string photo1, string photo2, string photo3)
        {
            if (photo1 != null && photo1 != "")
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

            CameraPhoto = photoName1 + "," + photoName2 + "," + photoName3;

        }
        private bool isPO = false;
        void menuCamera_Click(object sender, System.EventArgs e)
        {

            //FormCamera formCamera = new FormCamera();
            //PageTitle pageTitle = new PageTitle(formCamera.getTitle);
            //pageTitle(this);
            //formCamera.Show();


            //DataTable dt = (DataTable)dgTable.DataSource;
            //if (dt.Rows[dgTable.CurrentRowIndex]["状态"].ToString() == "失败")
            //{
            //    MessageBox.Show("不可照相");
            //}

            FormCamera formCamera = new FormCamera(photoName1, photoName2, photoName3,isPO);
            //formCamera.MyPhotoEvent += new FormCamera.MyPhotoDelegate(getPhotoName);
            PageTitle pageTitle = new PageTitle(formCamera.getTitle);
            pageTitle(this);
            formCamera.ShowDialog();

            DataRowCollection rows = ds.Tables[tableIndex].Rows;
            //rows[rowFlag]["photo"] = formCamera.photoName();

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
                //data[(int)DATA_INDEX.BCFROM] = tbBCDanFrom.Text;
                //data[(int)DATA_INDEX.BCTO] = tbBCDanTo.Text;
                data[(int)DATA_INDEX.BOXFROM] = tbBCBoxFrom.Text;
                data[(int)DATA_INDEX.BOXTO] = tbBCBoxTo.Text;
                data[(int)DATA_INDEX.FROM] = dtpFrom.Text;
                data[(int)DATA_INDEX.TO] = dtpTo.Text;
                //data[(int)DATA_INDEX.FROMLOCATION] = cbFromLocation.Text;
                //data[(int)DATA_INDEX.STATE] = cbState.Text;
                //data[(int)DATA_INDEX.TYPE] = cbType.Text;
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
                    //tbBCDanFrom.Text = sc.Data[(int)DATA_INDEX.BCFROM];
                    //tbBCDanTo.Text = sc.Data[(int)DATA_INDEX.BCTO];
                    tbBCBoxFrom.Text = sc.Data[(int)DATA_INDEX.BOXFROM];
                    tbBCBoxTo.Text = sc.Data[(int)DATA_INDEX.BOXTO];
                    dtpFrom.Value = DateTime.Parse(sc.Data[(int)DATA_INDEX.FROM]);
                    dtpTo.Value = DateTime.Parse(sc.Data[(int)DATA_INDEX.TO]);
                    //cbFromLocation.Text = sc.Data[(int)DATA_INDEX.FROMLOCATION];
                    //cbState.Text = sc.Data[(int)DATA_INDEX.STATE];
                    //cbType.Text = sc.Data[(int)DATA_INDEX.TYPE];
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