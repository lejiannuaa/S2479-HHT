using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HolaCore;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Threading;

namespace Hola_Business
{
    public partial class FormhhtDDOrderDetail : Form, ConnCallback, ISerializable
    {
        private const string guid = "DD13B795-FF80-103E-11F9-D1099F35D861";
        private const string OGuid = "00000000000000000000000000000000";
        private string ShopNo = null;
        //private Form Child = null;
        //private Form_ID ChildID = Form_ID.Null;
        private API_ID apiID = API_ID.NULL;
        private string xmlfile = null;
        private DataSet ds = null;
        private int TABLE_ROWMAX = 7;
        private int pageIndex = 1;
        //private int rowFlag = -1;
        private int pageCount = -1;
        //private bool bCheckAll = false;
        private string stk_opr_time = "";

        private int indexRow = -1;
        //private DataTable takeDataTable = null;//要提交的表
        private DataTable dtOrder = null;//默认排序表
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");

        #region 子窗口ID
        private enum Form_ID
        {
            DDOrederDetail,
            Null,
        }
        #endregion

        public FormhhtDDOrderDetail()
        {
            InitializeComponent();
            doLayout();
        }

        private int TaskBarHeight = 0;
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
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                button5.Top = dstHeight - button5.Height;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBarI.Top = button5.Top - pbBarI.Height * 3;
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }


        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104504,
            R104505,
            DOWNLOAD_XML
        }
        #endregion

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            //xmlFile = @"\Program Files\hhtiii\00501.xml";
            //xmlfile = @"\Program Files\hhtiii\00204.xml";

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
                        DataTable dt = ds.Tables["detail"];

                        if (ds.Tables["detail"] != null)
                        {
                            pageIndex = 1;
                            pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                            if (pageCount == 0)
                                Page.Text = "1/1";
                            else
                                Page.Text = "1/" + pageCount.ToString();


                            UpdatedgTable(true, true, false);
                        }
                        else
                        {
                            //btn03.Enabled = true;
                        }
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

        private void UpdatedgTable(bool bInitColumn, bool reOrder, bool turnPage)
        {
            try
            {
                if (turnPage)
                {
                    UpdateRow(indexRow, false, reOrder, turnPage);
                    return;
                }
                string[] colName = new string[] { "sku", "sku_dsc", "stk_order_qty", "sto_no_out" };
                string[] colValue = new string[] { "SKU", "品名", "申请量", "调出门店号" };
                int[] colWidth = new int[] { 19, 40, 15, 23};

                dtOrder = ds.Tables["detail"].DefaultView.ToTable();

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow(indexRow, true, reOrder, turnPage);

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();

                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colName.Length; i++)
                {
                    row[i] = colValue[i];
                }
                dtHeader.Rows.Add(row);

                dgHeader.Controls.Clear();
                dgHeader.TableStyles.Clear();
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, false));
                dgHeader.DataSource = dtHeader;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //index要刷新的行号，refersh是否全部至为不选中,翻页
        private void UpdateRow(int index, bool refersh, bool bReorder, bool turnPage)
        {
            DataTable dtResult = null;
            //如果需要重新排序则重新读取
            if (bReorder || turnPage)
            {
                dtResult = new DataTable();
                dtResult.TableName = dtOrder.TableName;
                GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
                for (int i = 0; i < styles.Count; i++)
                {
                    dtResult.Columns.Add(styles[i].MappingName);
                }

                int from = (pageIndex - 1) * TABLE_ROWMAX;
                int to = from + TABLE_ROWMAX;
                if (to > dtOrder.Rows.Count)
                {
                    to = dtOrder.Rows.Count;
                }

                for (int i = from; i < to; i++)
                {
                    DataRow rowNew = dtResult.NewRow();
                    for (int j = 0; j < styles.Count; j++)
                    {
                        string name = styles[j].MappingName;
                        rowNew[name] = dtOrder.Rows[i][name];
                    }

                    dtResult.Rows.Add(rowNew);
                }

                dgTable.DataSource = dtResult;
            }
            else
            {
                dgTable.DataSource = ds.Tables["detail"];
                dtResult = (DataTable)dgTable.DataSource;
            }
        }


        private DataGridTableStyle getTableStyle(string[] colName, int[] colWidth, bool p)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = ds.Tables["detail"].TableName;

            for (int i = 0; i < colName.Length; i++)
            {
                DataGridColumnStyle style1 = null;
                DataGridCustomColumnBase style = null;
                if (colName[i].Equals("sku"))
                {
                    if (p)
                    {
                        style = new DataGridCustomTextBoxColumn();
                        style.Owner = dgTable;
                        
                        style.Alignment = HorizontalAlignment.Right;
                        style.ReadOnly = true;
                        style.MappingName = colName[i];
                        style.Width = (int)(dstWidth * colWidth[i] / 100);
                        ts.GridColumnStyles.Add(style);
                    }
                    else
                    {
                        style1 = new DataGridTextBoxColumn();
                        style1.MappingName = colName[i];
                        style1.Width = (int)(dstWidth * colWidth[i] / 100);
                        ts.GridColumnStyles.Add(style1);
                    }
                }
                else
                {
                    style1 = new DataGridTextBoxColumn();
                    style1.MappingName = colName[i];
                    style1.Width = (int)(dstWidth * colWidth[i] / 100);
                    ts.GridColumnStyles.Add(style1);
                }
            }
            return ts;
        }

        //退出按钮
        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region ISerializable 成员

        public void init(object[] param)
        {
            ShopNo = (string)param[0];
            stk_opr_time = (string)param[1];

            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"\Program Files\hhtiii\00204.xml");
            //LoadXML();

            ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
            {
                this.Invoke(new InvokeDelegate(() =>
                {
                    Request01();
                }));
            }));
        }

        public void Serialize(string file)
        {

        }

        public void Deserialize(string file)
        {

        }

        #endregion

        #region ConnCallback 成员

        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
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
                    case API_ID.R104504:
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

                    case API_ID.R104505:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功!");
                            button4.Enabled = false;
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

        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R104504:
                        {
                            string file = Config.getApiFile("1045", "04");
                            from += "/1045/" + file;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckSubmit())
            {
                if (MessageBox.Show("确定导出excel？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Request02();
                }
                else
                {
                }
            }
        }

        private bool CheckSubmit()
        {
            try
            {
                //DataTable dt = ds.Tables["detail"];
                if (ds.Tables["detail"] != null && ds.Tables["detail"].Rows.Count > 0)
                {

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

        private void Request01()
        {
            apiID = API_ID.R104504;
            string msg = "request=1045;usr=" + Config.User + ";op=04" + ";sto=" + ShopNo + ";starttime=" + stk_opr_time;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void Request02()
        {
            apiID = API_ID.R104505;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "1045", "05");
            string msg = "request=1045;usr=" + Config.User + ";op=05" + ";sto=" + ShopNo + ";starttime=" + stk_opr_time + ";json=" + json;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }



        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                string[] colName = new string[] { "sku", "sku_dsc", "stk_order_qty", "sto_no_out" };

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sku"] = ds.Tables["detail"].Rows[i]["sku"];
                    rowNew["sku_dsc"] = ds.Tables["detail"].Rows[i]["sku_dsc"];
                    rowNew["stk_order_qty"] = ds.Tables["detail"].Rows[i]["stk_order_qty"];
                    rowNew["sto_no_out"] = ds.Tables["detail"].Rows[i]["sto_no_out"];
                    dtOK.Rows.Add(rowNew);
                }
            }
            catch (Exception)
            {
            }

            return dtOK;
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
                        for (int j = 0; j < 1; j++)
                        {
                            //dts.Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j];

                            if (dtt.Columns[j] != null)
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

        private int lastRowIndex = -1;
        //下一页
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (ds.Tables["detail"].Rows.Count > 0)
                    {
                        //UpdateDsData();
                        lastRowIndex = dgTable.CurrentRowIndex;
                        //refreshData(false);
                        int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);

                        pageIndex++;
                        if (pageIndex <= pageCount)
                        {
                            lastRowIndex = -1;
                            ClearTableStyle(dgTable.TableStyles);
                            UpdatedgTable(false, true, true);
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

        private void ClearTableStyle(GridTableStylesCollection ts)
        {
            foreach (DataGridTableStyle tstyle in ts)
            {
                foreach (DataGridTextBoxColumn cstyle in tstyle.GridColumnStyles)
                {
                    try
                    {
                        (cstyle as DataGridCustomColumnBase).Clear();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!busy && ds != null)
            {
                DataTable dt = ds.Tables["detail"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    //UpdateDsData();
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdatedgTable(false, true, true);
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
        private int rowOld = -1;
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

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (rowOld >= 0 && rowOld < (dgTable.DataSource as DataTable).Rows.Count)
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
    }
}