using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Xml.Serialization;
using System.Threading;
using HolaCore;
using System.Runtime.InteropServices;
namespace Hola_In
{
    public partial class FormReason : Form, ConnCallback, ISerializable
    {

        private const string guid = "EB1D85AF-45F8-E8B3-194F-EDF069A0E5A4";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        //第一次请求该页面后保留差异原因明细回传给上一页
        public delegate void getDtReason(DataSet dt);
        public getDtReason GetDtReason;

        //指示当前正在发送网络请求
        private bool busy = false;
        //private bool bCheckAll = false;//

        private int TABLE_ROWMAX = 7;

        private FormDownload formDownload = null;

        private string xmlFile = null;

        private SerializeClass sc = null;
        private int TaskBarHeight = 0;
        public DataSet ds = null;
        private string TableState = "";
        private DataTable dtDiff = null;
        private int tableIndex = -1;
        private int pageIndex = 1;
		//
        private int rowFlag = -1;
        private bool bInit = false;
        private bool bCount = false;
        Regex reg = new Regex(@"^\d+$");
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        private string isBDorPO = "";

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            O0301,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            SKU,
            NAME,
            TOTAL,
            COUNT,
            TABLEINDEX,
            PAGEINDEX,
            TABLESTATE,
            DATAMAX
        }
        #endregion

        public FormReason(string text)
        {
            InitializeComponent();
            isBDorPO = text;
            doLayout();
            QueryPerformanceFrequency(ref freq);
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                int yOffset = dstHeight - btnReturn.Height - btnReturn.Top;

                if (dstWidth > 240)
                {
                    dgTable.Height = dgTable.Height + 25;
                    TABLE_ROWMAX += 1;
                }

                btnReturn.Top = dstHeight - btnReturn.Height;
                btnConfirm.Top = dstHeight - btnConfirm.Height;

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
            Serialize(guid);
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
                    Cursor.Current = Cursors.WaitCursor;
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
                        pageIndex = 1;
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();
                        UpdateColumn(false);
                    }
                }
                else
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
                    ds.Dispose();
                    ds = null;
                    MessageBox.Show("请求文件不存在,请重新请求!");
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
                    pageIndex = 1;
                    Page.Text = "1/1";
                    ds.Dispose();
                    ds = null;
                    MessageBox.Show("无数据！");
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
                style.Owner = dgTable;
				//加CheckBox
                if (bCustom && name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                }
                if (bCustom && name[i].Equals("申诉数量")&&TableState.Equals("0"))
                {
                    style.ReadOnly = false;
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

            return ts;
        }
        private void UpdateColumn(bool bDeserialize)
        {
            try
            {

                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);
                //string[] colName = new string[] { "ID", "desc", "申诉数量" };
                //string[] colValue = new string[] { "ID", "申诉退货原因", "申诉数量" };
                //int[] colWidth = new int[] { 10, 44, 44 };

                string[] colName = new string[] { "ID", "desc", "申诉数量", "photo" };
                string[] colValue = new string[] { "ID", "申诉退货原因", "申诉数量", "photo" };
                int[] colWidth = new int[] { 10, 44, 43 ,1};

                if (!bDeserialize)
                {
                    DataColumnCollection cols = ds.Tables[tableIndex].Columns;
                    DataRowCollection rows = ds.Tables[tableIndex].Rows;
                    if (!cols.Contains("ID"))
                    {
                        cols.Add("ID");
                    }
                    if (!cols.Contains("申诉数量"))
                    {
                        cols.Add("申诉数量");
                    }
                    //加photo
                    if (!cols.Contains("photo"))
                    {
                        cols.Add("photo");
                    }

                    for (int i = 0; i < rows.Count; i++)
                    {
                        //加photo
                        rows[i]["photo"] = ""; 

                        rows[i]["ID"] = (i + 1).ToString();
                        rows[i]["申诉数量"] = "0";
                    }
    

					//加CheckBox
                    //if (!cols.Contains("CheckBox"))
                    //{
                    //    cols.Add("CheckBox");
                    //    for (int i = 0; i < rows.Count; i++)
                    //    {
                    //        rows[i]["CheckBox"] = "False";
                    //    }
                    //}
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

				//加CheckBox
                //for (int i = 0; i < colValue.Length; i++)
                //{
                //    if (colName[i].Equals("CheckBox"))
                //    {
                //        row[i] = bCheckAll;
                //    }
                //    else
                //    {
                //        row[i] = colValue[i];
                //    }
                //}
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
        // 
        private void UpdateRow()
        {
            DataGridCustomCheckBoxColumn colCheckBox = null;//加CheckBox
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
            DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
            DataTable dtResult = new DataTable();
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
				//加CheckBox
                if (styles[i].MappingName.Equals("CheckBox"))
                {
                    colCheckBox = (DataGridCustomCheckBoxColumn)styles[i];
                    dtResult.Columns[i].DataType = typeof(Boolean);
                }
            }
            dtResult.TableName = dt.TableName;
            DataTable dtDiffTmp = dtDiff.Clone();
            for (int i = 0; i < dtDiff.Rows.Count; i++)
            {
                if (tbSKU.Text.Equals(dtDiff.Rows[i]["SKU"].ToString()))
                {
                    DataRow rowNew = dtDiffTmp.NewRow();
                    for (int j = 0; j < dtDiffTmp.Columns.Count; j++)
                    {
                        rowNew[j] = dtDiff.Rows[i][j];
                    }
                    dtDiffTmp.Rows.Add(rowNew);
                }
            }

            int nCount = 0;
            int from = (pageIndex - 1) * TABLE_ROWMAX;
            int to = from + TABLE_ROWMAX;
            if (to > dt.Rows.Count)
            {
                to = dt.Rows.Count;
            }


            for (int i = from; i < to; i++)
            {
                string reason = dt.Rows[i]["reason"].ToString();
                DataRow rowNew = dtResult.NewRow();
                for (int j = 0; j < styles.Count; j++)
                {
                    string name = styles[j].MappingName;
                    rowNew[name] = dt.Rows[i][name];

                    if (name.Equals("申诉数量") && !bInit)
                    {
                        for (int k = 0; k < dtDiffTmp.Rows.Count; k++)
                        {
                            string reasonTmp = dtDiffTmp.Rows[k]["reason"].ToString();
                            if (reason.Equals(reasonTmp))
                            {
                                rowNew[name] = dtDiffTmp.Rows[k]["count"];
                                nCount += int.Parse(rowNew[name].ToString());
                                break;
                            }
                        }
						//加CheckBox
                        if (name.Equals("CheckBox"))
                        {
                            rowNew[name] = dt.Rows[i][name].Equals("True") ? true : false;
                        }
                        else
                        {
                            rowNew[name] = dt.Rows[i][name];
                        }
                    }
                }

                dtResult.Rows.Add(rowNew);
            }
            if (!bInit)
            {
                tbCount.Text = nCount.ToString();
                bInit = true;
            }
          
            dgTable.DataSource = dtResult;
        }

        private void updateData(string key, string col, string value)
        {
            try
            {
                DataTable dt = ds.Tables[tableIndex];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (key.Equals(dt.Rows[i]["ID"].ToString()))
                    {
                        dt.Rows[i][col] = value;
                        return;
                    }
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
                if (ds != null)
                {
                    DataTable dtReason = (DataTable)dgTable.DataSource;
                    int nTotal = int.Parse(tbTotal.Text), nCount = 0;
                    bCount = false;
                    for (int i = 0; i < dtReason.Rows.Count; i++)
                    {
                        string count = dtReason.Rows[i]["申诉数量"].ToString();
                        if (count.Length <= 9)
                        {
                            if (!reg.IsMatch(count))
                            {
                                MessageBox.Show("请输入非负整数！");
                                bCount = true;
                                return;
                            }
                            else
                            {
                                if (UInt64.Parse(count) > 0)
                                {
                                    nCount += int.Parse(count);
                                    string subtext = count.Substring(0, 1);
                                    if (UInt64.Parse(subtext) == 0)
                                    {
                                        MessageBox.Show("请输入非负整数！");
                                        bCount = true;
                                        return;
                                    }
                                    else if (nCount > nTotal)
                                    {
                                        MessageBox.Show("已超出申诉总数量!");
                                        bCount = true;
                                        return;
                                    }
                                    else
                                    {
                                        updateData(dtReason.Rows[i]["ID"].ToString(), "申诉数量", count);
                                    }
                                }
                                else if (UInt64.Parse(count) == 0)
                                {
                                    if (count.Length >= 2)
                                    {
                                        MessageBox.Show("请输入非负整数！");
                                        bCount = true;
                                        return;
                                    }
                                    else
                                    {
                                        updateData(dtReason.Rows[i]["ID"].ToString(), "申诉数量", count);
                                    }
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("输入字符过长!");
                            bCount = true;
                            return;
                        }
                    }
                    if (!bCount)
                    {
                        tbCount.Text = nCount.ToString();
                    }
                    Serialize(guid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    refreshData();
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

        #endregion

        #region 接口请求
        private void request01()
            {
           
            apiID = API_ID.O0301;
            ClearGrid();
            string op = "01";
            string msg = "request=003;usr=" + Config.User + ";op=" + op.ToUpper();
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
                    case API_ID.O0301:
                        {
                            string file = Config.getApiFile("003", "01");
                            from += "/003/" + file;
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

        #region UI事件响应

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
        #region delegates 传参

        public delegate void PageTitle(Form form);

        #endregion

        private bool isPO = false;

        void photoMenu_Click(object sender, System.EventArgs e)
        {
            FormCamera formCamera = new FormCamera(photoName1, photoName2, photoName3, isPO);
            //formCamera.MyPhotoEvent += new Hola_In.FormCamera.MyPhotoDelegate(getPhotoName);
            PageTitle pageTitle = new PageTitle(formCamera.getTitle);
            pageTitle(this);
            formCamera.ShowDialog();

            DataRowCollection rows = ds.Tables[tableIndex].Rows;
            rows[rowFlag]["photo"] = formCamera.photoName();
        }

        void contextMenu1_Popup(object sender, System.EventArgs e)
        {
            try
            {
              
                contextMenu1.MenuItems.Clear();

                if (!busy && tableIndex >= 0)
                {
                    int x = Control.MousePosition.X;
                    int y = Control.MousePosition.Y - dgTable.Top - TaskBarHeight;

                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);
       
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {


                        rowFlag = hitTest.Row;

                        dgTable.Select(rowFlag);

                        dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);

                        DataTable dtReason = ds.Tables[tableIndex];
                        //int colIndex = dgTable.CurrentCell.ColumnNumber;
                        if (dtReason.Rows[rowFlag]["申诉数量"].ToString() == "0")
                        {
                            MessageBox.Show("请修改申诉数量");
                        }
                        else
                        {
                            //contextMenu1.MenuItems.Add(this.photoMenu);
                            if (isBDorPO == "调拨收货")
                            {
                                contextMenu1.MenuItems.Add(this.photoMenu);
                            }
                            else
                            {
                                 
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


        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy && tableIndex >= 0 && ds != null)
                {
                    DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        if (dt != null && dt.Rows.Count >= 1)
                        {
                            refreshData();
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
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                if (!dt.Columns[colIndex].ColumnName.Equals("申诉数量"))
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
                }

                refreshData();
                Serialize(guid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            if (ds != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null && dt.Rows.Count >= 1)
                {
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    if (tableIndex >= 0 && !dt.Columns[colIndex].ColumnName.Equals("申诉数量"))
                    {
                        dgTable.Select(rowIndex);
                    }
                }
            }
        }

        private void ClearGrid()
        {
            tableIndex = -1;
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
            }
            dgHeader.Controls.Clear();
            dgHeader.DataSource = null;
            dgTable.Controls.Clear();
            dgTable.DataSource = null;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                try
                {
                    File.Delete(Config.DirLocal + guid);
                    if (ds != null)
                    {
                        GetDtReason(ds);
                    }
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                try
                {
                    if (ds==null || tableIndex < 0)
                    {
                        MessageBox.Show("无数据");
                        return;
                    }

                    refreshData();
                    if (!bCount)
                    {
                        if (!tbTotal.Text.Equals(tbCount.Text))
                        {
                            MessageBox.Show("请检查已填写数量！");
                        }
                        else
                        {
                            for (int i = dtDiff.Rows.Count - 1; i >= 0; i--)
                            {
                                if (tbSKU.Text.Equals(dtDiff.Rows[i]["SKU"].ToString()))
                                {
                                    dtDiff.Rows[i].Delete();
                                }
                            }

                            DataTable dtReason = ds.Tables[tableIndex];
                            for (int i = 0; i < dtReason.Rows.Count; i++)
                            {
                                if (UInt64.Parse(dtReason.Rows[i]["申诉数量"].ToString()) > 0)
                                {
                                    DataRow rowNew = dtDiff.NewRow();

                                    rowNew["SKU"] = tbSKU.Text;
                                    rowNew["reason"] = dtReason.Rows[i]["reason"].ToString();
                                    rowNew["count"] = dtReason.Rows[i]["申诉数量"].ToString();
                                    //加photo
                                    rowNew["photo"] = dtReason.Rows[i]["photo"].ToString();
                                    dtDiff.Rows.Add(rowNew);
                                }
                            }
                            if (ds != null)
                            {
                                GetDtReason(ds);
                            }
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
                    case API_ID.O0301:
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
            if (param != null)
            {
                tbBC.Text = (string)param[0];
                tbSKU.Text = (string)param[1];
                tbName.Text = ((string)param[2]).Equals("null") ? "" : ((string)param[2]);
                tbTotal.Text = (string)param[3];
                dtDiff = (DataTable)param[4];
                if (param.Length > 6)
                {
                    //传入的箱状态
                    TableState = (string)param[7];
                    //是否第一次请求该页面
                    bool bRequest = bool.Parse((string)param[6]);
                   
                    if (!bRequest)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                        {
                            this.Invoke(new InvokeDelegate(() =>
                            {
                                request01();
                            }));
                        }));
                    }
                    else
                    {
                        if ((DataSet)param[5] != null)
                        {
                            ds = (DataSet)param[5];


                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                if (ds.Tables[i].TableName == "info")
                                {
                                    tableIndex = i; ;
                                    break;
                                }
                            }
                            UpdateColumn(false);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                            {
                                this.Invoke(new InvokeDelegate(() =>
                                {
                                    request01();
                                }));
                            }));
                        }
                    }
                }
                else
                {
                    TableState = (string)param[5];
                }
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
                data[(int)DATA_INDEX.SKU] = tbSKU.Text;
                data[(int)DATA_INDEX.NAME] = tbName.Text;
                data[(int)DATA_INDEX.TOTAL] = tbTotal.Text;
                data[(int)DATA_INDEX.COUNT] = tbCount.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.PAGEINDEX] = pageIndex.ToString();
                data[(int)DATA_INDEX.TABLESTATE] = TableState;
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
                    tbSKU.Text = sc.Data[(int)DATA_INDEX.SKU];
                    tbName.Text = sc.Data[(int)DATA_INDEX.NAME];
                    tbTotal.Text = sc.Data[(int)DATA_INDEX.TOTAL];
                    tbCount.Text = sc.Data[(int)DATA_INDEX.COUNT];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PAGEINDEX]);
                    TableState = sc.Data[(int)DATA_INDEX.TABLESTATE];
                    ds = sc.DS;

                    int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                    if (pageCount == 0)
                        Page.Text = "1/1";
                    else
                        Page.Text = pageIndex.ToString()+"/" + pageCount.ToString();
                   
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