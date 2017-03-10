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
    public partial class FormP : Form,ISerializable,ConnCallback
    {
        private const string guid = "07AE8A78-0E59-4f1a-B6F8-88A2AF1524D7";
        private Form Child = null;
        private const string OGuid = "00000000000000000000000000000000";
 
        #region 子窗口ID

        private enum Form_ID
        {
            IBarCode,
            Discount,
            CombinedSKU,
            Null,
        }

        #endregion
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        private DataSet ds = null;
        private string xmlfile = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            SKUNO,
            GoodsNO,
            SKUName,
            PresPrice,
            OrgPrice,
            Unit,
            Model,
            Specifications,
            OrgPlace,
            ProStatus,
            Bvano,
            Child,
            DataMax
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R101101,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormP()
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

                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

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
                GoodsNO.Text = "";
                SKUName.Text = "";
                PresPrice.Text = "";
                OrgPrice.Text = "";
                Unit.Text = "";
                Model.Text = "";
                Specifications.Text = "";
                OrgPlace.Text = "";
                ProStatus.Text = "";
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
                        if (ds.Tables[i].TableName.Equals("info"))
                        {
                               bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {
                        DataTable dt = ds.Tables["info"];
                        if (dt.Rows.Count > 0)
                        {
                            SKUNO.Text = dt.Rows[0]["sku"].ToString();
                            GoodsNO.Text = dt.Rows[0]["huohao"].ToString();
                            SKUName.Text = dt.Rows[0]["sku_dsc"].ToString();
                            PresPrice.Text = dt.Rows[0]["cpr"].ToString();
                            OrgPrice.Text = dt.Rows[0]["rpr"].ToString();
                            Unit.Text = dt.Rows[0]["unit"].ToString();
                            Model.Text = dt.Rows[0]["model"].ToString();
                            Specifications.Text = dt.Rows[0]["style"].ToString();
                            OrgPlace.Text = dt.Rows[0]["origin"].ToString();
                            ProStatus.Text = dt.Rows[0]["status"].ToString();
                            Bvano.Text = dt.Rows[0]["bavNo"].ToString();
                        }
                        if (ds.Tables["detail"] == null)
                        {
                            btn03.Enabled = false;
                        }
                        else
                        {
                            btn03.Enabled = true;
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
            apiID = API_ID.R101101;
            string msg = "request=1011;usr=" + Config.User + ";op=01;sku=" + SKUNO.Text+";sto="+ShopNO.Text;
            msg=OGuid+msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void requestXML()
        {
            try
            {
                string from =  "Http://" + Config.IPServer + "/" + Config.Server.dir + "/" + Config.User;
                string to = Config.DirLocal;

                switch (apiID)
                {
                    case API_ID.R101101:
                        {
                            string file = Config.getApiFile("1011", "01");
                            from += "/1011/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOAD_XML;
                        }
                        break;
                    default:
                        return;
                }
                //MessageBox.Show(from);
                //MessageBox.Show(to);
                new ConnThread(this).Download(from, to, false);

                wait();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region UI响应

        private void btn01_Click(object sender, EventArgs e)
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
                }
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
                    ChildID = Form_ID.IBarCode;
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

                ChildID = Form_ID.CombinedSKU;
                ShowChild(false);
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
                    ChildID = Form_ID.Discount;
                    ShowChild(false);
                }

            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                File.Delete(Config.DirLocal + guid);
                Close();
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
                bool bRequest = false;
                DataTable dtSubSku=null;
                    switch (ChildID)
                    {
                        case Form_ID.IBarCode:
                            Child = new FormPNO();
                            if (bDeserialize)
                            {

                                ((ISerializable)Child).Deserialize(null);
                            }
                            else
                            {
                                ((ISerializable)Child).init(new object[] { ShopNO.Text, SKUNO.Text });
                                Serialize(guid);
                            }
                            break;
                        case Form_ID.CombinedSKU:
                            if (ds!=null && ds.Tables["detail"] != null)
                            {
                                dtSubSku = ds.Tables["detail"].DefaultView.ToTable();
                                Child = new FormPSKU();
                                bRequest = true;
                                ((FormPSKU)Child).GetRowSku += new FormPSKU.getRowSku(getSku);
                                if (bDeserialize)
                                {
                                    ((ISerializable)Child).Deserialize(null);
                                }
                                else
                                {
                                    ((ISerializable)Child).init(new object[] { ShopNO.Text, SKUNO.Text, ds.Tables["info"].Rows[0]["parent_sku"].ToString(), dtSubSku });
                                    Serialize(guid);
                                }
                            }
                            else
                            {
                                MessageBox.Show("没有子商品!");
                            }
                            break;
                        case Form_ID.Discount:
                            Child = new FormPDetail();
                            if (bDeserialize)
                            {

                                ((ISerializable)Child).Deserialize(null);
                            }
                            else
                            {
                                ((ISerializable)Child).init(new object[] { ShopNO.Text, SKUNO.Text });
                                Serialize(guid);
                            }
                            break;
                        default:
                            return;
                    }
                 
                    if (Child != null)
                    {
                        Child.ShowDialog();
                        Show();
                        Child.Dispose();
                        Child = null;
                    }
                    ChildID = Form_ID.Null;
                    Serialize(guid);
                    if (bRequest)
                    {
                        request01();
                    }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getSku(string sku)
        {
            SKUNO.Text = sku;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SKUNO.Text = "";
            GoodsNO.Text = "";
            SKUName.Text = "";
            OrgPrice.Text = "";
            PresPrice.Text = "";
            Unit.Text = "";
            Model.Text = "";
            Specifications.Text = "";
            OrgPlace.Text = "";
            ProStatus.Text = "";
            Bvano.Text = "";
            btn03.Enabled = false;
            SKUNO.Focus();
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

                    btn01.Focus();
                    request01();
                }
                btn03.Enabled = false;
                QC();
            }
        }

        private void QC()
        {
            GoodsNO.Text = "";
            SKUName.Text = "";
            OrgPrice.Text = "";
            PresPrice.Text = "";
            Unit.Text = "";
            Model.Text = "";
            Specifications.Text = "";
            OrgPlace.Text = "";
            ProStatus.Text = "";
            Bvano.Text = "";
            btn03.Enabled = false;
        }

        #endregion

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                //MessageBox.Show("init");
                ShopNO.Text = (string)param[0];
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
                //MessageBox.Show("Serialize");
                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DataMax];
                data[(int)DATA_INDEX.ShopNO] = ShopNO.Text;
                data[(int)DATA_INDEX.SKUNO] = SKUNO.Text;
                data[(int)DATA_INDEX.GoodsNO] = GoodsNO.Text;
                data[(int)DATA_INDEX.SKUName] = SKUName.Text;
                data[(int)DATA_INDEX.PresPrice] = PresPrice.Text;
                data[(int)DATA_INDEX.OrgPrice] = OrgPrice.Text;
                data[(int)DATA_INDEX.Unit] = Unit.Text;
                data[(int)DATA_INDEX.Model] = Model.Text;
                data[(int)DATA_INDEX.Specifications] = Specifications.Text;
                data[(int)DATA_INDEX.OrgPlace] = OrgPlace.Text;
                data[(int)DATA_INDEX.ProStatus] = ProStatus.Text;
                data[(int)DATA_INDEX.Bvano] = Bvano.Text;
                data[(int)DATA_INDEX.Child] = ChildID.ToString();
                sc.Data = data;
                sc.DS = ds;
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
                //MessageBox.Show("Deserialize");
                if (file == null)
                    file = guid;

                sc = SerializeClass.Deserialize(Config.DirLocal + file);

                if (sc != null)
                {
                    ShopNO.Text = sc.Data[(int)DATA_INDEX.ShopNO];
                    SKUNO.Text = sc.Data[(int)DATA_INDEX.SKUNO];
                    GoodsNO.Text = sc.Data[(int)DATA_INDEX.GoodsNO];
                    SKUName.Text = sc.Data[(int)DATA_INDEX.SKUName];
                    PresPrice.Text = sc.Data[(int)DATA_INDEX.PresPrice];
                    OrgPrice.Text = sc.Data[(int)DATA_INDEX.OrgPrice];
                    Unit.Text = sc.Data[(int)DATA_INDEX.Unit];
                    Model.Text = sc.Data[(int)DATA_INDEX.Model];
                    Specifications.Text = sc.Data[(int)DATA_INDEX.Specifications];
                    OrgPlace.Text = sc.Data[(int)DATA_INDEX.OrgPlace];
                    ProStatus.Text = sc.Data[(int)DATA_INDEX.ProStatus];
                    Bvano.Text = sc.Data[(int)DATA_INDEX.Bvano];

                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);

                    ds = sc.DS;
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
                    case API_ID.R101101:
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
                            OrgPrice.Text = "";
                            PresPrice.Text = "";
                            Unit.Text = "";
                            Model.Text = "";
                            Specifications.Text = "";
                            OrgPlace.Text = "";
                            ProStatus.Text = "";
                            Bvano.Text = "";

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

        private void FormP_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }

        }

    }
}