namespace Hola_In
{
    partial class FormPOSKU
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
            this.labelBC = new System.Windows.Forms.Label();
            this.labelSKU = new System.Windows.Forms.Label();
            this.tbSKU = new System.Windows.Forms.TextBox();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.tbManufacturerName = new System.Windows.Forms.TextBox();
            this.labelTotal = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.tbManufacturerID = new System.Windows.Forms.TextBox();
            this.btn04 = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btn03 = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.NextPage = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnBC = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.tbState = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.tbManufacturerType = new System.Windows.Forms.TextBox();
            this.btnStore = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelBC
            // 
            this.labelBC.Location = new System.Drawing.Point(7, 9);
            this.labelBC.Name = "labelBC";
            this.labelBC.Size = new System.Drawing.Size(46, 20);
            this.labelBC.Text = "单号:";
            // 
            // labelSKU
            // 
            this.labelSKU.Location = new System.Drawing.Point(11, 33);
            this.labelSKU.Name = "labelSKU";
            this.labelSKU.Size = new System.Drawing.Size(43, 20);
            this.labelSKU.Text = "SKU:";
            // 
            // tbSKU
            // 
            this.tbSKU.Location = new System.Drawing.Point(42, 32);
            this.tbSKU.Name = "tbSKU";
            this.tbSKU.Size = new System.Drawing.Size(68, 21);
            this.tbSKU.TabIndex = 1;
            this.tbSKU.GotFocus += new System.EventHandler(this.tbSKU_GotFocus);
            this.tbSKU.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSKU_KeyUp);
            this.tbSKU.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSKU_KeyPress);
            this.tbSKU.LostFocus += new System.EventHandler(this.tbSKU_LostFocus);
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.Location = new System.Drawing.Point(6, 58);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(54, 20);
            this.labelManufacturer.Text = "厂商:";
            // 
            // tbManufacturerName
            // 
            this.tbManufacturerName.Enabled = false;
            this.tbManufacturerName.Location = new System.Drawing.Point(116, 56);
            this.tbManufacturerName.Name = "tbManufacturerName";
            this.tbManufacturerName.Size = new System.Drawing.Size(69, 21);
            this.tbManufacturerName.TabIndex = 4;
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(127, 33);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(42, 20);
            this.labelTotal.Text = "数量:";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(161, 32);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(75, 21);
            this.tbCount.TabIndex = 2;
            this.tbCount.TextChanged += new System.EventHandler(this.tbCount_TextChanged);
            this.tbCount.GotFocus += new System.EventHandler(this.tbCount_GotFocus);
            this.tbCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCount_KeyPress);
            this.tbCount.LostFocus += new System.EventHandler(this.tbCount_LostFocus);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 144);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 96);
            this.dgTable.TabIndex = 9;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
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
            // tbManufacturerID
            // 
            this.tbManufacturerID.Enabled = false;
            this.tbManufacturerID.Location = new System.Drawing.Point(42, 56);
            this.tbManufacturerID.Name = "tbManufacturerID";
            this.tbManufacturerID.Size = new System.Drawing.Size(68, 21);
            this.tbManufacturerID.TabIndex = 3;
            // 
            // btn04
            // 
            this.btn04.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn04.Enabled = false;
            this.btn04.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn04.ForeColor = System.Drawing.Color.Sienna;
            this.btn04.Location = new System.Drawing.Point(163, 81);
            this.btn04.Name = "btn04";
            this.btn04.Size = new System.Drawing.Size(73, 20);
            this.btn04.TabIndex = 7;
            this.btn04.Text = "列印差异";
            this.btn04.Click += new System.EventHandler(this.btn04_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Enabled = false;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(170, 274);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(70, 20);
            this.btn02.TabIndex = 13;
            this.btn02.Text = "确  认";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Enabled = false;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(79, 81);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(77, 20);
            this.btn03.TabIndex = 6;
            this.btn03.Text = "列印收货";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 245);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 10;
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
            this.NextPage.TabIndex = 11;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 12;
            this.btnReturn.Text = "返  回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 245);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(48, 20);
            this.Page.Text = "1/1";
            this.Page.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbBar
            // 
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 126);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 8;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Enabled = false;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(1, 81);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 20);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "记 录";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnBC
            // 
            this.btnBC.BackColor = System.Drawing.Color.White;
            this.btnBC.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnBC.Location = new System.Drawing.Point(42, 5);
            this.btnBC.Name = "btnBC";
            this.btnBC.Size = new System.Drawing.Size(68, 21);
            this.btnBC.TabIndex = 25;
            this.btnBC.Click += new System.EventHandler(this.btnBC_Click);
            // 
            // labelState
            // 
            this.labelState.Location = new System.Drawing.Point(127, 6);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(48, 20);
            this.labelState.Text = "状态:";
            // 
            // tbState
            // 
            this.tbState.Enabled = false;
            this.tbState.Location = new System.Drawing.Point(161, 5);
            this.tbState.Name = "tbState";
            this.tbState.Size = new System.Drawing.Size(75, 21);
            this.tbState.TabIndex = 29;
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnSelect.Enabled = false;
            this.btnSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnSelect.ForeColor = System.Drawing.Color.Sienna;
            this.btnSelect.Location = new System.Drawing.Point(163, 103);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(73, 20);
            this.btnSelect.TabIndex = 37;
            this.btnSelect.Text = "申请收货";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // tbManufacturerType
            // 
            this.tbManufacturerType.Enabled = false;
            this.tbManufacturerType.Location = new System.Drawing.Point(191, 56);
            this.tbManufacturerType.Name = "tbManufacturerType";
            this.tbManufacturerType.Size = new System.Drawing.Size(45, 21);
            this.tbManufacturerType.TabIndex = 45;
            // 
            // btnStore
            // 
            this.btnStore.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnStore.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnStore.ForeColor = System.Drawing.Color.Sienna;
            this.btnStore.Location = new System.Drawing.Point(1, 103);
            this.btnStore.Name = "btnStore";
            this.btnStore.Size = new System.Drawing.Size(72, 20);
            this.btnStore.TabIndex = 53;
            this.btnStore.Text = "暂 存";
            this.btnStore.Click += new System.EventHandler(this.btnStore_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnRestore.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnRestore.ForeColor = System.Drawing.Color.Sienna;
            this.btnRestore.Location = new System.Drawing.Point(79, 103);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(77, 20);
            this.btnRestore.TabIndex = 54;
            this.btnRestore.Text = "恢 复";
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // FormPOSKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnStore);
            this.Controls.Add(this.tbManufacturerType);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.tbState);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.btnBC);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btn04);
            this.Controls.Add(this.tbManufacturerID);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.tbManufacturerName);
            this.Controls.Add(this.labelManufacturer);
            this.Controls.Add(this.tbSKU);
            this.Controls.Add(this.labelSKU);
            this.Controls.Add(this.labelBC);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormPOSKU";
            this.Text = "PO收货";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPOSKU_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.Label labelSKU;
        private System.Windows.Forms.TextBox tbSKU;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.TextBox tbManufacturerName;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.TextBox tbManufacturerID;
        private System.Windows.Forms.Button btn04;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.Button btnBC;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.TextBox tbState;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox tbManufacturerType;
        private System.Windows.Forms.Button btnStore;
        private System.Windows.Forms.Button btnRestore;
    }
}