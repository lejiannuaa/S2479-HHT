using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using HolaCore;
using System.Threading;
using System.Text.RegularExpressions;
namespace Hola_Business
{
    public partial class FormInvPO : Form,ISerializable,ConnCallback
    {
        private const string guid = "A70F7251-F673-415d-9E8F-E4D83BC2E899";
         private const string OGuid = "00000000000000000000000000000000";
      
        private DataSet ds = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            SKUNO,
            GoodsNO,
            SKUName,
            ManuID,
            ManuType,
            ManuName,
            InitMoney,
            Contact,
            OrderCycle,
            Tele,
            PackageNum,
            PageIndex,
            ShopNo,
            DataMax,
            ApCash
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R102201,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private string ShopNo = null;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private SerializeClass sc = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        private string xmlfile = null;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 4;
        private int colIndex = -1;
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormInvPO()
        {
            InitializeComponent();

            doLayout();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / (float)srcWidth, yTime = dstHeight / (float)srcHeight;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();
                if (dstWidth > 240)
                {
                    int muti = 1;
                    dgTable.Height = dgTable.Height + muti * (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += muti;
                }
                PrePage.Top = dgTable.Top + dgTable.Height + 3;
                NexPage.Top = dgTable.Top + dgTable.Height + 3;
                Page.Top = dgTable.Top + dgTable.Height + 3;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
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

        //SKU补零
        private string AddZero(string OLstring)
        {
            string SKU = OLstring;
            if (SKU.Length < 8)
            {
                StringBuilder addString = new StringBuilder();
                int addStringNo = 9 - SKU.Length;
                addString.Append('0', addStringNo);
                SKU = addString.ToString() + SKU;
            }
            return SKU;
        }

        private void ClearBox()
        {
            try
            {
                SKUNO.Text = "";
                GoodsNO.Text = "";
                SKUName.Text = "";
                ManuID.Text = "";
                ManuType.Text = "";
                ManuName.Text = "";
                Contact.Text = "";
                Tele.Text = "";
                InitMoney.Text = "";
                OrderCycle.Text = "";
                PackageNum.Text = "";
                ApCash.Text = "";
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                dgTable.DataSource = null;
            }
            catch (Exception)
            {
            }
        }


        #region XML加载与显示

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            bool BNoData=true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            try
            {
                if (File.Exists(xmlfile))
                {
                    doc = new XmlDocument();
                    doc.Load(xmlfile);
                    reader = new XmlNodeReader(doc);
                    ds = new DataSet();
                    ds.ReadXml(reader);

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("detail"))
                        {
                            bNoData = false;
                        }

                        if (ds.Tables[i].TableName.Equals("head"))
                        {
                            BNoData = false;
                        }
                    }
                    if (!bNoData && !BNoData)
                    {
                        DataTable dth = ds.Tables["head"];
                        DataTable dtd = ds.Tables["detail"];
                        if (dth.Rows.Count > 0)
                        {
                            
                            SKUNO.Text = dth.Rows[0]["sku"].ToString();
                            GoodsNO.Text = dth.Rows[0]["huohao"].ToString();
                            SKUName.Text = dth.Rows[0]["sku_dsc"].ToString();
                            ManuID.Text = dth.Rows[0]["vnd_code"].ToString();
                            ManuType.Text = dth.Rows[0]["vnd_name_type"].ToString();
                            ManuName.Text = dth.Rows[0]["vnd_name"].ToString();
                            Contact.Text = dth.Rows[0]["contact"].ToString();
                            Tele.Text = dth.Rows[0]["tel"].ToString();
                            InitMoney.Text = dth.Rows[0]["start_ap"].ToString();
                            OrderCycle.Text = dth.Rows[0]["order_cycle"].ToString();
                            PackageNum.Text = dth.Rows[0]["standard_pack_qty"].ToString();
                            //
                            ApCash.Text = dth.Rows[0]["ap_amt"].ToString();
                        }
                        if (dtd.Rows.Count > 0)
                        {
                            UpdatedgTable();

                        }
                    }
                    else if(!BNoData && bNoData)
                    {
                        DataTable dth = ds.Tables["head"];
       
                        if (dth.Rows.Count > 0)
                        {
                            SKUNO.Text = dth.Rows[0]["sku"].ToString();
                            GoodsNO.Text = dth.Rows[0]["huohao"].ToString();
                            SKUName.Text = dth.Rows[0]["sku_dsc"].ToString();
                            ManuID.Text = dth.Rows[0]["vnd_code"].ToString();
                            ManuType.Text = dth.Rows[0]["vnd_name_type"].ToString();
                            ManuName.Text = dth.Rows[0]["vnd_name"].ToString();
                            Contact.Text = dth.Rows[0]["contact"].ToString();
                            Tele.Text = dth.Rows[0]["tel"].ToString();
                            InitMoney.Text = dth.Rows[0]["start_ap"].ToString();
                            OrderCycle.Text = dth.Rows[0]["order_cycle"].ToString();
                            PackageNum.Text = dth.Rows[0]["standard_pack_qty"].ToString();
                            //
                            ApCash.Text = dth.Rows[0]["ap_amt"].ToString();
                        }
                        pageIndex = 1;
                        Page.Text = "1/1";
                        dgTable.DataSource = null;
                       
                    }
                    else if (!bNoData && BNoData)
                    {
                        DataTable dtd = ds.Tables["detail"];
                        if (dtd.Rows.Count > 0)
                        {
                            UpdatedgTable();
                        }
                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                        dgTable.DataSource = null;
                        MessageBox.Show("无数据!");
                    }
                }
                else
                {
                    pageIndex = 1;
                    Page.Text = "1/1";
                    dgTable.DataSource = null;
                    MessageBox.Show("请求文件不存在,请重新请求!");
                }
            }
            catch (Exception ex)
            {
                ds = null;
                dgTable.DataSource = null;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                if (reader != null)
                {
                    reader.Close();
                }
                File.Delete(xmlfile);
                GC.Collect();
                Serialize(guid);
            }
        }

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool GridNo)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables["detail"];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;

            for (int i = 0; i < name.Length; i++)
            {
                DataGridColumnStyle style = null;
                style = new DataGridTextBoxColumn();
                style.MappingName = name[i];
                style.Width = (int)(dstWidth * width[i] / 100);
                ts.GridColumnStyles.Add(style);
           
            }

            return ts;
        }

        private void UpdatedgTable()
        {
            try
            {
                string[] colName = new string[] { "pono", "order_date", "expect_arrival_date"};
                string[] colValue = new string[] { "PO单号", "下单日期", "预计到货日期"};
                int[] colWidth = new int[] { 35, 35, 27 };

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();
                UpdatedgHeader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateRow()
        {
            DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
    
            DataTable dtResult = new DataTable();
            dtResult.TableName = dt.TableName;
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);

            }
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
            int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
        }

        private void UpdatedgHeader()
        {
            try
            {
                string[] colName = new string[] { "pono", "order_date", "expect_arrival_date" };
                string[] colValue = new string[] { "PO单号", "下单日期", "预计到货日期" };
                int[] colWidth = new int[] { 35, 35, 27 };
                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();//ds.Tables["detail"].Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colValue.Length; i++)
                {
                    row[i] = colValue[i];
                }
                dtHeader.Rows.Add(row);
                dgHeader.Controls.Clear();
                dgHeader.TableStyles.Clear();
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, false));
                dgHeader.DataSource = dtHeader;
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 接口请求

        private void request01()
        {
            //ClearBox();
            apiID = API_ID.R102201;
            string msg = "request=1022;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
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
                    case API_ID.R102201:
                        {
                            string file = Config.getApiFile("1022", "01");
                            from += "/1022/" + file;
                            to += file;
                            xmlfile = to;
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

        #region UI响应

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy && SKUNO.Text != "")
            {
                if (e.KeyValue == 13)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    btn01.Focus();
                    request01();
                }
                QC();
            }
        }

        private void QC()
        {
            GoodsNO.Text = "";
            SKUName.Text = "";
            ManuID.Text = "";
            ManuType.Text = "";
            ManuName.Text = "";
            Contact.Text = "";
            Tele.Text = "";
            InitMoney.Text = "";
            OrderCycle.Text = "";
            PackageNum.Text = "";
            ApCash.Text = "";
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
            }
            dgTable.DataSource = null;
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtt = (DataTable)dgTable.DataSource;
                if (dtt != null)
                {
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);

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
                if (!busy)
                {
                    DataTable dtHeader = (DataTable)dgTable.DataSource;
                    if (dtHeader != null && dtHeader.Rows.Count >= 1)
                    {
                        DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {

                            string colName = dtHeader.Columns[hitTest.Column].ColumnName;

                            DataTable dt = ds.Tables["detail"];
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

                                UpdatedgTable();
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

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (dt != null && dt.Rows.Count >= 1)
                    {
                        int rowOld = dgTable.CurrentCell.RowNumber;
                        int columnOld = dgTable.CurrentCell.ColumnNumber;

                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            dgTable.Select(hitTest.Row);
                        }
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
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                    return;
                }

                if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                    return;
                }

                SKUNO.Text = AddZero(SKUNO.Text);
                request01();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                Close();
                File.Delete(Config.DirLocal + guid);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                ClearBox();
                SKUNO.Focus();
            }
        }

        #endregion

        #region 翻页

        private void PrePage_Click(object sender, EventArgs e)
        {

            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdatedgTable();
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

        private void NexPage_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {

                        UpdatedgTable();
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

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                ShopNo = (string)param[0];
                SKUNO.Text = (string)param[1];
                Serialize(guid);
                ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                {
                    this.Invoke(new InvokeDelegate(() =>
                    {
                        request01();
                    }));
                }));

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

                string[] data = new string[(int)DATA_INDEX.DataMax];
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.GoodsNO] = GoodsNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.ManuID] = ManuID.Text;
                data[(int)DATA_INDEX.ManuType] = ManuType.Text;
                data[(int)DATA_INDEX.ManuName] = ManuName.Text;
                data[(int)DATA_INDEX.Contact] = Contact.Text;
                data[(int)DATA_INDEX.Tele] = Tele.Text;
                data[(int)DATA_INDEX.InitMoney] = InitMoney.Text;
                data[(int)DATA_INDEX.OrderCycle] = OrderCycle.Text;
                data[(int)DATA_INDEX.PackageNum] = PackageNum.Text;
                data[(int)DATA_INDEX.ApCash] = ApCash.Text;
                data[(int)DATA_INDEX.ShopNo] = ShopNo;
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
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
                    SKUNO.Text= sc.Data[(int)DATA_INDEX.SKUNO] ;
                    GoodsNO.Text=sc.Data[(int)DATA_INDEX.GoodsNO];
                    SKUName.Text=sc.Data[(int)DATA_INDEX.SKUName];
                    ManuID.Text=sc.Data[(int)DATA_INDEX.ManuID];
                    ManuType.Text=sc.Data[(int)DATA_INDEX.ManuType];
                    ManuName.Text= sc.Data[(int)DATA_INDEX.ManuName];
                    Contact.Text=sc.Data[(int)DATA_INDEX.Contact];
                    Tele.Text=sc.Data[(int)DATA_INDEX.Tele];
                    InitMoney.Text=sc.Data[(int)DATA_INDEX.InitMoney];
                    OrderCycle.Text=sc.Data[(int)DATA_INDEX.OrderCycle];
                    PackageNum.Text=sc.Data[(int)DATA_INDEX.PackageNum];
                    ApCash.Text = sc.Data[(int)DATA_INDEX.ApCash];
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        if (ds.Tables["detail"].Rows.Count > 0)
                        {
                            UpdatedgTable();
                        }
                    }
                   
                }
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
                    case API_ID.R102201:
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
                            LoadXML();
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

        private void ManuName_TextChanged(object sender, EventArgs e)
        {

        }

      
    }
}