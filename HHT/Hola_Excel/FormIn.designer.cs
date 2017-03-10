namespace Hola_Excel
{
    partial class FormIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIn));
            this.labelDateFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.labelDateTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.tbBCDanTo = new System.Windows.Forms.TextBox();
            this.labelBCDanTo = new System.Windows.Forms.Label();
            this.tbBCDanFrom = new System.Windows.Forms.TextBox();
            this.labelBCDanFrom = new System.Windows.Forms.Label();
            this.labelOpUsr = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.btn01 = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.cbOpUsr = new System.Windows.Forms.ComboBox();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // labelDateFrom
            // 
            this.labelDateFrom.Location = new System.Drawing.Point(12, 5);
            this.labelDateFrom.Name = "labelDateFrom";
            this.labelDateFrom.Size = new System.Drawing.Size(44, 33);
            this.labelDateFrom.Text = "完成日期:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy-MM-dd";
            this.dtpFrom.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(50, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(72, 20);
            this.dtpFrom.TabIndex = 0;
            this.dtpFrom.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            // 
            // labelDateTo
            // 
            this.labelDateTo.Location = new System.Drawing.Point(132, 14);
            this.labelDateTo.Name = "labelDateTo";
            this.labelDateTo.Size = new System.Drawing.Size(33, 20);
            this.labelDateTo.Text = "至";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy-MM-dd";
            this.dtpTo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(156, 12);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(72, 20);
            this.dtpTo.TabIndex = 1;
            this.dtpTo.Value = new System.DateTime(2013, 12, 31, 0, 0, 0, 0);
            // 
            // tbBCDanTo
            // 
            this.tbBCDanTo.Location = new System.Drawing.Point(156, 39);
            this.tbBCDanTo.Name = "tbBCDanTo";
            this.tbBCDanTo.Size = new System.Drawing.Size(72, 21);
            this.tbBCDanTo.TabIndex = 3;
            this.tbBCDanTo.LostFocus += new System.EventHandler(this.tbBCDanTo_LostFocus);
            // 
            // labelBCDanTo
            // 
            this.labelBCDanTo.Location = new System.Drawing.Point(132, 42);
            this.labelBCDanTo.Name = "labelBCDanTo";
            this.labelBCDanTo.Size = new System.Drawing.Size(33, 20);
            this.labelBCDanTo.Text = "至";
            // 
            // tbBCDanFrom
            // 
            this.tbBCDanFrom.Location = new System.Drawing.Point(50, 39);
            this.tbBCDanFrom.Name = "tbBCDanFrom";
            this.tbBCDanFrom.Size = new System.Drawing.Size(72, 21);
            this.tbBCDanFrom.TabIndex = 2;
            this.tbBCDanFrom.LostFocus += new System.EventHandler(this.tbBCDanFrom_LostFocus);
            // 
            // labelBCDanFrom
            // 
            this.labelBCDanFrom.Location = new System.Drawing.Point(11, 43);
            this.labelBCDanFrom.Name = "labelBCDanFrom";
            this.labelBCDanFrom.Size = new System.Drawing.Size(64, 20);
            this.labelBCDanFrom.Text = "单号:";
            // 
            // labelOpUsr
            // 
            this.labelOpUsr.Location = new System.Drawing.Point(-2, 69);
            this.labelOpUsr.Name = "labelOpUsr";
            this.labelOpUsr.Size = new System.Drawing.Size(71, 20);
            this.labelOpUsr.Text = "操作人:";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(121, 69);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(51, 20);
            this.labelType.Text = "类型:";
            // 
            // cbType
            // 
            this.cbType.Items.Add("PO");
            this.cbType.Items.Add("TRF调拨");
            this.cbType.Location = new System.Drawing.Point(156, 65);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(72, 22);
            this.cbType.TabIndex = 5;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
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
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReset.ForeColor = System.Drawing.Color.Sienna;
            this.btnReset.Location = new System.Drawing.Point(11, 93);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(72, 20);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "清 空";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(156, 93);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 20);
            this.btn02.TabIndex = 7;
            this.btn02.Text = "列印选择";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(96, 247);
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
            // cbOpUsr
            // 
            this.cbOpUsr.Location = new System.Drawing.Point(50, 65);
            this.cbOpUsr.Name = "cbOpUsr";
            this.cbOpUsr.Size = new System.Drawing.Size(72, 22);
            this.cbOpUsr.TabIndex = 4;
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
            this.dgHeader.Location = new System.Drawing.Point(0, 116);
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
            this.dgTable.Location = new System.Drawing.Point(0, 134);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 111);
            this.dgTable.TabIndex = 9;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // FormIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.cbOpUsr);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelOpUsr);
            this.Controls.Add(this.tbBCDanTo);
            this.Controls.Add(this.labelBCDanTo);
            this.Controls.Add(this.tbBCDanFrom);
            this.Controls.Add(this.labelBCDanFrom);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.labelDateTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.labelDateFrom);
            this.Name = "FormIn";
            this.Text = "收货查询";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDateFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label labelDateTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.TextBox tbBCDanTo;
        private System.Windows.Forms.Label labelBCDanTo;
        private System.Windows.Forms.TextBox tbBCDanFrom;
        private System.Windows.Forms.Label labelBCDanFrom;
        private System.Windows.Forms.Label labelOpUsr;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.ComboBox cbOpUsr;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable;
    }
}