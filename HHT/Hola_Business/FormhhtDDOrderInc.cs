using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HolaCore;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;


namespace Hola_Business
{
    public partial class FormhhtDDOrderInc : Form, ConnCallback, ISerializable
    {
        private const string guid = "F71BA2EE-E27C-EFF7-A55C-958EC11565AA";
        private const string OGuid = "00000000000000000000000000000000";
        private string ShopNo = null;
        private SerializeClass sc = null;
        private Form child = null;
        private Form_ID ChildID = Form_ID.Null;
        private API_ID apiID = API_ID.NULL;
        private string xmlfile = null;
        private DataSet ds = null;
        private int TABLE_ROWMAX = 5;
        private int pageIndex = 1;
        private int rowFlag = -1;
        private int pageCount = -1;
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long Stop = 0;
        //private long freq = 0;
        private int lastRowIndex = -1;
        private int rowOld = -1;
        private int indexRow = -1;
        private DataTable takeDataTable = null;//要提交的表
        private DataTable dtOrder = null;//默认排序表

        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        private int TaskBarHeight = 0;

        List<string> skustoList = new List<string>();//sku 和 sto来确定数据唯一性

        #region 子窗口ID
        private enum Form_ID
        {
            DDOrederQD,
            Null,
        }
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104501,
            DOWNLOAD_XML
        }
        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            SKUNO,
            GoodsNO,
            SKUName,
            SDTP,
            Table,
            Child,
            DATAGUID,
            DataMax
        }
        #endregion


        public FormhhtDDOrderInc()
        {
            InitializeComponent();
            doLayout();

            //初始化提交表的架构
            RequsetDataTable();


            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"\Program Files\hhtiii\00201.xml");
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
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                button5.Top = dstHeight - button5.Height;
                button3.Top = dstHeight - button3.Height;
                button4.Top = dstHeight - button4.Height;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBar.Top = button5.Top - pbBar.Height * 3;
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }


        #region UI响应

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
                dtResult = (DataTable)dgTable.DataSource;
            }
        }

        //是否第一次加载，是否翻页
        private void UpdateColumn(bool bInitColumn, bool bReorder, bool bTurnPage)
        {
            try
            {
                if (bTurnPage)
                {
                    UpdateRow(indexRow, true, bReorder, bTurnPage);
                    return;
                }

                Start = 0;
                Stop = 0;
                QueryPerformanceCounter(ref Start);

                string[] colName = new string[] { "sku", "sku_dsc", "stk_order_qty", "sto", "inv", };
                string[] colValue = new string[] { "sku", "sku_dsc", "stk_order_qty", "调拨门店号", "库存量", };
                int[] colWidth = new int[] { 0, 0, 0, 48, 49, };

                //从XML首次加载需初始化列
                if (bInitColumn)
                {
                    DataColumnCollection cols = ds.Tables["detail"].Columns;
                    DataRowCollection rows = ds.Tables["detail"].Rows;
                    if (!cols.Contains("stk_order_qty"))
                    {
                        cols.Add("stk_order_qty");
                        for (int i = 0; i < rows.Count; i++)
                        {
                            rows[i]["stk_order_qty"] = "0";
                        }
                    }
                }
                dtOrder = ds.Tables["detail"].DefaultView.ToTable();

                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow(indexRow, true, bReorder, bTurnPage);

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

                QueryPerformanceCounter(ref Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                contextMenu1.MenuItems.Clear();

                if (ds != null && ds.Tables.Contains("detail"))
                {
                    DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
                    if (!busy)
                    {
                        int x = Control.MousePosition.X;
                        int y = Control.MousePosition.Y - dgTable.Top - TaskBarHeight;

                        DataGrid.HitTestInfo hitTest = dgTable.HitTest(x, y);

                        if (hitTest.Type == DataGrid.HitTestType.Cell)
                        {
                            rowFlag = hitTest.Row;
                            dgTable.Select(rowFlag);
                            if (textBox2.Text == "")
                            {
                                MessageBox.Show("没有输入申请量");
                                return;
                            }
                            else if (int.Parse(textBox2.Text.ToString()) <= 0)
                            {
                                MessageBox.Show("下单申请量不能小于0");
                            }
                            else
                            {
                                contextMenu1.MenuItems.Add(this.menuDetail);
                            }
                            dgTable.CurrentCell = new DataGridCell(hitTest.Row, hitTest.Column);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //pop 添加
        private void menuDetail_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
                indexRow = (pageIndex - 1) * TABLE_ROWMAX + rowFlag;

                int order_qty = int.Parse(dt.Rows[indexRow]["inv"].ToString());
                dt.Rows[indexRow]["stk_order_qty"] = textBox2.Text.ToString();

                if (order_qty < int.Parse(textBox2.Text.ToString()))
                {
                    MessageBox.Show("下单量大于库存量");
                    return;
                }

                string sto = dt.Rows[indexRow]["sto"].ToString();
                string sku = dt.Rows[indexRow]["sku"].ToString();
                string skusto = sku + sto;
                if (skustoList.Contains(skusto))
                {
                    foreach (DataRow item in takeDataTable.Rows)
                    {
                        int i = 0;
                        if (item["sto"].ToString() == sto && item["sku"].ToString() == sku)
                        {
                            if (0 < int.Parse(textBox2.Text.ToString()) && int.Parse(textBox2.Text.ToString()) <= order_qty)
                            {
                                if (MessageBox.Show("该笔数据已存在，是否覆盖该笔数据？", "",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    takeDataTable.Rows.Remove(item);
                                    takeDataTable.ImportRow(dt.Rows[indexRow]);
                                    chearAll();
                                    SKUNO.Focus();
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                                MessageBox.Show("下单量大于库存量");
                            }
                            return;
                        }
                        i++;
                    }
                }
                else
                {
                    skustoList.Add(skusto);
                    if (0 < int.Parse(textBox2.Text.ToString()) && int.Parse(textBox2.Text.ToString()) <= order_qty)
                    {
                        takeDataTable.ImportRow(dt.Rows[indexRow]);
                        chearAll();
                        SKUNO.Focus();
                    }
                    else
                    {
                        MessageBox.Show("下单量大于库存量");
                    }
                }
                //清除选中行
                indexRow = -1;
                rowFlag = -1;
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Enabled)
            {
                if (MessageBox.Show("是否退出？", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    skustoList.Clear();
                    Close();
                }
                else
                {
                    //Serialize(guid);
                    //MessageBox.Show("保存成功");
                    //DialogResult = DialogResult.Abort;
                }
            }
            else
            {
                File.Delete(Config.DirLocal + guid);
            }
        }
        //添加按钮
        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = ds.Tables["detail"].DefaultView.ToTable();

            if (indexRow != -1 && int.Parse(textBox2.Text.ToString()) > 0)
            {
                //取出要调出店的库存，申请的数量o
                int order_qty = int.Parse(dt.Rows[indexRow]["inv"].ToString());
                dt.Rows[indexRow]["stk_order_qty"] = textBox2.Text.ToString();

                takeDataTable.ImportRow(dt.Rows[indexRow]);

                if (indexRow >= 0)
                {
                    if (0 < int.Parse(textBox2.Text.ToString()) && int.Parse(textBox2.Text.ToString()) < order_qty)
                    {
                        MessageBox.Show("添加成功");
                    }
                    else
                    {
                        if (textBox2.Text != "")
                        {
                            MessageBox.Show("申请量为空");
                            textBox2.Focus();
                        }
                        else
                        {
                            MessageBox.Show("申请量大于库存量，请重新输入");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择调出门店和申请数量");
                }
            }
            else
            {
                MessageBox.Show("没有选中要添加的数据和合理的申请量");
            }

            //清楚除选中行的序号
            indexRow = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //检查takeDataTable的数据是否有变更

            //下单清单
            ChildID = Form_ID.DDOrederQD;
            showChild(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chearAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                }
                else
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    request01();

                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(@"\Program Files\hhtiii\00201.xml");
                    //LoadXML();
                }
            }
        }

        private void chearAll()
        {
            SKUNO.Text = "";
            SKUName.Text = "";
            GoodsNO.Text = "";
            NativeKC.Text = "";
            SDTP.Text = "";
            textBox2.Text = "";
            dgTable.DataSource = null;
            dgHeader.DataSource = null;
            SKUNO.Focus();
        }

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                }
                else if (e.KeyValue == 13)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    request01();
                }
                SKUName.Text = "";
                GoodsNO.Text = "";
                NativeKC.Text = "";
                SDTP.Text = "";
                textBox2.Text = "";
                dgTable.DataSource = null;
                dgHeader.DataSource = null;
                SKUNO.Focus();
            }
        }

        //上一页
        private void button6_Click(object sender, EventArgs e)
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
                        UpdateColumn(false, false, true);
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

        //下一页
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && ds != null)
                {
                    if (ds.Tables["detail"].Rows.Count > 0)
                    {
                        lastRowIndex = dgTable.CurrentRowIndex;
                        //refreshData(false);
                        int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);

                        pageIndex++;
                        if (pageIndex <= pageCount)
                        {
                            //UpdateDsData();
                            lastRowIndex = -1;
                            ClearTableStyle(dgTable.TableStyles);
                            UpdateColumn(false, false, true);
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

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (!regText.IsMatch(textBox2.Text))
            {
                MessageBox.Show("输入的申请量非法！");
            }
        }

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

        private void dgTable_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showChild(bool bDeserialize)
        {
            try
            {
                if (child != null)
                {
                    child.Dispose();
                    child = null;
                }

                switch (ChildID)
                {
                    case Form_ID.DDOrederQD:
                        FormhhtDDOrderQD form = new FormhhtDDOrderQD(takeDataTable);
                        form.myevent += new FormhhtDDOrderQD.mydelegate(givetable);
                        child = form;
                        break;

                    default:
                        return;
                }

                ((ISerializable)child).init(new string[] { ShopNo });
                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }

                if (child.ShowDialog() == DialogResult.OK)
                {
                    File.Delete(Config.DirLocal + guid);
                }
                else
                {
                    //Serialize(guid);
                }

                Show();
                child.Dispose();
                child = null;
                ChildID = Form_ID.Null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SKUNO_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion


        #region ISerializable 成员

        public void init(object[] param)
        {
            ShopNo = (string)param[0];
        }

        public void Serialize(string file)
        {
            try
            {
                //MessageBox.Show("FormhhtDDOrderInc,Serialize");
                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DataMax];
                data[(int)DATA_INDEX.ShopNO] = ShopNo;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.Child] = ChildID.ToString();

                sc.Data = data;

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
                MessageBox.Show("FormhhtDDOrderInc ,Deserialize");
                if (file == null)
                    file = guid;

                sc = SerializeClass.Deserialize(Config.DirLocal + file);

                if (sc != null)
                {
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNO];
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);
                    ds = sc.DS;

                    if (ChildID != Form_ID.Null)
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
                    //   case API_ID.R104501:
                    //       if (result == ConnThread.RESULT_OK)
                    //       {
                    //           requestXML();
                    //       }
                    //       else if (result == ConnThread.RESULT_DUPLOGIN)
                    //       {
                    //           if (MessageBox.Show("已登录，您确定要重新登录吗？", "",
                    //MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    //           {
                    //               Config.loginTwice = "True";
                    //               Config.save();
                    //               Application.Exit();
                    //           }
                    //       }
                    //       else
                    //       {
                    //           MessageBox.Show(data);
                    //           GoodsNO.Text = "";
                    //           SKUName.Text = "";
                    //       }
                    //       break;

                    case API_ID.R104501:
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
                            GoodsNO.Text = "";
                            SKUName.Text = "";
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

        #region XML加载与显示

        private void request01()
        {
            apiID = API_ID.R104501;
            string msg = "request=1045;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;
            //xmlfile = @"\program files\hhtiii\00201.xml";
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
                        if (ds.Tables[i].TableName.Equals("grid"))
                        {
                            bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {
                        DataTable dt = ds.Tables["grid"];
                        if (dt.Rows.Count > 0)
                        {
                            SKUNO.Text = dt.Rows[0]["sku"].ToString();
                            GoodsNO.Text = dt.Rows[0]["ipc"].ToString();
                            SKUName.Text = dt.Rows[0]["sku_dsc"].ToString();
                            NativeKC.Text = dt.Rows[0]["hhthan"].ToString();
                            SDTP.Text = dt.Rows[0]["s_dpt"].ToString();
                        }
                        if (ds.Tables["detail"] != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                pageIndex = 1;
                                pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                                if (pageCount == 0)
                                    Page.Text = "1/1";
                                else
                                    Page.Text = "1/" + pageCount.ToString();

                                UpdateColumn(true, true, false);
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            dgTable.DataSource = null;
                            MessageBox.Show("没有表数据");
                            Page.Text = "1/1";
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
                //上下页按钮显示，1页的时候不可用
                if (Page.Text == "1/1")
                {
                    button6.Enabled = false;
                    button7.Enabled = false;
                }
                else
                {
                    button6.Enabled = true;
                    button7.Enabled = true;
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

        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R104501:
                        {
                            string file = Config.getApiFile("1045", "01");
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

        private DataTable RequsetDataTable()
        {
            takeDataTable = new DataTable();
            takeDataTable.TableName = "takeTable";
            takeDataTable.Columns.Add("sku");
            takeDataTable.Columns.Add("sku_dsc");
            takeDataTable.Columns.Add("stk_order_qty");
            takeDataTable.Columns.Add("sto");
            takeDataTable.Columns.Add("inv");
            return takeDataTable;
        }

        public void givetable(DataTable table, List<string> skusto) //用于修改DataTable的方法
        {
            //是否提交成功
            if (table != null)
            {
                //如果清单页面有删除的数据，在list中找出删除
                if (skusto != null)
                {
                    for (int i = 0; i < skusto.Count; i++)
                    {
                        for (int j = 0; j < skustoList.Count; j++)
                        {
                            if (skusto[i] == skustoList[j])
                            {
                                skustoList.RemoveAt(j);
                            }
                        }
                    }
                }

                if (takeDataTable == null)
                {
                    takeDataTable = new DataTable();
                }
                else
                {
                    takeDataTable.Clear();
                }
                takeDataTable = table;
                ds.Tables.Remove("detail");
                ds.Tables.Add(takeDataTable.Copy());
                UpdateColumn(true, true, false);
            }
            else
            {
                takeDataTable.Rows.Clear();
            }
            //刷新表后清除dgtable
            chearAll();
            button6.Enabled = false;
            button7.Enabled = false;

            //if (table.Rows.Count > 0)
            //{
            //    for (int i = 0; i < table.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < table.Columns.Count; j++)
            //        {
            //            takeDataTable.Rows[i][j] = table.Rows[i][j];
            //        }
            //    }
            //}
        }

        #endregion

    }
}