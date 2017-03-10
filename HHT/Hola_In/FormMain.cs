using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using HolaCore;
using System.Data;
using System.Xml;
using System.Reflection;
namespace Hola_In
{
    public partial class FormMain : Form, ISerializable
    {
         
        //加载、解析XML动态库常驻内存
        private DataSet ds = null;

        private const string guid = "AFAC8CEF-07C4-F6C7-9941-9D4211992D96";

        private Form child = null;

        private string photoName1 = "";
        private string photoName2 = "";
        private string photoName3 = "";

        #region Form类型ID
        private enum FORM_ID
        {
            NONE,
            DB,
            PO
        }

        private FORM_ID formID = FORM_ID.NONE;
        #endregion

        private SerializeClass sc = null;

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
            this.Text += "-" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            dstHeight -= 26;
            try
            {
                SuspendLayout();

                btnDB.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("u19_original")));
                btnDB.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("u19_originaled")));
                btnPO.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("u15_original")));
                btnPO.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("u15_originaled")));

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
                    case FORM_ID.DB:
                        child = new FormDBSKU();
                        break;

                    case FORM_ID.PO:
                        child = new FormPOSKU();
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

        private void btnDB_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.DB;

            showChild(false);
        }

        private void ShowMenu(object sender)
        {
            try
            {
                Control btn = (Control)sender;
                contextMenu.Show(btn, new Point(btn.Width / 2, btn.Height / 2));
            }
            catch (Exception)
            {
            }
        }

        private void btnPO_Click(object sender, EventArgs e)
        {
            ShowMenu(sender);

            //formID = FORM_ID.PO;

            //showChild(false);
        }

        #region delegates 传参

        public delegate void PageTitle(Form form);

        #endregion

        void menuDetail_Click(object sender, System.EventArgs e)
        {
            formID = FORM_ID.PO;

            showChild(false);
        }
        private bool isPO = true;
        void menuPhoto_Click(object sender, System.EventArgs e)
        {
            //FormCamera formCamera = new FormCamera(photoName1, photoName2, photoName3, isPO);
            //formCamera.Show();


            FormCamera formCamera = new FormCamera(photoName1, photoName2, photoName3, isPO);
            //formCamera.MyPhotoEvent += new FormCamera.MyPhotoDelegate(getPhotoName);
            PageTitle pageTitle = new PageTitle(formCamera.getTitle);
            pageTitle(this);
            formCamera.ShowDialog();
        }


        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(Config.DirLocal + guid);

                Application.Exit();
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