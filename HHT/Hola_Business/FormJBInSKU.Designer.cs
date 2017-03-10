namespace Hola_Business
{
    partial class FormJBInSKU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJBInSKU));
            this.label1 = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.btnReturn = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.NexPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.Text = "价标扫描记录";
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 32);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 268);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 78;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(93, 262);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(48, 20);
            this.Page.Text = "1/1";
            this.Page.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // NexPage
            // 
            this.NexPage.BackColor = System.Drawing.Color.NavajoWhite;
            this.NexPage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.NexPage.ForeColor = System.Drawing.Color.Sienna;
            this.NexPage.Location = new System.Drawing.Point(141, 263);
            this.NexPage.Name = "NexPage";
            this.NexPage.Size = new System.Drawing.Size(72, 20);
            this.NexPage.TabIndex = 165;
            this.NexPage.Text = "下一页";
            this.NexPage.Click += new System.EventHandler(this.NexPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(20, 263);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 164;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 69);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 188);
            this.dgTable.TabIndex = 170;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menuDetail);
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuDetail
            // 
            this.menuDetail.Text = "详细";
            this.menuDetail.Click += new System.EventHandler(this.menuDetail_Click);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 51);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 41);
            this.dgHeader.TabIndex = 169;
            this.dgHeader.TabStop = false;
            // 
            // FormJBInSKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NexPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBarI);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbBar);
            this.KeyPreview = true;
            this.Name = "FormJBInSKU";
            this.Text = "盘点检核查询";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NexPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
    }
}