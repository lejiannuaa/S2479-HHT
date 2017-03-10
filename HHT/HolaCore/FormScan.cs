using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HolaCore
{
    public interface ScanCallback
    {
        void scanCallback(string barCode);
    }

    public partial class FormScan : NonFullscreenForm
    {
        ScanCallback sponsor = null;

        public FormScan()
        {
            InitializeComponent();

            doLayout();

            //FullscreenClass.ShowSIP(true);
        }

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;

            try
            {
                SuspendLayout();

                pictureBox1.Image = ((System.Drawing.Image)(HolaCore.Properties.Resources.ResourceManager.GetObject("fcheck")));

                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        public void init(ScanCallback sc, string text)
        {
            sponsor = sc;
            tbBarCode.Text = text;
            tbBarCode.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (sponsor != null)
            {
                sponsor.scanCallback(tbBarCode.Text);
            }

            this.DialogResult = DialogResult.OK;
            Close();
            FullscreenClass.ShowSIP(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == 0)
            {
                tbBarCode.Text = "";
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(null, null);
            }
        }
    }
}