using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using HolaCore;
using System.Reflection;
namespace Hola_Excel
{
    public partial class FormMain : Form
    {
        private const string guid = "DF99FD44-F8C5-3025-F434-C5737485236E";

        private Form child = null;


        #region Form类型ID
        private enum FORM_ID
        {
            NONE,
            IN,
            INWAVE,
            OUT
        }

        private FORM_ID formID = FORM_ID.NONE;
        #endregion

        private SerializeClass sc = null;
        private DataSet ds = null;
        #region 序列化索引
        private enum DATA_INDEX
        {
            CHILD,
            DATAMAX
        }
        #endregion
        private int TaskBarHeight = 0;
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
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            dstHeight -= TaskBarHeight;
            try
            {
                if (srcWidth != dstWidth || srcHeight != dstHeight)
                {
                    SuspendLayout();

                    btnIn.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("excel")));
                    btnIn.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("excel_p")));
                    btnOut.ButtonImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("excel2")));
                    btnOut.ButtonPressedImage = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("excel2_p")));

                    btnReturn.Top = dstHeight - btnReturn.Height;
                    pbBar.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("bar")));
                    pbBar.Location = new System.Drawing.Point(0, btnReturn.Top - 5);
                    pbBar.Size = new System.Drawing.Size(dstWidth, pbBar.Image.Height);

                    ResumeLayout(false);
                }
            }
            catch (Exception)
            {
            }
        }

        #region UI事件响应

        void menuWave1_Click(object sender, System.EventArgs e)
        {
            FormPrintInquiry formPrintInquiry = new FormPrintInquiry();
            formPrintInquiry.Show();
        }

        void menuDetail1_Click(object sender, System.EventArgs e)
        {
            formID = FORM_ID.OUT;

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
                    case FORM_ID.IN:
                        child = new FormIn();
                        break;

                    case FORM_ID.INWAVE:
                        child = new FormInWave();
                        break;

                    case FORM_ID.OUT:
                        child = new FormOut();
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
            catch (Exception)
            {
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            ShowMenu1(sender);

            //formID = FORM_ID.OUT;

            //showChild(false);
        }

        private void menuDetail_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.IN;

            showChild(false);
        }

        private void menuWave_Click(object sender, EventArgs e)
        {
            formID = FORM_ID.INWAVE;

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

        private void ShowMenu1(object sender)
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

        private void btnIn_Click(object sender, EventArgs e)
        {
            ShowMenu(sender);
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