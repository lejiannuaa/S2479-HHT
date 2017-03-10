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
using HolaCore;
namespace Hola_Inventory
{
    public partial class FormFP : Form,ISerializable,ConnCallback
    {


        private const string guid = "92AED076-52BB-44c2-899D-B0101C37D43D";
        private const string OGuid = "00000000000000000000000000000000";
        private Form Child = null;
        private DataSet ds = null;
        private string xmlfile = null;//Config.DirLocal + "70001.xml";
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        #region 子窗口ID

        private enum Form_ID
        {
            FPSKU,
            Null,
        }

        #endregion
        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            FPCode,
            CurrCBNO,
            EntCBNO,
            PageIndex,
            FPType,
            Child,
            DataMax,
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R60001,
            R60002,
            R60003,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        public enum FILE_ID
        {
            NULL = 0,
            O1,
            O2
        }
        private FILE_ID fileid = FILE_ID.NULL;
        private SerializeClass sc = null;
        private Form_ID ChildID = Form_ID.Null;
        private int pageIndex = 1;
        private int TABLE_ROWMAX = 5;
        private string FPType = "";
        private int TaskBarHeight = 0;
        public FormFP()
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
            int xOffSet = (int)(label1.Location.X * xTime);
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                if (dstHeight > srcHeight)
                {
                    Ent_Agn.Height += Ent_Agn.Top - FPCode.Bottom - 15;
                    btnConfirm.Top += 40;
                    btnReturn.Top += 40;
                }

                Ent_Agn.Width = dstWidth;
                Ent_Agn.Top = dstHeight - Ent_Agn.Height;
                dgTable.Width = dstWidth;
                PrePage.Top = dgTable.Top + dgTable.Height + 8;
                NexPage.Top = dgTable.Top + dgTable.Height + 8;
                Page.Top = dgTable.Top + dgTable.Height + 8;
                NexPage.Left = dstWidth - PrePage.Left - NexPage.Width;
                Page.Left = (int)(dstWidth / 2.0) - (int)(Page.Width / 2.0);
                btnReturn.Left = dstWidth - btnConfirm.Left - btnReturn.Width;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

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

        #region XML加载与显示

        private void UpdateDs(DataTable dt)
        {
            try
            {
                bool bTableAdd = false;
                DataTable dtt = dt.Copy();
                if (ds == null)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dtt);
                }
                else
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        if (dt.TableName.Equals(ds.Tables[i].TableName))
                        {
                            ds.Tables.RemoveAt(i);
                            ds.Tables.Add(dtt);
                            bTableAdd = true;
                            break;
                        }
                    }
                    if (!bTableAdd)
                    {
                        ds.Tables.Add(dtt);
                    }
                }
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

            try
            {
                if (File.Exists(xmlfile))
                {
                    doc = new XmlDocument();
                    doc.Load(xmlfile);
                    reader = new XmlNodeReader(doc);
                    DataSet DS = new DataSet();
                    DS.ReadXml(reader);

                    for (int i = 0; i < DS.Tables.Count; i++)
                    {
                        if (DS.Tables[i].TableName.Equals("detail"))
                        {
                            switch (fileid)
                            {
                                case FILE_ID.O1:
                                    DS.Tables[i].TableName = "stkno";
                                    break;
                                case FILE_ID.O2:
                                    DS.Tables[i].TableName = "locno";
                                    break;
                                default:
                                    break;
                            }
                            UpdateDs(DS.Tables[i]);
                            bNoData = false;
                        }
                    }
                    if (!bNoData)
                    {

                        if (fileid == FILE_ID.O2)
                        {
                            UpdateGrid();
                            int pageCount = (int)Math.Ceiling(ds.Tables["locno"].Rows.Count / (double)TABLE_ROWMAX);
                            if (pageCount == 0)
                                Page.Text = "1/1";
                            else
                                Page.Text = "1/" + pageCount.ToString();
                        }
                        else
                        {
                            btnFpCode.Visible = false;
                            for (int i = 0; i < ds.Tables["stkno"].Rows.Count; i++)
                            {
                                string rowValue = ds.Tables["stkno"].Rows[i][0].ToString();
                                FPCode.Items.Add(rowValue);
                            }
                        }
                    }
                    else
                    {
                        if (fileid == FILE_ID.O2)
                        {
                            dgTable.DataSource = null;
                            pageIndex = 1;
                            Page.Text = "1/1";
                        }
                        MessageBox.Show("无数据！");
                    }
                }
                else
                {
                    if (fileid == FILE_ID.O2)
                    {
                        dgTable.DataSource = null;
                        pageIndex = 1;
                        Page.Text = "1/1";
                    }
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
            }
        }

        private void UpdateGrid()
        {
            try
            {
                DataTable dt = ds.Tables["locno"];
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("柜号");
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
                    rowNew["柜号"] = dt.Rows[i][0];
                    dtResult.Rows.Add(rowNew);
                }

                dgTable.DataSource = dtResult;
                int pageCount = (int)Math.Ceiling(ds.Tables["locno"].Rows.Count / (double)TABLE_ROWMAX);
                if (pageCount == 0)
                {
                    pageCount = 1;
                }
                Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region 接口请求
        private void request01()
        {
            apiID = API_ID.R60001;
            string msg = "request=600;usr=" + Config.User + ";op=01;sto=" + ShopNO.Text;// +";type=" + FPType;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request02()
        {
            apiID = API_ID.R60002;
            string msg = "request=600;usr=" + Config.User + ";op=02;sto=" + ShopNO.Text + ";stk_no=" + FPCode.Text + ";type=" + FPType;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        private void request03()
        {
            apiID = API_ID.R60003;
            string msg = "request=600;usr=" + Config.User + ";op=03;stk_no=" + FPCode.Text + ";sto=" + ShopNO.Text + ";loc_no=" + CurrCBNO.Text+";type="+FPType;
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
                    case API_ID.R60001:
                        {
                            string file = Config.getApiFile("600", "01");
                            from += "/600/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOAD_XML;
                            fileid = FILE_ID.O1;
                        }
                        break;

                    case API_ID.R60002:
                        {
                            string file = Config.getApiFile("600", "02");
                            from += "/600/" + file;
                            to += file;
                            xmlfile = to;
                            apiID = API_ID.DOWNLOAD_XML;
                            fileid = FILE_ID.O2;
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

        private void Ent_Agn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!busy)
                {
                    if (Ent_Agn.SelectedIndex == 0)
                    {
                        FPCode.Enabled = true;
                        DataTable dtt = (DataTable)dgTable.DataSource;
                        if (dtt != null && dtt.Rows.Count > 0)
                        {
                            CurrCBNO.Text = dtt.Rows[dgTable.CurrentRowIndex][0].ToString();
                        }
                    }
                    else
                    {
                        if (btnFpCode.Visible && FPCode.SelectedValue==null)
                        {
                            MessageBox.Show("请选择盘点代号");
                            Ent_Agn.SelectedIndex = 0;
                        }
                        else
                        {
                            FPCode.Enabled = false;
                            DataTable dtt = (DataTable)dgTable.DataSource;
                            if (dtt == null)
                            {
                                if (FPCode.SelectedIndex >= 0)
                                {
                                    request02();
                                }
                            }
                            else
                            {
                                UpdateGrid();
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

        private void FPCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgTable.DataSource = null;
                if (Ent_Agn.SelectedIndex == 1)
                {
                    request02();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgTable_CurrentCellChanged(object sender, EventArgs e)
        {
            int rowNo = dgTable.CurrentRowIndex;
            dgTable.Select(rowNo);
        }

        private void btnFpCode_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (File.Exists(Config.DirLocal + guid))
                {
                    if (MessageBox.Show("检测到未完成数据，是否继续上一次操作？", "",
                      MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        btnFpCode.Visible = false;
                        Deserialize(null);
                    }
                }
                else
                {
                    request01();
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (ChildID == Form_ID.FPSKU)
                {

                }
                else
                {
                    File.Delete(Config.DirLocal + guid);
                }
                Close();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (CurrCBNO.Text != "" && CurrCBNO.Text.Equals(EntCBNO.Text))
                {
                    if (File.Exists(Config.DirLocal + guid))
                    {
                        Deserialize(null);
                    }
                    else
                    {
                        request03();
                    }
                }
                else
                {
                    MessageBox.Show("请检查柜号!");
                }
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
                    case Form_ID.FPSKU:
                        Child = new FormFPSKU();
                        break;

                    default:
                        return;
                }

                Serialize(guid);

                ((ISerializable)Child).init(new object[] { ShopNO.Text, FPCode.Text, EntCBNO.Text, FPType });
                if (bDeserialize)
                {
                    ((ISerializable)Child).Deserialize(null);
                }

                ((FormFPSKU)Child).OpLoc += new FormFPSKU.OpLocno(OpGrig);
                DialogResult dr = Child.ShowDialog();
                if (dr == DialogResult.OK || dr == DialogResult.Abort)
                {
                    ChildID = Form_ID.Null;
                    FPCode.Enabled = true;
                    CurrCBNO.Text = "";
                    EntCBNO.Enabled = true;
                    EntCBNO.Text = "";
                    File.Delete(Config.DirLocal + guid);
                }

                Show();
                Child.Dispose();
                Child = null;
            }
            catch (Exception)
            {
            }
        }

        private void OpGrig(string locno)
        {
            try
            {
                DataTable dt = ds.Tables["locno"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString().Equals(locno))
                    {
                        dt.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void EntCBNO_GotFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(true);
            }
        }


        private void EntCBNO_LostFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(false);
            }
        }

        private void EntCBNO_TextChanged(object sender, EventArgs e)
        {
            int start = EntCBNO.SelectionStart;
            EntCBNO.Text = EntCBNO.Text.Trim();
            EntCBNO.SelectionStart = start;
        }

        #endregion

        #region 翻页

        private void PrePage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && dgTable.DataSource != null)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["locno"].Rows.Count / (double)TABLE_ROWMAX);

                    pageIndex--;
                    if (pageIndex >= 1)
                    {
                        UpdateGrid();
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex++;
                        MessageBox.Show("已是首页!");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void NexPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!busy && dgTable.DataSource != null)
                {
                    int pageCount = (int)Math.Ceiling(ds.Tables["locno"].Rows.Count / (double)TABLE_ROWMAX);

                    pageIndex++;
                    if (pageIndex <= pageCount)
                    {
                        UpdateGrid();
                        Page.Text = pageIndex.ToString() + "/" + pageCount.ToString();
                    }
                    else
                    {
                        pageIndex--;
                        MessageBox.Show("已是最后一页!");
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
            this.Invoke(new InvokeDelegate(() =>
            {
                //formDownload.setProgress(total, progress);
            }));
        }
        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.R60001:
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

                    case API_ID.R60002:
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

                    case API_ID.R60003:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ChildID = Form_ID.FPSKU;
                           
                            ShowChild(false);
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
            ShopNO.Text = (string)param[0];
            FPType = (string)param[1];
            //ThreadPool.QueueUserWorkItem(new WaitCallback((stateInfo) =>
            //{
            //    this.Invoke(new InvokeDelegate(() =>
            //    {
            //        request01();
            //    }));
            //}));
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
                data[(int)DATA_INDEX.ShopNO] = ShopNO.Text;
                data[(int)DATA_INDEX.FPCode] = FPCode.SelectedIndex.ToString();
                data[(int)DATA_INDEX.FPType] = FPType;
                data[(int)DATA_INDEX.CurrCBNO] = CurrCBNO.Text;
                data[(int)DATA_INDEX.EntCBNO] = EntCBNO.Text;
                data[(int)DATA_INDEX.PageIndex] = pageIndex.ToString();
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
                if (file == null)
                    file = guid;

                sc = SerializeClass.Deserialize(Config.DirLocal + file);

                if (sc != null)
                {
                    ShopNO.Text = sc.Data[(int)DATA_INDEX.ShopNO];
                    CurrCBNO.Text = sc.Data[(int)DATA_INDEX.CurrCBNO];
                    EntCBNO.Text = sc.Data[(int)DATA_INDEX.EntCBNO];
                    pageIndex = int.Parse(sc.Data[(int)DATA_INDEX.PageIndex]);
                    FPType = sc.Data[(int)DATA_INDEX.FPType];
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);

                    ds = sc.DS;
                    if (ds != null)
                    {
                        DataTable dts = ds.Tables["stkno"];
                        DataTable dtl = ds.Tables["locno"];
                        if (dts != null&& dts.Rows.Count>0)
                        {
                            btnFpCode.Visible = false;
                            for (int i = 0; i < dts.Rows.Count; i++)
                            {
                                string RowValue = dts.Rows[i][0].ToString();
                                FPCode.Items.Add(RowValue);
                            }
                            if (int.Parse(sc.Data[(int)DATA_INDEX.FPCode]) <= FPCode.Items.Count)
                            {
                                FPCode.SelectedIndex = int.Parse(sc.Data[(int)DATA_INDEX.FPCode]);
                            }
                            btnFpCode.Visible = false;
                        }

                        if (dtl != null && dtl.Rows.Count>0)
                        {
                            UpdateGrid();
                        }
                    }
                    if (ChildID != Form_ID.Null)
                    {
                        btnFpCode.Visible = false;
                        FPCode.Enabled = false;
                        EntCBNO.Enabled = false;

                        this.Invoke(new EventHandler(delegate
                        {
                            ShowChild(true);
                        }));
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion     
    }
}