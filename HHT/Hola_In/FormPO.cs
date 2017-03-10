using System;

using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Security.Permissions;
using HolaCore;

namespace Hola_In
{
    public partial class FormPO : Form, ISerializable, ConnCallback
    {
        private const string guid = "E2D5FE5B-FD4C-E506-93BA-A38D53F4002F";
        private const string OGuid = "00000000000000000000000000000000";
        private SerializeClass sc = null;
        //指示当前正在发送网络请求
        private bool busy = false;

        private Form child = null;
        private FormScan formScan = null;
        private FormDownload formDownload = null;
        private delegate void InvokeDelegete();
        private string xmlFile = null;
        private int TaskBarHeight = 0;
        private DataSet ds = null;
        private int TABLE_ROWMAX = 7;
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private int tableIndex = -1;
        private int pageIndex = 1;
        private int colIndex = -1;
        private string BC = "";
        private long Start = 0;
        private long Stop = 0;
        private long freq = 0;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            STATE,
            FROM,
            TO,
            OPUSR,
            TABLEINDEX,
            CHILD,
            DATAMAX
        }
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            O0401,
            O0402,
            O0403,
            O0404,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion

        public FormPO()
        {
            InitializeComponent();
            
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
               
                btnReturn.Top += yOffset;
                btn01.Top += yOffset;

                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                //DateTime from = DateTime.Now;
                //DateTime to = from.AddYears(1);
                
                //dtpFrom.Value = from;
                //dtpTo.Value = to;
                NextPage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                PrePage.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                Page.Top = dgTable.Bottom + (pbBar.Top - dgTable.Bottom - NextPage.Height) / 2;
                dtpFrom.Value = new System.DateTime(2012, 10, 1, 0, 0, 0, 0);
                dtpTo.Value = new System.DateTime(2012, 12, 31, 0, 0, 0, 0);
                cbOpUsr.Items.Add("");
                cbOpUsr.Items.Add(Config.User);
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
            Serialize();
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
                    Config.writeFile(str1, Config.DirLocal + "LogTime.txt");

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("info"))
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
                        UpdateColumn(false);
                       
                    }
                }
                else
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
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
                    ds = null;
                    tableIndex = -1;
                    MessageBox.Show("无数据！");   
                }
                File.Delete(xmlFile);

                GC.Collect();
                Serialize();
                QueryPerformanceCounter(ref Stop);
                string str1 = "Serialize ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str1, Config.DirLocal + "LogTime.txt");
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
                DataGridColumnStyle style = new DataGridTextBoxColumn();
                style.MappingName = name[i];
                style.Width = (int)(dstWidth * width[i] / 100);
                ts.GridColumnStyles.Add(style);
            }

            return ts;
        }

        private void UpdateColumn(bool bSerilize)
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
                    Page.Text = "1/1";
                }
                else
                {
                    string[] colName = new string[] { "PO单号", "收货状态", "预计到货日" };
                    int[] colWidth = new int[] { 33, 32, 32 };
                    DataTable dt=ds.Tables[tableIndex];
                    DataColumnCollection cols=dt.Columns;
                    if (!cols.Contains("DataGuid02"))
                    {
                        cols.Add("DataGuid02");
                    }
                    if (!cols.Contains("DataGuid03")) 
                    {
                        cols.Add("DataGuid03");
                    }
                    if(!cols.Contains("DataGuid04"))
                    {
                        cols.Add("DataGuid04");
                    }
                    if (!bSerilize)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["DataGuid02"] = Guid.NewGuid().ToString().Replace("-", "");
                            dt.Rows[i]["DataGuid03"] = Guid.NewGuid().ToString().Replace("-", "");
                            dt.Rows[i]["DataGuid04"] = Guid.NewGuid().ToString().Replace("-", "");
                        }
                    }
                    if (dgTable.TableStyles.Count <= 0)
                    {
                        dgTable.Controls.Clear();
                        dgTable.TableStyles.Clear();
                        dgTable.TableStyles.Add(getTableStyle(colName, colWidth));

                    }
                    UpdateRow();

                    DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                    DataRow row = dtHeader.NewRow();
                    for (int i = 0; i < colName.Length; i++)
                    {
                        row[i] = colName[i];
                    }
                    dtHeader.Rows.Add(row);
                    if (dgHeader.TableStyles.Count <= 0)
                    {
                        dgHeader.Controls.Clear();
                        dgHeader.TableStyles.Clear();
                        dgHeader.TableStyles.Add(getTableStyle(colName, colWidth));
                    }
                    dgHeader.DataSource = dtHeader;
                    if (tbBC.Text != "")
                    {
                        dgTable.Select(dgTable.CurrentCell.RowNumber);
                    }
                }
                QueryPerformanceCounter(ref Stop);
                string str = "Show ;" + "Time=" + ((Stop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str, Config.DirLocal + "LogTime.txt");
      
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

                    if (name.Equals("收货状态"))
                    {
                        switch (rowNew[name].ToString())
                        {
                            case "0":
                                rowNew[name] = "删除";
                                break;
                            case "1":
                                rowNew[name] = "可申请";
                                break;
                            case "2":
                                rowNew[name] = "申请中";
                                break;
                            case "3":
                                rowNew[name] = "可收货";
                                break;
                            case "4":
                                rowNew[name] = "收货中";
                                break;
                            case "5":
                                rowNew[name] = "已收货";
                                break;
                            default:
                                break;
                        }
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
            apiID = API_ID.O0401;
            ClearGrid();
            string op = "01";
            string from = dtpFrom.Text.Replace("-", "");
            string to = dtpTo.Text.Replace("-", "");
            string state = cbState.SelectedIndex > 0 ? (cbState.SelectedIndex - 1).ToString() : "";
            string opusr = cbOpUsr.SelectedIndex > 0 ? Config.User : "";
            string msg = "request=004;usr=" + Config.User + ";op=" + op + ";bc=" + tbBC.Text
                + ";from=" + from + ";to=" + to + ";state=" + state + ";opusr=" + opusr;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request(string op, string bc)
        {
           
            string msg = "request=004;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            if (op.Equals("01"))
            {
                msg = OGuid + msg;
            }
            else 
            {
                DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
                int row = dgTable.CurrentRowIndex;
                string DataGuid = dt.Rows[(pageIndex - 1) * TABLE_ROWMAX + row]["DataGuid" + op].ToString();
                msg = DataGuid + msg;
            }
           
            new ConnThread(this).Send(msg);
            wait();
          
        }

        private void request02()
        {
            apiID = API_ID.O0402;

            request("02", BC);
        }

        private void request03()
        {
            apiID = API_ID.O0403;

            request("03", BC);
        }

        private void request04()
        {
            apiID = API_ID.O0404;

            request("04", BC);
        }

        private void requestXML()
        {
            try
            {


                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.O0401:
                        {
                            string file = Config.getApiFile("004", "01");
                            from += "/004/" + file;
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

        private void showChild(bool bDeserialize)
        {
            try
            {

                if (child != null)
                {
                    child.Dispose();
                    child = null;
                }
                 child = new FormPOSKU();

                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize();
                }
                else
                {
                    ((ISerializable)child).init(new string[] { BC });
                    Serialize();
                }

                child.ShowDialog();
                Show();
                child.Dispose();

                child = null;
                Serialize();

                request01();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                        UpdateRow();
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
                        UpdateRow();
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
            this.Invoke(new InvokeDelegete(() =>
            {
                if (formDownload != null)
                {
                    formDownload.setProgress(total, progress);
                }
            }));
            
        }

        public void requestCallback(string data, int result)
        {
            this.Invoke(new InvokeDelegete(() =>
            {
                idle();

                switch (apiID)
                {
                    case API_ID.O0401:
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

                    case API_ID.O0402:
                    case API_ID.O0403:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                            request01();
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

                    case API_ID.O0404:
                        if (result == ConnThread.RESULT_OK)
                        {
                            showChild(false);
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
            try
            {
                if (!busy && tableIndex >= 0)
                {
                    DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        DataTable dt = ds.Tables[tableIndex];
                        DataColumn col = dt.Columns[hitTest.Column];
                        if (dt != null && dt.Rows.Count >= 1)
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

                            UpdateRow();
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
            int index = dgTable.CurrentCell.RowNumber;
            dgTable.Select(index);
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

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null && dt.Rows.Count >= 1)
                {
                    int index = dgTable.CurrentCell.RowNumber;
                    if (dt != null && index < dt.Rows.Count)
                    {
                        dgTable.Select(index);
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
                menu02.Enabled = false;
                menu03.Enabled = false;
                menu04.Enabled = false;
                contextMenu1.MenuItems.Clear();
                if (!busy && tableIndex >= 0)
                {
                    dgTable.Focus();

                    int x = Control.MousePosition.X;
                    int y = Control.MousePosition.Y - dgTable.Top-TaskBarHeight;

                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        contextMenu1.MenuItems.Add(this.menu02);
                        contextMenu1.MenuItems.Add(this.menu03);
                        contextMenu1.MenuItems.Add(this.menu04);
                        if (dgTable.CurrentRowIndex != hitTest.Row)
                        {
                            dgTable.UnSelect(dgTable.CurrentRowIndex);
                            dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                        }
                        dgTable.Select(dgTable.CurrentRowIndex);

                        DataTable dt = ds.Tables[tableIndex].DefaultView.ToTable();
                        int nIndex = (pageIndex - 1) * TABLE_ROWMAX + dgTable.CurrentRowIndex;
                        BC = dt.Rows[nIndex]["PO单号"].ToString();
                        string state = dt.Rows[nIndex]["收货状态"].ToString();
                        switch (UInt64.Parse(state))
                        {
                            case 0: //删除
                                break;

                            case 1: //待申请
                                menu02.Enabled = true;
                                break;

                            case 2: //申请中
                                break;

                            case 3: //可收货
                                menu03.Enabled = true;
                                menu04.Enabled = true;
                                break;

                            case 4: //收货中
                                menu03.Enabled = true;
                                break;

                            case 5: //已收货
                                break;

                            default:
                                break;
                        }
                    }

                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menu02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request02();
            }
        }

        private void menu03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request03();
            }
        }

        private void menu04_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request04();
            }
        }

        private void tbBC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == 0)
            {
                tbBC.Text = "";
            }

        }

        private void FormPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (!busy)
                {
                    if (checkValidation())
                    {
                        request01();
                    }
                }
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (checkValidation())
                {
                    if (tbBC.Text == "")
                    {
                        BC = "";
                    }
                    request01();
                }
            }
        }

        private bool checkValidation()
        {
            try
            {
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                tbBC.Text = "";
                cbState.SelectedIndex = 0;
                dtpFrom.Value = new System.DateTime(2012, 10, 1, 0, 0, 0, 0);
                dtpTo.Value = new System.DateTime(2012, 12, 31, 0, 0, 0, 0);
                cbOpUsr.SelectedIndex = 0;
                if (ds != null)
                {
                    ds.Dispose();
                }
                ds = null;

                UpdateColumn(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        #region 实现ISerialize接口
        public void init(object[] param)
        {
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
                data[(int)DATA_INDEX.BARCODE] = tbBC.Text;
                data[(int)DATA_INDEX.STATE] = cbState.Text;
                data[(int)DATA_INDEX.FROM] = dtpFrom.Text;
                data[(int)DATA_INDEX.TO] = dtpTo.Text;
                data[(int)DATA_INDEX.OPUSR] = cbOpUsr.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();

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
                    tbBC.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    cbState.Text = sc.Data[(int)DATA_INDEX.STATE];
                    dtpFrom.Text = sc.Data[(int)DATA_INDEX.FROM];
                    dtpTo.Text = sc.Data[(int)DATA_INDEX.TO];
                    cbOpUsr.Text = sc.Data[(int)DATA_INDEX.OPUSR];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                 

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