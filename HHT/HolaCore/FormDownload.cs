using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HolaCore
{
    public partial class FormDownload : NonFullscreenForm
    {
        public FormDownload()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void setProgress(int total, int progress)
        {
            pbDownload.Value = progress * 100 / total;
        }
    }
}