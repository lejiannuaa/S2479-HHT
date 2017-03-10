using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using HolaCore;
using System.Text.RegularExpressions;
namespace Hola_Business
{
    public partial class FormInv : Form,ISerializable,ConnCallback
    {
        private const string guid = "86D22F5F-A740-414d-92AE-912271A13435";
         private const string OGuid = "00000000000000000000000000000000";
        
        private Form Child = null;
        private string ShopNo = null;
        #region 子窗口ID

        private enum Form_ID
        {
            InventoryDB,
            InventoryPO,
            InventoryTD,
            Null,

        }

        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            SKUNO,
            GoodsNO,
            SKUName,
            SDPT,
            PrePrice,
            OrgPrice,
            AllowOrder,
            Status,
            LastOrderDate,
            Inventory,
            POInTransit,
            DBInTransit,
            DBNeed,
            TWeekSales,
            LWeekSales,
            TMAvg,
            EMAvg,
            Items,
            Bvano,
            ShopNo,
            Child,
            DataMax
        }
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R102101,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        private DataSet ds = null;
        private string xmlfile = null;
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormInv()
        {
            InitializeComponent();

            doLayout();
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
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }
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

        private void ClearBox()
        {
            try
            {
                SKUNO.Text = "";
                GoodsNO.Text = "";
                SKUName.Text = "";
                SDPT.Text = "";
                PrePrice.Text = "";
                OrgPrice.Text = "";
                AllowOrder.Text = "";
                Status.Text = "";
                LastOrderDate.Text = "";
                Inventory.Text = "";
                POInTransit.Text = "";
                DBInTransit.Text = "";
                DBNeed.Text = "";
                TWeekSales.Text = "";
                LWeekSales.Text = "";
                TMAvg.Text = "";
                EMAvg.Text = "";
                Items.Text = "";
                Bvano.Text = "";
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #region XML加载与显示

        private void LoadXML()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bNoData = true;
            XmlDocument doc = null;
            XmlNodeReader reader = null;

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
                            SKUName.Text = dt.Rows[0]["sku_dsc"].ToString();
                            GoodsNO.Text = dt.Rows[0]["huohao"].ToString();
                            PrePrice.Text = dt.Rows[0]["cpr"].ToString();
                            OrgPrice.Text = dt.Rows[0]["rpr"].ToString();
                            SDPT.Text = dt.Rows[0]["s_dpt"].ToString();
                            AllowOrder.Text = dt.Rows[0]["is_hold"].ToString();
                            Status.Text = dt.Rows[0]["status"].ToString();
                            LastOrderDate.Text = dt.Rows[0]["last_recipe_date"].ToString();
                            Inventory.Text = dt.Rows[0]["inv_number"].ToString();
                            POInTransit.Text = dt.Rows[0]["po_on_the_way"].ToString();
                            DBInTransit.Text = dt.Rows[0]["trf_on_the_way"].ToString();
                            DBNeed.Text = dt.Rows[0]["trf_request_qty"].ToString();
                            TWeekSales.Text = dt.Rows[0]["c_week_sale"].ToString();
                            LWeekSales.Text = dt.Rows[0]["l_week_sale"].ToString();
                            TMAvg.Text = dt.Rows[0]["three_week_sale"].ToString();
                            EMAvg.Text = dt.Rows[0]["eight_week_sale"].ToString();
                            Items.Text = dt.Rows[0]["show_qty"].ToString();
                            Bvano.Text = dt.Rows[0]["bavNo"].ToString();
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

        #region 接口请求

        private void request01()
        {
            //ClearBox();
            apiID = API_ID.R102101;
            string msg = "request=1021;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text + ";sto=" + ShopNo;
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
                    case API_ID.R102101:
                        {
                            string file = Config.getApiFile("1021", "01");
                            from += "/1021/" + file;
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

        #region UI响应

        private void SKUNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    //MessageBox.Show("请输入SKU！");
                    return;
                }

                if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                    return;
                }

                if (e.KeyValue == 13)
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    btn01.Focus();
                    request01();
                }

                QC();
            }
        }

        private void QC()
        {
            GoodsNO.Text = "";
            SKUName.Text = "";
            SDPT.Text = "";
            PrePrice.Text = "";
            OrgPrice.Text = "";
            AllowOrder.Text = "";
            Status.Text = "";
            LastOrderDate.Text = "";
            Inventory.Text = "";
            POInTransit.Text = "";
            DBInTransit.Text = "";
            DBNeed.Text = "";
            TWeekSales.Text = "";
            LWeekSales.Text = "";
            TMAvg.Text = "";
            EMAvg.Text = "";
            Items.Text = "";
            Bvano.Text = "";
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
            }
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
                    case Form_ID.InventoryDB:
                        Child = new FormInvDB();
                        break;
                    case Form_ID.InventoryPO:
                        Child = new FormInvPO();
                        break;
                    case Form_ID.InventoryTD:
                        Child = new FormInvTD();
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
                    ((ISerializable)Child).init(new object[] {ShopNo, SKUNO.Text,SKUName.Text});
                    Serialize(guid);
                }
                Child.ShowDialog();
                Show();
                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
                Serialize(guid);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                    return;
                }

                if (!regText.IsMatch(SKUNO.Text))
                {
                    MessageBox.Show("输入SKU非法！");
                    return;
                }

                SKUNO.Text = AddZero(SKUNO.Text);
                request01();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    ChildID = Form_ID.InventoryPO;
                    ShowChild(false);
                }
                
            }
        }

        private void btn03_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    ChildID = Form_ID.InventoryDB;
                    ShowChild(false);
                }

            }
        }

        private void btn04_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (SKUNO.Text == "")
                {
                    MessageBox.Show("请输入SKU！");
                }
                else
                {
                    SKUNO.Text = AddZero(SKUNO.Text);
                    ChildID = Form_ID.InventoryTD;
                    ShowChild(false);
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                ClearBox();
                SKUNO.Focus();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                Close();
                File.Delete(Config.DirLocal + guid);
            }
        }
        #endregion

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                ShopNo = (string)param[0];
                Serialize(guid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.GoodsNO] = GoodsNO.Text;
                data[(int)DATA_INDEX.SKUName]=SKUName.Text;
                data[(int)DATA_INDEX.SDPT] = SDPT.Text;
                data[(int)DATA_INDEX.PrePrice] = PrePrice.Text;
                data[(int)DATA_INDEX.OrgPrice] = OrgPrice.Text;
                data[(int)DATA_INDEX.AllowOrder] = AllowOrder.Text;
                data[(int)DATA_INDEX.Status] = Status.Text;
                data[(int)DATA_INDEX.LastOrderDate] = LastOrderDate.Text;
                data[(int)DATA_INDEX.Inventory] = Inventory.Text;
                data[(int)DATA_INDEX.POInTransit] = POInTransit.Text;
                data[(int)DATA_INDEX.DBInTransit] = DBInTransit.Text;
                data[(int)DATA_INDEX.DBNeed] = DBNeed.Text;
                data[(int)DATA_INDEX.TWeekSales] = TWeekSales.Text;
                data[(int)DATA_INDEX.LWeekSales] = LWeekSales.Text;
                data[(int)DATA_INDEX.TMAvg] = TMAvg.Text;
                data[(int)DATA_INDEX.EMAvg] = EMAvg.Text;
                data[(int)DATA_INDEX.Items] = Items.Text;
                data[(int)DATA_INDEX.Bvano] = Bvano.Text;
                data[(int)DATA_INDEX.ShopNo] = ShopNo;
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
                if (file == null)
                    file = guid;

                sc = SerializeClass.Deserialize(Config.DirLocal + file);

                if (sc != null)
                {
                    SKUNO.Text=sc.Data[(int)DATA_INDEX.SKUNO];
                    GoodsNO.Text= sc.Data[(int)DATA_INDEX.GoodsNO];
                    SKUName.Text= sc.Data[(int)DATA_INDEX.SKUName];
                    SDPT.Text=sc.Data[(int)DATA_INDEX.SDPT];
                    PrePrice.Text=sc.Data[(int)DATA_INDEX.PrePrice];
                    OrgPrice.Text=sc.Data[(int)DATA_INDEX.OrgPrice];
                    AllowOrder.Text=sc.Data[(int)DATA_INDEX.AllowOrder];
                    Status.Text=sc.Data[(int)DATA_INDEX.Status];
                    LastOrderDate.Text=sc.Data[(int)DATA_INDEX.LastOrderDate];
                    Inventory.Text=sc.Data[(int)DATA_INDEX.Inventory];
                    POInTransit.Text=sc.Data[(int)DATA_INDEX.POInTransit];
                    DBInTransit.Text=sc.Data[(int)DATA_INDEX.DBInTransit];
                    DBNeed.Text=sc.Data[(int)DATA_INDEX.DBNeed];
                    TWeekSales.Text=sc.Data[(int)DATA_INDEX.TWeekSales];
                    LWeekSales.Text=sc.Data[(int)DATA_INDEX.LWeekSales];
                    TMAvg.Text=sc.Data[(int)DATA_INDEX.TMAvg];
                    EMAvg.Text=sc.Data[(int)DATA_INDEX.EMAvg];
                    Items.Text= sc.Data[(int)DATA_INDEX.Items];
                    Bvano.Text=sc.Data[(int)DATA_INDEX.Bvano];
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);

                    if (ChildID != Form_ID.Null)
                    {

                        ShowChild(true);
                    }
                }
            }
            catch (Exception)
            {
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
                    case API_ID.R102101:
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
                            QC();
                            btnDelete.Focus();
                        }
                        break;

                    case API_ID.DOWNLOAD_XML:
                        if (result == ConnThread.RESULT_FILE)
                        {
                            LoadXML();
                            btnDelete.Focus();
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