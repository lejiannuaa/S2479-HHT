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
    public partial class FormHHTOrderInc : Form, ConnCallback, ISerializable
    {
        private const string guid = "F71BA2EE-E27C-EFF7-A55C-958EC11565AA";//
        private const string OGuid = "00000000000000000000000000000000";
        private string ShopNo = null;
        private SerializeClass sc = null;
        private Form child = null;
        private Form_ID ChildID = Form_ID.Null;
        private API_ID apiID = API_ID.NULL;
        private string xmlfile = null;
        private DataSet ds = null;

        private DataTable takeDataTable = null;//要提交的表
        //private DataTable dtOrder = null; //默认排序表
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");

        #region 子窗口ID
        private enum Form_ID
        {
            OrederQD,
            Null,
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104601,
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
            Child,
            DataMax
        }
        #endregion

        private int TaskBarHeight = 0;

        public FormHHTOrderInc()
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
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                button5.Top = dstHeight - button5.Height;
                button4.Top = dstHeight - button4.Height;
                button3.Top = dstHeight - button4.Height - 50;

                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBarI.Top = button5.Top - pbBarI.Height * 3;
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        #region UI响应

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

        private void request01()
        {
            apiID = API_ID.R104601;
            string msg = "request=1046;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
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
                else
                {
                    File.Delete(Config.DirLocal + guid);
                }
            }
            else
            {
                File.Delete(Config.DirLocal + guid);
            }
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
                //MessageBox.Show("FormhhtDDOrderInc ,Deserialize");
                //if (file == null)
                //    file = guid;

                //sc = SerializeClass.Deserialize(Config.DirLocal + file);

                //if (sc != null)
                //{
                //    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                //    ShopNo = sc.Data[(int)DATA_INDEX.ShopNO];
                //    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);

                //    if (ChildID != Form_ID.Null)
                //    {
                //        //ShowChild(true);
                //    }
                //}
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
                    case API_ID.R104601:
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

        private void requestXML()
        {
            try
            {
                string from = "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R104601:
                        {
                            string file = Config.getApiFile("1046", "01");
                            from += "/1046/" + file;
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

            //xmlFile = @"\Program Files\hhtiii\00501.xml";
            //xmlfile = @"\Program Files\hhtiii\00201.xml";

            try
            {
                if (File.Exists(xmlfile))
                {
                    //Start = 0;
                    //Stop = 0;
                    //QueryPerformanceCounter(ref Start);

                    doc = new XmlDocument();
                    doc.Load(xmlfile);
                    reader = new XmlNodeReader(doc);
                    ds = new DataSet();
                    ds.ReadXml(reader);

                    //QueryPerformanceCounter(ref Stop);
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
                            is_hold.Text = dt.Rows[0]["is_hold"].ToString();
                            inv_no.Text = dt.Rows[0]["inv_no"].ToString();
                            //hhtdsp.Text = dt.Rows[0]["hhtdsp"].ToString();
                            start_ap.Text = dt.Rows[0]["start_ap"].ToString();
                            min_package_num.Text = dt.Rows[0]["min_package_num"].ToString();
                            trf_on_the_way.Text = dt.Rows[0]["trf_on_the_way"].ToString();
                            po_on_the_way.Text = dt.Rows[0]["po_on_the_way"].ToString();
                            c_week_sale.Text = dt.Rows[0]["c_week_sale"].ToString();
                            l_week_sale.Text = dt.Rows[0]["l_week_sale"].ToString();
                            three_week_sale.Text = dt.Rows[0]["three_week_sale"].ToString();
                            vtn_code.Text = dt.Rows[0]["vtn_code"].ToString();
                            vtn_type_desc.Text = dt.Rows[0]["vtn_type_desc"].ToString();
                            last_recipe_date.Text = dt.Rows[0]["last_recipe_date"].ToString();
                            if (vtn_type_desc.Text == "DC")
                            {
                                MessageBox.Show("此SKU不可PO下单");
                                button4.Enabled = false;
                                SKUNO.Text = "";
                                chearAllbutSKU();
                                SKUNO.Focus();
                            }
                            else
                            {
                                button4.Enabled = true;
                                textBox1.Focus();
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



        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            //下单清单
            ChildID = Form_ID.OrederQD;
            showChild(false);
        }

        public delegate void mydelegate(DataTable table);//定义一个委托
        //public event mydelegate myevent;//定义上诉委托类型的事件

        public void givetable(DataTable table, List<string> sku) //用于修改DataTable的方法
        {
            //是否提交成功
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
            SKUNO.Text = "";
            chearAllbutSKU();
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
                    case Form_ID.OrederQD:
                        FormHHTOrderQD form = new FormHHTOrderQD(takeDataTable);
                        form.myEvent += new FormHHTOrderQD.mydelegate(givetable);
                        child = form;
                        break;

                    default:
                        return;
                }

                ((ISerializable)child).init(new string[] { ShopNo });
                //if (bDeserialize)
                //{
                //    ((ISerializable)child).Deserialize(null);
                //}
                if (child.ShowDialog() == DialogResult.OK)
                {
                    File.Delete(Config.DirLocal + guid);
                }
                else
                {
                    Serialize(guid);
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

        private void button2_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            SKUNO.Text = "";
            chearAllbutSKU();
            SKUNO.Focus();
        }

        private void chearAllbutSKU()
        {
            GoodsNO.Text = "";
            SKUName.Text = "";
            SDTP.Text = "";
            status.Text = "";
            is_hold.Text = "";
            inv_no.Text = "";
            //hhtdsp.Text = "";
            start_ap.Text = "";
            min_package_num.Text = "";
            trf_on_the_way.Text = "";
            po_on_the_way.Text = "";
            c_week_sale.Text = "";
            l_week_sale.Text = "";
            three_week_sale.Text = "";
            vtn_code.Text = "";
            last_recipe_date.Text = "";
            vtn_type_desc.Text = "";
            textBox1.Text = "";

            SKUNO.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private DataTable RequsetDataTable()
        {
            takeDataTable = new DataTable();
            takeDataTable.TableName = "takeTable";
            takeDataTable.Columns.Add("sku");
            takeDataTable.Columns.Add("sku_dsc");
            takeDataTable.Columns.Add("stk_order_qty");
            return takeDataTable;
        }


        List<string> skuList = new List<string>();
        string delSku = null;
        //添加
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                addbutton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addbutton()
        {
            if (textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("请核对好下单数量");
                return;
            }

            if (takeDataTable != null)
            {
                DataRow row = takeDataTable.NewRow();

                for (int i = 0; i < takeDataTable.Columns.Count; i++)
                {
                    if (takeDataTable.Columns.Contains("sku"))
                    {
                        row["sku"] = SKUNO.Text;
                    }
                    if (takeDataTable.Columns.Contains("sku_dsc"))
                    {
                        row["sku_dsc"] = SKUName.Text;
                    }
                    if (takeDataTable.Columns.Contains("stk_order_qty"))
                    {
                        if (int.Parse(textBox1.Text.ToString()) > 0)
                        {
                            row["stk_order_qty"] = textBox1.Text;
                        }
                        else
                        {
                            MessageBox.Show("请核对好下单数量");
                            return;
                        }
                    }
                }

                if (skuList.Contains(SKUNO.Text))
                {
                    int index1 = 0;
                    for (int i = 0; i < takeDataTable.Rows.Count; i++)
                    {
                        if (takeDataTable.Rows[i]["sku"].ToString() == SKUNO.Text)
                        {
                            delSku = SKUNO.Text;
                            index1 = i;
                        }
                    }
                    if (delSku != null)
                    {
                        if (MessageBox.Show("该笔数据已存在，是否覆盖该笔数据？", "",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            takeDataTable.Rows.Remove(takeDataTable.Rows[index1]);
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    skuList.Add(SKUNO.Text);
                }
                takeDataTable.Rows.Add(row);

                button4.Enabled = false;
                SKUNO.Text = "";
                chearAllbutSKU();
                SKUNO.Focus();
            }
        }

        private void SKUNO_TextChanged(object sender, EventArgs e)
        {
            //GoodsNO.Text = "";
            //SKUName.Text = "";
            //SDTP.Text = "";
            //status.Text = "";
            //is_hold.Text = "";
            //inv_no.Text = "";
            //hhtdsp.Text = "";
            //start_ap.Text = "";
            //min_package_num.Text = "";
            //trf_on_the_way.Text = "";
            //po_on_the_way.Text = "";
            //c_week_sale.Text = "";
            //l_week_sale.Text = "";
            //three_week_sale.Text = "";
            //vtn_code.Text = "";
            //last_recipe_date.Text = "";
            //textBox1.Text = "";

            //button4.Enabled = false;
        }

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
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
                else if (e.KeyValue == 13)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    request01();
                }
                button4.Enabled = false;
                chearAllbutSKU();
                SKUNO.Focus();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!regText.IsMatch(textBox1.Text))
            {
                MessageBox.Show("输入的申请量非法！");
            }

            if (e.KeyValue == 13)
            {
                addbutton();
            }
        }

        private void vtn_code_ParentChanged(object sender, EventArgs e)
        {

        }
    }
}