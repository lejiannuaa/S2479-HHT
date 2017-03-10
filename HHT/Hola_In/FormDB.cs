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
using System.Threading;
using System.Diagnostics;
using System.IO;
using HolaCore;
using System.Runtime.InteropServices;
namespace Hola_In
{
    public partial class FormDB : Form, ConnCallback, ScanCallback, ISerializable
    {
        private const string guid = "6AE7BE73-7A85-C035-FAB6-6C8E3B3EF9B3";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private string DataGuid02 = "";
        private string DataGuid03 = "";
        private string DataGuid04 = "";
        private string DataGuid05 = "";
        //指示当前正在发送网络请求
        private bool busy = false;

        private ConnThread DBconnThread = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            STATE,
            TABLEINDEX,
            CHILD,
            BTN03,
            BTN04,
            BTN05,
            BTN06,
            DATAGUID02,
            DATAGUID03,
            DATAGUID04,
            DATAGUID05,
            DATAMAX
        }
        #endregion

        private int TABLE_ROWMAX = 8;
        private string xmlSchema = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xs:schema id=\"root\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\"><xs:element name=\"root\" msdata:IsDataSet=\"true\" msdata:Locale=\"en-US\"><xs:complexType><xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\"><xs:element name=\"config\"><xs:complexType><xs:sequence><xs:element name=\"type\" type=\"xs:string\" minOccurs=\"0\" /><xs:element name=\"direction\" type=\"xs:string\" minOccurs=\"0\" /><xs:element name=\"id\" type=\"xs:string\" minOccurs=\"0\" /></xs:sequence></xs:complexType></xs:element><xs:element name=\"info\"><xs:complexType><xs:sequence><xs:element name=\"state\" type=\"xs:string\" minOccurs=\"0\" /></xs:sequence></xs:complexType></xs:element><xs:element name=\"detail\"><xs:complexType><xs:sequence><xs:element name=\"HHTSTA\" type=\"xs:string\" minOccurs=\"0\" /><xs:element name=\"调拨单号\" type=\"xs:string\" minOccurs=\"0\" /><xs:element name=\"调拨类型\" type=\"xs:string\" minOccurs=\"0\" /></xs:sequence></xs:complexType></xs:element></xs:choice></xs:complexType></xs:element></xs:schema>";
        private FormDownload formDownload = null;
        private FormScan formScan = null;
        private Form child = null;
        private string xmlFile =null;

        private SerializeClass sc = null;
        private DataSet ds = null;
        private int tableIndex = -1;
        private int pageIndex = 1;
        private int colIndex = -1;
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
            O0101,
            O0102,
            O0103,
            O0104,
            O0105,
            O0106,
            DOWNLOAD_XML
        }

        private API_ID apiID = API_ID.NONE;
        #endregion
 
        public FormDB()
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

            try
            {
                SuspendLayout();

                int yOffset = dstHeight - btnReturn.Height - btnReturn.Top;

                dgTable.Height += yOffset;
                //TABLE_ROWMAX = dgTable.Height / 18;
                PrePage.Top += yOffset;
                Page.Top += yOffset;
                NextPage.Top += yOffset;
                btnReturn.Top += yOffset;

                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

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
            FileStream fs = null;
            try
            {
                Start=0;
                Stop=0;
                QueryPerformanceCounter(ref Start);
                if (File.Exists(xmlFile))
                {
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
                        if (ds.Tables[i].TableName.Equals("detail"))
                        {
                            tableIndex = i;
                            bNoData = false;
                        }
                        else if (ds.Tables[i].TableName.Equals("info"))
                        {
                            string state = ds.Tables[i].Rows[0]["state"].ToString();
                            switch (state)
                            {
                                case "0":
                                    tbState.Text = "待收货";
                                    setButton(true, true, true, false);
                                    break;

                                case "1":
                                    tbState.Text = "收货中";
                                    setButton(false, false, false, true);
                                    break;

                                case "2":
                                    tbState.Text = "已收货";
                                    setButton(false , false, false, false);
                                    break;
                                default:
                                    tbState.Text = state;
                                    break;
                            }
                        }
                    }
   

                    if (!bNoData)
                    {
                        DataGuid02 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid03 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid04 = Guid.NewGuid().ToString().Replace("-", "");
                        DataGuid05 = Guid.NewGuid().ToString().Replace("-", "");
                        pageIndex = 1;
                        UpdateColumn();
                        int pageCount = (int)Math.Ceiling(ds.Tables[tableIndex].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                    }
                }
                else
                {
                    pageIndex = 1;
                    tbState.Text = "";
                    btnBC.Text = "";
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
                //if (reader != null)
                    //reader.Close();
                if (File.Exists(xmlFile))
                {
                    if (bNoData)
                    {
                        MessageBox.Show("无数据！");
                        setButton(false, false, false, false);
                        tbState.Text = "";
                    }
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

        private void UpdateColumn()
        {
            try
            {
                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);
                string[] colName = new string[] { "调拨单号", "调拨类型" };
                int[] colWidth = new int[] { 49, 49 };

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth));

                UpdateRow();

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colName.Length; i++)
                {
                    row[i] = colName[i];
                }
                dtHeader.Rows.Add(row);

                dgHeader.Controls.Clear();
                dgHeader.TableStyles.Clear();
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth));
                dgHeader.DataSource = dtHeader;

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

        private void setButton(bool b3,bool b4,bool b5,bool b6)
        {

            btn00103.Enabled = b3;
            btn00104.Enabled = b4;
            btn05.Enabled = b5;
            btn06.Enabled = b6;
        }

        #region 接口请求
        private void request(string op, string bc)
        {
          
            string msg = "request=001;usr=" + Config.User + ";op=" + op.ToUpper() + ";bc=" + bc.ToUpper();
            if (op.Equals("02"))
            {
                msg = DataGuid02 + msg;
            }
            else if (op.Equals("03"))
            {
                msg = DataGuid03 + msg;
            }
            else if (op.Equals("04"))
            {
                msg = DataGuid04 + msg;
            }
            else if (op.Equals("05"))
            {
                msg = DataGuid05 + msg;
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
            apiID = API_ID.O0101;
            ClearGrid();
            request("01", btnBC.Text);
        }

        private void request00102()
        {
            apiID = API_ID.O0102;

            request("02", btnBC.Text);
        }

        private void request00103()
        {
            apiID = API_ID.O0103;

            request("03", btnBC.Text);
        }

        private void request00104()
        {
            apiID = API_ID.O0104;

            request("04", btnBC.Text);
        }

        private void request05()
        {
            apiID = API_ID.O0105;

            request("05", btnBC.Text);
        }

        private void request06()
        {
            apiID = API_ID.O0106;

            request("06", btnBC.Text);
        }

        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;
                
                switch (apiID)
                {
                    case API_ID.O0101:
                        {
                            string file = Config.getApiFile("001", "01");
                            from += "/001/" + file;
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
                child = new FormDBSKU();

                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize();
                }
                else
                {
                    ((ISerializable)child).init(new string[] { btnBC.Text });
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

        public void requestCallback(string data, int result)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.O0101:
                        if (result == ConnThread.RESULT_OK)
                        {
                            requestXML();
                        }
                         else if(result==ConnThread.RESULT_DUPLOGIN)
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

                    case API_ID.O0102:
                    case API_ID.O0103:
                    case API_ID.O0104:
                    case API_ID.O0106:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功！");
                            Serialize();
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

                    case API_ID.O0105:
                        if (result == ConnThread.RESULT_OK)
                        {
                            showChild(false);
                        }
                        else if (result == ConnThread.RESULT_DUPLOGIN)
                        {
                            apiID = API_ID.NONE;
                            Serialize();
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
                            Serialize();
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
            try
            {
                this.btnBC.Text = barCode;
                request01();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                data[(int)DATA_INDEX.BARCODE] = btnBC.Text;
                data[(int)DATA_INDEX.STATE] = tbState.Text;
                data[(int)DATA_INDEX.TABLEINDEX] = tableIndex.ToString();
                data[(int)DATA_INDEX.BTN03] = btn00103.Enabled.ToString();
                data[(int)DATA_INDEX.BTN04] = btn00104.Enabled.ToString();
                data[(int)DATA_INDEX.BTN05] = btn05.Enabled.ToString();
                data[(int)DATA_INDEX.BTN06] = btn06.Enabled.ToString();
                data[(int)DATA_INDEX.DATAGUID02] = DataGuid02;
                data[(int)DATA_INDEX.DATAGUID03] = DataGuid03;
                data[(int)DATA_INDEX.DATAGUID04] = DataGuid04;
                data[(int)DATA_INDEX.DATAGUID05] = DataGuid05;
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
            catch (Exception ex)
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
                    btnBC.Text = sc.Data[(int)DATA_INDEX.BARCODE];
                    tbState.Text = sc.Data[(int)DATA_INDEX.STATE];
                    tableIndex = int.Parse(sc.Data[(int)DATA_INDEX.TABLEINDEX]);
                    btn00103.Enabled=bool.Parse(sc.Data[(int)DATA_INDEX.BTN03]);
                    btn00104.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN04]);
                    btn05.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN05]);
                    btn06.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTN06]);
                    DataGuid02 = sc.Data[(int)DATA_INDEX.DATAGUID02];
                    DataGuid03 = sc.Data[(int)DATA_INDEX.DATAGUID03];
                    DataGuid04 = sc.Data[(int)DATA_INDEX.DATAGUID04];
                    DataGuid05 = sc.Data[(int)DATA_INDEX.DATAGUID05];
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
                    if (!string.IsNullOrEmpty(sc.Data[(int)DATA_INDEX.CHILD]))
                    {
                        showChild(true);
                    }
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                        if (dt != null && dt.Rows.Count >= 1)
                        {
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

        private void dgTable_GotFocus(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgTable.DataSource;
            if (dt != null && dt.Rows.Count >= 1)
            {
                int index = dgTable.CurrentCell.RowNumber;
                if (index < dt.Rows.Count)
                {
                    dgTable.Select(index);
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

        private void btnBC_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (formScan == null)
                {
                    formScan = new FormScan();
                }
                formScan.init(this, btnBC.Text);
                FullscreenClass.ShowSIP(true);
                formScan.ShowDialog();
                FullscreenClass.ShowSIP(false);
                formScan.Dispose();
                formScan = null;
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request02();
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request03();
            }
        }

        private void btn04_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request04();
            }
        }

        private void btn05_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request05();
            }
        }

        private void btn06_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                request06();
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