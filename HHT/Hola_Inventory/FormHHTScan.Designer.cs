namespace Hola_Inventory
{
    partial class FormHHTScan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHHTScan));
            this.ShopNO = new System.Windows.Forms.Label();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.SKUNOScan = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBNOScan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SKUName = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.Page = new System.Windows.Forms.Label();
            this.NexPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 9);
            this.ShopNO.Name = "ShopNO";
            this.ShopNO.Size = new System.Drawing.Size(86, 20);
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 32);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // SKUNOScan
            // 
            this.SKUNOScan.Location = new System.Drawing.Point(75, 62);
            this.SKUNOScan.Name = "SKUNOScan";
            this.SKUNOScan.Size = new System.Drawing.Size(138, 21);
            this.SKUNOScan.TabIndex = 22;
            this.SKUNOScan.TextChanged += new System.EventHandler(this.SKUNOScan_TextChanged);
            this.SKUNOScan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNOScan_KeyUp);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.Text = "SKU扫描:";
            // 
            // CBNOScan
            // 
            this.CBNOScan.Location = new System.Drawing.Point(75, 38);
            this.CBNOScan.Name = "CBNOScan";
            this.CBNOScan.Size = new System.Drawing.Size(138, 21);
            this.CBNOScan.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.Text = "柜号扫描:";
            // 
            // SKUName
            // 
            this.SKUName.Location = new System.Drawing.Point(75, 86);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(64, 20);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.btnConfirm.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirm.Location = new System.Drawing.Point(0, 274);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(82, 20);
            this.btnConfirm.TabIndex = 30;
            this.btnConfirm.Text = "测试数据确认";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 31;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.Text = "店号:";
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 130);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 103);
            this.dgTable.TabIndex = 59;
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
            this.dgHeader.Location = new System.Drawing.Point(0, 112);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 30);
            this.dgHeader.TabIndex = 58;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(93, 242);
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
            this.NexPage.Location = new System.Drawing.Point(141, 243);
            this.NexPage.Name = "NexPage";
            this.NexPage.Size = new System.Drawing.Size(72, 20);
            this.NexPage.TabIndex = 72;
            this.NexPage.Text = "下一页";
            this.NexPage.Click += new System.EventHandler(this.NexPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(20, 243);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 71;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(141, 89);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 20);
            this.btnAdd.TabIndex = 73;
            this.btnAdd.Text = "添 加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(33, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.Text = "品名:";
            // 
            // FormHHTScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NexPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.SKUName);
            this.Controls.Add(this.SKUNOScan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CBNOScan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.pbBarI);
            this.Name = "FormHHTScan";
            this.Text = " ";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.TextBox SKUNOScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CBNOScan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SKUName;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NexPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label4;
    }
}