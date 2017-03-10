using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HolaCore;
using System.Xml;
using System.Reflection;
namespace Hola_Out
{
    public partial class FormMain : Form, ISerializable
    {
        private const string guid = "E029C176-068C-C256-3DF9-3AEC7BC0C1AD";

        private Form child = null;

        bool bNew = true;

        #region Form类型ID
        private enum FORM_ID
        {
            NONE,
            MANUFACTURER,
            WAREHOUSE,
            SHOP,
            PURCHASE,
            PURCHASE2,

            FormNormalReserve,
            FormOtherReserve,
            FormQueryReserve,
            FormNormalObjOut,
            FormOtherObjOut,
            FormQueryObjOut
        }

        private FORM_ID formID = FORM_ID.NONE;
        #endregion
        private int TaskBarHeight = 0;
        private SerializeClass sc = null;
        private DataSet ds = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            CHILD,
            DATAMAX
        }
        #endregion

        public FormMain()
        {
            InitializeComponent();

            doLayout();

            Deserialize(guid);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<root><config><type>0</type><direction>Server-&gt;Client:0</direction><id>00401</id></config></root>");
            XmlNodeReader reader = new XmlNodeReader(doc);
            ds = new DataSet();
            ds.ReadXml(reader);
            this.Text += "-"+Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();

                btnManufacturer.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c7_original")));
                btnManufacturer.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c7_originaled")));
                btnWarehouse.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c11_original")));
                btnWarehouse.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c11_originaled")));
                btnShop.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c19_original")));
                btnShop.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c19_originaled")));
                btnPurchase.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c23_original")));
                btnPurchase.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("c23_originaled")));
                btnReserve.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("ping")));
                btnReserve.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("ping_p")));
                btnObjOut.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("_out")));
                btnObjOut.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("out_p")));

                btnReturn.Top = dstHeight - btnReturn.Height;
                pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        #region UI事件响应
        private void showChild1(bool bDeserialize)
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
                    case FORM_ID.FormNormalReserve:
                        child = new FormNormalReserve();
                        break;
                    case FORM_ID.FormOtherReserve:
                        child = new FormOtherReserve2();
                        break;
                    case FORM_ID.FormQueryReserve:
                        child = new FormQueryReserve();
                        break;
                    case FORM_ID.FormNormalObjOut:
                        child = new FormNormalObjOut();
                        break;
                    case FORM_ID.FormOtherObjOut:
                        child = new FormOtherObjOut();
                        break;
                    case FORM_ID.FormQueryObjOut:
                        child = new FormQueryObjOut();
                        break;
                    default:
                        return;
                }

                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }
                else
                {
                    Serialize(guid);
                    ((ISerializable)child).init(new string[] {});
                }
                child.ShowDialog();
                Show();
                child.Dispose();

                child = null;
                formID = FORM_ID.NONE;
                Serialize(guid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                    case FORM_ID.MANUFACTURER:
                        child = new FormManufacturer();
                        break;

                    //case FORM_ID.WAREHOUSE:
                    //    child = new FormWarehouse();
                    //    break;

                    case FORM_ID.SHOP:
                        child = new FormShop();
                        break;

                    case FORM_ID.PURCHASE:
                        child = new FormPurchase();
                        break;

                    case FORM_ID.PURCHASE2:
                        child = new FormPurchase2();
                        break;

                    case FORM_ID.FormNormalReserve:
                        child = new FormNormalReserve();
                        break;

                    default:
                        return;
                }

                if (bDeserialize)
                {
                    ((ISerializable)child).Deserialize(null);
                }
                else
                {
                    Serialize(guid);
                    ((ISerializable)child).init(new string[] { bNew.ToString() });
                }
                child.ShowDialog();
                Show();
                child.Dispose();

                child = null;
                formID = FORM_ID.NONE;
                Serialize(guid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ShowMenu(object sender)
        {
            try
            {
                Control btn = (Control)sender;
                contextMenu.MenuItems.Clear();
                contextMenu.MenuItems.Add(menuNew);
                contextMenu.MenuItems.Add(menuModify);
                contextMenu.Show(btn, new Point(btn.Width / 2, btn.Height / 2));
            }
            catch (Exception)
            {
            }
        }

        private void ShowMenu1(object sender)
        {
            try
            {
                Control btn = (Control)sender;
                contextMenu.MenuItems.Clear();
                contextMenu.MenuItems.Add(normalReserve);
                contextMenu.MenuItems.Add(otherReserve);
                contextMenu.MenuItems.Add(queryReserve);
                contextMenu.Show(btn, new Point(btn.Width / 3, btn.Height / 3));
            }
            catch (Exception)
            {
            }
        }

        private void ShowMenu2(object sender)
        {
            try
            {
                Control btn = (Control)sender;
                contextMenu.MenuItems.Clear();
                contextMenu.MenuItems.Add(normalObjOut);
                contextMenu.MenuItems.Add(otherObjOut);
                contextMenu.MenuItems.Add(ObjOutPrint);
                contextMenu.Show(btn, new Point(btn.Width / 3, btn.Height / 3));
            }
            catch (Exception)
            {
            }
        }
        //点击主菜单
        private void btnReserve_Click(object sender, EventArgs e)
        {
            ShowMenu1(sender);
        }

        private void btnObjOut_Click(object sender, EventArgs e)
        {
            ShowMenu2(sender);
        }

        private void btnManufacturer_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.MANUFACTURER;
            ShowMenu(sender);
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.WAREHOUSE;
            ShowMenu(sender);
        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.SHOP;
            ShowMenu(sender);
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.PURCHASE;
            ShowMenu(sender);
        }
        //meunItems   click
        private void normalReserve_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.FormNormalReserve;
            showChild1(false);
            contextMenu.MenuItems.Remove(normalReserve);
            contextMenu.MenuItems.Remove(otherReserve);
            contextMenu.MenuItems.Remove(ObjOutPrint);
        }

        private void otherReserve_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.FormOtherReserve;
            showChild1(false);
            contextMenu.MenuItems.Remove(normalReserve);
            contextMenu.MenuItems.Remove(otherReserve);
            contextMenu.MenuItems.Remove(ObjOutPrint);
        }
        private void queryReserve_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.FormQueryReserve;
            showChild1(false);
            contextMenu.MenuItems.Remove(normalReserve);
            contextMenu.MenuItems.Remove(otherReserve);
            contextMenu.MenuItems.Remove(ObjOutPrint);
        }
        private void normalObjOut_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.FormNormalObjOut;
            showChild1(false);
            contextMenu.MenuItems.Remove(normalReserve);
            contextMenu.MenuItems.Remove(otherReserve);
            contextMenu.MenuItems.Remove(ObjOutPrint);
        }
        private void otherObjOut_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.FormOtherObjOut;
            showChild1(false);
            contextMenu.MenuItems.Remove(normalReserve);
            contextMenu.MenuItems.Remove(otherReserve);
            contextMenu.MenuItems.Remove(ObjOutPrint);
        }
        private void ObjOutPrint_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.FormQueryObjOut;
            showChild1(false);
            contextMenu.MenuItems.Remove(normalReserve);
            contextMenu.MenuItems.Remove(otherReserve);
            contextMenu.MenuItems.Remove(ObjOutPrint);
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            bNew = true;
            switch (formID)
            {
                case FORM_ID.MANUFACTURER:
                case FORM_ID.WAREHOUSE:
                case FORM_ID.SHOP:
                case FORM_ID.PURCHASE:
                    break;

                default:
                    return;
            }

            showChild(false);
        }

        private void menuModify_Click(object sender, EventArgs e)
        {
            bNew = false;
            switch (formID)
            {
                case FORM_ID.MANUFACTURER:
                case FORM_ID.WAREHOUSE:
                case FORM_ID.SHOP:
                    break;

                case FORM_ID.PURCHASE:
                    formID = FORM_ID.PURCHASE2;
                    break;

                default:
                    return;
            }

            showChild(false);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            File.Delete(Config.DirLocal + guid);

            Application.Exit();
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

                string[] data = new string[(int)DATA_INDEX.DATAMAX];
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