namespace Hola_Out
{
    partial class FormManufacturer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManufacturer));
            this.labelBC = new System.Windows.Forms.Label();
            this.labelPriceAvail = new System.Windows.Forms.Label();
            this.tbPriceAvail = new System.Windows.Forms.TextBox();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.labelSKU = new System.Windows.Forms.Label();
            this.labelCountAvail = new System.Windows.Forms.Label();
            this.btn02 = new System.Windows.Forms.Button();
            this.btn03 = new System.Windows.Forms.Button();
            this.labelSKUName = new System.Windows.Forms.Label();
            this.tbSKUName = new System.Windows.Forms.TextBox();
            this.tbCountAvail = new System.Windows.Forms.TextBox();
            this.labelPrice = new System.Windows.Forms.Label();
            this.tbPrice = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.btn05 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.PrePage = new System.Windows.Forms.Button();
            this.NextPage = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.cbReason = new System.Windows.Forms.ComboBox();
            this.labelReason = new System.Windows.Forms.Label();
            this.btn01 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btn04 = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.tbManufacturerName = new System.Windows.Forms.TextBox();
            this.btnReason = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelBC
            // 
            this.labelBC.Location = new System.Drawing.Point(4, 13);
            this.labelBC.Name = "labelBC";
            this.labelBC.Size = new System.Drawing.Size(37, 20);
            this.labelBC.Text = "箱号:";
            // 
            // labelPriceAvail
            // 
            this.labelPriceAvail.Location = new System.Drawing.Point(114, 5);
            this.labelPriceAvail.Name = "labelPriceAvail";
            this.labelPriceAvail.Size = new System.Drawing.Size(37, 31);
            this.labelPriceAvail.Text = "可退金额:";
            // 
            // tbPriceAvail
            // 
            this.tbPriceAvail.Enabled = false;
            this.tbPriceAvail.Location = new System.Drawing.Point(157, 11);
            this.tbPriceAvail.Name = "tbPriceAvail";
            this.tbPriceAvail.Size = new System.Drawing.Size(76, 21);
            this.tbPriceAvail.TabIndex = 1;
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.Location = new System.Drawing.Point(4, 37);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(37, 20);
            this.labelManufacturer.Text = "厂商:";
            // 
            // labelSKU
            // 
            this.labelSKU.Location = new System.Drawing.Point(8, 63);
            this.labelSKU.Name = "labelSKU";
            this.labelSKU.Size = new System.Drawing.Size(37, 20);
            this.labelSKU.Text = "SKU:";
            // 
            // labelCountAvail
            // 
            this.labelCountAvail.Location = new System.Drawing.Point(3, 83);
            this.labelCountAvail.Name = "labelCountAvail";
            this.labelCountAvail.Size = new System.Drawing.Size(37, 29);
            this.labelCountAvail.Text = "可用库存:";
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.Location = new System.Drawing.Point(41, 35);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(68, 21);
            this.btn02.TabIndex = 2;
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.Location = new System.Drawing.Point(41, 60);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(68, 21);
            this.btn03.TabIndex = 4;
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // labelSKUName
            // 
            this.labelSKUName.Location = new System.Drawing.Point(114, 63);
            this.labelSKUName.Name = "labelSKUName";
            this.labelSKUName.Size = new System.Drawing.Size(37, 20);
            this.labelSKUName.Text = "品名:";
            // 
            // tbSKUName
            // 
            this.tbSKUName.Enabled = false;
            this.tbSKUName.Location = new System.Drawing.Point(157, 60);
            this.tbSKUName.Name = "tbSKUName";
            this.tbSKUName.Size = new System.Drawing.Size(76, 21);
            this.tbSKUName.TabIndex = 5;
            // 
            // tbCountAvail
            // 
            this.tbCountAvail.Enabled = false;
            this.tbCountAvail.Location = new System.Drawing.Point(41, 87);
            this.tbCountAvail.Name = "tbCountAvail";
            this.tbCountAvail.Size = new System.Drawing.Size(68, 21);
            this.tbCountAvail.TabIndex = 6;
            // 
            // labelPrice
            // 
            this.labelPrice.Location = new System.Drawing.Point(114, 82);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(37, 29);
            this.labelPrice.Text = "已退金额:";
            // 
            // tbPrice
            // 
            this.tbPrice.Enabled = false;
            this.tbPrice.Location = new System.Drawing.Point(157, 87);
            this.tbPrice.Name = "tbPrice";
            this.tbPrice.Size = new System.Drawing.Size(76, 21);
            this.tbPrice.TabIndex = 7;
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(3, 117);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(37, 20);
            this.labelCount.Text = "数量:";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(41, 114);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(68, 21);
            this.tbCount.TabIndex = 8;
            this.tbCount.TextChanged += new System.EventHandler(this.tbCount_TextChanged);
            // 
            // btn05
            // 
            this.btn05.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn05.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn05.ForeColor = System.Drawing.Color.Sienna;
            this.btn05.Location = new System.Drawing.Point(87, 141);
            this.btn05.Name = "btn05";
            this.btn05.Size = new System.Drawing.Size(68, 20);
            this.btn05.TabIndex = 12;
            this.btn05.Text = "列 印";
            this.btn05.Click += new System.EventHandler(this.btn05_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(160, 141);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 20);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "删除选定";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(11, 141);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 20);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "添 加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 182);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 60);
            this.dgTable.TabIndex = 15;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 245);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 16;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // NextPage
            // 
            this.NextPage.BackColor = System.Drawing.Color.NavajoWhite;
            this.NextPage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.NextPage.ForeColor = System.Drawing.Color.Sienna;
            this.NextPage.Location = new System.Drawing.Point(156, 245);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 17;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 245);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(48, 20);
            this.Page.Text = "1/1";
            this.Page.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 164);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 14;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // cbReason
            // 
            this.cbReason.Enabled = false;
            this.cbReason.Location = new System.Drawing.Point(157, 113);
            this.cbReason.Name = "cbReason";
            this.cbReason.Size = new System.Drawing.Size(72, 22);
            this.cbReason.TabIndex = 10;
            // 
            // labelReason
            // 
            this.labelReason.Location = new System.Drawing.Point(114, 111);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(37, 29);
            this.labelReason.Text = "退货原因:";
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.White;
            this.btn01.Enabled = false;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.Location = new System.Drawing.Point(41, 11);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(68, 21);
            this.btn01.TabIndex = 0;
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 18;
            this.btnReturn.Text = "返  回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btn04
            // 
            this.btn04.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn04.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn04.ForeColor = System.Drawing.Color.Sienna;
            this.btn04.Location = new System.Drawing.Point(168, 274);
            this.btn04.Name = "btn04";
            this.btn04.Size = new System.Drawing.Size(72, 20);
            this.btn04.TabIndex = 19;
            this.btn04.Text = "提  交";
            this.btn04.Click += new System.EventHandler(this.btn04_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // tbManufacturerName
            // 
            this.tbManufacturerName.Enabled = false;
            this.tbManufacturerName.Location = new System.Drawing.Point(114, 35);
            this.tbManufacturerName.Name = "tbManufacturerName";
            this.tbManufacturerName.Size = new System.Drawing.Size(119, 21);
            this.tbManufacturerName.TabIndex = 3;
            // 
            // btnReason
            // 
            this.btnReason.BackColor = System.Drawing.Color.White;
            this.btnReason.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReason.Location = new System.Drawing.Point(157, 113);
            this.btnReason.Name = "btnReason";
            this.btnReason.Size = new System.Drawing.Size(76, 22);
            this.btnReason.TabIndex = 9;
            this.btnReason.Text = "请选择";
            this.btnReason.Click += new System.EventHandler(this.btnReason_Click);
            // 
            // FormManufacturer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.btnReason);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn04);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.labelReason);
            this.Controls.Add(this.cbReason);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn05);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.tbPrice);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.tbCountAvail);
            this.Controls.Add(this.tbSKUName);
            this.Controls.Add(this.labelSKUName);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.tbManufacturerName);
            this.Controls.Add(this.labelCountAvail);
            this.Controls.Add(this.labelSKU);
            this.Controls.Add(this.labelManufacturer);
            this.Controls.Add(this.tbPriceAvail);
            this.Controls.Add(this.labelPriceAvail);
            this.Controls.Add(this.labelBC);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormManufacturer";
            this.Text = "厂商/DC";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.Label labelPriceAvail;
        private System.Windows.Forms.TextBox tbPriceAvail;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.Label labelSKU;
        private System.Windows.Forms.Label labelCountAvail;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Label labelSKUName;
        private System.Windows.Forms.TextBox tbSKUName;
        private System.Windows.Forms.TextBox tbCountAvail;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.TextBox tbPrice;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Button btn05;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.ComboBox cbReason;
        private System.Windows.Forms.Label labelReason;
        public System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btn04;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.TextBox tbManufacturerName;
        public System.Windows.Forms.Button btnReason;
    }
}