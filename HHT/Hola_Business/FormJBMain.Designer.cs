namespace Hola_Business
{
    partial class FormJBMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJBMain));
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuNew = new System.Windows.Forms.MenuItem();
            this.menuModify = new System.Windows.Forms.MenuItem();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnJBOrder = new HolaCore.ImageButton();
            this.btnJBScan = new HolaCore.ImageButton();
            this.btnJBIn = new HolaCore.ImageButton();
            this.SuspendLayout();
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menuNew);
            this.contextMenu1.MenuItems.Add(this.menuModify);
            // 
            // menuNew
            // 
            this.menuNew.Text = "新增";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuModify
            // 
            this.menuModify.Text = "查询";
            this.menuModify.Click += new System.EventHandler(this.menuModify_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 266);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 146;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnJBOrder
            // 
            this.btnJBOrder.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnJBOrder.Location = new System.Drawing.Point(23, 131);
            this.btnJBOrder.MultiLine = false;
            this.btnJBOrder.Name = "btnJBOrder";
            this.btnJBOrder.ShowFrame = true;
            this.btnJBOrder.ShowText = true;
            this.btnJBOrder.Size = new System.Drawing.Size(80, 85);
            this.btnJBOrder.TabIndex = 149;
            this.btnJBOrder.Text = "HHT下单";
            this.btnJBOrder.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnJBOrder.Visible = false;
            this.btnJBOrder.Click += new System.EventHandler(this.btnJBOrder_Click);
            // 
            // btnJBScan
            // 
            this.btnJBScan.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnJBScan.Location = new System.Drawing.Point(23, 16);
            this.btnJBScan.MultiLine = false;
            this.btnJBScan.Name = "btnJBScan";
            this.btnJBScan.ShowFrame = true;
            this.btnJBScan.ShowText = true;
            this.btnJBScan.Size = new System.Drawing.Size(80, 85);
            this.btnJBScan.TabIndex = 148;
            this.btnJBScan.Text = "A4价签";
            this.btnJBScan.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnJBScan.Click += new System.EventHandler(this.btnJBScan_Click);
            // 
            // btnJBIn
            // 
            this.btnJBIn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnJBIn.Location = new System.Drawing.Point(134, 16);
            this.btnJBIn.MultiLine = false;
            this.btnJBIn.Name = "btnJBIn";
            this.btnJBIn.ShowFrame = true;
            this.btnJBIn.ShowText = true;
            this.btnJBIn.Size = new System.Drawing.Size(80, 85);
            this.btnJBIn.TabIndex = 147;
            this.btnJBIn.Text = "盘点检核";
            this.btnJBIn.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnJBIn.Visible = false;
            this.btnJBIn.Click += new System.EventHandler(this.btnJBIn_Click);
            // 
            // FormJBMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnJBOrder);
            this.Controls.Add(this.btnJBScan);
            this.Controls.Add(this.btnJBIn);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnReturn);
            this.Name = "FormJBMain";
            this.Text = "价标菜单选择";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuNew;
        private System.Windows.Forms.MenuItem menuModify;
        private HolaCore.ImageButton btnJBOrder;
        private HolaCore.ImageButton btnJBScan;
        private HolaCore.ImageButton btnJBIn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnReturn;
    }
}