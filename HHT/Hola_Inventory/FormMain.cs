using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using HolaCore;

namespace Hola_Inventory
{
    public partial class FormMain : Form,ISerializable,ConnCallback
    {
        private const string guid = "DF410473-A010-4e4b-B65A-E5620325D388";
        private Form Child = null;
        private delegate void InvokeDelegate();
        //指示当前正在发送网络请求
        private bool busy = false;
        private FormDownload formDownload = null;

       // [DllImport("iphlpapi.dll")]
        private const string OGuid = "00000000000000000000000000000000";
        private int TaskBarHeight = 0;
        #region 子窗口ID

        private enum Form_ID
        {
            ChuPan,
            FuPanI,
            FuPanII,
            ChouPan,
            JianCe,
            Null,
            PanDian
        }

        #endregion
        private string FPType = null;
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        private API_ID apiID = API_ID.NULL;

        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNo,
            Child,
            DataMax
        }
        #endregion

        #region 请求ID
        private enum API_ID
        {
            NULL,
            API_ID01,
        }
        #endregion


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

        public FormMain()
        {
            InitializeComponent();

            Deserialize(null);
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
                pbBarI.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBarI.Size = new System.Drawing.Size(dstWidth, pbBarI.Image.Height);
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        #region UI响应

        private void FunChoose_AfterSelect(object sender, TreeViewEventArgs e)
        {

            //FunChoose.SelectedNode = null;
            //if (e.Action.ToString() == "ByMouse")
            //{
            //    if (e.Node.Parent == null)
            //    {
            //        if (e.Node.Index == 0)
            //        {
            //            ChildID = Form_ID.ChuPan;
            //            ShowChild(true);
            //        }
            //        else if (e.Node.Index == 1)
            //        {
            //            e.Node.Toggle();
            //        }
            //        else if (e.Node.Index == 2)
            //        {
            //            ChildID = Form_ID.ChouPan;
            //            ShowChild(true);
            //        }
            //        else if (e.Node.Index == 3)
            //        {
            //            File.Delete(Config.DirLocal + guid);
            //            Close();
            //        }
            //        else
            //        {
            //            ChildID = Form_ID.JianCe;
            //            ShowChild(false);
            //        }
            //    }
            //    else
            //    {
            //        if (e.Node.Index == 0)
            //        {
            //            ChildID = Form_ID.FuPanI;
            //            FPType = "0";
            //        }
            //        else
            //        {
            //            ChildID = Form_ID.FuPanII;
            //            FPType = "1";
            //        }
            //        ShowChild(true);
            //    }
            //    Serialize(guid);
            FunChoose.SelectedNode = null;
            if (e.Action.ToString() == "ByMouse")
            {
                if (e.Node.Parent == null)
                {
                    if (e.Node.Index == 0)
                    {
                        ChildID = Form_ID.PanDian;
                        ShowChild(true);
                    }
                    if (e.Node.Index == 1)
                    {
                        ChildID = Form_ID.ChuPan;
                        ShowChild(true);
                    }
                    else if (e.Node.Index == 2)
                    {
                        e.Node.Toggle();
                    }
                    else if (e.Node.Index == 3)
                    {
                        ChildID = Form_ID.ChouPan;
                        ShowChild(true);
                    }
                    else if (e.Node.Index == 4)
                    {
                        File.Delete(Config.DirLocal + guid);
                        Close();
                    }
                    else
                    {
                        //ChildID = Form_ID.JianCe;
                        //ShowChild(false);
                    }
                }
                else
                {
                    if (e.Node.Index == 0)
                    {
                        ChildID = Form_ID.FuPanI;
                        FPType = "0";
                    }
                    else
                    {
                        ChildID = Form_ID.FuPanII;
                        FPType = "1";
                    }
                    ShowChild(true);
                }
                Serialize(guid);
            }
            else
            {
                FunChoose.SelectedNode = null;
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

        private void showMain()
        {
            Show();
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
                    case Form_ID.ChuPan:
                        Child = new FormCP();
                        break;
                    case Form_ID.FuPanI:
                    case Form_ID.FuPanII:
                        Child = new FormFP();
                        break;
                    case Form_ID.ChouPan:
                        Child = new FormCPI();
                        break;
                    case Form_ID.JianCe:
                        Child = new FormHHTScan();
                        break;
                    case Form_ID.PanDian:
                        Child = new FormJBIn();
                        break;
                    default:
                        return;
                } 

                Serialize(guid);

                ((ISerializable)Child).init(new object[] { ShopNO.Text, FPType });
                if (bDeserialize)
                {
                    ((ISerializable)Child).Deserialize(null);
                }

                Child.ShowDialog();
                Show();
                Child.Dispose();
                Child = null;
                ChildID = Form_ID.Null;
                Serialize(guid);
            }
            catch (Exception)
            {
            }
        }

        #endregion 

        #region 接口请求

        private void request01()
        {
            apiID = API_ID.API_ID01;
            string msg = "request=A00;usr="+Config.User+";op=01;hhtIp="+Config.IPLocal;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
        }

        #endregion

        #region 实现回调

        public void progressCallback(int total, int progress)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                formDownload.setProgress(total, progress);
            }));
        }
        public void requestCallback(string data, int result, string date)
        {
            this.Invoke(new InvokeDelegate(() =>
            {
                idle();
                switch (apiID)
                {
                    case API_ID.API_ID01:
                        if (result == ConnThread.RESULT_OK)
                        {
                            ShopNO.Text = data;
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
                data[(int)DATA_INDEX.ShopNo] = ShopNO.Text;
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
                    ShopNO.Text = sc.Data[(int)DATA_INDEX.ShopNo];
                    ChildID = (Form_ID)Enum.Parse(typeof(Form_ID), sc.Data[(int)DATA_INDEX.Child], true);
                    if (ChildID != Form_ID.Null)
                    {
                        ShowChild(true);
                    }
                }
                else
                {
                    request01();
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion



    }
}