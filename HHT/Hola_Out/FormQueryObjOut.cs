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
    public partial class FormQueryObjOut : Form, ScanCallback, ISerializable, ConnCallback
    {
        #region 页面常量
        private const string guid = "3B93B98D-2F94-5ED5-E0D1-4B193E79D2ER";
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
        //private FormScan formScan = null;
        private DataTable Reqtable = null;//要提交的表
        private DataTable dtOrder = null;//默认排序表
        private bool bCheckAll = false;
        public string[] VWarray = null;
        private string colModify = "";
        //bool turnpage = false; 
        private int TaskBarHeight = 0;
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            l1301,
            l1302,
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

        public FormQueryObjOut()
        {
            InitializeComponent();
            doLayout();
            //RequsetDataTable();
            //ds = new DataSet();
            //ds.Tables.Add(Reqtable.Copy());
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
            apiID = API_ID.l1301;
            string msg = "request=113;usr=" + Config.User + ";op=01" + ";from=" + dtpFrom.Text.ToString().ToUpper() +
                            ";to=" + dtpTo.Text.ToString().ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

           
        private void requestR01()
        { 
            apiID = API_ID.l1302;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "113", "02");
            string msg = "request=113;usr=" + Config.User + ";op=02" + ";from=" + dtpFrom.Text.ToString().ToUpper() +
                            ";to=" + dtpTo.Text.ToString().ToUpper();
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;
            //xmlfile = @"\program files\hhtiii\00204.xml";
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
                                pageIndex = 1;
                                pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
                                if (pageCount == 0)
                                    Page.Text = "1/1";
                                else
                                    Page.Text = "1/" + pageCount.ToString();

                                dt.Columns.Add("CheckBox", typeof(string));
                                for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                                {
                                    dt.Rows[i]["CheckBox"] = "false";
                                }
                                Reqtable = dt.Copy();
                                Reqtable.TableName = "detail";
                                ds.Tables.Remove("detail");
                                ds.Tables.Add(Reqtable);
                                UpdatedgTable(false, false);
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
                string[] colName = { "HHTCNO", "HHTVOL", "HHTWEI" };
                DataTable dt = ds.Tables["detail"];

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();
                    rowNew["HHTCNO"] = dt.Rows[i]["HHTCNO"];
                    //rowNew["HHTVOL"] = dt.Rows[i]["HHTVOL"];
                    //rowNew["HHTWEI"] = dt.Rows[i]["HHTWEI"];
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
                case API_ID.l1301:
                    {
                        string file = Config.getApiFile("113", "01");
                        from += "/113/" + file;
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
                request01();
                //XmlDocument doc = new XmlDocument();
                //doc.Load(@"\Program Files\hhtiii\00204.xml");
                //LoadData();
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
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
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
                    int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
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

        private void UpdatedgTable(bool bInitColumn, bool turnPage)
        {
            try
            {
                string[] colName = new string[] { "HHTCNO", "HHTVOL", "HHTWEI", "CheckBox" };
                string[] colValue = new string[] { "箱号", "材积", "重量/件数", "false" };
                //int[] colWidth = new int[] { 35, 31, 32, 0 };
                int[] colWidth = new int[] { 51, 23, 23, 0 }; 
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
                dgHeader.TableStyles.Add(getTableStyle(colName, colWidth, false));
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
            DataTable dt = ds.Tables["detail"].DefaultView.ToTable();
            DataTable dtResult = new DataTable();
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
                DataTable dt = ds.Tables["detail"];
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

            //DataTable dt = Reqtable;
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = ds.Tables["detail"].TableName;
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
                DataRow newrow = Dt.NewRow();
                //newrow["HHTCNO"] = btnBC.Text;
                newrow["HHTVOL"] = VWarray[0];
                newrow["HHTWEI"] = VWarray[1];
                newrow["CheckBox"] = "false";
                Dt.Rows.Add(newrow);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Reqtable.Rows.Count > 0)
            {
                requestR01();
            }
            else
            {
                MessageBox.Show("表格内无数据！");
            }

            //if (!busy)
            //{
            //    if (MessageBox.Show("确定删除选中行？", "",
            //     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //    {
            //        //UpdateDsData();
            //        DataTable dts = ds.Tables["detail"];
            //        if (dts != null)
            //        {
            //            bool Checked = false;
            //            if (dts.Rows.Count >= 1)
            //            {
            //                for (int i = dts.Rows.Count - 1; i >= 0; i--)
            //                {
            //                    if (bool.Parse(dts.Rows[i]["CheckBox"].ToString()))
            //                    {
            //                        dts.Rows.RemoveAt(i);
            //                        Checked = true;
            //                    }
            //                }
            //                //for (int i = 0; i < dts.Rows.Count; i++)
            //                //{
            //                //    dts.Rows[i]["sel_no"] = i + 1;
            //                //}
            //                if (!Checked)
            //                {
            //                    MessageBox.Show("未选中任何行!");
            //                }
            //                else
            //                {
            //                    //删除所有页数据行
            //                    if (bCheckAll)
            //                    {
            //                        pageIndex = 1;
            //                        bCheckAll = false;
            //                        //UpdatedgHeader();
            //                    }
            //                    //删除部分数据行
            //                    else
            //                    {
            //                        int pageCount = (int)Math.Ceiling(ds.Tables["detail"].Rows.Count / (double)TABLE_ROWMAX);
            //                        if (pageIndex > pageCount)
            //                        {
            //                            pageIndex--;
            //                        }
            //                        if (pageIndex <= 0)
            //                        {
            //                            pageIndex = 1;
            //                        }
            //                    }
            //                    UpdatedgTable(false, false);
            //                    //ItemsAlr.Text = ds.Tables["detail"].Rows.Count.ToString();
            //                    Serialize(guid);
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("表格内无数据!");
            //            }
            //        }
            //    }
            //}
        }

        private void btnReserve_Click(object sender, EventArgs e)
        {
            requestR01();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private DataTable RequsetDataTable()
        {
            Reqtable = new DataTable();
            Reqtable.TableName = "detail";
            Reqtable.Columns.Add("HHTCNO");
            Reqtable.Columns.Add("HHTVOL");
            Reqtable.Columns.Add("HHTWEI");
            Reqtable.Columns.Add("CheckBox");
            return Reqtable;
        }


        #endregion

        #region 实现ScanCallback接口
        public void scanCallback(string barCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(barCode))
                {
                    //this.btnBC.Text = barCode;
                    request01();
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
                    case API_ID.l1301:
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

                    case API_ID.l1302:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("列印成功");
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
                            btnConfirm.Enabled = true;
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
    }
}