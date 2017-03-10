using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HolaCore;
namespace Hola_Business
{
    public partial class FormJBMain : Form, ISerializable
    {
        private const string guid = "25D52F2F-4D55-4831-80D3-84B16EE1A2F1";
        private int TaskBarHeight = 0;
        private SerializeClass sc = null;
        private Form child = null;
        private string ShopNo = "";
        #region Form类型ID
        private enum FORM_ID
        {
            NONE,
            JBIn,
            JBScan,
            JBScanI,
            JBOrder,
            JBOrderI,
        }

        private FORM_ID formID = FORM_ID.NONE;
        #endregion

        # region btnID

        private enum BTN_ID
        {
            NONE,
            btnIn,
            btnScan,
            btnOrder,
        }

        private BTN_ID btnId = BTN_ID.NONE;

        #endregion

        #region 序列化索引
        private enum DATA_INDEX
        {
            ShopNO,
            CHILD,
            DATAMAX
        }
        #endregion
        public FormJBMain()
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
                
                btnJBIn.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("JBIn")));
                btnJBScan.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("JBScan")));
                btnJBOrder.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("JBOrder")));
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

        #region UI响应

        private void ShowMenu(object sender)
        {
            try
            {
                Control btn = (Control)sender;
                contextMenu1.Show(btn, new Point(btn.Width / 2, btn.Height / 2));
            }
            catch (Exception)
            {
            }
        }

        private void btnJBIn_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.JBIn;
            showChild(false);
        }

        private void btnJBScan_Click(object sender, EventArgs e)
        {
            btnId = BTN_ID.btnScan;
            ShowMenu(sender);
        }

        private void btnJBOrder_Click(object sender, EventArgs e)
        {
            btnId = BTN_ID.btnOrder;
            ShowMenu(sender);
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            switch (btnId)
            {
                case BTN_ID.btnScan:
                    formID = FORM_ID.JBScan;
                    break;

                case BTN_ID.btnOrder:
                    formID = FORM_ID.JBOrder;
                    break;

                default:
                    return;
            }

            showChild(true);
        }

        private void menuModify_Click(object sender, EventArgs e)
        {
            switch (btnId)
            {
                case BTN_ID.btnScan:
                    formID = FORM_ID.JBScanI;
                    break;

                case BTN_ID.btnOrder:
                    formID = FORM_ID.JBOrderI;
                    break;

                default:
                    return;
            }
            
     
            showChild(false);
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

                switch (formID)
                {
                    case FORM_ID.JBIn:
                        child = new FormJBIn();
                        break;

                    case FORM_ID.JBScan:
                        child = new FormJBScan();
                        break;

                    case FORM_ID.JBScanI:
                        child = new FormJBScanI();
                        break;

                    case FORM_ID.JBOrder:
                        child = new FormJBOrder();
                        break;

                    case FORM_ID.JBOrderI:
                        child = new FormJBOrderI();
                        break;

                    default:
                        return;
                }

                ((ISerializable)child).init(new string[] { ShopNo });
                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }


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
                formID = FORM_ID.NONE;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Close();
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

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
                data[(int)DATA_INDEX.ShopNO] = ShopNo;
                data[(int)DATA_INDEX.CHILD] = formID.ToString();

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
                    ShopNo = sc.Data[(int)DATA_INDEX.ShopNO];
                    formID = (FORM_ID)Enum.Parse(typeof(FORM_ID), sc.Data[(int)DATA_INDEX.CHILD], true);
                    if (formID != FORM_ID.NONE)
                    {
                        showChild(true);
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