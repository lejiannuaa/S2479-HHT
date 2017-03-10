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

namespace Hola_Business
{
    public partial class FormJBOrder : Form,ISerializable,ConnCallback
    {
        private const string guid = "29CB5181-1D03-4897-B686-72ADBD8AEC87";
        private const string OGuid = "00000000000000000000000000000000";
        private string DataGuid = "";
        private Form Child = null;
        #region 子窗口ID

        private enum Form_ID
        {
            OrderDetail,
            Null,

        }

        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            SKUNO,
            SKUName,
            PageIndex,
            ShopNo,
            btn02,
            Child,
            DataMax
        }
        #endregion

        private string ShopNo = null;
        private string xmlfile = null;
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        private DataSet ds = null;
        private int pageIndex = 1;
        private int colIndex = -1;
        private int TABLE_ROWMAX = 6;
        private bool bCheckAll = false;
        private int rowFlag = -1;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            S0001,
            R103502,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private int TaskBarHeight = 0;
        public FormJBOrder()
        {
            InitializeComponent();
            doLayout();
            //LoadXML();
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
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                btn02.Top = dstHeight - btnReturn.Height;
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

        #region XML加载与显示

        private void CreateDS()
        {
            try
            {
                string[] colName = { "sku", "sku_dsc", "sku_order_qty", "CheckBox" };
                DataTable dt = new DataTable();
                ds = new DataSet();
                dt.TableName = "detail";
                for (int i = 0; i < colName.Length; i++)
                {
                    dt.Columns.Add(colName[i]);
                }
                ds.Tables.Add(dt);
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            //xmlfile = @"\Program Files\hhtiii\S0001.xml";

            try
            {
                if (File.Exists(xmlfile))
                {
                    doc = new XmlDocument();
                    doc.Load(xmlfile);
                    reader = new XmlNodeReader(doc);
                    DataSet DS = new DataSet();
                    DS.ReadXml(reader);

                    for (int i = 0; i < DS.Tables.Count; i++)
                    {
                        if (DS.Tables[i].TableName.Equals("detail"))
                        {
                            bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {
                        SKUName.Text = DS.Tables["detail"].Rows[0][1].ToString();
                    }
                    else
                    {
                        MessageBox.Show("无数据！");
                    }
                }
                else
                {
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
                DataGridColumnStyle style1 = null;
                if (name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }
                else if (name[i].Equals("sku_order_qty"))
                {
                    style = new DataGridCustomTextBoxColumn();

                    style.Owner = dgTable;
                    style.ReadOnly = true;
                    style.MappingName = name[i];
                    style.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style);
                }
                else if (name[i].Equals("sku"))
                {
                    if (GridNo)
                    {
                        style = new DataGridCustomTextBoxColumn();

                        style.Owner = dgTable;

                        style.Alignment = HorizontalAlignment.Right;
                        style.ReadOnly = true;
                        style.MappingName = name[i];
                        style.Width = (int)(dstWidth * width[i] / 100);
                        ts.GridColumnStyles.Add(style);
                    }
                    else
                    {
                        style1 = new DataGridTextBoxColumn();
                        style1.MappingName = name[i];
                        style1.Width = (int)(dstWidth * width[i] / 100);
                        ts.GridColumnStyles.Add(style1);
                    }
                }


                else
                {
                    style1 = new DataGridTextBoxColumn();
                    style1.MappingName = name[i];
                    style1.Width = (int)(dstWidth * width[i] / 100);
                    ts.GridColumnStyles.Add(style1);
                }
            }

            return ts;
        }

        private void UpdatedgTable(bool bSerialize)
        {
            try
            {

                string[] colName = new string[] { "sku", "sku_dsc", "sku_order_qty", "CheckBox" };
                string[] colValue = new string[] { "SKU", "品名", "建议数量", "false" };
                //int[] colWidth = new int[] { 35, 35, 20, 7 };
                int[] colWidth = new int[] { 19, 51, 20, 7 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;
                if (!cols.Contains("CheckBox"))
                {
                    cols.Add("CheckBox");
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["CheckBox"] = "false";
                    }
                }


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
                string[] colName = new string[] { "sku", "sku_dsc", "sku_order_qty", "CheckBox" };
                string[] colValue = new string[] { "SKU", "品名", "建议数量", "false" };
                //int[] colWidth = new int[] { 35, 35, 20, 7 };
                int[] colWidth = new int[] { 19, 51, 20, 7 };

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colValue.Length; i++)
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
            DataGridCustomCheckBoxColumn CheckBoxCol = null;
            //DataGridCustomTextBoxColumn TextBoxCol = null;
            DataTable dtResult = new DataTable();
            dtResult.TableName = dt.TableName;
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("CheckBox"))
                {
                    dtResult.Columns[i].DataType = typeof(Boolean);
                    CheckBoxCol = (DataGridCustomCheckBoxColumn)styles[i];
                }
                //if (styles[i].MappingName.Equals("sku_qty"))
                //{
                //    TextBoxCol = (DataGridCustomTextBoxColumn)styles[i];
                //}
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
                    if (name.Equals("CheckBox"))
                    {
                        rowNew[name] = bool.Parse(dt.Rows[i][name].ToString());
                    }
                    else
                    {
                        rowNew[name] = dt.Rows[i][name];

                    }
                }

                dtResult.Rows.Add(rowNew);
            }
            dgTable.DataSource = dtResult;
            if (CheckBoxCol != null && to > 0)
            {
                CheckBox Cbox = (CheckBox)CheckBoxCol.HostedControl;
                Cbox.CheckStateChanged += new EventHandler(Cbox_CheckStateChanged);
            }
            int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
        }

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                string[] colName = { "sku", "sku_dsc", "sku_order_qty" };
                DataTable dt = ds.Tables["detail"];

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sku"] = dt.Rows[i]["sku"];
                    rowNew["sku_dsc"] = dt.Rows[i]["sku_dsc"];
                    rowNew["sku_order_qty"] = dt.Rows[i]["sku_order_qty"];
                    dtOK.Rows.Add(rowNew);
                }
            }
            catch (Exception)
            {
            }

            return dtOK;
        }

        #endregion

        #region 接口请求
        private void requesS01()
        {
            apiID = API_ID.S0001;
            SKUName.Text = "";
            string msg = "request=S00;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text+";sto="+ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR02()
        {
            apiID = API_ID.R103502;
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "1035", "02");
            string msg = "request=1035;usr=" + Config.User + ";op=02;sto=" + ShopNo+";json="+json;
            msg = DataGuid + msg;
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

                    case API_ID.S0001:
                        {
                            string file = Config.getApiFile("S00", "01");
                            from += "/S00/" + file;
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

        #region 翻页

        private void NexPage_Click(object sender, EventArgs e)
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

        private void PrePage_Click(object sender, EventArgs e)
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

        #region UI响应

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy && SKUNO.Text != "")
            {
                if (e.KeyValue == 13)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    btnAdd.Focus();
                    requesS01();
                }
            }
        }

        private void Cbox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            bool bCheck = box.CheckState == CheckState.Checked ? true : false;

            UpdateDsData();

            if (bCheckAll && !bCheck)
            {
                bCheckAll = false;
                DataTable dtHeader = (DataTable)dgHeader.DataSource;
                dtHeader.Rows[0]["CheckBox"] = false;
            }
        }

        private void checkAll(bool bAll)
        {
            bCheckAll = bAll;

            DataTable dt = ds.Tables["detail"];
            if (dt.Rows.Count >= 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["CheckBox"] = bAll.ToString();
                }

                UpdatedgHeader();
                UpdatedgTable(false);
            }

        }

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

                            if (colName.Equals("CheckBox"))
                            {

                                if (bCheckAll)
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
                    int colIndex = dgTable.CurrentCell.ColumnNumber;
                    string ColName = dtt.Columns[colIndex].ColumnName;
                    if (!ColName.Equals("CheckBox"))
                    {
                        int rowIndex = dgTable.CurrentCell.RowNumber;
                        dgTable.Select(rowIndex);
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
                            if (hitTest.Column == 3)
                            {
                                dt.Rows[hitTest.Row][hitTest.Column] = !(bool)dt.Rows[hitTest.Row][hitTest.Column];
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

        private void UpdateDsData()
        {
            try
            {
                DataTable dts = ds.Tables["detail"].DefaultView.ToTable();
                DataTable dtt = (DataTable)dgTable.DataSource;
                if (dts != null && dtt != null)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        for (int j = 2; j < 3; j++)
                        {
                            if (dtt.Columns[j].ColumnName.Equals("CheckBox"))
                            {
                                dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j].ToString();
                            }
                            else
                            {
                                dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j];
                            }
                        }
                    }
                    ds.Tables.Remove("detail");
                    ds.Tables.Add(dts);
                    Serialize(guid);
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
                            rowFlag = hitTest.Row;
                            dgTable.Select(dgTable.CurrentRowIndex);
                            contextMenu1.MenuItems.Add(menuDetail);
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
                ChildID = Form_ID.OrderDetail;
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
                    case Form_ID.OrderDetail:
                        Child = new FormJBOrdetail();
                        break;
                    default:
                        return;
                }
                DataTable dt = (DataTable)dgTable.DataSource; 
                ((FormJBOrdetail)Child).getMod += new FormJBOrdetail.GetModNO(getModNO);

                if (bDeserialize)
                {
                    ((ISerializable)Child).Deserialize(null);
                }
                else
                {
                    ((ISerializable)Child).init(new object[] {ShopNo,dt.Rows[rowFlag]["sku"].ToString() });
                }

                Child.ShowDialog();
                Serialize(guid);

                Show();
                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getModNO(int Num)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                int row = dgTable.CurrentCell.RowNumber;
                dt.Rows[row]["sku_order_qty"] = Num.ToString();
                UpdateDsData();

            }
            catch (Exception)
            {
            }
        }

        private bool checkAdd()
        {
            try
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU!");
                    return false;
                }

                if (SKUName.Text == "")
                {
                    MessageBox.Show("请先获取商品名!");
                    return false;
                }
                if (ds != null)
                {
                    DataTable dt = ds.Tables["detail"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string tSku = dt.Rows[i]["sku"].ToString();
                        if (SKUNO.Text.Equals(tSku))
                        {
                            MessageBox.Show("该商品已存在于列表!");
                            return false;
                        }
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (checkAdd())
                    {
                        DataRow newrow = ds.Tables["detail"].NewRow();
                        newrow["sku"] = SKUNO.Text;
                        newrow["sku_dsc"] = SKUName.Text;
                        newrow["sku_order_qty"] = textBox1.Text;
                        newrow["CheckBox"] = bCheckAll.ToString();
                        ds.Tables["detail"].Rows.Add(newrow);
                        UpdatedgTable(false);
                        Serialize(guid);
                        SKUNO.Text = "";
                        SKUName.Text = "";
                        SKUNO.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (MessageBox.Show("确定删除选中行？", "",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        UpdateDsData();
                        DataTable dts = ds.Tables["detail"];
                        if (dts != null)
                        {
                            bool Checked = false;
                            if (dts.Rows.Count >= 1)
                            {
                                for (int i = dts.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (bool.Parse(dts.Rows[i]["CheckBox"].ToString()))
                                    {
                                        dts.Rows.RemoveAt(i);
                                        Checked = true;
                                    }
                                }
                                if (!Checked)
                                {
                                    MessageBox.Show("未选中任何行!");
                                }
                                else
                                {
                                    UpdatedgTable(false);
                                }
                            }
                            else
                            {
                                MessageBox.Show("表格内无数据!");
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

        private bool CheckSubmit()
        {
            try
            {
                UpdateDsData();
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ;
                }
                else
                {
                    MessageBox.Show("表格内无数据!");
                    return false;
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
                if (CheckSubmit())
                {
                    requestR02();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                DialogResult = DialogResult.OK;
                if (btn02.Enabled)
                {
                    if (MessageBox.Show("是否保存当前数据？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        File.Delete(Config.DirLocal + guid);
                    }
                    else
                    {
                        DialogResult = DialogResult.Abort;
                    }
                }
                else
                {
                    File.Delete(Config.DirLocal + guid);
                }
                Close();
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
                    case API_ID.S0001:
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
                    case API_ID.R103502:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功!");
                            btn02.Enabled = false;
                            SKUNO.Enabled = false;
                            btnClear.Enabled = false;
                            btnAdd.Enabled = false;
                            btnDelete.Enabled = false;
                            dgTable.Enabled = false;
                            Serialize(guid);
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
                CreateDS();
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
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
                data[(int)DATA_INDEX.ShopNo] = ShopNo;
                data[(int)DATA_INDEX.btn02] = btn02.Enabled.ToString();
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
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                    btn02.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.btn02]);
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);
                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        if (ds.Tables["detail"].Rows.Count > 0)
                        {
                            UpdatedgTable(true);
                        }
                    }
                    else
                    {
                        CreateDS();
                    }
                    if (ChildID != Form_ID.Null)
                    {
                        ShowChild(false);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            SKUNO.Text = "";
            SKUName.Text = "";
        }
    }
}