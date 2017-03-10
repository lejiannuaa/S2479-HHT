using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HolaCore;
using System.Xml;
using System.IO;

namespace Hola_Out
{
    public partial class FormNormalObjOut : Form, ScanCallback, ISerializable, ConnCallback
    {
        #region 页面常量
        private const string guid = "3B93B98D-2F94-5ED5-E0D1-4B193E79D2DF";
        private const string OGuid = "00000000000000000000000000000000";
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        //指示当前正在发送网络请求
        private bool busy = false;
        private API_ID apiID = API_ID.NONE;
        private int TABLE_ROWMAX = 6;
        public DataSet ds = null;
        //private int tableIndex = -1;
        private int pageIndex = 1;
        private int pageCount = -1;
        private string xmlfile = null;
        private FormScan formScan = null;
        private DataTable Reqtable = null;//要提交的表
        private DataTable dtOrder = null;//默认排序表
        private bool bCheckAll = false;
        public string[] VWarray = null;
        private string colModify = "";
        private string isReserve = "";
        bool turnpage = false;
        private int TaskBarHeight = 0;
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            l1101,
            l1102,
            DOWNLOAD_01,
        }

        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            BARCODE,
            FROM,
            TO,
            TOID,
            TONAME,
            NEED,
            COUNT,
            TABLEINDEX,
            PAGEINDEX,
            CHILD,
            COLINDEX,
            LASTBC,
            SORTFLAG,
            DATAMAX
        }
        #endregion

        public FormNormalObjOut()
        {
            InitializeComponent();
            doLayout();
            RequsetDataTable();
            //ds = new DataSet();
            //ds.Tables.Add(Reqtable.Copy());
            pageIndex = 1;
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                btnConfirm.Top = dstHeight - btnConfirm.Height;
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }


        #region 委托传值
        public void giveStringList(string[] VW) //用于修改DataTable的方法
        {
            try
            {
                if (VW != null)
                {
                    VWarray = VW;
                }
                else
                {
                    MessageBox.Show("没有填好体积和重量");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region UI响应

        private void request01()
        {
            apiID = API_ID.l1101;
            string msg = "request=111;usr=" + Config.User + ";op=01" + ";hhtcno=" + btnBC.Text.ToString().ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR01()
        {
            apiID = API_ID.l1102;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "111", "02");
            string msg = "request=111;usr=" + Config.User + ";op=02" + ";hhtcno=" + btnBC.Text.ToString().ToUpper() + ";json=" + json;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private DataTable RequsetDataTable()
        {
            Reqtable = new DataTable();
            Reqtable.TableName = "detail";
            Reqtable.Columns.Add("HHTCNO");
            Reqtable.Columns.Add("HHTSTS");
            Reqtable.Columns.Add("CheckBox");
            return Reqtable;
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;
            //xmlfile = @"\program files\hhtiii\00205.xml";
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
                            if (dt.Rows.Count > 0)
                            {
                                //pageIndex = 1;
                                //pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                                //if (pageCount == 0)
                                //    Page.Text = "1/1";
                                //else
                                //    Page.Text = "1/" + pageCount.ToString();

                                for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                                {
                                    DataRow row = Reqtable.NewRow();
                                    row["HHTCNO"] = ds.Tables["detail"].Rows[i]["HHTCNO"].ToString();
                                    row["HHTSTS"] = ds.Tables["detail"].Rows[i]["HHTSTS"].ToString();
                                    row["CheckBox"] = "false";
                                    Reqtable.Rows.Add(row);
                                }

                                pageCount = (int)Math.Ceiling(Reqtable.Rows.Count / (double)TABLE_ROWMAX);
                                if (pageCount == 0)
                                {
                                    Page.Text = "1/1";
                                }
                                else
                                {
                                    Page.Text = "1/" + pageCount.ToString();
                                }

                                isReserve = ds.Tables["detail"].Rows[0]["HHTSTS"].ToString();
                                btnBC.Text = ds.Tables["detail"].Rows[0]["HHTCNO"].ToString();

                                btnAdd.Enabled = true;
                                btnAdd.Focus();
                                btnConfirm.Enabled = true;
                                NextPage.Enabled = true;
                                PrePage.Enabled = true;
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
                //File.Delete(xmlfile);
                //GC.Collect();
                Serialize(guid);
            }
        }

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                //string[] colName = { "hhtcno", "hhtvol", "hhtwei" };
                string[] colName = { "hhtcno", "hhtsts" };
                //DataTable dt = ds.Tables["detail"];
                DataTable dt = Reqtable;

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();
                    rowNew["hhtcno"] = dt.Rows[i]["HHTCNO"];
                    rowNew["hhtsts"] = dt.Rows[i]["HHTSTS"];
                    //rowNew["hhtvol"] = dt.Rows[i]["HHTVOL"];
                    //rowNew["hhtwei"] = dt.Rows[i]["HHTWEI"];
                    dtOK.Rows.Add(rowNew);
                }
            }
            catch (Exception)
            {
            }
            return dtOK;
        }

        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;

            switch (apiID)
            {
                case API_ID.l1101:
                    {
                        string file = Config.getApiFile("111", "01");
                        from += "/111/" + file;
                        to += file;
                        xmlfile = to;
                        apiID = API_ID.DOWNLOAD_01;
                    }
                    break;
                default:
                    return;
            }

            new ConnThread(this).Download(from, to, false);
            wait();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (btnBC.Text == dt.Rows[i]["HHTCNO"].ToString())
                        {
                            MessageBox.Show("该箱号已经添加，不能重复添加！");
                            return;
                        }
                    }
                }
                UpdatedgTable(false, turnpage);
                this.btnBC.Focus();
                //addbutton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PrePage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable.DataSource != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(Reqtable.Rows.Count / (double)TABLE_ROWMAX);
                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdatedgTable(false, false);
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

        private void NextPage_Click(object sender, EventArgs e)
        {
            if (!busy && dgTable.DataSource != null)
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                if (dt.Rows.Count > 0)
                {
                    int pageCount = (int)Math.Ceiling(Reqtable.Rows.Count / (double)TABLE_ROWMAX);
                    //UpdateDsData();
                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {
                        UpdatedgTable(false, false);
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

        private void UpdatedgTable(bool bInitColumn, bool turnPage)
        {
            try
            {
                string[] colName = new string[] { "HHTCNO", "HHTSTS", "CheckBox" };
                string[] colValue = new string[] { "出货箱号", "当前状态", "false" };
                //int[] colWidth = new int[] { 47, 35, 15 };
                int[] colWidth = new int[] { 52, 35, 10 }; 
                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colValue.Length; i++)
                {
                    if (colName[i].Equals("CheckBox"))
                    {
                        row[i] = "false";
                    }
                    else
                    {
                        row[i] = colValue[i];
                    }
                }
                dtHeader.Rows.Add(row);
                dgHeader.Controls.Clear();
                dgHeader.TableStyles.Clear();
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, true));
                dgHeader.DataSource = dtHeader;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateRow()
        {
            DataGridCustomCheckBoxColumn colCheckBox = null;
            GridColumnStylesCollection styles = dgTable.TableStyles[0].GridColumnStyles;
            //DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
            DataTable dt = Reqtable;
            DataTable dtResult = new DataTable();

            //if (!dt.Columns.Contains("CheckBox"))
            //{
            //    dt.Columns.Add(new DataColumn("CheckBox", typeof(String)));
            //}
            //for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
            //{
            //    ds.Tables["detail"].Rows[i]["CheckBox"] = "false";
            //}

            for (int i = 0; i < styles.Count; i++)
            {
                dtResult.Columns.Add(styles[i].MappingName);
                if (styles[i].MappingName.Equals("CheckBox"))
                {
                    colCheckBox = (DataGridCustomCheckBoxColumn)styles[i];
                    dtResult.Columns[i].DataType = typeof(Boolean);
                }
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
                    if (name.Equals("CheckBox"))
                    {
                        rowNew[name] = dt.Rows[i][name].Equals("True") ? true : false;
                    }
                    else
                    {
                        rowNew[name] = dt.Rows[i][name];
                    }
                }

                dtResult.Rows.Add(rowNew);
            }

            dgTable.DataSource = dtResult;
            if (colCheckBox != null && to > 0)
            {
                CheckBox box = (CheckBox)colCheckBox.HostedControl;
                box.CheckStateChanged += new EventHandler(dgTable_CheckStateChanged);
            }
        }

        private void dgTable_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox box = (CheckBox)sender;
                bool bCheck = box.CheckState == CheckState.Checked ? true : false;
                DataTable dt = (DataTable)dgTable.DataSource;
                int rowIndex = dgTable.CurrentCell.RowNumber;
                colModify = "CheckBox";
                updateData(dt.Rows[rowIndex]["HHTCNO"].ToString(), bCheck.ToString());

                if (bCheckAll && !bCheck)
                {
                    bCheckAll = false;
                    DataTable dtHeader = (DataTable)dgHeader.DataSource;
                    dtHeader.Rows[0]["CheckBox"] = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateData(string key, string value)
        {
            try
            {
                DataTable dt = Reqtable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (key.Equals(dt.Rows[i]["HHTCNO"].ToString()))
                    {
                        if (!value.Equals(dt.Rows[i][colModify]))
                        {
                            if (colModify.Equals("CheckBox"))
                            {
                                dt.Rows[i][colModify] = bool.Parse(value);
                            }
                            else
                            {
                                dt.Rows[i][colModify] = value;
                            }
                            UpdatedgTable(false, false);
                            Serialize(guid);
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //private void dgTable_CheckStateChanged(object sender, EventArgs e)
        //{
        //    CheckBox box = (CheckBox)sender;
        //    bool bCheck = box.CheckState == CheckState.Checked ? true : false;
        //    //UpdateDsData();
        //    if (bCheckAll && !bCheck)
        //    {
        //        bCheckAll = false;
        //        DataTable dtHeader = (DataTable)dgHeader.DataSource;
        //        dtHeader.Rows[0]["CheckBox"] = false;
        //    }
        //}

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool bCustom)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = Reqtable; ;
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;

            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = null;
                if (bCustom && name[i].Equals("CheckBox"))
                {
                    style = new DataGridCustomCheckBoxColumn();
                }
                else
                {
                    style = new DataGridCustomTextBoxColumn();
                }
                style.Owner = dgTable;
                if (style is DataGridCustomTextBoxColumn)
                {
                    style.ReadOnly = true;
                }
                style.MappingName = name[i];
                style.Width = (int)(dstWidth * width[i] / 100);
                style.AlternatingBackColor = SystemColors.ControlDark;
                ts.GridColumnStyles.Add(style);
            }

            return ts;
        }

        private void addbutton()
        {
            if (Reqtable != null)
            {
                DataTable Dt = ds.Tables["detail"];
                if (!Dt.Columns.Contains("CheckBox"))
                {
                    Dt.Columns.Add(new DataColumn("CheckBox", typeof(String)));
                }
                DataRow newrow = Dt.NewRow();
                newrow["HHTCNO"] = btnBC.Text;
                newrow["HHTSTS"] = isReserve;
                newrow["CheckBox"] = "false";
                Dt.Rows.Add(newrow);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (MessageBox.Show("确定删除选中行？", "",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    //UpdateDsData();
                    //DataTable dts = ds.Tables["detail"];
                    DataTable dts = Reqtable;
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
                            //for (int i = 0; i < dts.Rows.Count; i++)
                            //{
                            //    dts.Rows[i]["sel_no"] = i + 1;
                            //}
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
                                    //UpdatedgHeader();
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
                                UpdatedgTable(false, false);
                                //ItemsAlr.Text = ds.Tables["detail"].Rows.Count.ToString();
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

        private void btnReserve_Click(object sender, EventArgs e)
        {
            requestR01();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Close();
        }


        #endregion

        #region 实现ScanCallback接口
        public void scanCallback(string barCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barCode))
                {
                    this.btnBC.Text = barCode;
                    for (int i = 0; i < Reqtable.Rows.Count; i++)
                    {
                        if (btnBC.Text == Reqtable.Rows[i]["HHTCNO"].ToString())
                        {
                            return;//箱号已经存在不再请求数据
                        }
                    }
                    this.btnBC.Focus();
                    request01();
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(@"\Program Files\hhtiii\00205.xml");
                    //LoadData();
                }
                else
                {
                    MessageBox.Show("箱号不可为空!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ISerializable 成员

        public void init(object[] param)
        {

        }

        public void Serialize(string file)
        {

        }

        public void Deserialize(string file)
        {

        }

        #endregion

        #region ConnCallback 成员
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
                    case API_ID.l1101:
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

                    case API_ID.l1102:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("出货成功");

                            btnAdd.Enabled = false;
                            btnConfirm.Enabled = false;
                            NextPage.Enabled = false;
                            PrePage.Enabled = false;
                            btnDelete.Enabled = false;
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

                    case API_ID.DOWNLOAD_01:
                        if (result == ConnThread.RESULT_FILE)
                        {
                            LoadData();
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
                            if (hitTest.Column == 2)
                            {
                                //dt.Rows[hitTest.Row][hitTest.Column] = !(bool)dt.Rows[hitTest.Row][hitTest.Column];
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

        //private void btnAdd_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
        //    {
        //        btnAdd_Click(sender, null);
        //    }
        //}
    }
}