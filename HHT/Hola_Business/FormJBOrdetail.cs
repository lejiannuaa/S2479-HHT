using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using System.Text.RegularExpressions;
using HolaCore;

namespace Hola_Business
{
    public partial class FormJBOrdetail : Form,ISerializable,ConnCallback
    {
        private string guid = "D73D3CC2-9ADE-430e-B295-608C5303709E";
        private const string OGuid = "00000000000000000000000000000000";
        
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R103601,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        #region 序列化索引
        private enum DATA_INDEX
        {
            SKUNO,
            GoodsNO,
            SDPT,
            SKUName,
            Status,
            AllowOrder,
            InitMoney,
            LeastNO,
            Inventory,
            POInTransit,
            DBInTransit,
            ManuID,
            TWeekSales,
            LWeekSales,
            TMAvg,
            EMAvg,
            LastOrderDate,
            ManuType,
            OrderCycle,
            AdvNo,
            ShopNo,
            DataMax
        }
        #endregion
        private string ShopNo = null;
        private DataSet ds = null;
        private string SKUNO = null;
        private string xmlfile = null;
        private bool busy = false;
        private SerializeClass sc = null;
        Regex reg = new Regex(@"^\d+$");
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        public delegate void GetModNO(int num);
        public GetModNO getMod = null;
        private int TaskBarHeight = 0;
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

        public FormJBOrdetail()
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
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                button1.Top = dstHeight - btnReturn.Height;
                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
                ResumeLayout(false);
            }
            catch (Exception)
            {
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
                            
                            SKUName.Text = dt.Rows[0]["sku_dsc"].ToString();
                            GoodsNO.Text = dt.Rows[0]["huohao"].ToString();
                            InitMoney.Text = dt.Rows[0]["start_ap"].ToString();
                            LeastNO.Text = dt.Rows[0]["min_package_num"].ToString();
                            SDPT.Text = dt.Rows[0]["s_dpt"].ToString();
                            AllowOrder.Text = dt.Rows[0]["is_hold"].ToString();
                            Status.Text = dt.Rows[0]["status"].ToString();
                            LastOrderDate.Text = dt.Rows[0]["last_recipe_date"].ToString();
                            Inventory.Text = dt.Rows[0]["inv_number"].ToString();
                            POInTransit.Text = dt.Rows[0]["po_on_the_way"].ToString();
                            DBInTransit.Text = dt.Rows[0]["trf_on_the_way"].ToString();
                            ManuID.Text = dt.Rows[0]["vtn_code"].ToString();
                            ManuType.Text = dt.Rows[0]["vtn_type_desc"].ToString();
                            TWeekSales.Text = dt.Rows[0]["c_week_sale"].ToString();
                            LWeekSales.Text = dt.Rows[0]["l_week_sale"].ToString();
                            TMAvg.Text = dt.Rows[0]["three_week_sale"].ToString();
                            EMAvg.Text = dt.Rows[0]["eight_week_sale"].ToString();
                            //OrderCycle.Text = dt.Rows[0]["OrderCycle"].ToString();
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
            apiID = API_ID.R103601;
            string msg = "request=1036;usr=" + Config.User + ";op=01;sku=" + SKUNO + ";sto=" + ShopNo;
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
                    case API_ID.R103601:
                        {
                            string file = Config.getApiFile("1036", "01");
                            from += "/1036/" + file;
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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                File.Delete(Config.DirLocal + guid);
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (reg.IsMatch(AdvNo.Text))
                {
                    getMod(int.Parse(AdvNo.Text));
                    File.Delete(Config.DirLocal + guid);
                    Close();
                }
                else
                {
                    MessageBox.Show("请输入非负整数!");
                }
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
                    case API_ID.R103601:
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
            try
            {
                ShopNo = (string)param[0];
                SKUNO = (string)param[1];
                ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
                {
                    this.Invoke(new InvokeDelegate(() =>
                    {
                        request01();

                    }));
                }));
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
                data[(int)DATA_INDEX.SKUNO] = SKUNO;
                data[(int)DATA_INDEX.GoodsNO] = GoodsNO.Text;
                data[(int)DATA_INDEX.InitMoney]=InitMoney.Text;
                data[(int)DATA_INDEX.LeastNO] = LeastNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.SDPT] = SDPT.Text;
                data[(int)DATA_INDEX.AllowOrder] = AllowOrder.Text;
                data[(int)DATA_INDEX.Status] = Status.Text;
                data[(int)DATA_INDEX.LastOrderDate] = LastOrderDate.Text;
                data[(int)DATA_INDEX.Inventory] = Inventory.Text;
                data[(int)DATA_INDEX.POInTransit] = POInTransit.Text;
                data[(int)DATA_INDEX.DBInTransit] = DBInTransit.Text;
                data[(int)DATA_INDEX.TWeekSales] = TWeekSales.Text;
                data[(int)DATA_INDEX.LWeekSales] = LWeekSales.Text;
                data[(int)DATA_INDEX.TMAvg] = TMAvg.Text;
                data[(int)DATA_INDEX.EMAvg] = EMAvg.Text;
                data[(int)DATA_INDEX.ManuID] = ManuID.Text;
                data[(int)DATA_INDEX.ManuType] = ManuType.Text;
                data[(int)DATA_INDEX.OrderCycle] = OrderCycle.Text;
                data[(int)DATA_INDEX.AdvNo] = AdvNo.Text;
                data[(int)DATA_INDEX.ShopNo] = ShopNo;

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
                    SKUNO = sc.Data[(int)DATA_INDEX.SKUNO];
                    GoodsNO.Text = sc.Data[(int)DATA_INDEX.GoodsNO];
                    InitMoney.Text = sc.Data[(int)DATA_INDEX.InitMoney];
                    LeastNO.Text = sc.Data[(int)DATA_INDEX.LeastNO];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    SDPT.Text = sc.Data[(int)DATA_INDEX.SDPT];
                    AllowOrder.Text = sc.Data[(int)DATA_INDEX.AllowOrder];
                    Status.Text = sc.Data[(int)DATA_INDEX.Status];
                    LastOrderDate.Text = sc.Data[(int)DATA_INDEX.LastOrderDate];
                    Inventory.Text = sc.Data[(int)DATA_INDEX.Inventory];
                    POInTransit.Text = sc.Data[(int)DATA_INDEX.POInTransit];
                    DBInTransit.Text = sc.Data[(int)DATA_INDEX.DBInTransit];
                    TWeekSales.Text = sc.Data[(int)DATA_INDEX.TWeekSales];
                    LWeekSales.Text = sc.Data[(int)DATA_INDEX.LWeekSales];
                    TMAvg.Text = sc.Data[(int)DATA_INDEX.TMAvg];
                    EMAvg.Text = sc.Data[(int)DATA_INDEX.EMAvg];
                    ManuID.Text = sc.Data[(int)DATA_INDEX.ManuID];
                    ManuType.Text = sc.Data[(int)DATA_INDEX.ManuType];
                    OrderCycle.Text = sc.Data[(int)DATA_INDEX.OrderCycle];
                    AdvNo.Text = sc.Data[(int)DATA_INDEX.AdvNo];
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}