using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using HolaCore;

namespace Hola_Inventory
{
    public partial class FormJBInSKU : Form,ISerializable,ConnCallback
    {
        private const string guid = "4C2FB7F2-B33D-4780-8532-A7AF069E3B5A";
         private const string OGuid = "00000000000000000000000000000000";
        
        private int TaskBarHeight = 0;
        //指示当前正在发送网络请求
        private bool busy = false;
        private DataSet ds = null;
        private string ShopNo = null;
        private string CBNO = null;
        private Form Child = null;
        private string xmlfile = null;
        #region 子窗口ID

        private enum Form_ID
        {
            InDetail,
            Null,
        }
        #endregion
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        private int pageIndex = 1;
        private int colIndex = -1;
        private int TABLE_ROWMAX = 6;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNo,
            PageIndex,
            Child,
            DataMax
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R103201,
            R103202,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        public FormJBInSKU()
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
                PrePage.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Top = dgTable.Top + dgTable.Height + 5;
                Page.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBarI.Top = btnReturn.Top - pbBar.Height * 3;
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

        #region XML加载与显示

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
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
                    }
                    if (!bNoData)
                    {
                        DataTable dt= ds.Tables["detail"];
                        if (dt.Rows.Count > 0)
                        {
                            UpdatedgTable(false);
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
                DataGridCustomColumnBase style = null;
                if (name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }
                else
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = true;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }

            }

            return ts;
        }

        private void UpdatedgTable(bool bSerialize)
        {
            try
            {
                string[] colName = new string[] { "loc_no", "sku_kind_qty", "date"};
                string[] colValue = new string[] { "柜号", "SKU品数", "日期" };
                int[] colWidth = new int[] { 28, 27, 42 };

              
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

        private void UpdatedgHeader()
        {
            try
            {
                string[] colName = new string[] { "loc_no", "sku_kind_qty", "date" };
                string[] colValue = new string[] { "柜号", "SKU品数", "日期" };
                int[] colWidth = new int[] { 28, 27, 42};
                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
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

        #endregion

        #region 接口请求

        private void request01()
        {
            apiID = API_ID.R103201;
            string msg = "request=1032;usr=" + Config.User + ";op=01;loc_no=" + CBNO + ";sto=" + ShopNo;
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
                    case API_ID.R103201:
                        {
                            string file = Config.getApiFile("1032", "01");
                            from += "/1032/" + file;
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

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dtHeader = (DataTable)dgTable.DataSource;
                    if (dtHeader != null && dtHeader.Rows.Count > 0)
                    {
                        DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {

                            DataRow row = dtHeader.Rows[hitTest.Row];
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

                                UpdatedgTable(false);
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
                DataTable dtt = (DataTable)dgTable.DataSource;
                if (dtt != null)
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
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

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    contextMenu1.MenuItems.Clear();
                    if (dt != null && dt.Rows.Count > 0)
                    {
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
                            contextMenu1.MenuItems.Add(menuDetail);
                            CBNO = dt.Rows[hitTest.Row]["loc_no"].ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                ChildID = Form_ID.InDetail;
                ShowChild(false);
            }
        }

        private void ShowChild(bool bDeserialize)
        {
            try
            {
                if (Child != null)
                {
                    Child.Dispose();
                    Child = null;
                }
                switch (ChildID)
                {
                    case Form_ID.InDetail:
                        Child = new FormJBInDetail();
                        break;
                    default:
                        return;
                }
                if (bDeserialize)
                {
                    ((ISerializable)Child).Deserialize(null);
                }
                else
                {
                    Serialize(guid);
                    ((ISerializable)Child).init(new object[] { ShopNo,CBNO, "true" });
                }
                if (Child.ShowDialog() == DialogResult.OK)
                {
                    request01();
                }
                Show();
                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
                Serialize(guid);
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
                Close();
                File.Delete(Config.DirLocal + guid);
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
                        UpdatedgTable(false);
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

                        UpdatedgTable(false);
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
        }

        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.R103201:
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
                    case API_ID.R103202:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功!");
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

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                ShopNo = (string)param[0];
                CBNO = (string)param[1];
                ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                {
                    this.Invoke(new InvokeDelegate(() =>
                    {
                        request01();

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
                string[] data = new string[(int)DATA_INDEX.DataMax];
                data[(int)DATA_INDEX.ShopNo] = ShopNo;
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
                data[(int)DATA_INDEX.Child] = ChildID.ToString();
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

                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                    pageIndex=int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);
                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        if (ds.Tables["detail"].Rows.Count > 0)
                        {
                            UpdatedgTable(true);
                        }
                    }
                
                    if (ChildID != Form_ID.Null)
                    {
                        ShowChild(true);
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