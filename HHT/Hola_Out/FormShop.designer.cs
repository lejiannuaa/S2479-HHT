namespace Hola_Out
{
    partial class FormShop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShop));
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.labelSKU = new System.Windows.Forms.Label();
            this.labelBC = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.tbCountAvail = new System.Windows.Forms.TextBox();
            this.labelCountAvail = new System.Windows.Forms.Label();
            this.tbSKUName = new System.Windows.Forms.TextBox();
            this.labelSKUName = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btn04 = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.labelFrom = new System.Windows.Forms.Label();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.btn01 = new System.Windows.Forms.Button();
            this.btn03 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 247);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(48, 20);
            this.Page.Text = "1/1";
            this.Page.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // NextPage
            // 
            this.NextPage.BackColor = System.Drawing.Color.NavajoWhite;
            this.NextPage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.NextPage.ForeColor = System.Drawing.Color.Sienna;
            this.NextPage.Location = new System.Drawing.Point(156, 247);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 12;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 247);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 11;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // labelSKU
            // 
            this.labelSKU.Location = new System.Drawing.Point(9, 30);
            this.labelSKU.Name = "labelSKU";
            this.labelSKU.Size = new System.Drawing.Size(37, 20);
            this.labelSKU.Text = "SKU:";
            // 
            // labelBC
            // 
            this.labelBC.Location = new System.Drawing.Point(4, 7);
            this.labelBC.Name = "labelBC";
            this.labelBC.Size = new System.Drawing.Size(37, 20);
            this.labelBC.Text = "箱号:";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(40, 76);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(72, 21);
            this.tbCount.TabIndex = 5;
            this.tbCount.TextChanged += new System.EventHandler(this.tbCount_TextChanged);
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(4, 79);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(37, 20);
            this.labelCount.Text = "数量:";
            // 
            // tbCountAvail
            // 
            this.tbCountAvail.Enabled = false;
            this.tbCountAvail.Location = new System.Drawing.Point(40, 52);
            this.tbCountAvail.Name = "tbCountAvail";
            this.tbCountAvail.Size = new System.Drawing.Size(72, 21);
            this.tbCountAvail.TabIndex = 4;
            // 
            // labelCountAvail
            // 
            this.labelCountAvail.Location = new System.Drawing.Point(4, 46);
            this.labelCountAvail.Name = "labelCountAvail";
            this.labelCountAvail.Size = new System.Drawing.Size(37, 30);
            this.labelCountAvail.Text = "可用库存:";
            // 
            // tbSKUName
            // 
            this.tbSKUName.Enabled = false;
            this.tbSKUName.Location = new System.Drawing.Point(160, 28);
            this.tbSKUName.Name = "tbSKUName";
            this.tbSKUName.Size = new System.Drawing.Size(72, 21);
            this.tbSKUName.TabIndex = 3;
            // 
            // labelSKUName
            // 
            this.labelSKUName.Location = new System.Drawing.Point(124, 31);
            this.labelSKUName.Name = "labelSKUName";
            this.labelSKUName.Size = new System.Drawing.Size(37, 20);
            this.labelSKUName.Text = "品名:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(160, 102);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 20);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "删除选定";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btn04
            // 
            this.btn04.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn04.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn04.ForeColor = System.Drawing.Color.Sienna;
            this.btn04.Location = new System.Drawing.Point(85, 102);
            this.btn04.Name = "btn04";
            this.btn04.Size = new System.Drawing.Size(68, 20);
            this.btn04.TabIndex = 7;
            this.btn04.Text = "列 印";
            this.btn04.Click += new System.EventHandler(this.btn04_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(11, 102);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 20);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "添 加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 143);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 94);
            this.dgTable.TabIndex = 10;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(111, 6);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(52, 20);
            this.labelFrom.Text = "调入店:";
            // 
            // tbFrom
            // 
            this.tbFrom.Enabled = false;
            this.tbFrom.Location = new System.Drawing.Point(160, 4);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(72, 21);
            this.tbFrom.TabIndex = 1;
            this.tbFrom.GotFocus += new System.EventHandler(this.tbFrom_GotFocus);
            this.tbFrom.LostFocus += new System.EventHandler(this.tbFrom_LostFocus);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 125);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 9;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.White;
            this.btn01.Enabled = false;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.Location = new System.Drawing.Point(40, 4);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(72, 21);
            this.btn01.TabIndex = 0;
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(168, 274);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(72, 20);
            this.btn03.TabIndex = 14;
            this.btn03.Text = "提  交";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 13;
            this.btnReturn.Text = "返  回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.White;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.Location = new System.Drawing.Point(40, 28);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 21);
            this.btn02.TabIndex = 22;
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnClear.ForeColor = System.Drawing.Color.Sienna;
            this.btnClear.Location = new System.Drawing.Point(160, 52);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(68, 20);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "清 空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FormShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.tbFrom);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn04);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.tbCountAvail);
            this.Controls.Add(this.labelCountAvail);
            this.Controls.Add(this.tbSKUName);
            this.Controls.Add(this.labelSKUName);
            this.Controls.Add(this.labelSKU);
            this.Controls.Add(this.labelBC);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormShop";
            this.Text = "调拨-门店";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Label labelSKU;
        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox tbCountAvail;
        private System.Windows.Forms.Label labelCountAvail;
        private System.Windows.Forms.TextBox tbSKUName;
        private System.Windows.Forms.Label labelSKUName;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btn04;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.DataGrid dgHeader;
        public System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnClear;
    }
}