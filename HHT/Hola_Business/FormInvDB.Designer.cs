namespace Hola_Business
{
    partial class FormInvDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInvDB));
            this.SKUName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.GoodsNO = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btn01 = new System.Windows.Forms.Button();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BelongDC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LeadTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DCAvailable = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LeastNO = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.Page = new System.Windows.Forms.Label();
            this.NexPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.FromLocation = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ToLocation = new System.Windows.Forms.TextBox();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // SKUName
            // 
            this.SKUName.Enabled = false;
            this.SKUName.Location = new System.Drawing.Point(159, 29);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(74, 21);
            this.SKUName.TabIndex = 101;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(125, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.Text = "品名:";
            // 
            // GoodsNO
            // 
            this.GoodsNO.Enabled = false;
            this.GoodsNO.Location = new System.Drawing.Point(159, 54);
            this.GoodsNO.Name = "GoodsNO";
            this.GoodsNO.Size = new System.Drawing.Size(74, 21);
            this.GoodsNO.TabIndex = 100;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(123, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.Text = "货号:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(180, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 20);
            this.btnDelete.TabIndex = 99;
            this.btnDelete.Text = "清 除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(117, 2);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(60, 20);
            this.btn01.TabIndex = 98;
            this.btn01.Text = "查 询";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(38, 2);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(76, 21);
            this.SKUNO.TabIndex = 97;
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.Text = "SKU:";
            // 
            // BelongDC
            // 
            this.BelongDC.Enabled = false;
            this.BelongDC.Location = new System.Drawing.Point(56, 29);
            this.BelongDC.Name = "BelongDC";
            this.BelongDC.Size = new System.Drawing.Size(58, 21);
            this.BelongDC.TabIndex = 106;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.Text = "归属DC:";
            // 
            // LeadTime
            // 
            this.LeadTime.Enabled = false;
            this.LeadTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.LeadTime.Location = new System.Drawing.Point(182, 77);
            this.LeadTime.Name = "LeadTime";
            this.LeadTime.ReadOnly = true;
            this.LeadTime.Size = new System.Drawing.Size(51, 20);
            this.LeadTime.TabIndex = 109;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label2.Location = new System.Drawing.Point(120, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.Text = "LeadTime:";
            // 
            // DCAvailable
            // 
            this.DCAvailable.Enabled = false;
            this.DCAvailable.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.DCAvailable.Location = new System.Drawing.Point(84, 54);
            this.DCAvailable.Name = "DCAvailable";
            this.DCAvailable.ReadOnly = true;
            this.DCAvailable.Size = new System.Drawing.Size(30, 20);
            this.DCAvailable.TabIndex = 112;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label6.Location = new System.Drawing.Point(8, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 20);
            this.label6.Text = "DC可用库存:";
            // 
            // LeastNO
            // 
            this.LeastNO.Enabled = false;
            this.LeastNO.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.LeastNO.Location = new System.Drawing.Point(84, 79);
            this.LeastNO.Name = "LeastNO";
            this.LeastNO.ReadOnly = true;
            this.LeastNO.Size = new System.Drawing.Size(30, 20);
            this.LeastNO.TabIndex = 115;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label7.Location = new System.Drawing.Point(8, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 20);
            this.label7.Text = "最小包装数:";
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 125;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(93, 273);
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
            this.NexPage.Location = new System.Drawing.Point(141, 274);
            this.NexPage.Name = "NexPage";
            this.NexPage.Size = new System.Drawing.Size(72, 20);
            this.NexPage.TabIndex = 156;
            this.NexPage.Text = "下一页";
            this.NexPage.Click += new System.EventHandler(this.NexPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(20, 274);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 155;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // FromLocation
            // 
            this.FromLocation.Enabled = false;
            this.FromLocation.Location = new System.Drawing.Point(45, 102);
            this.FromLocation.Name = "FromLocation";
            this.FromLocation.Size = new System.Drawing.Size(69, 21);
            this.FromLocation.TabIndex = 168;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 20);
            this.label8.Text = "From:";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(127, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 20);
            this.label9.Text = "To:";
            // 
            // ToLocation
            // 
            this.ToLocation.Enabled = false;
            this.ToLocation.Location = new System.Drawing.Point(164, 101);
            this.ToLocation.Name = "ToLocation";
            this.ToLocation.Size = new System.Drawing.Size(69, 21);
            this.ToLocation.TabIndex = 172;
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 147);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 121);
            this.dgTable.TabIndex = 185;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 129);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 184;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // FormInvDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.ToLocation);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.FromLocation);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NexPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.LeastNO);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DCAvailable);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LeadTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BelongDC);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SKUName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GoodsNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.SKUNO);
            this.Controls.Add(this.label3);
            this.KeyPreview = true;
            this.Name = "FormInvDB";
            this.Text = "在途调拨查询";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox SKUName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox GoodsNO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.TextBox SKUNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BelongDC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LeadTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DCAvailable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox LeastNO;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NexPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.TextBox FromLocation;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ToLocation;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
    }
}