namespace Hola_Inventory
{
    partial class FormCPSKU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCPSKU));
            this.label4 = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.CP_UP = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.SKUName = new System.Windows.Forms.TextBox();
            this.ItemsAlr = new System.Windows.Forms.Label();
            this.Page = new System.Windows.Forms.Label();
            this.NexPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.SKUNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.DupSum = new System.Windows.Forms.Label();
            this.ZeroSum = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnConfirmI = new System.Windows.Forms.Button();
            this.NumSum = new System.Windows.Forms.Label();
            this.ItemsSum = new System.Windows.Forms.Label();
            this.CBNOI = new System.Windows.Forms.Label();
            this.CPCode = new System.Windows.Forms.Label();
            this.CBNO = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ShopNO = new System.Windows.Forms.Label();
            this.CP_UP.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.Text = "店号:";
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 33);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // CP_UP
            // 
            this.CP_UP.Controls.Add(this.tabPage1);
            this.CP_UP.Controls.Add(this.tabPage2);
            this.CP_UP.Dock = System.Windows.Forms.DockStyle.None;
            this.CP_UP.Location = new System.Drawing.Point(0, 46);
            this.CP_UP.Name = "CP_UP";
            this.CP_UP.SelectedIndex = 0;
            this.CP_UP.Size = new System.Drawing.Size(240, 246);
            this.CP_UP.TabIndex = 9;
            this.CP_UP.SelectedIndexChanged += new System.EventHandler(this.CP_UP_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SKUNO);
            this.tabPage1.Controls.Add(this.SKUName);
            this.tabPage1.Controls.Add(this.ItemsAlr);
            this.tabPage1.Controls.Add(this.Page);
            this.tabPage1.Controls.Add(this.NexPage);
            this.tabPage1.Controls.Add(this.PrePage);
            this.tabPage1.Controls.Add(this.dgTable);
            this.tabPage1.Controls.Add(this.dgHeader);
            this.tabPage1.Controls.Add(this.btnReturn);
            this.tabPage1.Controls.Add(this.btnConfirm);
            this.tabPage1.Controls.Add(this.btnDelete);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.SKUNum);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 223);
            this.tabPage1.Text = "初盘";
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(42, 0);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(85, 21);
            this.SKUNO.TabIndex = 1;
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // SKUName
            // 
            this.SKUName.Enabled = false;
            this.SKUName.Location = new System.Drawing.Point(42, 30);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(85, 21);
            this.SKUName.TabIndex = 64;
            this.SKUName.TabStop = false;
            // 
            // ItemsAlr
            // 
            this.ItemsAlr.Location = new System.Drawing.Point(192, 0);
            this.ItemsAlr.Name = "ItemsAlr";
            this.ItemsAlr.Size = new System.Drawing.Size(39, 20);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(93, 197);
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
            this.NexPage.Location = new System.Drawing.Point(141, 198);
            this.NexPage.Name = "NexPage";
            this.NexPage.Size = new System.Drawing.Size(72, 20);
            this.NexPage.TabIndex = 7;
            this.NexPage.Text = "下一页";
            this.NexPage.Click += new System.EventHandler(this.NexPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(20, 198);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 6;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 101);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 93);
            this.dgTable.TabIndex = 55;
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
            this.dgHeader.Location = new System.Drawing.Point(0, 83);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 54;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(165, 58);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(61, 20);
            this.btnReturn.TabIndex = 5;
            this.btnReturn.Text = "离 开";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnConfirm.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirm.Location = new System.Drawing.Point(91, 58);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(61, 20);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "添 加";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(19, 58);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 20);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删 除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(133, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 20);
            this.label7.Text = "已盘品项:";
            // 
            // SKUNum
            // 
            this.SKUNum.Location = new System.Drawing.Point(177, 29);
            this.SKUNum.Name = "SKUNum";
            this.SKUNum.Size = new System.Drawing.Size(49, 21);
            this.SKUNum.TabIndex = 2;
            this.SKUNum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNum_KeyUp);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(134, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.Text = "数量:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 20);
            this.label5.Text = "品名:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.Text = "SKU:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.DupSum);
            this.tabPage2.Controls.Add(this.ZeroSum);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnConfirmI);
            this.tabPage2.Controls.Add(this.NumSum);
            this.tabPage2.Controls.Add(this.ItemsSum);
            this.tabPage2.Controls.Add(this.CBNOI);
            this.tabPage2.Controls.Add(this.CPCode);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(240, 223);
            this.tabPage2.Text = "上传";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(17, 168);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(118, 20);
            this.label14.Text = "重复输入的品项数:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(17, 136);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 20);
            this.label11.Text = "数量为0的品项数:";
            // 
            // DupSum
            // 
            this.DupSum.Location = new System.Drawing.Point(141, 168);
            this.DupSum.Name = "DupSum";
            this.DupSum.Size = new System.Drawing.Size(70, 20);
            // 
            // ZeroSum
            // 
            this.ZeroSum.Location = new System.Drawing.Point(138, 136);
            this.ZeroSum.Name = "ZeroSum";
            this.ZeroSum.Size = new System.Drawing.Size(70, 20);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(17, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 20);
            this.label10.Text = "总数量:";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(17, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 20);
            this.label9.Text = "总品项:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(17, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 20);
            this.label8.Text = "柜号:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(17, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.Text = "盘点代号:";
            // 
            // btnConfirmI
            // 
            this.btnConfirmI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmI.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirmI.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnConfirmI.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirmI.Location = new System.Drawing.Point(157, 195);
            this.btnConfirmI.Name = "btnConfirmI";
            this.btnConfirmI.Size = new System.Drawing.Size(61, 20);
            this.btnConfirmI.TabIndex = 47;
            this.btnConfirmI.Text = "确 定";
            this.btnConfirmI.Click += new System.EventHandler(this.btnConfirmI_Click);
            // 
            // NumSum
            // 
            this.NumSum.Location = new System.Drawing.Point(80, 104);
            this.NumSum.Name = "NumSum";
            this.NumSum.Size = new System.Drawing.Size(70, 20);
            // 
            // ItemsSum
            // 
            this.ItemsSum.Location = new System.Drawing.Point(80, 74);
            this.ItemsSum.Name = "ItemsSum";
            this.ItemsSum.Size = new System.Drawing.Size(70, 20);
            // 
            // CBNOI
            // 
            this.CBNOI.Location = new System.Drawing.Point(65, 45);
            this.CBNOI.Name = "CBNOI";
            this.CBNOI.Size = new System.Drawing.Size(70, 20);
            // 
            // CPCode
            // 
            this.CPCode.Location = new System.Drawing.Point(93, 16);
            this.CPCode.Name = "CPCode";
            this.CPCode.Size = new System.Drawing.Size(125, 20);
            // 
            // CBNO
            // 
            this.CBNO.Location = new System.Drawing.Point(157, 9);
            this.CBNO.Name = "CBNO";
            this.CBNO.Size = new System.Drawing.Size(55, 20);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(116, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "柜号:";
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 9);
            this.ShopNO.Name = "ShopNO";
            this.ShopNO.Size = new System.Drawing.Size(39, 20);
            // 
            // FormCPSKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.CP_UP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CBNO);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.label2);
            this.Name = "FormCPSKU";
            this.Text = "初盘明细";
            this.TopMost = true;
            this.CP_UP.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.TabControl CP_UP;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SKUNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label ItemsSum;
        private System.Windows.Forms.Label CBNOI;
        private System.Windows.Forms.Label CPCode;
        private System.Windows.Forms.Button btnConfirmI;
        private System.Windows.Forms.Label NumSum;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NexPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Label CBNO;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label ItemsAlr;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SKUName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label DupSum;
        private System.Windows.Forms.Label ZeroSum;
        private System.Windows.Forms.TextBox SKUNO;
    }
}