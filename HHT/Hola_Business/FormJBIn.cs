using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HolaCore;
using System.Text.RegularExpressions;
namespace Hola_Business
{
    public partial class FormJBIn : Form,ISerializable,ConnCallback
    {
        private const string guid = "8E2A1C7B-93E6-4a7c-B6F0-14C5AD805904";
         private const string OGuid = "00000000000000000000000000000000";
        
        private string ShopNo = null;
        private Form Child = null;
        #region 子窗口ID

        private enum Form_ID
        {
            InDetail,
            InSKU,
            Null,
        }
        #endregion
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            CBNO,
            ShopNo,
            Child,
            DataMax
        }
        #endregion
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R103101,
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private int TaskBarHeight = 0;
        //指示当前正在发送网络请求
        private bool busy = false;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormJBIn()
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
                pbBarI.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                pbBarI.Top = btnReturn.Top - pbBarI.Height * 3;
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

        #region 接口请求

        private void request01()
        {
            apiID = API_ID.R103101;
            string msg = "request=1031;usr=" + Config.User + ";op=01;loc_no=" + CBNO.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        #endregion

        #region UI响应

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if(CBNO.Text=="")
                {
                    MessageBox.Show("请输入柜号!");
                    return;
                }

                if (!regText.IsMatch(CBNO.Text))
                {
                    MessageBox.Show("输入柜号非法！");
                    return;
                }

                //if (File.Exists(Config.DirLocal + guid))
                //{
                //    if (MessageBox.Show("检测到未完成数据，是否继续？", "",
                //        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                //    {
                //        Deserialize(null);
                //        return;
                //    }
                //}

                request01();
                //ChildID = Form_ID.InDetail;
                //ShowChild(false);
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (CBNO.Text.Length == 0)
                {
                    MessageBox.Show("请输入柜号！");
                    return;
                }

                if (!regText.IsMatch(CBNO.Text))
                {
                    MessageBox.Show("输入柜号非法！");
                    return;
                }

                //if (File.Exists(Config.DirLocal + guid))
                //{
                //    if (MessageBox.Show("检测到未完成数据，是否继续？", "",
                //        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                //    {
                //        Deserialize(null);
                //        return;
                //    }
                //    else
                //    {
                //        File.Delete(Config.DirLocal + guid);
                //        MessageBox.Show("1");
                //    }
                //}

                ChildID = Form_ID.InSKU;
                ShowChild(false);
                
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
                switch (ChildID)
                {
                    case Form_ID.InDetail:
                        Child = new FormJBInDetail();
                        break;
                    case Form_ID.InSKU:
                        Child = new FormJBInSKU();
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
                    ((ISerializable)Child).init(new object[] { ShopNo, CBNO.Text,"false"});
                }

                if (Child.ShowDialog() == DialogResult.OK)
                {
                    File.Delete(Config.DirLocal + guid);
                }
                else
                {
                    Serialize(guid);
                }

                Show();
                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 实现ISerializable接口
        public void init(object[] param)
        {
            try
            {
                ShopNo = (string)param[0];
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
                data[(int)DATA_INDEX.CBNO] = CBNO.Text;
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
                    CBNO.Text = sc.Data[(int)DATA_INDEX.CBNO];
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
                    case API_ID.R103101:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ChildID = Form_ID.InDetail;
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

        private void CBNO_GotFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(true);
            }
        }

        private void CBNO_LostFocus(object sender, EventArgs e)
        {
            if (!busy)
            {
                FullscreenClass.ShowSIP(false);
            }
        }
    }
}