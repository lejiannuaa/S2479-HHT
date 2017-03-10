namespace HolaCore
{
    partial class FormApp
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        /// 
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.PictureBox pbBar;
        private ImageButton btnIn;
        private ImageButton btnOut;
        private ImageButton btnExcel;
        private ImageButton btnPing;
        private ImageButton btnInventory;
        private ImageButton btnBusiness;
        private System.Windows.Forms.Button Goback;


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApp));
            this.btnLogout = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.Goback = new System.Windows.Forms.Button();
            this.btnExcel = new HolaCore.ImageButton();
            this.btnOut = new HolaCore.ImageButton();
            this.btnIn = new HolaCore.ImageButton();
            this.btnPing = new HolaCore.ImageButton();
            this.btnBusiness = new HolaCore.ImageButton();
            this.btnInventory = new HolaCore.ImageButton();
            this.SuspendLayout();
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.btnLogout.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.btnLogout.Location = new System.Drawing.Point(0, 272);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(82, 22);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "注  销";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // Goback
            // 
            this.Goback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.Goback.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.Goback.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.Goback.Location = new System.Drawing.Point(158, 272);
            this.Goback.Name = "Goback";
            this.Goback.Size = new System.Drawing.Size(82, 22);
            this.Goback.TabIndex = 5;
            this.Goback.Text = "退出";
            this.Goback.Click += new System.EventHandler(this.Goback_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnExcel.Location = new System.Drawing.Point(20, 91);
            this.btnExcel.MultiLine = false;
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.ShowFrame = true;
            this.btnExcel.ShowText = true;
            this.btnExcel.Size = new System.Drawing.Size(80, 85);
            this.btnExcel.TabIndex = 3;
            this.btnExcel.Text = "查询&报表";
            this.btnExcel.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnOut
            // 
            this.btnOut.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnOut.Location = new System.Drawing.Point(140, 3);
            this.btnOut.MultiLine = false;
            this.btnOut.Name = "btnOut";
            this.btnOut.ShowFrame = true;
            this.btnOut.ShowText = true;
            this.btnOut.Size = new System.Drawing.Size(80, 85);
            this.btnOut.TabIndex = 2;
            this.btnOut.Text = "出货作业";
            this.btnOut.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnIn
            // 
            this.btnIn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnIn.Location = new System.Drawing.Point(20, 3);
            this.btnIn.MultiLine = false;
            this.btnIn.Name = "btnIn";
            this.btnIn.ShowFrame = true;
            this.btnIn.ShowText = true;
            this.btnIn.Size = new System.Drawing.Size(80, 85);
            this.btnIn.TabIndex = 1;
            this.btnIn.Text = "收货作业";
            this.btnIn.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnPing
            // 
            this.btnPing.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnPing.Location = new System.Drawing.Point(140, 91);
            this.btnPing.MultiLine = false;
            this.btnPing.Name = "btnPing";
            this.btnPing.ShowFrame = true;
            this.btnPing.ShowText = true;
            this.btnPing.Size = new System.Drawing.Size(80, 85);
            this.btnPing.TabIndex = 7;
            this.btnPing.Text = "网络查看";
            this.btnPing.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // btnBusiness
            // 
            this.btnBusiness.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnBusiness.Location = new System.Drawing.Point(20, 179);
            this.btnBusiness.MultiLine = false;
            this.btnBusiness.Name = "btnBusiness";
            this.btnBusiness.ShowFrame = true;
            this.btnBusiness.ShowText = true;
            this.btnBusiness.Size = new System.Drawing.Size(80, 85);
            this.btnBusiness.TabIndex = 5;
            this.btnBusiness.Text = "销售课";
            this.btnBusiness.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnBusiness.Click += new System.EventHandler(this.btnYyk_Click);
            // 
            // btnInventory
            // 
            this.btnInventory.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnInventory.Location = new System.Drawing.Point(140, 179);
            this.btnInventory.MultiLine = false;
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.ShowFrame = true;
            this.btnInventory.ShowText = true;
            this.btnInventory.Size = new System.Drawing.Size(80, 85);
            this.btnInventory.TabIndex = 4;
            this.btnInventory.Text = "盘点";
            this.btnInventory.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnInventory.Click += new System.EventHandler(this.btnPd_Click);
            // 
            // FormApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.Goback);
            this.Controls.Add(this.btnBusiness);
            this.Controls.Add(this.btnInventory);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnLogout);
            this.MinimizeBox = false;
            this.Name = "FormApp";
            this.Text = "应用选择";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion
    }
}