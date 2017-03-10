namespace Hola_Excel
{
    partial class FormMain
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnOut = new HolaCore.ImageButton();
            this.btnIn = new HolaCore.ImageButton();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.menuWave = new System.Windows.Forms.MenuItem();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail1 = new System.Windows.Forms.MenuItem();
            this.menuWave1 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(30, 59);
            this.btnOut.Name = "btnOut";
            this.btnOut.ShowFrame = true;
            this.btnOut.ShowText = true;
            this.btnOut.Size = new System.Drawing.Size(68, 76);
            this.btnOut.TabIndex = 0;
            this.btnOut.Text = "出货查询";
            this.btnOut.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(131, 59);
            this.btnIn.Name = "btnIn";
            this.btnIn.ShowFrame = true;
            this.btnIn.ShowText = true;
            this.btnIn.Size = new System.Drawing.Size(68, 76);
            this.btnIn.TabIndex = 1;
            this.btnIn.Text = "收货查询";
            this.btnIn.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 2;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 270);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.Add(this.menuDetail);
            this.contextMenu.MenuItems.Add(this.menuWave);
            // 
            // menuDetail
            // 
            this.menuDetail.Text = "详细";
            this.menuDetail.Click += new System.EventHandler(this.menuDetail_Click);
            // 
            // menuWave
            // 
            this.menuWave.Text = "波次";
            this.menuWave.Click += new System.EventHandler(this.menuWave_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menuDetail1);
            this.contextMenu1.MenuItems.Add(this.menuWave1);
            // 
            // menuDetail1
            // 
            this.menuDetail1.Text = "系统出货";
            this.menuDetail1.Click += new System.EventHandler(this.menuDetail1_Click);
            // 
            // menuWave1
            // 
            this.menuWave1.Text = "照相打印";
            this.menuWave1.Click += new System.EventHandler(this.menuWave1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnOut);
            this.Name = "FormMain";
            this.Text = "查询";
            this.TopMost = true;
            this.ResumeLayout(false);

        }



        #endregion

        private HolaCore.ImageButton btnOut;
        private HolaCore.ImageButton btnIn;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
        private System.Windows.Forms.MenuItem menuWave;
        private System.Windows.Forms.MenuItem menuDetail1;
        private System.Windows.Forms.MenuItem menuWave1;
    }
}