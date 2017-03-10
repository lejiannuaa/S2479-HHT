using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HolaCore
{
    public partial class FormScanAdd : NonFullscreenForm
    {

        private string[] volumeWeight =new string[2];
        public delegate void volumeWeightdelegate(string[] VM);//定义一个委托
        public event volumeWeightdelegate volumeWeightevent;//定义上诉委托类型的事件

        public FormScanAdd()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (volume.Text == "" || weigh.Text == "")
                {
                    MessageBox.Show("请填好体积和重量");
                    return;
                }
                if (double.Parse(volume.Text.ToString()) <= 0 || double.Parse(weigh.Text.ToString()) <= 0)
                {
                    MessageBox.Show("请输入大于0的重量和材积");
                    return;
                }
                volumeWeight[0] = volume.Text;
                volumeWeight[1] = weigh.Text;
                if (volumeWeightevent != null)
                {
                    volumeWeightevent(volumeWeight);
                }
                Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (volumeWeightevent != null)
                {
                    volumeWeight = null;
                    volumeWeightevent(volumeWeight);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FormScanAdd_Activated(object sender, EventArgs e)
        {
            this.volume.Focus();
        }

        private void volume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13))
            {
                this.weigh.Focus();
            }
        }

        private void weigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13))
            {
                this.btnOK.Focus();
            }
        }
    }
}