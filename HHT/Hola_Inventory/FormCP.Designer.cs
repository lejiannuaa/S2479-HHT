namespace Hola_Inventory
{
    partial class FormCP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCP));
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Ent_Agn = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.EntCBNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CurrCBNO = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Page = new System.Windows.Forms.Label();
            this.NexPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.CPCode = new System.Windows.Forms.ComboBox();
            this.ShopNO = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCpCode = new System.Windows.Forms.Button();
            this.Ent_Agn.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 32);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.Text = "盘点代号:";
            // 
            // Ent_Agn
            // 
            this.Ent_Agn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ent_Agn.Controls.Add(this.tabPage1);
            this.Ent_Agn.Controls.Add(this.tabPage2);
            this.Ent_Agn.Dock = System.Windows.Forms.DockStyle.None;
            this.Ent_Agn.Location = new System.Drawing.Point(0, 99);
            this.Ent_Agn.Name = "Ent_Agn";
            this.Ent_Agn.SelectedIndex = 0;
            this.Ent_Agn.Size = new System.Drawing.Size(240, 195);
            this.Ent_Agn.TabIndex = 16;
            this.Ent_Agn.SelectedIndexChanged += new System.EventHandler(this.Ent_Agn_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnReturn);
            this.tabPage1.Controls.Add(this.btnConfirm);
            this.tabPage1.Controls.Add(this.EntCBNO);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.CurrCBNO);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 172);
            this.tabPage1.Text = "输入";
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(141, 135);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 21;
            this.btnReturn.Text = "离 开";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnConfirm.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirm.Location = new System.Drawing.Point(20, 135);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(72, 20);
            this.btnConfirm.TabIndex = 20;
            this.btnConfirm.Text = "确 定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // EntCBNO
            // 
            this.EntCBNO.Location = new System.Drawing.Point(75, 30);
            this.EntCBNO.Name = "EntCBNO";
            this.EntCBNO.Size = new System.Drawing.Size(138, 21);
            this.EntCBNO.TabIndex = 19;
            this.EntCBNO.TextChanged += new System.EventHandler(this.EntCBNO_TextChanged);
            this.EntCBNO.GotFocus += new System.EventHandler(this.EntCBNO_GotFocus);
            this.EntCBNO.LostFocus += new System.EventHandler(this.EntCBNO_LostFocus);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.Text = "初盘柜号:";
            // 
            // CurrCBNO
            // 
            this.CurrCBNO.Location = new System.Drawing.Point(75, 1);
            this.CurrCBNO.Name = "CurrCBNO";
            this.CurrCBNO.ReadOnly = true;
            this.CurrCBNO.Size = new System.Drawing.Size(138, 21);
            this.CurrCBNO.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.Text = "目前柜号:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Page);
            this.tabPage2.Controls.Add(this.NexPage);
            this.tabPage2.Controls.Add(this.PrePage);
            this.tabPage2.Controls.Add(this.dgTable);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(232, 169);
            this.tabPage2.Text = "需盘点柜号";
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(93, 175);
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
            this.NexPage.Location = new System.Drawing.Point(141, 175);
            this.NexPage.Name = "NexPage";
            this.NexPage.Size = new System.Drawing.Size(72, 20);
            this.NexPage.TabIndex = 22;
            this.NexPage.Text = "下一页";
            this.NexPage.Click += new System.EventHandler(this.NexPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(20, 175);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 21;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgTable.HeaderBackColor = System.Drawing.Color.NavajoWhite;
            this.dgTable.HeaderForeColor = System.Drawing.Color.Sienna;
            this.dgTable.Location = new System.Drawing.Point(0, 0);
            this.dgTable.Name = "dgTable";
            this.dgTable.Size = new System.Drawing.Size(240, 169);
            this.dgTable.TabIndex = 0;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            // 
            // CPCode
            // 
            this.CPCode.Location = new System.Drawing.Point(75, 71);
            this.CPCode.Name = "CPCode";
            this.CPCode.Size = new System.Drawing.Size(138, 22);
            this.CPCode.TabIndex = 21;
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
            // btnCpCode
            // 
            this.btnCpCode.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCpCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnCpCode.Location = new System.Drawing.Point(75, 71);
            this.btnCpCode.Name = "btnCpCode";
            this.btnCpCode.Size = new System.Drawing.Size(138, 22);
            this.btnCpCode.TabIndex = 24;
            this.btnCpCode.Text = "请选择";
            this.btnCpCode.Click += new System.EventHandler(this.btnCpCode_Click);
            // 
            // FormCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnCpCode);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CPCode);
            this.Controls.Add(this.Ent_Agn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbBar);
            this.Name = "FormCP";
            this.Text = "初盘";
            this.TopMost = true;
            this.Ent_Agn.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl Ent_Agn;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox EntCBNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CurrCBNO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.ComboBox CPCode;
        private System.Windows.Forms.Button NexPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCpCode;
    }
}