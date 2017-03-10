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
    public partial class FormOtherReserve2 : Form, ScanCallback, ISerializable, ConnCallback
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
        private int pageIndex = 1;
        private int pageCount = -1;
        private DataTable temporaryTable = null;//临时表
        private DataTable dtOrder = null;//默认排序表
        private bool bCheckAll = false;
        public string[] VWarray = null;
        private string xmlfile = null;
        private int TaskBarHeight = 0;
        List<string> hhtcno = new List<string>();
        //bool turnpage = false;
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NONE = 0,
            l0901,
            l0902,
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

        public FormOtherReserve2()
        {
            InitializeComponent();
            doLayout();
            setlableORtext(false);
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

                btnReserve.Top = dstHeight - btnReserve.Height;
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
            apiID = API_ID.l0901;
            string msg = "request=109;usr=" + Config.User + ";op=01";
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestR01()
        {
            apiID = API_ID.l0902;
            string json = Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[] { getDTOK() }), "109", "02");
            string msg = "request=109;usr=" + Config.User + ";op=02" + ";json=" + json;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                string[] colName = { "hhtcno", "hhtvol", "hhtnum", };
                DataTable dt = ds.Tables["detail"];

                dtOK.TableName = ds.Tables["detail"].TableName;
                for (int i = 0; i < colName.Length; i++)
                {
                    dtOK.Columns.Add(colName[i]);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = dtOK.NewRow();
                    rowNew["hhtcno"] = dt.Rows[i]["HHTCNO"];
                    //rowNew["HHTCNO"] = dt.Rows[i]["HHTCNO"];
                    rowNew["hhtvol"] = dt.Rows[i]["HHTVOL"];
                    rowNew["hhtnum"] = dt.Rows[i]["HHTNUM"];
                    dtOK.Rows.Add(rowNew);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return dtOK;
        }

        private void requestXML()
        {
            string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
            string to = Config.DirLocal;
            switch (apiID)
            {
                case API_ID.l0901:
                    {
                        string file = Config.getApiFile("109", "01");
                        from += "/109/" + file;
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

        private DataGridTableStyle getTableStyle(string[] name, int[] width, bool bCustom)
        {
            int dstWidth = Screen.PrimaryScreen.Bounds.Width;

            DataTable dt = ds.Tables["detail"];
            DataGridTableStyle ts = new DataGridTableStyle();
            ts.MappingName = dt.TableName;

            for (int i = 0; i < name.Length; i++)
            {
                DataGridCustomColumnBase style = null;
                if (name[i].Equals("HHTVOL"))
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                }
                else if (name[i].Equals("HHTNUM"))
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = false;
                }
                else
                {
                    style = new DataGridCustomTextBoxColumn();
                    style.Owner = dgTable;
                    style.ReadOnly = true;
                }
                //if (style is DataGridCustomTextBoxColumn)
                //{
                //    style.ReadOnly = true;
                //    style.Owner = dgTable;
                //}
                style.MappingName = name[i];
                style.Width = (int)(dstWidth * width[i] / 100);
                style.AlternatingBackColor = SystemColors.ControlDark;
                ts.GridColumnStyles.Add(style);
            }

            return ts;
        }

        private void btnReserve_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgTable.DataSource;
                DataTable DT = ds.Tables["detail"];
                DT.TableName = "detail";

                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        DT.Rows[i]["HHTVOL"] = textBox0.Text;
                        DT.Rows[i]["HHTNUM"] = textBox1.Text;
                        DT.Rows[i]["HHTCNO"] = hhtcno[i];
                    }
                    if (i == 1)
                    {
                        DT.Rows[i]["HHTVOL"] = textBox2.Text;
                        DT.Rows[i]["HHTNUM"] = textBox3.Text;
                        DT.Rows[i]["HHTCNO"] = hhtcno[i];
                    }
                    if (i == 2)
                    {
                        DT.Rows[i]["HHTVOL"] = textBox4.Text;
                        DT.Rows[i]["HHTNUM"] = textBox5.Text;
                        DT.Rows[i]["HHTCNO"] = hhtcno[i];
                    }
                    if (i == 3)
                    {
                        DT.Rows[i]["HHTVOL"] = textBox6.Text;
                        DT.Rows[i]["HHTNUM"] = textBox7.Text;
                        DT.Rows[i]["HHTCNO"] = hhtcno[i];
                    }
                }
                ds.Tables.Remove("detail");
                ds.Tables.Add(DT);

                requestR01();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
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
            //request01();

            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"\Program Files\hhtiii\00203.xml");
            //LoadData();
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
                    case API_ID.l0901:
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

                    case API_ID.l0902:
                        if (result == ConnThread.RESULT_OK)
                        {
                            MessageBox.Show("预约成功");
                            setlableORtext(false);
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

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            //xmlfile = @"\program files\hhtiii\00203.xml";
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

                                UpdatedgTable(true,true);

                                DataTable dtResult = new DataTable();
                                dtResult.TableName = dt.TableName;
                                string[] name = new string[] { "HHTCNO", "HHTVOL", "HHTNUM", "TBLDSC" };

                                for (int i = 0; i < ds.Tables["detail"].Rows.Count; i++)
                                {
                                    for (int j = 0; j < ds.Tables["detail"].Columns.Count; j++)
                                    {
                                        if (dt.Rows[i][name[j]].ToString() == "JCP000001")
                                        {
                                            label1.Text = "寄仓品";
                                            hhtcno.Add(dt.Rows[i][name[j]].ToString());
                                        }
                                        else if (dt.Rows[i][name[j]].ToString() == "PSP000001")
                                        {
                                            label2.Text = "破损品";
                                            hhtcno.Add(dt.Rows[i][name[j]].ToString());
                                        }
                                        else if (dt.Rows[i][name[j]].ToString() == "WLX000001")
                                        {
                                            label3.Text = "物流周转箱";
                                            hhtcno.Add(dt.Rows[i][name[j]].ToString());
                                        }
                                        else if (dt.Rows[i][name[j]].ToString() == "XLP000001")
                                        {
                                            label4.Text = "行李/个人物品";
                                            hhtcno.Add(dt.Rows[i][name[j]].ToString());
                                        }
                                        else
                                        {
                                            if (i == 0 && j == 1)
                                            {
                                                textBox0.Text = dt.Rows[i][name[j]].ToString();
                                            }
                                            if (i == 0 && j == 2)
                                            {
                                                textBox1.Text = dt.Rows[i][name[j]].ToString();
                                            }

                                            if (i == 1 && j == 1)
                                            {
                                                textBox2.Text = dt.Rows[i][name[j]].ToString();
                                            }
                                            if (i == 1 && j == 2)
                                            {
                                                textBox3.Text = dt.Rows[i][name[j]].ToString();
                                            }

                                            if (i == 2 && j == 1)
                                            {
                                                textBox4.Text = dt.Rows[i][name[j]].ToString();
                                            }
                                            if (i == 2 && j == 2)
                                            {
                                                textBox5.Text = dt.Rows[i][name[j]].ToString();
                                            }


                                            if (i == 3 && j == 1)
                                            {
                                                textBox6.Text = dt.Rows[i][name[j]].ToString();
                                            }
                                            if (i == 3 && j == 2)
                                            {
                                                textBox7.Text = dt.Rows[i][name[j]].ToString();
                                            }
                                        }
                                    }
                                }
                                setlableORtext(true);
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            //dgTable.DataSource = null;
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

        private void UpdatedgTable(bool bInitColumn, bool turnPage)
        {
            try
            {
                string[] colName = new string[] { "HHTCNO", "HHTVOL", "HHTNUM", "TBLDSC" };
                string[] colValue = new string[] { "箱号", "总材积", "总件数", "TBLDSC" };
                int[] colWidth = new int[] { 34, 32, 31, 0 };

                //dtOrder = ds.Tables["detail"].DefaultView.ToTable();
                dgTable.Controls.Clear();
                dgTable.TableStyles.Clear();
                dgTable.TableStyles.Add(getTableStyle(colName, colWidth, true));
                UpdateRow();

                DataTable dtHeader = ((DataTable)dgTable.DataSource).Clone();
                DataRow row = dtHeader.NewRow();
                for (int i = 0; i < colValue.Length; i++)
                {
                    row[i] = colValue[i];
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
            DataGridCustomTextBoxColumn colTextBox = null;
            DataGridCustomTextBoxColumn colTextBox1 = null;
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
                if (styles[i].MappingName.Equals("HHTVOL"))
                {
                    colTextBox = (DataGridCustomTextBoxColumn)styles[i];
                }
                if (styles[i].MappingName.Equals("HHTNUM"))
                {
                    colTextBox1 = (DataGridCustomTextBoxColumn)styles[i];
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
                        if (dt.Rows[i][name].ToString() == "JCP000001")
                        {
                            rowNew[name] = "物流周转箱";
                        }
                        else if (dt.Rows[i][name].ToString() == "PSP000001")
                        {
                            rowNew[name] = "破损品";
                        }
                        else if (dt.Rows[i][name].ToString() == "WLX000001")
                        {
                            rowNew[name] = "行李/个人物品";
                        }
                        else if (dt.Rows[i][name].ToString() == "XLP000001")
                        {
                            rowNew[name] = "寄仓品";
                        }
                        else
                        {
                            rowNew[name] = dt.Rows[i][name];
                        }
                    }
                }
                dtResult.Rows.Add(rowNew);
            }

            dgTable.DataSource = dtResult;
            if (colCheckBox != null && to > 0)
            {
                CheckBox box = (CheckBox)colCheckBox.HostedControl;
                //box.CheckStateChanged += new EventHandler(dgTable_CheckStateChanged);
            }
            if (colTextBox != null && to > 0)
            {
                TextBox Tbox = (TextBox)colTextBox.HostedControl;
                //Tbox.LostFocus += new EventHandler(Tbox_LostFocus);
            }
            if (colTextBox1 != null && to > 0)
            {
                TextBox Tbox1 = (TextBox)colTextBox1.HostedControl;
                //Tbox1.LostFocus += new EventHandler(Tbox1_LostFocus);
            }
        }


        private void setlableORtext(bool lableVisible)
        {
            if (lableVisible)
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                textBox0.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                textBox7.Visible = true;
            }
            else {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                textBox0.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
            }

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            request01();
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"\Program Files\hhtiii\00203.xml");
            //LoadData();
        }


    }
}