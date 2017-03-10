using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using HolaCore;
using System.Threading;
namespace Hola_Inventory
{
    public partial class FormFPSKU : Form,ISerializable,ConnCallback
    {

        private const string guid = "176F5459-890A-46fc-B029-AAD316EC6923";
        private const string OGuid = "00000000000000000000000000000000";
        private DataSet ds = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        #region 子窗口ID

        private enum Form_ID
        {
            FPSKUMod,
            Null,
        }

        #endregion
        private Form Child = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            FPCode,
            CabinetNO,
            CBNO,
            NumSum,
            NumArl,
            PageIndex,
            Child,
            BTNCOMFIRM,
            INumArl,
            DataMax,
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R60101,
            R60102,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private SerializeClass sc = null;
        private Form_ID ChildID = Form_ID.Null;
        private string xmlfile = null;//Config.DirLocal+"60101.xml";
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 5;
        private int colIndex = -1;
        private string FPCode = "";
        Regex reg = new Regex(@"^\d+$");
        private DataTable dtResult = null;
        private int rowFlag = -1;
        private string FPType = "";
        private int numArl = 0;
        private bool bSkuCheck = false;
        public delegate void OpLocno(string locno);
        public OpLocno OpLoc = null;
        private int TaskBarHeight = 0;
        public FormFPSKU()
        {
            InitializeComponent();
            doLayout();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / (float)srcWidth, yTime = dstHeight / (float)srcHeight;
            xTime -= 1.0f;
            yTime -= 1.0f;
            int xOffSet = (int)(label1.Location.X * xTime);
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();
                if (dstWidth > 240)
                {
                    int muti = 2;
                    dgTable.Height = dgTable.Height + muti*(int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += muti;
                }

                PrePage.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Top = dgTable.Top + dgTable.Height + 5;
                Page.Top = dgTable.Top + dgTable.Height + 5;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                btnComfirm.Top = dstHeight - btnComfirm.Height;
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                

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

        private void dsDataTypeChange()
        {
            try
            {
                if (ds != null && ds.Tables.Count>=1)
                {
                    DataTable dtConfig = null;
                    DataTable dtInfo = null;
                    DataTable dtHead=null;
                    DataTable dtDetail = null;
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals("config"))
                        {
                            dtConfig = ds.Tables[i].Copy();
                        }
                        else if (ds.Tables[i].TableName.Equals("info"))
                        {
                            dtInfo = ds.Tables[i].Copy();
                        }
                        else if (ds.Tables[i].TableName.Equals("head"))
                        {
                            dtHead = ds.Tables[i].Copy();
                        }
                        else if (ds.Tables[i].TableName.Equals("detail"))
                        {
                            dtDetail = new DataTable();
                        } 
                    }

                    if (dtDetail != null)
                    {
                        dtDetail.TableName = ds.Tables["detail"].TableName;
                        DataRowCollection rows = ds.Tables["detail"].Rows;
                        DataColumnCollection cols = ds.Tables["detail"].Columns;

                        for (int i = 0; i < cols.Count; i++)
                        {
                            dtDetail.Columns.Add(cols[i].ColumnName);
                            if (dtDetail.Columns[i].ColumnName.Equals("sel_no"))
                            {
                                dtDetail.Columns[i].DataType = typeof(Int32);
                            }
                        }

                        for (int i = 0; i < rows.Count; i++)
                        {
                            DataRow newRow = dtDetail.NewRow();
                            for (int j = 0; j < cols.Count; j++)
                            {
                                newRow[j] = rows[i][j];
                            }
                            dtDetail.Rows.Add(newRow);
                        }
                    }
                    ds = new DataSet();
                    if (dtConfig != null)
                    {
                        ds.Tables.Add(dtConfig);
                    }
                    if (dtInfo != null)
                    {
                        ds.Tables.Add(dtInfo);
                    }
                    if (dtHead != null)
                    {
                        ds.Tables.Add(dtHead);
                    }
                    if (dtDetail != null)
                    {
                        ds.Tables.Add(dtDetail);
                    }
                }
            }
            catch (Exception)
            {
            }
            
        }

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
                            if (!ds.Tables[i].Columns.Contains("adj_reason"))
                            {
                                ds.Tables[i].Columns.Add("adj_reason");
                            }
                        }
                        
                    }
                    if (!bNoData)
                    {

                        NumSum.Text = ds.Tables["detail"].Rows.Count.ToString();
                        NumArl.Text = "0";
                        dsDataTypeChange();
                        UpdatedgTable(true);
                        int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageCount == 0)
                            Page.Text = "1/1";
                        else
                            Page.Text = "1/" + pageCount.ToString();
                        
                    }
                    else
                    {
                        pageIndex = 1;
                        Page.Text = "1/1";
                        dgTable.DataSource = null;
                        MessageBox.Show("无数据！");
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
            DataGridCustomColumnBase style = null;
            DataGridColumnStyle style1 = null;
            for (int i = 0; i < name.Length; i++)
            {
               
               if (name[i].Equals("sku_second_qty"))
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

        private void UpdatedgTable(bool Init)
        {
            try
            {
                //string[] colValue = new string[] { "序号", "验", "SKU", "品名", "数量", "复验量" };
                //string[] colName = new string[] { "sel_no", "check", "sku", "sku_dsc", "sku_first_qty", "sku_second_qty" };
                //int[] colWidth = new int[] { 10, 7, 19, 41, 10, 10 };
                string[] colValue = new string[] { "序号", "SKU", "品名", "数量", "复验量", "验" };
                string[] colName = new string[] { "sel_no", "sku", "sku_dsc", "sku_first_qty", "sku_second_qty", "check" };
                int[] colWidth = new int[] { 0, 19, 51, 10, 10, 7 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;
                if (!cols.Contains("check"))
                {
                    cols.Add("check");
                }
                if (!cols.Contains("sku_second_qty"))
                {
                    cols.Add("sku_second_qty");
                }
                if (Init)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["check"] = "";
                    }
                }

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();
                UpdatedgHeader();
            }
            catch (Exception)
            {
            }
        }
        //1. 复盘详细页面
        private void UpdatedgHeader()
        {
            try
            {
                //string[] colValue = new string[] { "序号", "验", "SKU", "品名", "数量", "复验量" };
                //string[] colName = new string[] { "sel_no", "check", "sku", "sku_dsc", "sku_first_qty", "sku_second_qty" };
                //int[] colWidth = new int[] { 10, 7, 19, 41, 10, 10 };
                string[] colValue = new string[] { "序号", "SKU", "品名", "数量", "复验量", "验"};
                string[] colName = new string[] { "sel_no", "sku", "sku_dsc", "sku_first_qty", "sku_second_qty", "check" };
                int[] colWidth = new int[] { 0, 19, 51, 10, 10, 7};
                DataTable dtHeader = new DataTable();
                dtHeader.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colValue.Length; i++)
                {
                     dtHeader.Columns.Add(colName[i]);
                }
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
            DataGridCustomTextBoxColumn TextBoxCol = null;
            dtResult = new DataTable();

            dtResult.TableName = dt.TableName;
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("sku_second_qty"))
                {
                    TextBoxCol = (DataGridCustomTextBoxColumn)styles[i];
                }
            }
            if (!dtResult.Columns.Contains("adj_reason"))
            {
                dtResult.Columns.Add("adj_reason");
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
            if (TextBoxCol != null && to > 0)
            {
                TextBox Tbox = (TextBox)TextBoxCol.HostedControl;
                Tbox.LostFocus += new EventHandler(Tbox_LostFocus);
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
                string[] colName = { "sel_no", "sku", "sku_second_qty", "adj_reason" };
                DataTable dt = ds.Tables["detail"];

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sel_no"] = dt.Rows[i]["sel_no"];
                    rowNew["sku"] = dt.Rows[i]["sku"];
                    string qty = dt.Rows[i]["sku_second_qty"].ToString();
                    if (qty.Length == 0)
                    {
                        qty = dt.Rows[i]["sku_first_qty"].ToString();
                    }
                    if (qty.Length == 0)
                    {
                        qty = "0";
                    }
                    rowNew["sku_second_qty"] = qty;
                    rowNew["adj_reason"] = dt.Rows[i]["adj_reason"];
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
        private void request01()
        {
            apiID = API_ID.R60101;
            string msg = "request=601;usr=" + Config.User + ";op=01;stk_no="+FPCode+";loc_no="+CBNO.Text+";sto="+ShopNO.Text+";type="+FPType;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request02()
        {
            apiID = API_ID.R60102;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "601", "02");
            string msg = "request=601;usr=" + Config.User + ";op=02;sto=" + ShopNO.Text + ";stk_no=" + FPCode + ";loc_no=" + CBNO.Text + ";type="+FPType+";json="+json;
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
                    case API_ID.R60101:
                        {
                            string file = Config.getApiFile("601", "01");
                            from += "/601/" + file;
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

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
            checkData();
        }

        private void dgHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (!busy)
            {
                DataTable dtHeader = (DataTable)dgTable.DataSource;
                if (dtHeader != null && dtHeader.Rows.Count>=1)
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

                            UpdatedgTable(false);
                        }
                    }
                }
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
                    //if (!ColName.Equals("sku_second_qty"))
                    //{
                        int rowIndex = dgTable.CurrentCell.RowNumber;
                        dgTable.Select(rowIndex);
                    //}
                }
            }
            catch (Exception)
            {
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                contextMenu1.MenuItems.Clear();
                if (!busy)
                {
                    DataTable dt=(DataTable)dgTable.DataSource;
                    if (dt!= null && dt.Rows.Count>0)
                    {
                        int x = Control.MousePosition.X;
                        int y = Control.MousePosition.Y - dgTable.Top-TaskBarHeight;

                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);
                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            contextMenu1.MenuItems.Add(menuDetail);
                            if (dt.Rows[hitTest.Row]["check"].ToString().Equals("V"))
                            {
                                bSkuCheck = true;
                            }
                            if (dgTable.CurrentRowIndex != hitTest.Row)
                            {
                                dgTable.UnSelect(dgTable.CurrentRowIndex);
                                dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                            }
                            dgTable.Select(dgTable.CurrentRowIndex);
                            rowFlag = dgTable.CurrentRowIndex;
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
            try
            {
                if (!busy)
                {
                    DataTable dt = (DataTable)dgTable.DataSource;
                    if (dt.Rows[dgTable.CurrentRowIndex]["check"].ToString().Equals("V"))
                    {
                        if (MessageBox.Show("此商品重复复盘,确认修改？", "",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            ChildID = Form_ID.FPSKUMod;
                            ShowChild(false);
                        }
                    }
                    else
                    {
                        ChildID = Form_ID.FPSKUMod;
                        ShowChild(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = dt.Rows[dgTable.CurrentRowIndex]["sku_second_qty"].ToString();

                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        UpdatedgTable(false);
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 999)
                        {
                            MessageBox.Show("不可大于999！");
                            UpdatedgTable(false);
                        }
                        else if (UInt64.Parse(ok) > 0)
                        {
                            string subtext = ok.Substring(0, 1);
                            if (UInt64.Parse(subtext) == 0)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdatedgTable(false);
                            }
                            else
                            {
                                UpdateDsData();
                            }
                        }
                        else if (UInt64.Parse(ok) == 0)
                        {
                            if (ok.Length >= 2)
                            {
                                MessageBox.Show("请输入非负整数！");
                                UpdatedgTable(false);
                            }
                            else
                            {
                                UpdateDsData();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
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
                       dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i]["check"] = dtt.Rows[i]["check"].ToString();
                       dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i]["sku_second_qty"] = dtt.Rows[i]["sku_second_qty"].ToString();
                       dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i]["adj_reason"] = dtt.Rows[i]["adj_reason"].ToString();
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

        private void getModNum(int Num,string reason)
        {
            try
            {
                DataTable dt =(DataTable)dgTable.DataSource;
                int row = dgTable.CurrentCell.RowNumber;
                dt.Rows[row]["sku_second_qty"] = Num.ToString();
                dt.Rows[row]["check"] = "V";
                dt.Rows[row]["adj_reason"] = reason;
                if (!bSkuCheck)
                {
                    numArl++;
                }
                NumArl.Text = numArl.ToString();
                bSkuCheck = false;
                UpdateDsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getModReason(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    if (dt.TableName.Equals("Reason"))
                    {
                        if (ds.Tables["Reason"] == null)
                        {
                            ds.Tables.Add(dt);
                        }
                    }
                }
                
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void btnComfirm_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (MessageBox.Show("复盘数据已完成，确认提交吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes
                    && CheckSubmit())
                {
                    request02();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                //未提交
                if (btnComfirm.Enabled)
                {
                    //确认离开
                    if (MessageBox.Show("确认离开吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        DialogResult = DialogResult.Abort;
                        //保存数据
                        if (MessageBox.Show("保存当前数据？", "",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            Serialize(guid);
                        }
                        //不保存
                        else
                        {
                            File.Delete(Config.DirLocal + guid);
                        }
                        Close();
                    }
                }
                //已提交
                else
                {
                    DialogResult = DialogResult.OK;
                    File.Delete(Config.DirLocal + guid);
                    Close();
                }
            }
        }

        private void ShowChild(bool bDeserialize)
        {
            try
            {
                DataTable dtt=(DataTable)dgTable.DataSource;
                if (Child != null)
                {
                    Child.Dispose();
                    Child = null;
                }
                
                DataRow row = dtt.Rows[dgTable.CurrentRowIndex];
                switch (ChildID)
                {
                    case Form_ID.FPSKUMod:
                        Child = new FormFPSKUMod();
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
                    DataTable dtReason = ds.Tables["Reason"];
                    ((ISerializable)Child).init(new object[] { ShopNO.Text, CBNO.Text, FPCode,row["sku"].ToString(),row["sku_first_qty"].ToString(),row["sku_dsc"].ToString(),dtReason });
                    Serialize(guid);
                }
                ((FormFPSKUMod)Child).getMod += new FormFPSKUMod.GetModDele(getModNum);
                ((FormFPSKUMod)Child).getReason += new FormFPSKUMod.GetReasonTb(getModReason);
                Child.ShowDialog();
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

        #endregion

        #region 翻页

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable.DataSource != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    UpdateDsData();
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
            if (!busy && dgTable.DataSource != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    UpdateDsData();
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
                    case API_ID.R60101:
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

                    case API_ID.R60102:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("上传成功!");
                            btnComfirm.Enabled = false;
                            dgTable.Enabled = false;
                            OpLoc(CBNO.Text);
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
            ShopNO.Text = (string)param[0];
            FPCode = (string)param[1];
            CBNO.Text = (string)param[2];
            FPType = (string)param[3];
            ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
            {
                this.Invoke(new InvokeDelegate(() =>
                {
                    request01();
                }));
            }));
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
                data[(int)DATA_INDEX.ShopNO] = ShopNO.Text;
                data[(int)DATA_INDEX.FPCode] = FPCode;
                data[(int)DATA_INDEX.CBNO] = CBNO.Text;
                data[(int)DATA_INDEX.NumArl] = NumArl.Text;
                data[(int)DATA_INDEX.NumSum] = NumSum.Text;
                data[(int)DATA_INDEX.Child] = ChildID.ToString();
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
                data[(int)DATA_INDEX.BTNCOMFIRM] = btnComfirm.Enabled.ToString();
                data[(int)DATA_INDEX.INumArl] = numArl.ToString();
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
                    ShopNO.Text = sc.Data[(int)DATA_INDEX.ShopNO];
                    FPCode = sc.Data[(int)DATA_INDEX.FPCode];
                    CBNO.Text = sc.Data[(int)DATA_INDEX.CBNO];
                    NumArl.Text = sc.Data[(int)DATA_INDEX.NumArl];
                    NumSum.Text = sc.Data[(int)DATA_INDEX.NumSum];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    btnComfirm.Enabled = bool.Parse(sc.Data[(int)DATA_INDEX.BTNCOMFIRM]);
                    numArl = int.Parse(sc.Data[(int)DATA_INDEX.INumArl]);
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);

                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        if (ds.Tables["detail"].Rows.Count > 0)
                        {
                            dsDataTypeChange();
                            UpdatedgTable(false);
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