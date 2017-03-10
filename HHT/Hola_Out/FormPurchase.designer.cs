namespace Hola_Out
{
    partial class FormPurchase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPurchase));
            this.tbRTV = new System.Windows.Forms.TextBox();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.tbCountAvail = new System.Windows.Forms.TextBox();
            this.labelCountAvail = new System.Windows.Forms.Label();
            this.labelSKUName = new System.Windows.Forms.Label();
            this.btn01 = new System.Windows.Forms.Button();
            this.labelSKU = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btn03 = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbBox = new System.Windows.Forms.TextBox();
            this.tbSKUName = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.labelRTV = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbRTV
            // 
            this.tbRTV.Location = new System.Drawing.Point(42, 26);
            this.tbRTV.Name = "tbRTV";
            this.tbRTV.Size = new System.Drawing.Size(72, 21);
            this.tbRTV.TabIndex = 0;
            this.tbRTV.LostFocus += new System.EventHandler(this.tbRTV_LostFocus);
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(42, 78);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(72, 21);
            this.tbCount.TabIndex = 4;
            this.tbCount.TextChanged += new System.EventHandler(this.tbCount_TextChanged);
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(4, 81);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(37, 20);
            this.labelCount.Text = "数量:";
            // 
            // tbCountAvail
            // 
            this.tbCountAvail.Enabled = false;
            this.tbCountAvail.Location = new System.Drawing.Point(160, 3);
            this.tbCountAvail.Name = "tbCountAvail";
            this.tbCountAvail.Size = new System.Drawing.Size(72, 21);
            this.tbCountAvail.TabIndex = 1;
            // 
            // labelCountAvail
            // 
            this.labelCountAvail.Location = new System.Drawing.Point(124, 0);
            this.labelCountAvail.Name = "labelCountAvail";
            this.labelCountAvail.Size = new System.Drawing.Size(37, 30);
            this.labelCountAvail.Text = "可用库存:";
            // 
            // labelSKUName
            // 
            this.labelSKUName.Location = new System.Drawing.Point(124, 29);
            this.labelSKUName.Name = "labelSKUName";
            this.labelSKUName.Size = new System.Drawing.Size(37, 20);
            this.labelSKUName.Text = "品名:";
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.Location = new System.Drawing.Point(42, 51);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(72, 21);
            this.btn01.TabIndex = 2;
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // labelSKU
            // 
            this.labelSKU.Location = new System.Drawing.Point(5, 53);
            this.labelSKU.Name = "labelSKU";
            this.labelSKU.Size = new System.Drawing.Size(37, 20);
            this.labelSKU.Text = "SKU:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(163, 106);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 20);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "删除选定";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(87, 106);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(68, 20);
            this.btn03.TabIndex = 8;
            this.btn03.Text = "列 印";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(10, 106);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 20);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "添 加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 246);
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
            this.NextPage.Location = new System.Drawing.Point(156, 246);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 13;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 246);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 12;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 146);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 96);
            this.dgTable.TabIndex = 11;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 128);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 10;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(168, 274);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 20);
            this.btn02.TabIndex = 15;
            this.btn02.Text = "确  定";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 14;
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 20);
            this.label1.Text = "箱号:";
            // 
            // tbBox
            // 
            this.tbBox.Enabled = false;
            this.tbBox.Location = new System.Drawing.Point(43, 3);
            this.tbBox.Name = "tbBox";
            this.tbBox.Size = new System.Drawing.Size(72, 21);
            this.tbBox.TabIndex = 104;
            // 
            // tbSKUName
            // 
            this.tbSKUName.Enabled = false;
            this.tbSKUName.Location = new System.Drawing.Point(160, 26);
            this.tbSKUName.Name = "tbSKUName";
            this.tbSKUName.Size = new System.Drawing.Size(72, 21);
            this.tbSKUName.TabIndex = 114;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnClear.ForeColor = System.Drawing.Color.Sienna;
            this.btnClear.Location = new System.Drawing.Point(160, 53);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(68, 20);
            this.btnClear.TabIndex = 123;
            this.btnClear.Text = "清 空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // labelRTV
            // 
            this.labelRTV.Location = new System.Drawing.Point(4, 20);
            this.labelRTV.Name = "labelRTV";
            this.labelRTV.Size = new System.Drawing.Size(37, 30);
            this.labelRTV.Text = "出货单号:";
            // 
            // FormPurchase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.labelRTV);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.tbSKUName);
            this.Controls.Add(this.tbBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.tbRTV);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.tbCountAvail);
            this.Controls.Add(this.labelCountAvail);
            this.Controls.Add(this.labelSKUName);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.labelSKU);
            this.MinimizeBox = false;
            this.Name = "FormPurchase";
            this.Text = "采购出货";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbRTV;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox tbCountAvail;
        private System.Windows.Forms.Label labelCountAvail;
        private System.Windows.Forms.Label labelSKUName;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Label labelSKU;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbBox;
        private System.Windows.Forms.TextBox tbSKUName;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label labelRTV;
    }
}