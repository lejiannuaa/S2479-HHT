using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HolaCore;

namespace Hola_Business
{
    public partial class FormInventoryQD : Form, ConnCallback, ISerializable
    {
        private const string guid = "F71BA2EE-E27C-EFF7-A55C-958EC11565AA";//同上一页面
        private const string OGuid = "00000000000000000000000000000000";
        private string DataGuid = "";
        private string ShopNo = "";
        private DataSet ds = null;
        //private Form child = null;
        private int TABLE_ROWMAX = 7;
        private int pageIndex = 1;
        private int indexRow = -1;
        private int rowFlag = -1;
        //private int indexRow = -1;
        //private int[] indexNumber = null;
        private DataTable Reqtable = null;//提交数据表
        private DataTable dtOrder = null;//默认排序表
        private int TaskBarHeight = 0;
        private API_ID apiID = API_ID.NULL;
        private Form Child = null;
        private Form_ID ChildID = Form_ID.Null;
        private string stk_opr_time = null;
        private enum Form_ID
        {
            InventoryInc,
            Null,
        }
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();

        public delegate void mydelegate(DataTable table, List<string> sku);//定义一个委托
        public event mydelegate myevent;//定义上诉委托类型的事件

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104702,
        }
        #endregion


        private bool ismaintain = false;
        //是否是维护提交，正常提交
        public FormInventoryQD(DataTable table, bool maintain)
        {
            InitializeComponent();
            doLayout();

            if (maintain)
            {
                //是维护的页面
                ismaintain = maintain;
                button3.Enabled = false;
            }
            else
            {
                //button6.Enabled=false;
            }

            if (Reqtable == null)
            {
                Reqtable = new DataTable();
            }
            else
            {
                Reqtable.Clear();
            }

            Reqtable = table;
            Reqtable.TableName = "detail";
            ds = new DataSet();
            ds.Tables.Add(Reqtable.Copy());

            pageIndex = 1;
            int pageCount = (int)Math.Ceiling(Reqtable.Rows.Count / (double)TABLE_ROWMAX);
            if (pageCount == 0)
                Page.Text = "1/1";
            else
                Page.Text = "1/" + pageCount.ToString();

            UpdatedgTable(true, true, false);
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
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                button5.Top = dstHeight - button5.Height;
                button4.Top = dstHeight - button4.Height;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBarI.Top = button5.Top - pbBarI.Height * 3;
                ResumeLayout(false);
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
                    int x = Control.MousePosition.X;
                    int y = Control.MousePosition.Y - dgTable.Top - TaskBarHeight;

                    DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);

                    if (hitTest.Type == DataGrid.HitTestType.Cell)
                    {
                        rowFlag = hitTest.Row;
                        dgTable.Select(rowFlag);

                        contextMenu1.MenuItems.Add(this.menuDetail);
                        dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //pop 删除
        List<string> skuList = new List<string>();
        private void menuDetail_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
                indexRow = (pageIndex - 1) * TABLE_ROWMAX + rowFlag;

                if (indexRow >= 0)
                {
                    string sku = ds.Tables["detail"].Rows[indexRow]["sku"].ToString();
                    skuList.Add(sku);
                    ds.Tables["detail"].Rows.RemoveAt(indexRow);
                }
                else
                {
                    MessageBox.Show("indexRow小于0");
                }
                UpdatedgTable(false, false, false);
            }
        }

        //退出
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (ismaintain)
                {
                    Close();
                }
                else
                {
                    if (myevent != null)
                    {

                        if (ds == null || Reqtable.Rows.Count == 0)
                        {
                            Reqtable = null;
                            myevent(Reqtable, skuList);
                        }
                        else
                        {
                            Reqtable = ds.Tables["detail"];
                            myevent(Reqtable, skuList);
                        }
                    }
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //提交
        private void button4_Click(object sender, EventArgs e)
        {
            if (CheckSubmit())
            {
                if (MessageBox.Show("确定提交？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    requestR02();
                }
                else
                {
                }
            }
        }

        #region ISerializable 成员
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
        
        public void init(object[] param)
        {
            ShopNo = (string)param[0];
            stk_opr_time = (string)param[1];
        }

        public void Serialize(string file)
        {

        }

        public void Deserialize(string file)
        {

        }

        #endregion

        #region ConnCallback 成员

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
                    case API_ID.R104702:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("请求成功!");
                            for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                            {
                                string sku = ds.Tables["detail"].Rows[i]["sku"].ToString();
                                skuList.Add(sku);
                            }
                            dgTable.Enabled = false;
                            button4.Enabled = false;
                            button3.Enabled = false;
                            button2.Enabled = false;
                            button1.Enabled = false;
                            ds = null;
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

        private void requestR02()
        {
            apiID = API_ID.R104702;
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "1047", "02");
            string msg = null;
            if (ismaintain)
            {
                msg = "request=1047;usr=" + Config.User + ";op=02;sto=" + ShopNo + ";maintain=" + ismaintain.ToString() + ";starttime=" + stk_opr_time + ";json=" + json;
            }
            else
            {
                msg = "request=1047;usr=" + Config.User + ";op=02;sto=" + ShopNo + ";maintain=" + ismaintain.ToString() + ";json=" + json;
            }

            msg = DataGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                //string[] colName = new string[] { "sku", "sku_dsc", "inv_adj_no", "inv_act_no", "adj_reason" };
                string[] colName = new string[] { "sku", "inv_act_no", "inv_adj_no", "adj_reason" };

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }

                for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();

                    rowNew["sku"] = ds.Tables["detail"].Rows[i]["sku"];
                    //rowNew["sku_dsc"] = ds.Tables["detail"].Rows[i]["sku_dsc"];
                    rowNew["inv_act_no"] = ds.Tables["detail"].Rows[i]["inv_act_no"];
                    rowNew["inv_adj_no"] = ds.Tables["detail"].Rows[i]["inv_adj_no"];
                    rowNew["adj_reason"] = ds.Tables["detail"].Rows[i]["adj_reason"];
                    dtOK.Rows.Add(rowNew);
                }
            } 
            catch (Exception)
            {
            }
            return dtOK;
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
                string[] colName = new string[] { "sku", "sku_dsc", "inv_act_no", "inv_adj_no", "adj_reason" };
                string[] colValue = new string[] { "SKU", "品名", "实际库存量", "差异数量", "原因" };
                int[] colWidth = new int[] { 19, 40, 12, 13, 13 };

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

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool GridNo)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = ds.Tables["detail"].TableName;

            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = null;
                DataGridColumnStyle style1 = null;
                style1 = new DataGridTextBoxColumn();
                if (name[i].Equals("sku"))
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
        private int lastRowIndex = -1;
        //下一页
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
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
                            UpdatedgTable(false, false, true);
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

        private void UpdateDsData()
        {
            try
            {
                DataTable dts = ds.Tables["detail"].DefaultView.ToTable();
                DataTable dtt = (DataTable)dgTable.DataSource;
                if (ds.Tables["detail"] != null && dtt != null)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        for (int j = 2; j < 6; j++)
                        {
                            if (dtt.Columns[j].ColumnName.Equals("CheckBox"))
                            {
                                ds.Tables["detail"].Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j].ToString();
                            }
                            else
                            {
                                ds.Tables["detail"].Rows[(pageIndex - 1) * TABLE_ROWMAX + i][j] = dtt.Rows[i][j];
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                //DataTable dt = ds.Tables["detail"];

                if (ds.Tables["detail"] != null && ds.Tables["detail"].Rows.Count > 0)
                {
                    //UpdateDsData();
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdatedgTable(false, false, true);
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

        private void FormInventoryQD_Load(object sender, EventArgs e)
        {

        }

        //全部删除
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否全部删除？", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                {
                    string sku = ds.Tables["detail"].Rows[i]["sku"].ToString();
                    skuList.Add(sku);
                }
                ds.Tables["detail"].Rows.Clear();
                UpdatedgTable(false, false, false);
            }
            else
            {

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

        private void button6_Click(object sender, EventArgs e)
        {
            ChildID = Form_ID.InventoryInc;
            ShowChild(false);
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
                    case Form_ID.InventoryInc:
                        Child = new FormInventoryInc();
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

                    ((ISerializable)Child).init(new object[] { ShopNo, "", "" });
                    Serialize(guid);
                }

                Child.ShowDialog();
                Show();
                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
                Serialize(guid);
            }
            catch (Exception)
            {
            }
        }

    }
}