namespace Hola_Inventory
{
    partial class FormCPISKU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCPISKU));
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.pCode = new System.Windows.Forms.Label();
            this.CBNO = new System.Windows.Forms.Label();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ShopNO = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SKUName = new System.Windows.Forms.Label();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable2 = new System.Windows.Forms.DataGrid();
            this.dgHeader2 = new System.Windows.Forms.DataGrid();
            this.Page = new System.Windows.Forms.Label();
            this.NexPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Ent_Agn = new System.Windows.Forms.TabControl();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Ent_Agn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 31);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGrid1.Location = new System.Drawing.Point(13, 17);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(213, 113);
            this.dataGrid1.TabIndex = 0;
            // 
            // pCode
            // 
            this.pCode.Location = new System.Drawing.Point(84, 40);
            this.pCode.Name = "pCode";
            this.pCode.Size = new System.Drawing.Size(138, 20);
            // 
            // CBNO
            // 
            this.CBNO.Location = new System.Drawing.Point(157, 9);
            this.CBNO.Name = "CBNO";
            this.CBNO.Size = new System.Drawing.Size(56, 20);
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(84, 7);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(138, 21);
            this.SKUNO.TabIndex = 32;
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(23, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.Text = "SKU:";
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 9);
            this.ShopNO.Name = "ShopNO";
            this.ShopNO.Size = new System.Drawing.Size(39, 20);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.Text = "店号:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.Text = "盘点代号:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(116, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 20);
            this.label2.Text = "柜号:";
            // 
            // SKUName
            // 
            this.SKUName.Location = new System.Drawing.Point(84, 31);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(138, 30);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 82);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 89);
            this.dgTable.TabIndex = 57;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 64);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 56;
            this.dgHeader.TabStop = false;
            // 
            // dgTable2
            // 
            this.dgTable2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable2.ColumnHeadersVisible = false;
            this.dgTable2.Location = new System.Drawing.Point(0, 20);
            this.dgTable2.Name = "dgTable2";
            this.dgTable2.RowHeadersVisible = false;
            this.dgTable2.Size = new System.Drawing.Size(240, 129);
            this.dgTable2.TabIndex = 71;
            this.dgTable2.TabStop = false;
            // 
            // dgHeader2
            // 
            this.dgHeader2.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader2.ColumnHeadersVisible = false;
            this.dgHeader2.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader2.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader2.Location = new System.Drawing.Point(0, 2);
            this.dgHeader2.Name = "dgHeader2";
            this.dgHeader2.RowHeadersVisible = false;
            this.dgHeader2.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader2.Size = new System.Drawing.Size(240, 20);
            this.dgHeader2.TabIndex = 70;
            this.dgHeader2.TabStop = false;
            this.dgHeader2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader2_MouseDown);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 152);
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
            this.NexPage.Location = new System.Drawing.Point(146, 152);
            this.NexPage.Name = "NexPage";
            this.NexPage.Size = new System.Drawing.Size(72, 20);
            this.NexPage.TabIndex = 69;
            this.NexPage.Text = "下一页";
            this.NexPage.Click += new System.EventHandler(this.NexPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(16, 152);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 68;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 268);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(22, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 20);
            this.label5.Text = "品名:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SKUNO);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.SKUName);
            this.tabPage1.Controls.Add(this.dgHeader);
            this.tabPage1.Controls.Add(this.dgTable);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 208);
            this.tabPage1.Text = "抽盘";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Page);
            this.tabPage2.Controls.Add(this.NexPage);
            this.tabPage2.Controls.Add(this.PrePage);
            this.tabPage2.Controls.Add(this.dgHeader2);
            this.tabPage2.Controls.Add(this.dgTable2);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(240, 208);
            this.tabPage2.Text = "抽盘明细";
            // 
            // Ent_Agn
            // 
            this.Ent_Agn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ent_Agn.Controls.Add(this.tabPage1);
            this.Ent_Agn.Controls.Add(this.tabPage2);
            this.Ent_Agn.Dock = System.Windows.Forms.DockStyle.None;
            this.Ent_Agn.Location = new System.Drawing.Point(0, 63);
            this.Ent_Agn.Name = "Ent_Agn";
            this.Ent_Agn.SelectedIndex = 0;
            this.Ent_Agn.Size = new System.Drawing.Size(240, 231);
            this.Ent_Agn.TabIndex = 16;
            this.Ent_Agn.SelectedIndexChanged += new System.EventHandler(this.Ent_Agn_SelectedIndexChanged);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(146, 244);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 38;
            this.btnReturn.Text = "离 开";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnConfirm.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirm.Location = new System.Drawing.Point(16, 244);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(72, 20);
            this.btnConfirm.TabIndex = 39;
            this.btnConfirm.Text = "添 加";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FormCPISKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CBNO);
            this.Controls.Add(this.pCode);
            this.Controls.Add(this.pbBarI);
            this.Controls.Add(this.Ent_Agn);
            this.Name = "FormCPISKU";
            this.Text = "抽盘明细";
            this.TopMost = true;
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.Ent_Agn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.Label pCode;
        private System.Windows.Forms.Label CBNO;
        private System.Windows.Forms.TextBox SKUNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SKUName;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable2;
        private System.Windows.Forms.DataGrid dgHeader2;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NexPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl Ent_Agn;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnConfirm;
    }
}