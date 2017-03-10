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
namespace Hola_Business
{
    public partial class FormDJInc : Form,ISerializable,ConnCallback
    {
        private const string guid = "E3B33F7F-2F8F-44e0-85A0-7C6693EB93E9";
        private const string OGuid = "00000000000000000000000000000000";
        private string DataGuid = ""; 
        private Form Child = null;
        private string ShopNo = "";
     
        #region 子窗口ID
        private enum Form_ID
        {
            DJSKUDetail,
            Null,

        }
        #endregion

        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104202,
            DOWNLOAD_XML
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        #region 序列化索引
        private enum DATA_INDEX
        {
            DJId,
            DJName,
            DJDateFrom,
            DJDateTo,
            ShopNo,
            DataGuid,
            Child,
            DataMax
        }
        #endregion
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private int TaskBarHeight = 0;
        public FormDJInc()
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
                btn01.Top = dstHeight - btn01.Height;
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBar.Top = btnReturn.Top - pbBar.Height * 3;
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

        private DataTable getDTOK()
        {
            DataTable dtOK = new DataTable();
            try
            {
                dtOK.TableName = "detail";
                dtOK.Columns.Add("group_desc");
                dtOK.Columns.Add("start_valid_date");
                dtOK.Columns.Add("end_valid_date");

                DataRow rowNew = dtOK.NewRow();
                rowNew["group_desc"] = DJName.Text;
                rowNew["start_valid_date"] = DJDateFrom.Value.ToString("yyyy-MM-dd");
                rowNew["end_valid_date"] = DJDateTo.Value.ToString("yyyy-MM-dd");

                dtOK.Rows.Add(rowNew);
            }
            catch (Exception)
            {
            }

            return dtOK;
        }

        #endregion

        #region 接口请求

        private void request02()
        {
            apiID = API_ID.R104202;
            DataGuid = Guid.NewGuid().ToString().Replace("-", "");
            string json=Config.addJSONConfig(JSONClass.DataTableToString(new DataTable[]{getDTOK()}),"1042","02");
            string msg = "request=1042;usr=" + Config.User + ";op=02;groupId=" + DJId.Text + ";sto=" + ShopNo + ";json="+json;
            msg = DataGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        #endregion

        #region UI响应
        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (File.Exists(Config.DirLocal + guid))
                {
                    //if (MessageBox.Show("检测到未完成数据，是否继续？", "",
                    //    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    //{
                    //    Deserialize(null);
                    //}
                    //else
                    //{
                    //    request02();
                    //}
                    request02();
                }
                else
                {
                    if (DJDateFrom.Value > DJDateTo.Value)
                    {
                        MessageBox.Show("起始日期不可大于截止如期！");
                    }
                    else
                    {
                        request02();
                    }
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
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
                switch (ChildID)
                {
                    case Form_ID.DJSKUDetail:
                        Child = new FormDJSKU();
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
                    ((ISerializable)Child).init(new object[] { ShopNo, DJId.Text, DJName.Text,"true"});
                }

                if (Child.ShowDialog() == DialogResult.OK)
                {
                    File.Delete(Config.DirLocal + guid);
                }
                else
                {
                    Serialize(guid);
                }

                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    case API_ID.R104202:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ChildID = Form_ID.DJSKUDetail;
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

                    default:
                        break;
                }

            }));
        }

        #endregion      

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            ShopNo = (string)param[0];
            DJId.Text = (string)param[1];
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
                data[(int)DATA_INDEX.DJId] = DJId.Text;
                data[(int)DATA_INDEX.DJName] = DJName.Text;
                data[(int)DATA_INDEX.DJDateFrom] = DJDateFrom.Value.ToString();
                data[(int)DATA_INDEX.DJDateTo] = DJDateTo.Value.ToString();
                data[(int)DATA_INDEX.ShopNo]=ShopNo;
                data[(int)DATA_INDEX.DataGuid] = DataGuid;
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
                    DJId.Text = sc.Data[(int)DATA_INDEX.DJId];
                    DJName.Text = sc.Data[(int)DATA_INDEX.DJName];
                    DJDateFrom.Value = DateTime.Parse(sc.Data[(int)DATA_INDEX.DJDateFrom]);
                    DJDateTo.Value = DateTime.Parse(sc.Data[(int)DATA_INDEX.DJDateTo]);
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNo];
                    DataGuid = sc.Data[(int)DATA_INDEX.DataGuid];
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

        private void DJName_GotFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(true);
            }
        }

        private void DJName_LostFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(false);
            }
        }
    }
}