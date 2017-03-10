using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using HolaCore;

namespace Hola_Business
{
    public partial class FormMain : Form,ISerializable,ConnCallback
    {
        private const string guid = "9486AD66-E38F-42ab-931F-FD0836179AB1";
        private const string OGuid = "00000000000000000000000000000000";
        private Form Child = null;
        //指示当前正在发送网络请求
        private bool busy = false;
        #region 子窗口ID

        private enum Form_ID
        {
            Price,
            Inventory,
            PriceDetail,
            InventoryTD,
            JBMain,
            SideFrame,

            DDOrderInc,
            DDOrdeCX,
            OrderInc,
            OrderCX,
            InventoryInc,
            InventoryQD,
            //查询
            InventoryInquiry,
            //查询详细
            InventoryCX,

            Null,
        }

        #endregion
        private delegate void InvokeDelegate();
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
        //private string Shopsto = "13105";
        private API_ID apiID = API_ID.NULL;
        private Form_ID ChildID = Form_ID.Null;
        private SerializeClass sc = null;
        [DllImport("cellcore.dll", SetLastError = true)]
        private static extern int ConnMgrConnectionStatus(IntPtr hConnection, out long pdwStatus);
        private int TaskBarHeight = 0;
        public FormMain()
        {
            InitializeComponent();

            Deserialize(null);
            doLayout();
            //ShopNO.Text = Shopsto;
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
                pbBarI.Top = btnReturn.Top - pbBar.Height * 3;
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
            apiID = API_ID.API_ID01;
            string msg = "request=A00;usr=" + Config.User + ";op=01;hhtIp=" + Config.IPLocal;
            msg = OGuid + msg;
            new ConnThread(this).Send(msg);
            wait();
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

        private void FunChoose_AfterSelect(object sender, TreeViewEventArgs e)
        {
             FunChoose.SelectedNode = null;
             if (e.Action.ToString() == "ByMouse")
             {
                 
                 if (e.Node.Parent == null)
                 {
                     if (e.Node.Index == 0)
                     {
                         ChildID = Form_ID.Price;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 1)
                     {
                         ChildID = Form_ID.Inventory;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 2)
                     {
                         ChildID = Form_ID.PriceDetail;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 3)
                     {
                         ChildID = Form_ID.InventoryTD;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 4)
                     {
                         ChildID = Form_ID.JBMain;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 5)
                     {
                         ChildID = Form_ID.SideFrame;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 6)
                     {
                         e.Node.Toggle();
                     }
                     else if (e.Node.Index == 11)
                     {
                         ChildID = Form_ID.InventoryInc;
                         ShowChild(false);
                     }
                     else
                     {
                         ;
                     }
                 }
                 else if (e.Node.Parent.Nodes.Count == 4)
                 {
                     if (e.Node.Index == 0)
                     {
                         ChildID = Form_ID.DDOrderInc;
                         ShowChild(false);
                     }
                     else if(e.Node.Index == 1)
                     {
                         ChildID = Form_ID.DDOrdeCX;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 2)
                     {
                         ChildID = Form_ID.OrderInc;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 3)
                     {
                         ChildID = Form_ID.OrderCX;
                         ShowChild(false);
                     }

                     //ChildID = Form_ID.JBMain;
                     //ShowChild(false);
                 }
                 else
                 {
                     if (e.Node.Index == 0)
                     {
                         ChildID = Form_ID.InventoryInc;
                         ShowChild(false);
                     }
                     else if (e.Node.Index == 1)
                     {
                         ChildID = Form_ID.InventoryInquiry;
                         ShowChild(false);
                     }
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
                    case Form_ID.Price:
                        Child = new FormP();
                        break;
                    case Form_ID.Inventory:
                        Child = new FormInv();
                        break;
                    case Form_ID.PriceDetail:
                        Child = new FormPDetail();
                        break;
                    case Form_ID.InventoryTD:
                        Child = new FormInvTD();
                        break;
                    case Form_ID.JBMain:
                        Child = new FormJBMain();
                        break;
                    case Form_ID.SideFrame:
                        Child = new FormDJ();
                        break;

                    case Form_ID.DDOrderInc:
                        Child = new FormhhtDDOrderInc();
                        break;
                    case Form_ID.DDOrdeCX:
                        Child = new FormhhtDDOrderCX();
                        break;
                    case Form_ID.OrderInc:
                        Child = new FormHHTOrderInc();
                        break;
                    case Form_ID.OrderCX:
                        Child = new FormHHTOrderCX();
                        break;

                    case Form_ID.InventoryInc:
                        Child = new FormInventoryInc();
                        break;
                    case Form_ID.InventoryInquiry:
                        Child = new FormInventoryInquiry();
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

                    ((ISerializable)Child).init(new object[] { ShopNO.Text, "", "" });
                    Serialize(guid);
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

        #region 实现回调

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
            //MessageBox.Show("init,formmain");
        }

        public void Serialize(string file)
        {
            try
            {
                //MessageBox.Show("Serialize,formmain");
                if (sc == null)
                {
                    sc = new SerializeClass();
                }

                string[] data = new string[(int)DATA_INDEX.DataMax];
                data[(int)DATA_INDEX.ShopNo] = ShopNO.Text;
                data[(int)DATA_INDEX.Child] = ChildID.ToString();

                sc.Data = data;
                // sc对象序列化为XML 文档写入文件。
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
                //MessageBox.Show("Deserialize,formmain");
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