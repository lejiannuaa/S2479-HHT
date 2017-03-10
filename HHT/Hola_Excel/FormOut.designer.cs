namespace Hola_Excel
{
    partial class FormOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOut));
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.labelBCBoxFrom = new System.Windows.Forms.Label();
            this.tbBCDanFrom = new System.Windows.Forms.TextBox();
            this.labelBCDanFrom = new System.Windows.Forms.Label();
            this.labelDateTo = new System.Windows.Forms.Label();
            this.labelDateFrom = new System.Windows.Forms.Label();
            this.labelBCDanTo = new System.Windows.Forms.Label();
            this.tbBCDanTo = new System.Windows.Forms.TextBox();
            this.tbBCBoxFrom = new System.Windows.Forms.TextBox();
            this.labelBCBoxTo = new System.Windows.Forms.Label();
            this.tbBCBoxTo = new System.Windows.Forms.TextBox();
            this.labelType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.labelState = new System.Windows.Forms.Label();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.photoMenu = new System.Windows.Forms.MenuItem();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btn01 = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.labelFLocation = new System.Windows.Forms.Label();
            this.cbFromLocation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.dtpTo.CustomFormat = "yyyy-MM-dd";
            this.dtpTo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(161, 31);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(72, 20);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.Value = new System.DateTime(2013, 12, 31, 0, 0, 0, 0);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy-MM-dd";
            this.dtpFrom.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(49, 31);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(72, 20);
            this.dtpFrom.TabIndex = 2;
            this.dtpFrom.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            // 
            // labelBCBoxFrom
            // 
            this.labelBCBoxFrom.Location = new System.Drawing.Point(14, 57);
            this.labelBCBoxFrom.Name = "labelBCBoxFrom";
            this.labelBCBoxFrom.Size = new System.Drawing.Size(64, 20);
            this.labelBCBoxFrom.Text = "箱号:";
            // 
            // tbBCDanFrom
            // 
            this.tbBCDanFrom.Location = new System.Drawing.Point(49, 7);
            this.tbBCDanFrom.Name = "tbBCDanFrom";
            this.tbBCDanFrom.Size = new System.Drawing.Size(72, 21);
            this.tbBCDanFrom.TabIndex = 0;
            this.tbBCDanFrom.LostFocus += new System.EventHandler(this.tbBCDanFrom_LostFocus);
            // 
            // labelBCDanFrom
            // 
            this.labelBCDanFrom.Location = new System.Drawing.Point(15, 10);
            this.labelBCDanFrom.Name = "labelBCDanFrom";
            this.labelBCDanFrom.Size = new System.Drawing.Size(64, 20);
            this.labelBCDanFrom.Text = "单号:";
            // 
            // labelDateTo
            // 
            this.labelDateTo.Location = new System.Drawing.Point(140, 32);
            this.labelDateTo.Name = "labelDateTo";
            this.labelDateTo.Size = new System.Drawing.Size(33, 20);
            this.labelDateTo.Text = "至";
            // 
            // labelDateFrom
            // 
            this.labelDateFrom.Location = new System.Drawing.Point(14, 34);
            this.labelDateFrom.Name = "labelDateFrom";
            this.labelDateFrom.Size = new System.Drawing.Size(54, 16);
            this.labelDateFrom.Text = "日期:";
            // 
            // labelBCDanTo
            // 
            this.labelBCDanTo.Location = new System.Drawing.Point(140, 7);
            this.labelBCDanTo.Name = "labelBCDanTo";
            this.labelBCDanTo.Size = new System.Drawing.Size(33, 20);
            this.labelBCDanTo.Text = "至";
            // 
            // tbBCDanTo
            // 
            this.tbBCDanTo.Location = new System.Drawing.Point(161, 7);
            this.tbBCDanTo.Name = "tbBCDanTo";
            this.tbBCDanTo.Size = new System.Drawing.Size(72, 21);
            this.tbBCDanTo.TabIndex = 1;
            this.tbBCDanTo.LostFocus += new System.EventHandler(this.tbBCDanTo_LostFocus);
            // 
            // tbBCBoxFrom
            // 
            this.tbBCBoxFrom.Location = new System.Drawing.Point(49, 55);
            this.tbBCBoxFrom.Name = "tbBCBoxFrom";
            this.tbBCBoxFrom.Size = new System.Drawing.Size(72, 21);
            this.tbBCBoxFrom.TabIndex = 4;
            this.tbBCBoxFrom.LostFocus += new System.EventHandler(this.tbBCBoxFrom_LostFocus);
            // 
            // labelBCBoxTo
            // 
            this.labelBCBoxTo.Location = new System.Drawing.Point(140, 56);
            this.labelBCBoxTo.Name = "labelBCBoxTo";
            this.labelBCBoxTo.Size = new System.Drawing.Size(33, 20);
            this.labelBCBoxTo.Text = "至";
            // 
            // tbBCBoxTo
            // 
            this.tbBCBoxTo.Location = new System.Drawing.Point(161, 55);
            this.tbBCBoxTo.Name = "tbBCBoxTo";
            this.tbBCBoxTo.Size = new System.Drawing.Size(72, 21);
            this.tbBCBoxTo.TabIndex = 5;
            this.tbBCBoxTo.LostFocus += new System.EventHandler(this.tbBCBoxTo_LostFocus);
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(14, 81);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(52, 20);
            this.labelType.Text = "类型:";
            // 
            // cbType
            // 
            this.cbType.Items.Add("RTV退货");
            this.cbType.Items.Add("TRF调拨");
            this.cbType.Location = new System.Drawing.Point(49, 79);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(72, 22);
            this.cbType.TabIndex = 6;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // labelState
            // 
            this.labelState.Location = new System.Drawing.Point(123, 81);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(50, 20);
            this.labelState.Text = "状态:";
            // 
            // cbState
            // 
            this.cbState.Items.Add("");
            this.cbState.Items.Add("成功");
            this.cbState.Items.Add("失败");
            this.cbState.Location = new System.Drawing.Point(161, 79);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(72, 22);
            this.cbState.TabIndex = 7;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menuDetail);
            this.contextMenu1.MenuItems.Add(this.photoMenu);
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuDetail
            // 
            this.menuDetail.Text = "详细";
            this.menuDetail.Click += new System.EventHandler(this.menuDetail_Click);
            // 
            // photoMenu
            // 
            this.photoMenu.Text = "照相";
            this.photoMenu.Click += new System.EventHandler(photoMenu_Click);

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
            this.NextPage.TabIndex = 11;
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
            this.PrePage.TabIndex = 10;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
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
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(168, 274);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(72, 20);
            this.btn01.TabIndex = 13;
            this.btn01.Text = "查 询";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 270);
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
            this.dgHeader.Location = new System.Drawing.Point(0, 131);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 8;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 148);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 96);
            this.dgTable.TabIndex = 9;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // labelFLocation
            // 
            this.labelFLocation.Location = new System.Drawing.Point(1, 105);
            this.labelFLocation.Name = "labelFLocation";
            this.labelFLocation.Size = new System.Drawing.Size(52, 20);
            this.labelFLocation.Text = "发起方:";
            // 
            // cbFromLocation
            // 
            this.cbFromLocation.Items.Add("HHT");
            this.cbFromLocation.Items.Add("JDA");
            this.cbFromLocation.Location = new System.Drawing.Point(49, 104);
            this.cbFromLocation.Name = "cbFromLocation";
            this.cbFromLocation.Size = new System.Drawing.Size(72, 22);
            this.cbFromLocation.TabIndex = 25;
            this.cbFromLocation.SelectedIndexChanged += new System.EventHandler(this.cbFromLocation_SelectedIndexChanged);
            // 
            // FormOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.cbFromLocation);
            this.Controls.Add(this.labelFLocation);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.cbState);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.tbBCBoxTo);
            this.Controls.Add(this.labelBCBoxTo);
            this.Controls.Add(this.tbBCBoxFrom);
            this.Controls.Add(this.tbBCDanTo);
            this.Controls.Add(this.labelBCDanTo);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.labelBCBoxFrom);
            this.Controls.Add(this.tbBCDanFrom);
            this.Controls.Add(this.labelBCDanFrom);
            this.Controls.Add(this.labelDateTo);
            this.Controls.Add(this.labelDateFrom);
            this.KeyPreview = true;
            this.Name = "FormOut";
            this.Text = "出货查询";
            this.TopMost = true;
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label labelBCBoxFrom;
        private System.Windows.Forms.TextBox tbBCDanFrom;
        private System.Windows.Forms.Label labelBCDanFrom;
        private System.Windows.Forms.Label labelDateTo;
        private System.Windows.Forms.Label labelDateFrom;
        private System.Windows.Forms.Label labelBCDanTo;
        private System.Windows.Forms.TextBox tbBCDanTo;
        private System.Windows.Forms.TextBox tbBCBoxFrom;
        private System.Windows.Forms.Label labelBCBoxTo;
        private System.Windows.Forms.TextBox tbBCBoxTo;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ComboBox cbState;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
        private System.Windows.Forms.MenuItem photoMenu;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Label labelFLocation;
        private System.Windows.Forms.ComboBox cbFromLocation;
    }
}