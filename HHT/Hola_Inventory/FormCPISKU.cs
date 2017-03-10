using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using HolaCore;
using System.Threading;
namespace Hola_Inventory
{
    public partial class FormCPISKU : Form, ISerializable, ConnCallback
    {
        private const string guid = "6965442F-FCF4-493d-8B71-2219485223DE";
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
            pCode,
            CBNO,
            SKUNO,
            SKUName,
            PageIndex,
            DataMax,
        }

        #endregion
        private string xmlfile = null;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R70101,
            R70102,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private SerializeClass sc = null;
        private DataTable dtResult = null;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 5;
        private int colIndex = -1;
        Regex reg = new Regex(@"^\d+$");
        private int TaskBarHeight = 0;

        //判断sku和序号
        List<string> skuAndnumList = new List<string>();

        public FormCPISKU()
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

                if (dstWidth > srcWidth)
                {
                    int muti = 2;
                    dgTable.Height = dgTable.Height + muti * (int)Math.Ceiling((dgTable.Height - 2) / (double)TABLE_ROWMAX);
                    TABLE_ROWMAX += muti;
                }

                btnConfirm.Top = dstHeight - btnConfirm.Height - 35;
                btnReturn.Top = dstHeight - btnReturn.Height - 35;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3 + 35;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

                SKUNO.Focus();

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

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            bool BNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            try
            {
                if (File.Exists(xmlfile))
                {
                    doc = new XmlDocument();
                    doc.Load(xmlfile);
                    reader = new XmlNodeReader(doc);

                    DataTable dts = null;
                    if (ds != null)
                    {
                        dts = ds.Tables["submit"];
                        ds.Tables.Remove(dts);
                    }

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
                            SKUNO.Text = ds.Tables[i].Rows[0]["sku"].ToString();
                            SKUName.Text = ds.Tables[i].Rows[0]["sku_dsc"].ToString();
                        }
                    }

                    if (dts != null)
                    {
                        ds.Tables.Add(dts);
                    }

                    if (!bNoData && !BNoData)
                    {
                        UpdatedgTable(true);
                    }
                    else if (bNoData && !BNoData)
                    {
                        dgTable.DataSource = null;
                        dgTable.Controls.Clear();
                        MessageBox.Show("无此抽盘明细数据！");
                    }
                    else if (BNoData && !bNoData)
                    {
                        MessageBox.Show("无此SKU基本数据!");
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
            DataGridCustomColumnBase style = null;
            DataGridColumnStyle style1 = null;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i].Equals("sku_ext_qty"))
                {
                    style = new DataGridCustomTextBoxColumn();

                    style.Owner = dgTable;
                    style.ReadOnly = false;
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

            return ts;
        }

        private DataGridTableStyle getTableStyle2(string[] name, int[] width, bool GridNo)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables["submit"];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;
            DataGridColumnStyle style1 = null;
            for (int i = 0; i < name.Length; i++)
            {
                style1 = new DataGridTextBoxColumn();
                style1.MappingName = name[i];
                style1.Width = (int)(dstWidth * width[i] / 100);
                ts.GridColumnStyles.Add(style1);
            }

            return ts;
        }

        private void UpdatedgTable(bool Init)
        {
            try
            {
                string[] colValue = new string[] { "序号", "验", "盘点量", "抽盘量" };
                string[] colName = new string[] { "sel_no", "check", "sku_first_qty", "sku_ext_qty" };
                int[] colWidth = new int[] { 23, 14, 25, 35 };
                DataColumnCollection cols = ds.Tables["detail"].Columns;
                DataRowCollection rows = ds.Tables["detail"].Rows;
                if (!cols.Contains("check"))
                {
                    cols.Add("check");
                }
                if (!cols.Contains("sku_ext_qty"))
                {
                    cols.Add("sku_ext_qty");
                }
                if (!cols.Contains("sku"))
                {
                    cols.Add("sku");
                }
                if (Init)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        rows[i]["check"] = "";
                        rows[i]["sku_ext_qty"] = "0";
                        rows[i]["sku"] = SKUNO.Text;
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

        private void UpdatedgTable2(bool Init)
        {
            try
            {
                string[] colValue = new string[] { "序号", "SKU", "盘点量", "抽盘量" };
                string[] colName = new string[] { "sel_no", "sku", "sku_first_qty", "sku_ext_qty" };
                //int[] colWidth = new int[] { 23, 35, 19, 19 };
                int[] colWidth = new int[] { 12, 46, 19, 19 };

                dgTable2.Controls.Clear();
                dgTable2.TableStyles.Clear();
                dgTable2.TableStyles.Add(getTableStyle2(colName, colWidth, true));
                UpdateRow2();
            }
            catch (Exception)
            {
            }
        }

        private void UpdatedgHeader()
        {
            try
            {
                string[] colValue = new string[] { "序号", "验", "盘点量", "抽盘量" };
                string[] colName = new string[] { "sel_no", "check", "sku_first_qty", "sku_ext_qty" };
                int[] colWidth = new int[] { 23, 14, 25, 35 };
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

        private void UpdatedgHeader2()
        {
            try
            {
                string[] colValue = new string[] { "序号", "SKU", "盘点量", "抽盘量" };
                string[] colName = new string[] { "sel_no", "sku", "sku_first_qty", "sku_ext_qty" };
                //int[] colWidth = new int[] { 23, 35, 19, 19 };
                int[] colWidth = new int[] { 12, 46, 19, 19 };
                DataTable dtHeader2 = new DataTable();
                dtHeader2.TableName = ds.Tables["submit"].TableName;
                for (int i = 0; i < colValue.Length; i++)
                {
                    dtHeader2.Columns.Add(colName[i]);
                }
                DataRow row = dtHeader2.NewRow();

                for (int i = 0; i < colValue.Length; i++)
                {
                    row[i] = colValue[i];
                }
                dtHeader2.Rows.Add(row);
                dgHeader2.Controls.Clear();
                dgHeader2.TableStyles.Clear();
                dgHeader2.TableStyles.Add(getTableStyle2(colName, colWidth, false));
                dgHeader2.DataSource = dtHeader2;
            }
            catch (Exception)
            {
            }
        }

        private void UpdateRow()
        {
            try
            {
                DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                DataGridCustomTextBoxColumn TextBoxCol = null;
                dtResult = new DataTable();

                dtResult.TableName = dt.TableName;
                for (int i = 0; i < styles.Count; i++)
                {
                    dtResult.Columns.Add(styles[i].MappingName);
                    if (styles[i].MappingName.Equals("sku_ext_qty"))
                    {
                        TextBoxCol = (DataGridCustomTextBoxColumn)styles[i];
                    }
                }
                int from = 0;
                int to = dt.Rows.Count;

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
            }
            catch (Exception)
            {
            }
        }

        private void UpdateRow2()
        {
            try
            {
                DataTable dt = ds.Tables["submit"].DefaultView.ToTable();
                GridColumnStylesCollection styles = dgTable2.TableStyles[0].GridColumnStyles;
                dtResult = new DataTable();

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

                for (int i = from; i >= 0 && i < to; i++)
                {
                    DataRow rowNew = dtResult.NewRow();

                    for (int j = 0; j < styles.Count; j++)
                    {
                        string name = styles[j].MappingName;

                        rowNew[name] = dt.Rows[i][name];
                    }

                    dtResult.Rows.Add(rowNew);
                }
                dgTable2.DataSource = dtResult;

                int pageCount = (int)Math.Ceiling(ds.Tables["submit"].Rows.Count / (double)TABLE_ROWMAX);
                if (pageCount == 0)
                {
                    pageCount = 1;
                }
                Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.R70101;
            string msg = "request=701;usr=" + Config.User + ";op=01;stk_no=" + pCode.Text + ";loc_no=" + CBNO.Text + ";sto=" + ShopNO.Text + ";sku=" + SKUNO.Text;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request02()
        {
            apiID = API_ID.R70102;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "701", "02");
            json = json.Replace("\"submit\"", "\"detail\"");
            string msg = "request=701;usr=" + Config.User + ";op=02;sto=" + ShopNO.Text + ";stk_no=" + pCode.Text + ";loc_no=" + CBNO.Text + ";json=" + json;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }
        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                //string[] colName = new string[] { "loc_no", "sku", "sku_dsc", "plu_mango", "sel_no" };
                string[] colName = { "sel_no", "sku", "sku_dsc", "sku_first_qty", "sku_ext_qty" };
                DataTable dt = ds.Tables["submit"];

                dtOK.TableName = ds.Tables["submit"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sel_no"] = dt.Rows[i]["sel_no"];
                    rowNew["sku"] = dt.Rows[i]["sku"];
                    //rowNew["sku_dsc"] = dt.Rows[i]["sku_dsc"];
                    rowNew["sku_first_qty"] = dt.Rows[i]["sku_first_qty"];
                    rowNew["sku_ext_qty"] = dt.Rows[i]["sku_ext_qty"];
                    dtOK.Rows.Add(rowNew);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dtOK;
        }


        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R70101:
                        {
                            string file = Config.getApiFile("701", "01");
                            from += "/701/" + file;
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

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable2.DataSource != null)
            {
                int pageCount = (int)Math.Ceiling(ds.Tables["submit"].Rows.Count / (double)TABLE_ROWMAX);
                pageIndex--;
                if (pageIndex >= 1)
                {
                    UpdatedgTable2(false);
                    Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                }
                else
                {
                    pageIndex++;
                    MessageBox.Show("已是首页!");
                }
            }
        }

        private void NexPage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable2.DataSource != null)
            {
                int pageCount = (int)Math.Ceiling(ds.Tables["submit"].Rows.Count / (double)TABLE_ROWMAX);
                pageIndex++;
                if (pageIndex <= pageCount)
                {
                    UpdatedgTable2(false);
                    Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                }
                else
                {
                    pageIndex--;
                    MessageBox.Show("已是最后一页!");
                }
            }
        }

        #endregion

        #region UI响应

        private void Tbox_LostFocus(object sender, EventArgs e)
        {
            checkData();
        }

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy && SKUNO.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    SKUName.Text = "";
                    dgTable.DataSource = null;

                    request01();
                }
            }
        }

        private void dgHeader2_MouseDown(object sender, MouseEventArgs e)
        {
            if (!busy)
            {
                DataTable dtHeader = (DataTable)dgTable2.DataSource;
                if (dtHeader != null && dtHeader.Rows.Count >= 1)
                {
                    DataGrid.HitTestInfo hitTest = dgHeader.HitTest(e.X, e.Y);
                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        string colName = dtHeader.Columns[hitTest.Column].ColumnName;

                        DataTable dt = ds.Tables["submit"];
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

                            UpdatedgTable2(false);
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
                    if (!ColName.Equals("sku_ext_qty"))
                    {
                        int rowIndex = dgTable.CurrentCell.RowNumber;
                        dgTable.Select(rowIndex);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private bool checkData()
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                string ok = dt.Rows[dgTable.CurrentRowIndex]["sku_ext_qty"].ToString();

                if (ok.Length <= 9)
                {
                    if (!reg.IsMatch(ok))
                    {
                        MessageBox.Show("请输入非负整数！");
                        UpdatedgTable(false);
                    }
                    else
                    {
                        if (UInt64.Parse(ok) > 0)
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
                                dt.Rows[dgTable.CurrentRowIndex]["check"] = "V";
                                return true;
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
                                dt.Rows[dgTable.CurrentRowIndex]["check"] = "V";
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入字符过长!");
                }

            }
            catch (Exception)
            {
            }

            return false;
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
                        for (int j = 3; j < 4; j++)
                        {
                            dts.Rows[i][j] = dtt.Rows[i][j].ToString();
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

        private bool CheckSubmit()
        {
            bool bCheck = false;
            try
            {
                DataTable dt = ds.Tables["submit"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    bCheck = true;
                }
                else
                {
                    MessageBox.Show("表格内无数据!");
                }
            }
            catch (Exception)
            {
            }

            return bCheck;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (Ent_Agn.SelectedIndex == 0)
                    {
                        if (SKUNO.Text == "" || SKUName.Text == "")
                        {
                            MessageBox.Show("请检查商品!");
                        }
                        else if (checkData())
                        {
                            DataTable dtd = ds.Tables["detail"];
                            DataTable dts = ds.Tables["submit"];
                            for (int i = 0; i < dtd.Rows.Count; i++)
                            {
                                DataRow row = dtd.Rows[i];
                                DataRow rowNew = dts.NewRow();

                                rowNew["sel_no"] = row["sel_no"];
                                rowNew["sku"] = row["sku"];

                                string sel_noAndsku = row["sel_no"].ToString() + row["sku"].ToString();

                                if (skuAndnumList.Contains(sel_noAndsku))
                                {
                                    MessageBox.Show("该sku已存在");
                                    return;
                                }
                                else
                                {
                                    skuAndnumList.Add(sel_noAndsku);
                                }

                                rowNew["sku_dsc"] = SKUName.Text;
                                rowNew["sku_first_qty"] = row["sku_first_qty"];
                                rowNew["sku_ext_qty"] = row["sku_ext_qty"];

                                dts.Rows.Add(rowNew);
                            }
                            pageIndex = (int)Math.Ceiling(ds.Tables["submit"].Rows.Count / (double)TABLE_ROWMAX);
                            UpdatedgTable2(false);
                            Serialize(guid);

                            SKUNO.Text = "";
                            SKUNO.Focus();
                            SKUName.Text = "";
                            dgTable.DataSource = null;
                            dgTable.Controls.Clear();
                        }
                    }
                    else if (CheckSubmit())
                    {
                        request02();
                    }
                }
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
                //未提交
                if (btnConfirm.Enabled)
                {
                    //确认离开
                    if (MessageBox.Show("确认离开吗？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
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

        private void Ent_Agn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Ent_Agn.SelectedIndex == 0)
            {
                btnConfirm.Text = "添 加";
            }
            else
            {
                btnConfirm.Text = "提 交";
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
                    case API_ID.R70101:
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

                    case API_ID.R70102:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("上传成功!");
                            btnConfirm.Enabled = false;
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
            pCode.Text = (string)param[1];
            CBNO.Text = (string)param[2];

            string[] colName = { "sel_no", "sku", "sku_dsc", "sku_first_qty", "sku_ext_qty" };
            DataTable dt = new DataTable();
            if (ds == null)
            {
                ds = new DataSet();
            }
            dt.TableName = "submit";
            for (int i = 0; i < colName.Length; i++)
            {
                dt.Columns.Add(colName[i]);
            }
            ds.Tables.Add(dt);
            dt.Dispose();
            UpdatedgHeader2();
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
                data[(int)DATA_INDEX.pCode] = pCode.Text;
                data[(int)DATA_INDEX.CBNO] = CBNO.Text;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
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
                    ShopNO.Text = sc.Data[(int)DATA_INDEX.ShopNO];
                    pCode.Text = sc.Data[(int)DATA_INDEX.pCode];
                    CBNO.Text = sc.Data[(int)DATA_INDEX.CBNO];
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    ds = sc.DS;
                    if (ds != null)
                    {
                        DataTable dts = ds.Tables["submit"];
                        if (dts != null && dts.Rows.Count > 0)
                        {
                            UpdatedgHeader2();
                            UpdatedgTable2(false);
                        }
                        DataTable dtd = ds.Tables["detail"];
                        if (dtd != null && dtd.Rows.Count > 0)
                        {
                            UpdatedgTable(false);
                        }
                        DataTable dth = ds.Tables["head"];
                        if (dth != null && dth.Rows.Count > 0)
                        {
                            SKUNO.Text = dth.Rows[0][0].ToString();
                            SKUName.Text = dth.Rows[0][1].ToString();
                        }
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