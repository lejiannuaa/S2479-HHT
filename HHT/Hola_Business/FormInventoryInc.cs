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

namespace Hola_Business
{
    public partial class FormInventoryInc : Form, ConnCallback, ISerializable
    {
        private const string guid = "10D1A3F3-E512-13D8-6556-F347E1F12109";
        private const string OGuid = "00000000000000000000000000000000";
        //private SerializeClass sc = null;
        private Form child = null;
        private Form_ID ChildID = Form_ID.Null;
        private API_ID apiID = API_ID.NULL;
        private string xmlfile = null;
        private DataSet ds = null;
        private string ShopNo = null;
        private string Value = "";//原因对应的值
        private DataTable takeDataTable = null;//要提交的表
        private DataTable comboxTable = null;

        Regex regText = new Regex(@"^[A-Za-z0-9]+$");

        public FormInventoryInc()
        {

            InitializeComponent();
            doLayout();
            //初始化提交表的架构
            RequsetDataTable();

            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"\Program Files\hhtiii\00207.xml");
            //LoadXML();
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

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104701,
            DOWNLOAD_XML
        }
        #endregion

        #region 子窗口ID
        private enum Form_ID
        {
            InventoryCX,
            InventoryQD,
            Null,
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
                    case API_ID.R104701:
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

        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R104701:
                        {
                            string file = Config.getApiFile("1047", "01");
                            from += "/1047/" + file;
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

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

            //xmlfile = @"\Program Files\hhtiii\00207.xml";

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
                        if (dt.Rows.Count > 0)
                        {
                            SKUNO.Text = dt.Rows[0]["sku"].ToString();
                            GoodsNO.Text = dt.Rows[0]["ipc"].ToString();
                            SKUName.Text = dt.Rows[0]["sku_dsc"].ToString();
                            SDTP.Text = dt.Rows[0]["s_dpt"].ToString();
                            status.Text = dt.Rows[0]["status"].ToString();
                            inv_no.Text = dt.Rows[0]["inv_no"].ToString();
                        }
                        if (ds.Tables["grid"] != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                //加载原因表
                                comboxTable = (DataTable)ds.Tables["grid"];
                                comboBox1.Items.Add("请输入");
                                comboBox1.Text = "请输入";

                                //comboBox1.DataSource = (DataTable)ds.Tables["grid"];
                                //comboBox1.DisplayMember = "tbldsc";
                                //comboBox1.ValueMember = "tblval";

                                comboBox1.Enabled = true;
                                button4.Enabled = true;
                                trueKC.Enabled = true;
                                trueKC.Focus();
                            }
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

        public void Clear()
        {
            SKUNO.Text = "";
            SKUName.Text = "";
            GoodsNO.Text = "";
            SDTP.Text = "";
            status.Text = "";
            inv_no.Text = "";
            trueKC.Text = "";
            Diff.Text = "";
            comboBox1.DataSource = null;
        }

        #endregion

        #region ISerializable 成员

        public void init(object[] param)
        {
            ShopNo = (string)param[0];
        }

        public void Serialize(string file)
        {

        }

        public void Deserialize(string file)
        {

        }

        #endregion

        //查询
        private void button1_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    //MessageBox.Show("请输入SKU！");
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
                    //doc.Load(@"\Program Files\hhtiii\00207.xml");
                    //LoadXML();
                }
            }
        }

        private void request01()
        {
            apiID = API_ID.R104701;
            string msg = "request=1047;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.DataSource != null && comboBox1.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    Value = comboBox1.SelectedValue.ToString();
                }
                else
                {
                    comboBox1.Text = "请输入";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //添加
        //string delSku = null;
        List<string> skuList = new List<string>();
        private void button4_Click(object sender, EventArgs e)
        {
            if (trueKC.Text == null || trueKC.Text == "")
            {
                MessageBox.Show("请输入实际库存量");
                return;
            }
            if (Value == "" || Value == null)
            {
                MessageBox.Show("请选择库存调整原因");
                return;
            }
            if (int.Parse(trueKC.Text.ToString()) < 0)
            {
                if (MessageBox.Show("输入为负数，确定吗？", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                }
                else
                {
                    return;
                }
            }
            DataRow row = takeDataTable.NewRow();
            List<string> list = new List<string>();
            list.Add(SKUNO.Text);
            list.Add(SKUName.Text);
            list.Add(trueKC.Text.ToString());
            list.Add(Diff.Text);
            list.Add(Value);
            for (int i = 0; i < takeDataTable.Columns.Count; i++)
            {
                row[i] = list[i];
            }
            skuList.Add(SKUNO.Text);
            takeDataTable.Rows.Add(row);
            Clear();
            button4.Enabled = false;
            SKUNO.Focus();
            comboBox1.Enabled = false;
            trueKC.Enabled = false;
        }

        private DataTable RequsetDataTable()
        {
            takeDataTable = new DataTable();
            takeDataTable.TableName = "takeTable";
            takeDataTable.Columns.Add("sku");
            takeDataTable.Columns.Add("sku_dsc");
            takeDataTable.Columns.Add("inv_act_no");
            takeDataTable.Columns.Add("inv_adj_no");
            takeDataTable.Columns.Add("adj_reason");
            return takeDataTable;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (trueKC.Text != null)
                {
                    if (trueKC.Text != null && trueKC.Text != "")
                    {
                        int diff = int.Parse(trueKC.Text.ToString()) - int.Parse(inv_no.Text.ToString());
                        Diff.Text = diff.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //下单清单
            ChildID = Form_ID.InventoryQD;
            showChild(false);
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
                    case Form_ID.InventoryQD:
                        FormInventoryQD form = new FormInventoryQD(takeDataTable, false);
                        form.myevent += new FormInventoryQD.mydelegate(givetable);
                        child = form;
                        break;

                    default:
                        return;
                }

                ((ISerializable)child).init(new string[] { ShopNo, "" });
                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }


                if (child.ShowDialog() == DialogResult.OK)
                {
                    //File.Delete(Config.DirLocal + guid);
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

        public void givetable(DataTable table, List<string> sku) //用于修改DataTable的方法
        {
            if (table != null)
            {
                //如果清单页面有删除的数据，在list中找出删除
                if (sku != null)
                {
                    for (int i = 0; i < sku.Count; i++)
                    {
                        for (int j = 0; j < skuList.Count; j++)
                        {
                            if (sku[i] == skuList[j])
                            {
                                skuList.RemoveAt(j);
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
            }
            else
            {
                takeDataTable.Rows.Clear();
            }

            Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void SKUNO_TextChanged(object sender, EventArgs e)
        {
            //SKUName.Text = "";
            //GoodsNO.Text = "";
            //SDTP.Text = "";
            //status.Text = "";
            //inv_no.Text = "";
            //trueKC.Text = "";
            //comboBox1.DataSource = null;
        }

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    return;
                    //MessageBox.Show("请输入SKU！");
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
                button4.Enabled = false;

                SKUName.Text = "";
                GoodsNO.Text = "";
                SDTP.Text = "";
                status.Text = "";
                inv_no.Text = "";
                trueKC.Text = "";
                comboBox1.DataSource = null;
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Enabled)
            {
                if (MessageBox.Show("是否退出？", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
            }
        }

        private void FormInventoryInc_Load(object sender, EventArgs e)
        {
        }

        private void comboBox1_LostFocus(object sender, EventArgs e)
        {

        }

        private void comboBox1_GotFocus(object sender, EventArgs e)
        {
            try
            {
                //comboBox1.Items.Clear();
                //comboBox1.Text = null;
                comboBox1.DataSource = null;

                comboBox1.DataSource = comboxTable;
                comboBox1.DisplayMember = "tbldsc";
                comboBox1.ValueMember = "tblval";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void trueKC_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!regText.IsMatch(trueKC.Text))
            //{
            //    //MessageBox.Show("输入的实际库存量非法！");
            //}
        }
    }
}