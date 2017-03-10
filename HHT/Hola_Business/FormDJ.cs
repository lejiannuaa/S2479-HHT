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
    
    public partial class FormDJ : Form,ISerializable,ConnCallback
    {
        private const string guid = "3E39ED08-DCB8-4e9e-AD11-E31DF85E084F";
        private const string OGuid = "00000000000000000000000000000000";
        
        private Form Child = null;
        #region 子窗口ID

        private enum Form_ID
        {
            DJIncrease,
            DJInquiry,
            Null,
        }

        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            DJId,
            ShopNo,
            Child,
            DataMax
        }
        #endregion
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        private string ShopNo = null;
        #region 接口请求编号
        public enum API_ID
        {
            NULL = 0,
            R104101,
            //API_ID01
        }
        #endregion
        private API_ID apiID = API_ID.NULL;
        //指示当前正在发送网络请求
        private bool busy = false;
        //用于回调时在UI线程执行的委托
        private delegate void InvokeDelegate();
        private int TaskBarHeight = 0;
        Regex regText = new Regex(@"^[A-Za-z0-9]+$");
        public FormDJ()
        {
            
            InitializeComponent();
            doLayout();
            //获取店号
            //request02();
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

        #region 接口请求

        private void request01()
        {
            apiID = API_ID.R104101;
            string msg = "request=1041;usr=" + Config.User + ";op=01;groupId=" + DJId.Text + ";sto=" + ShopNo;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        //接口请求获取店号
        //private void request02()
        //{
        //    apiID = API_ID.API_ID01;
        //    string msg = "request=A00;usr=" + Config.User + ";op=01;hhtIp=" + Config.IPLocal;
        //    msg = OGuid + msg;
        //    new ConnThread(this).Send(msg);
        //}

        #endregion

        #region UI响应

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (DJId.Text == "")
                {
                    MessageBox.Show("请输入端架号!");
                    return;
                }

                if (!regText.IsMatch(DJId.Text))
                {
                    MessageBox.Show("输入端架号非法！");
                    return;
                }

                if (DJId.Text.Length > 15)
                {
                    MessageBox.Show("端架号不可超过15位！");
                    return;
                }

                request01();
            }
        }

        private void btn02_Click(object sender, EventArgs e)
        {
            if (!busy)
            {
                if (DJId.Text == "")
                {
                    //MessageBox.Show("请输入端架号!");
                    //return;
                }

                if (!regText.IsMatch(DJId.Text))
                {
                    MessageBox.Show("输入端架号非法！");
                    return;
                }

                if (DJId.Text.Length > 15)
                {
                    MessageBox.Show("端架号不可超过15位！");
                    return;
                }

                ChildID = Form_ID.DJInquiry;
                ShowChild(false);
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
                    case Form_ID.DJIncrease:
                        Child = new FormDJInc();
                        break;
                    case Form_ID.DJInquiry:
                        Child = new FormDJInq();
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
                    Serialize(guid);
                    ((ISerializable)Child).init(new object[] {ShopNo,DJId.Text });
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
                MessageBox.Show(ex.Message + " 1");
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
                    case API_ID.R104101:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ChildID = Form_ID.DJIncrease;
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
                    DJId.Text = sc.Data[(int)DATA_INDEX.DJId];
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

        private void DJId_GotFocus(object sender, EventArgs e)
        {
            FullscreenClass.ShowSIP(true);
        }

        private void DJId_TextChanged(object sender, EventArgs e)
        {
            if (DJId.Text != null)
            {
                DJId.Text = DJId.Text.ToString().Trim();
            }
        }
       
    }
}