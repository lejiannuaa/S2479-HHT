namespace Hola_In
{
    partial class FormDBSKU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDBSKU));
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.NextPage = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.btn03 = new System.Windows.Forms.Button();
            this.btn04 = new System.Windows.Forms.Button();
            this.btnBC = new System.Windows.Forms.Button();
            this.tbState = new System.Windows.Forms.TextBox();
            this.lableState = new System.Windows.Forms.Label();
            this.lableBC = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.tbSKU = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.labelSKU = new System.Windows.Forms.Label();
            this.btn00103 = new System.Windows.Forms.Button();
            this.btn00104 = new System.Windows.Forms.Button();
            this.btn00106 = new System.Windows.Forms.Button();
            this.Camera = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 129);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 96);
            this.dgTable.TabIndex = 5;
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
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Enabled = false;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(1, 85);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 20);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "记 录";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 246);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 6;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // NextPage
            // 
            this.NextPage.BackColor = System.Drawing.Color.NavajoWhite;
            this.NextPage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.NextPage.ForeColor = System.Drawing.Color.Sienna;
            this.NextPage.Location = new System.Drawing.Point(156, 246);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 7;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Enabled = false;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(168, 274);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 20);
            this.btn02.TabIndex = 9;
            this.btn02.Text = "确 认";
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
            this.btnReturn.TabIndex = 8;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 246);
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
            this.dgHeader.Location = new System.Drawing.Point(0, 111);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 4;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Enabled = false;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(66, 85);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(62, 20);
            this.btn03.TabIndex = 12;
            this.btn03.Text = "列印收货";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // btn04
            // 
            this.btn04.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn04.Enabled = false;
            this.btn04.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn04.ForeColor = System.Drawing.Color.Sienna;
            this.btn04.Location = new System.Drawing.Point(130, 85);
            this.btn04.Name = "btn04";
            this.btn04.Size = new System.Drawing.Size(62, 20);
            this.btn04.TabIndex = 13;
            this.btn04.Text = "列印差异";
            this.btn04.Click += new System.EventHandler(this.btn04_Click);
            // 
            // btnBC
            // 
            this.btnBC.BackColor = System.Drawing.Color.White;
            this.btnBC.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnBC.Location = new System.Drawing.Point(44, 3);
            this.btnBC.Name = "btnBC";
            this.btnBC.Size = new System.Drawing.Size(79, 21);
            this.btnBC.TabIndex = 20;
            this.btnBC.Click += new System.EventHandler(this.btnBC_Click);
            // 
            // tbState
            // 
            this.tbState.Enabled = false;
            this.tbState.Location = new System.Drawing.Point(162, 4);
            this.tbState.Name = "tbState";
            this.tbState.Size = new System.Drawing.Size(69, 21);
            this.tbState.TabIndex = 21;
            // 
            // lableState
            // 
            this.lableState.Location = new System.Drawing.Point(127, 7);
            this.lableState.Name = "lableState";
            this.lableState.Size = new System.Drawing.Size(47, 20);
            this.lableState.Text = "状态:";
            // 
            // lableBC
            // 
            this.lableBC.Location = new System.Drawing.Point(9, 7);
            this.lableBC.Name = "lableBC";
            this.lableBC.Size = new System.Drawing.Size(49, 20);
            this.lableBC.Text = "箱号:";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(162, 29);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(69, 21);
            this.tbCount.TabIndex = 27;
            this.tbCount.GotFocus += new System.EventHandler(this.tbCount_GotFocus);
            this.tbCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCount_KeyPress);
            this.tbCount.LostFocus += new System.EventHandler(this.tbCount_LostFocus);
            // 
            // tbSKU
            // 
            this.tbSKU.Location = new System.Drawing.Point(44, 31);
            this.tbSKU.Name = "tbSKU";
            this.tbSKU.Size = new System.Drawing.Size(79, 21);
            this.tbSKU.TabIndex = 26;
            this.tbSKU.GotFocus += new System.EventHandler(this.tbSKU_GotFocus);
            this.tbSKU.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSKU_KeyUp);
            this.tbSKU.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSKU_KeyPress);
            this.tbSKU.LostFocus += new System.EventHandler(this.tbSKU_LostFocus);
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(127, 32);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(46, 20);
            this.labelCount.Text = "数量:";
            // 
            // labelSKU
            // 
            this.labelSKU.Location = new System.Drawing.Point(11, 34);
            this.labelSKU.Name = "labelSKU";
            this.labelSKU.Size = new System.Drawing.Size(40, 20);
            this.labelSKU.Text = "SKU:";
            // 
            // btn00103
            // 
            this.btn00103.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn00103.Enabled = false;
            this.btn00103.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn00103.ForeColor = System.Drawing.Color.Sienna;
            this.btn00103.Location = new System.Drawing.Point(129, 59);
            this.btn00103.Name = "btn00103";
            this.btn00103.Size = new System.Drawing.Size(62, 20);
            this.btn00103.TabIndex = 32;
            this.btn00103.Text = "无该箱";
            this.btn00103.Click += new System.EventHandler(this.btn00103_Click);
            // 
            // btn00104
            // 
            this.btn00104.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn00104.Enabled = false;
            this.btn00104.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn00104.ForeColor = System.Drawing.Color.Sienna;
            this.btn00104.Location = new System.Drawing.Point(65, 59);
            this.btn00104.Name = "btn00104";
            this.btn00104.Size = new System.Drawing.Size(62, 20);
            this.btn00104.TabIndex = 31;
            this.btn00104.Text = "整箱破损";
            this.btn00104.Click += new System.EventHandler(this.btn00104_Click);
            // 
            // btn00106
            // 
            this.btn00106.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn00106.Enabled = false;
            this.btn00106.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn00106.ForeColor = System.Drawing.Color.Sienna;
            this.btn00106.Location = new System.Drawing.Point(1, 59);
            this.btn00106.Name = "btn00106";
            this.btn00106.Size = new System.Drawing.Size(62, 20);
            this.btn00106.TabIndex = 30;
            this.btn00106.Text = "解除收货";
            this.btn00106.Click += new System.EventHandler(this.btn00106_Click);
            // 
            // Camera
            // 
            this.Camera.Image = ((System.Drawing.Image)(resources.GetObject("Camera.Image")));
            this.Camera.Location = new System.Drawing.Point(197, 59);
            this.Camera.Name = "Camera";
            this.Camera.Size = new System.Drawing.Size(40, 46);
            this.Camera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Camera.Click += new System.EventHandler(this.Camera_Click);
            // 
            // FormDBSKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.Camera);
            this.Controls.Add(this.btn00103);
            this.Controls.Add(this.btn00104);
            this.Controls.Add(this.btn00106);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.tbSKU);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.labelSKU);
            this.Controls.Add(this.btnBC);
            this.Controls.Add(this.tbState);
            this.Controls.Add(this.lableState);
            this.Controls.Add(this.lableBC);
            this.Controls.Add(this.btn04);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgTable);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormDBSKU";
            this.Text = "调拨收货";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormDBSKU_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Button btn04;
        private System.Windows.Forms.Button btnBC;
        private System.Windows.Forms.TextBox tbState;
        private System.Windows.Forms.Label lableState;
        private System.Windows.Forms.Label lableBC;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.TextBox tbSKU;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelSKU;
        private System.Windows.Forms.Button btn00103;
        private System.Windows.Forms.Button btn00104;
        private System.Windows.Forms.Button btn00106;
        private System.Windows.Forms.PictureBox Camera;
    }
}