namespace Hola_In
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
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnPO = new HolaCore.ImageButton();
            this.btnDB = new HolaCore.ImageButton();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.menuPhoto = new System.Windows.Forms.MenuItem();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.Add(this.menuDetail);
            this.contextMenu.MenuItems.Add(this.menuPhoto);
            // 
            // menuPhoto
            // 
            this.menuPhoto.Text = "PO照相";
            this.menuPhoto.Click += new System.EventHandler(menuPhoto_Click);
            // 
            // menuDetail
            // 
            this.menuDetail.Text = "PO收货";
            this.menuDetail.Click += new System.EventHandler(menuDetail_Click);
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
            // btnPO
            // 
            this.btnPO.Location = new System.Drawing.Point(140, 49);
            this.btnPO.Name = "btnPO";
            this.btnPO.ShowFrame = true;
            this.btnPO.ShowText = true;
            this.btnPO.Size = new System.Drawing.Size(80, 85);
            this.btnPO.TabIndex = 1;
            this.btnPO.Text = "PO收货";
            this.btnPO.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnPO.Click += new System.EventHandler(this.btnPO_Click);
            // 
            // btnDB
            // 
            this.btnDB.Location = new System.Drawing.Point(20, 49);
            this.btnDB.Name = "btnDB";
            this.btnDB.ShowFrame = true;
            this.btnDB.ShowText = true;
            this.btnDB.Size = new System.Drawing.Size(80, 85);
            this.btnDB.TabIndex = 0;
            this.btnDB.Text = "调拨收货";
            this.btnDB.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnDB.Click += new System.EventHandler(this.btnDB_Click);
            // 
            // pbBar
            // 
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnPO);
            this.Controls.Add(this.btnDB);
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "收货";
            this.TopMost = true;
            this.ResumeLayout(false);

        }




        #endregion

        private HolaCore.ImageButton btnDB;
        private HolaCore.ImageButton btnPO;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuDetail;
        private System.Windows.Forms.MenuItem menuPhoto;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
    }
}