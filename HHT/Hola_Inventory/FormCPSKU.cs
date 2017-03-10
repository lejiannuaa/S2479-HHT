using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;
using HolaCore;
namespace Hola_Inventory
{
    public partial class FormCPSKU : Form, ISerializable, ConnCallback
    {
        private const string guid = "ADA5F6F0-68A8-4672-8E9E-7030AE8DFE0F";
        private const string OGuid = "00000000000000000000000000000000";
        private DataSet ds = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            CabinetNO,
            SKUNO,
            SKUName,
            SKUNum,
            ItemsAlr,
            CPCode,
            ItemsSum,
            NumSum,
            ZeroSum,
            DupSum,
            PageIndex,
            DataMax,
        }

        #endregion
        private string xmlFile = null;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            S0001,
            l0405,
            R50101,
            R50102,
            R50103,
            DOWNLOAD_01,
            DOWNLOAD_02
        }
        #endregion
        private int TaskBarHeight = 0;
        private API_ID apiID = API_ID.NULL;
        private SerializeClass sc = null;
        private bool bCheckAll = false;
        private DataTable dtResult = null;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 5;
        //保留上次请求的SKU
        private string lastSKU = null;
        private int colIndex = -1;
        Regex reg = new Regex(@"^\d+$");
        public delegate void OpLocno(string locno);
        public OpLocno OpLoc = null;
        private bool isPD = false;

        public FormCPSKU()
        {
            InitializeComponent();

            doLayout();
            SKUNO.Focus();
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
                    
                    dgTable.Height = dgTable.Height + (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += 1;
                }
                CP_UP.Height = dstHeight - CP_UP.Top;

                PrePage.Top = dgTable.Top + dgTable.Height + 8;
                NexPage.Top = dgTable.Top + dgTable.Height + 8;
                Page.Top = dgTable.Top + dgTable.Height + 8;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                // btnReturn.Left = dstWidth - btnConfirm.Left - btnReturn.Width;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

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
            string[] colName = { "sel_no", "sku", "sku_dsc", "sku_first_qty", "CheckBox" };
            Type[] colType = { typeof(int), typeof(string), typeof(string), typeof(int), typeof(string) };
            DataTable dt = new DataTable();
            ds = new DataSet();
            dt.TableName = "detail";
            for (int i = 0; i < colName.Length; i++)
            {
                dt.Columns.Add(colName[i], colType[i]);
            }
            ds.Tables.Add(dt);
            dt.Dispose();
        }

        private void showXML()
        {
            switch (apiID)
            {
                case API_ID.DOWNLOAD_01:
                    break;

                case API_ID.DOWNLOAD_02:
                    break;

                default:
                    return;
            }

            LoadData();
        }

        private void addTable(DataTable dt, string name)
        {
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].TableName.Equals(name))
                        {
                            ds.Tables.RemoveAt(i);
                            break;
                        }
                    }
                }

                DataTable dtNew = dt.Clone();
                dtNew.TableName = name;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dtNew.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row[j] = dt.Rows[i][j];
                    }
                    dtNew.Rows.Add(row);
                }
                if (ds != null)
                {
                    ds.Tables.Add(dtNew);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool Load01(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {
                DataSet  DS = new DataSet();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return bNoData;
        }

        private bool Load02(XmlNodeReader reader)
        {
            bool bNoData = true;
            try
            {
                DataSet dsSKU = new DataSet();
                dsSKU.ReadXml(reader);

                for (int i = 0; i < dsSKU.Tables.Count; i++)
                {
                    DataTable dt = dsSKU.Tables[i];
                    if (dt.TableName == "info")
                    {
                        bNoData = false;
                        addTable(dt, "SKU");
                        SKUName.Text = dt.Rows[0]["品名"].ToString().Equals("null") ? "" : dt.Rows[0]["品名"].ToString();
                        //if (SKUNO.Text.Length > 9 || SKUNO.Text.Length == 8)
                        //{
                        //    SKUNO.Text = dt.Rows[0]["HHTSKU"].ToString();
                        //}
                        SKUNO.Text = dt.Rows[0]["HHTSKU"].ToString();
                        lastSKU = SKUNO.Text;
                        break;
                    }
                }

                if (bNoData)
                {
                    lastSKU = "";
                }
                dsSKU.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return bNoData;
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;

            bool bNoData = false;
            XmlDocument doc = null;
            XmlNodeReader reader = null;
            try
            {
                if (File.Exists(xmlFile))
                {
                    doc = new XmlDocument();
                    doc.Load(xmlFile);
                    reader = new XmlNodeReader(doc);
                }
                switch (apiID)
                {
                    case API_ID.DOWNLOAD_01:

                        if (File.Exists(xmlFile))
                        {
                            bNoData = Load01(reader);

                            if (bNoData)
                            {
                                MessageBox.Show("无数据！");
                            }
                        }
                        else
                        {
                            pageIndex = 1;
                            ds = new DataSet();
                            CreateDS();
                            MessageBox.Show("请求文件不存在，请重新请求!");
                        }
                        break;

                    case API_ID.DOWNLOAD_02:
                        if (File.Exists(xmlFile))
                        {
                            bNoData = Load02(reader);
                            if (bNoData)
                            {
                                MessageBox.Show("无数据！");
                            }
                            else
                            {
                                SKUNum.Focus();
                            }
                        }
                        else
                        {
                            SKUName.Text = "";
                            lastSKU = "";
                            MessageBox.Show("请求文件不存在，请重新请求!");
                        }
                        break;

                    default:
                        break;
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
                    reader.Close();
                File.Delete(xmlFile);
                GC.Collect();
                Serialize(guid);
            }
        }

        private DataGridTableStyle getTableStyle(string[] name, int[] width,bool GridNo)
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
                else if (name[i].Equals("sku_first_qty"))
                {
                    style = new DataGridCustomTextBoxColumn();

                    style.Owner = dgTable;
                    style.ReadOnly = false;
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
                string[] colName = new string[] { "sel_no", "sku", "sku_dsc", "sku_first_qty", "CheckBox" };
                string[] colValue = new string[] { "序号", "SKU", "品名", "数量", "false" };
                //int[] colWidth = new int[] { 12, 25, 35, 18, 7 };
                int[] colWidth = new int[] { 12, 19, 41, 18, 7 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;

                if (!cols.Contains("Checkbox"))
                {
                    cols.Add("CheckBox");
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["CheckBox"] = "false";
                    }
                }

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth,true));
                UpdateRow();
                UpdatedgHeader();
            }
            catch (Exception)
            {
            }
        }

        private void UpdatedgHeader()
        {
            try
            {
                string[] colValue = new string[] { "序号", "SKU", "品名", "数量", "false" };
                string[] colName = new string[] { "sel_no", "sku", "sku_dsc", "sku_first_qty", "CheckBox" };
                //int[] colWidth = new int[] { 12, 25, 35, 18, 7 };
                int[] colWidth = new int[] { 12, 19, 41, 18, 7 };
                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();//ds.Tables["detail"].Clone();
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
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth,false));
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
            DataGridCustomTextBoxColumn TextBoxCol = null;
            dtResult = new DataTable();
            dtResult.TableName = dt.TableName;
            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("CheckBox"))
                {
                    dtResult.Columns[i].DataType = typeof(Boolean);
                    CheckBoxCol = (DataGridCustomCheckBoxColumn)styles[i];
                }
                if (styles[i].MappingName.Equals("sku_first_qty"))
                {
                    TextBoxCol = (DataGridCustomTextBoxColumn)styles[i];
                }
            
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
                 Cbox.CheckStateChanged +=new EventHandler(Cbox_CheckStateChanged);
             }
             if (TextBoxCol != null && to > 0)
             {
                 TextBox Tbox = (TextBox)TextBoxCol.HostedControl;
                 Tbox.LostFocus+=new EventHandler(Tbox_LostFocus);

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
                string[] colName = { "sel_no", "sku", "sku_dsc", "sku_first_qty" };
                DataTable dt = ds.Tables["detail"];

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sel_no"] =dt.Rows[i]["sel_no"];
                    rowNew["sku"] = dt.Rows[i]["sku"];
                    //rowNew["sku_dsc"] = "";
                    // dt.Rows[i]["sku_dsc"];
                    rowNew["sku_first_qty"] =dt.Rows[i]["sku_first_qty"];
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
            string msg = "request=S00;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNO.Text;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR01()
        {
            apiID = API_ID.R50101;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "501", "02");
            string msg = "request=501;usr=" + Config.User + ";op=01;sto=" + ShopNO.Text + ";stk_no=" + CPCode.Text+";loc_no="+CBNO.Text+";json="+json;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request05()
        {
            apiID = API_ID.l0405;
            string msg = "request=104;usr=" + Config.User + ";op=05;bc=" + SKUNO.Text.ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }
        //发送盘点中状态
        private void requestR02()
        {
            apiID = API_ID.R50102;
            string msg = "request=501;usr=" + Config.User + ";op=02;sto=" + ShopNO.Text + ";stk_no=" + CPCode.Text + ";loc_no=" + CBNO.Text;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }
        //结束盘点中状态
        private void requestR03()
        {
            apiID = API_ID.R50103;
            string msg = "request=501;usr=" + Config.User + ";op=03;sto=" + ShopNO.Text + ";stk_no=" + CPCode.Text + ";loc_no=" + CBNO.Text;
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
                    case API_ID.S0001:
                        {
                            string file = Config.getApiFile("S00", "01");
                            from += "/S00/" + file;
                            to += file;
                            xmlFile = to;
                            apiID = API_ID.DOWNLOAD_01;
                        }
                        break;

                    case API_ID.l0405:
                        {
                            string file = Config.getApiFile("104", "05");
                            from += "/104/" + file;
                            to += file;
                            xmlFile = to;
                            apiID = API_ID.DOWNLOAD_02;
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
            if (!busy && e.KeyValue == 13)
            {
                if (SKUNO.Text != "")
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    SKUName.Text = "";
                    request05();
                }
                else
                {
                    MessageBox.Show("查询SKU不可为空!");
                }
                Serialize(guid);
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

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
            checkData();
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

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
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
                        SKUNO.Text = dt.Rows[hitTest.Row]["sku"].ToString();
                        SKUNum.Text = dt.Rows[hitTest.Row]["sku_first_qty"].ToString();
                        SKUName.Text = dt.Rows[hitTest.Row]["sku_dsc"].ToString();
                        if (hitTest.Column == 4)
                        {

                            DataGridCustomColumnBase style = (DataGridCustomColumnBase)dgTable.TableStyles[0].GridColumnStyles[hitTest.Column];

                            //if (bCheckAll)
                            //{
                            //    bCheckAll = false;
                            //    DataTable dtHeader = (DataTable)dgHeader.DataSource;
                            //    dtHeader.Rows[0]["CheckBox"] = false;
                            //}
                            dt.Rows[hitTest.Row][hitTest.Column] = !(bool)dt.Rows[hitTest.Row][hitTest.Column];

                            //UpdateDsData();
                        }
                    }
                }

            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            DataTable dtt = (DataTable)dgTable.DataSource;
            if (dtt != null)
            {
                int colIndex = dgTable.CurrentCell.ColumnNumber;
                string ColName = dtt.Columns[colIndex].ColumnName;
                if (!ColName.Equals("CheckBox") && !ColName.Equals("sku_first_qty"))
                {
                    int rowIndex = dgTable.CurrentCell.RowNumber;
                    dgTable.Select(rowIndex);
                }
            }
        }

        private void checkData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = dt.Rows[dgTable.CurrentRowIndex]["sku_first_qty"].ToString();

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
                        else if (UInt64.Parse(ok) >= 0)
                        {
                            UpdateDsData();
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

        private bool checkValidation()
        {
            try
            {

                if (string.IsNullOrEmpty(SKUNO.Text))
                {
                    MessageBox.Show("请输入SKU！");
                    return false;
                }

                if (string.IsNullOrEmpty(SKUName.Text))
                {
                    MessageBox.Show("无此SKU！");
                    return false;
                }

                if (reg.IsMatch(SKUNum.Text))
                {
                    string Num = SKUNum.Text;
                    if (UInt64.Parse(Num) > 999)
                    {
                        MessageBox.Show("不可大于999！");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("请输入非负整数！");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
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
                        for (int j = 3; j < 5; j++)
                        {
                            dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] =dtt.Rows[i][j].ToString();
                        }
                    }
                    ds.Tables.Remove("detail");
                    ds.Tables.Add(dts);
                    Serialize(guid);
                }
            }
            catch (Exception)
            {
            }
        }

        private int SKUNumSum()
        {
            int numSum = 0;
            try
            {
                DataTable dt = ds.Tables["detail"];
             
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Num = dt.Rows[i]["sku_first_qty"].ToString();
                    if (reg.IsMatch(Num))
                    {
                        numSum += int.Parse(Num);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return numSum;
        }

        private void CP_UP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (CP_UP.SelectedIndex == 0)
                {

                }
                else
                {
                    CBNOI.Text = CBNO.Text;
                    ItemsSum.Text = ItemsAlr.Text;
                    NumSum.Text = SKUNumSum().ToString();

                    int nZero = 0;
                    Dictionary<string, int> dictOriginal = new Dictionary<string, int>();
                    Dictionary<string, int> dictDup = new Dictionary<string, int>();
                    DataTable dt = ds.Tables["detail"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sku = (string)dt.Rows[i]["sku"];
                        if (dictOriginal.ContainsKey(sku))
                        {
                            if (!dictDup.ContainsKey(sku))
                            {
                                dictDup.Add(sku, 1);
                            }
                        }
                        else
                        {
                            dictOriginal.Add(sku, 0);
                        }

                        if ((int)dt.Rows[i]["sku_first_qty"] == 0)
                        {
                            nZero++;
                        }
                    }
                    ZeroSum.Text = nZero.ToString();
                    DupSum.Text = dictDup.Count.ToString();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
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
                            for (int i = 0; i < dts.Rows.Count; i++)
                            {
                                dts.Rows[i]["sel_no"] = i + 1;
                            }
                            if (!Checked)
                            {
                                MessageBox.Show("未选中任何行!");
                            }
                            else
                            {
                                //删除所有页数据行
                                if (bCheckAll)
                                {
                                    pageIndex = 1;
                                    bCheckAll = false;
                                    UpdatedgHeader();
                                }
                                //删除部分数据行
                                else
                                {
                                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                                    if (pageIndex > pageCount)
                                    {
                                        pageIndex--;
                                    }
                                    if (pageIndex <= 0)
                                    {
                                        pageIndex = 1;
                                    }
                                }
                                UpdatedgTable(false);
                                ItemsAlr.Text = ds.Tables["detail"].Rows.Count.ToString();
                                Serialize(guid);
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (checkValidation())
                    {
                        DataTable Dt = ds.Tables["detail"];
                        DataRow newrow = Dt.NewRow();
                        newrow["sel_no"] = Dt.Rows.Count + 1;
                        newrow["sku"] = SKUNO.Text;
                        newrow["sku_dsc"] = SKUName.Text;
                        newrow["sku_first_qty"] = SKUNum.Text; ;
                        newrow["CheckBox"] = bCheckAll.ToString();
                        Dt.Rows.Add(newrow);
                        UpdatedgTable(false);

                        //自动翻页至最后一页
                        int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                        if (pageIndex != pageCount)
                        {
                            UpdateDsData();
                            pageIndex = pageCount;
                            UpdatedgTable(false);
                            Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                        }

                        SKUNO.Text = "";
                        SKUNO.Focus();
                        SKUName.Text = "";
                        SKUNum.Text = "";
                    }

                    ItemsAlr.Text = ds.Tables["detail"].Rows.Count.ToString();
                    Serialize(guid);
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

        private void btnConfirmI_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (CheckSubmit())
                {
                    if (MessageBox.Show("确定上传吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        requestR01();
                    }
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                //未提交
                if (btnConfirmI.Enabled)
                {
                    //确认离开
                    if (MessageBox.Show("确认离开吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        if (isPD)
                        {
                            requestR03();
                        }

                        //保存数据
                        if (MessageBox.Show("保存当前数据？", "",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            Serialize(guid);
                            DialogResult = DialogResult.Abort;
                        }
                        //不保存
                        else
                        {
                            DialogResult = DialogResult.OK;
                            File.Delete(Config.DirLocal + guid);
                        }
                        //Close();
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

                    case API_ID.l0405:
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
                            lastSKU = "";
                            MessageBox.Show(data);
                        }
                        break;
                  
                    case API_ID.R50101:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("上传成功!");

                            isPD = false;
                            btnConfirmI.Enabled = false;
                            SKUNO.Enabled = false;
                            btnDelete.Enabled = false;
                            btnConfirm.Enabled = false;
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

                    case API_ID.R50102:
                        if (result == ConnThread.RESULT_OK)
                        {
                            isPD = true;
                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;
                    case API_ID.R50103:
                        if (result == ConnThread.RESULT_OK)
                        {
                            isPD = false;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show(data);
                        }
                        break;


                    case API_ID.DOWNLOAD_01:


                    case API_ID.DOWNLOAD_02:
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

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            ShopNO.Text = (string)param[0];
            CPCode.Text = (string)param[1];
            CBNO.Text = (string)param[2];
            CBNOI.Text = (string)param[2];
            requestR02();
            CreateDS();
            Serialize(guid);
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
                data[(int)DATA_INDEX.CabinetNO] = CBNO.Text;
                data[(int)DATA_INDEX.ShopNO] = ShopNO.Text;
                data[(int)DATA_INDEX.CPCode] = CPCode.Text;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.SKUName]=SKUName.Text;
                data[(int)DATA_INDEX.SKUNum]=SKUNum.Text;
                data[(int)DATA_INDEX.ItemsAlr] = ItemsAlr.Text;
                data[(int)DATA_INDEX.ItemsSum] = ItemsSum.Text;
                data[(int)DATA_INDEX.NumSum] = NumSum.Text;
                data[(int)DATA_INDEX.ZeroSum] = ZeroSum.Text;
                data[(int)DATA_INDEX.DupSum] = DupSum.Text;
                data[(int)DATA_INDEX.PageIndex]=pageIndex.ToString();

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
                    CPCode.Text = sc.Data[(int)DATA_INDEX.CPCode];
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    SKUNum.Text = sc.Data[(int)DATA_INDEX.SKUNum];
                    CBNO.Text = sc.Data[(int)DATA_INDEX.CabinetNO];
                    CBNOI.Text = sc.Data[(int)DATA_INDEX.CabinetNO];
                    ItemsAlr.Text = sc.Data[(int)DATA_INDEX.ItemsAlr];
                    ItemsSum.Text = sc.Data[(int)DATA_INDEX.ItemsSum];
                    NumSum.Text = sc.Data[(int)DATA_INDEX.NumSum];
                    ZeroSum.Text = sc.Data[(int)DATA_INDEX.ZeroSum];
                    DupSum.Text = sc.Data[(int)DATA_INDEX.DupSum];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    ds = sc.DS;
                    if (ds != null && ds.Tables["detail"] != null)
                    {
                        UpdatedgTable(true);
                    }
                    else
                    {
                        CreateDS();
                    }
                   
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        private void SKUNum_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnConfirm_Click(null, null);
            }
        }
    }
}