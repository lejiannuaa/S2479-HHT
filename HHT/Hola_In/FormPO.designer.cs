namespace Hola_In
{
    partial class FormPO
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
            this.btnReturn = new System.Windows.Forms.Button();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menu02 = new System.Windows.Forms.MenuItem();
            this.menu03 = new System.Windows.Forms.MenuItem();
            this.menu04 = new System.Windows.Forms.MenuItem();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.labelBC = new System.Windows.Forms.Label();
            this.btn01 = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.labelOpUsr = new System.Windows.Forms.Label();
            this.cbOpUsr = new System.Windows.Forms.ComboBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.tbBC = new System.Windows.Forms.TextBox();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 10;
            this.btnReturn.Text = "返 回 ";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // NextPage
            // 
            this.NextPage.BackColor = System.Drawing.Color.NavajoWhite;
            this.NextPage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.NextPage.ForeColor = System.Drawing.Color.Sienna;
            this.NextPage.Location = new System.Drawing.Point(156, 244);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 9;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 244);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 8;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReset.ForeColor = System.Drawing.Color.Sienna;
            this.btnReset.Location = new System.Drawing.Point(156, 65);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(72, 22);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "清 空";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 111);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 130);
            this.dgTable.TabIndex = 7;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menu02);
            this.contextMenu1.MenuItems.Add(this.menu03);
            this.contextMenu1.MenuItems.Add(this.menu04);
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menu02
            // 
            this.menu02.Text = "申请收货";
            this.menu02.Click += new System.EventHandler(this.menu02_Click);
            // 
            // menu03
            // 
            this.menu03.Text = "解除收货";
            this.menu03.Click += new System.EventHandler(this.menu03_Click);
            // 
            // menu04
            // 
            this.menu04.Text = "进入收货";
            this.menu04.Click += new System.EventHandler(this.menu04_Click);
            // 
            // labelTo
            // 
            this.labelTo.Location = new System.Drawing.Point(137, 39);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(22, 20);
            this.labelTo.Text = "至";
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(1, 36);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(46, 29);
            this.labelFrom.Text = "预计到货日期:";
            // 
            // labelState
            // 
            this.labelState.Location = new System.Drawing.Point(127, 13);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(48, 20);
            this.labelState.Text = "状态:";
            // 
            // cbState
            // 
            this.cbState.Items.Add("");
            this.cbState.Items.Add("删除");
            this.cbState.Items.Add("可申请");
            this.cbState.Items.Add("申请中");
            this.cbState.Items.Add("可收货");
            this.cbState.Items.Add("收货中");
            this.cbState.Items.Add("已收货");
            this.cbState.Location = new System.Drawing.Point(160, 10);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(73, 22);
            this.cbState.TabIndex = 1;
            // 
            // labelBC
            // 
            this.labelBC.Location = new System.Drawing.Point(3, 13);
            this.labelBC.Name = "labelBC";
            this.labelBC.Size = new System.Drawing.Size(56, 20);
            this.labelBC.Text = "PO单号:";
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(168, 274);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(72, 20);
            this.btn01.TabIndex = 11;
            this.btn01.Text = "查 询";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 244);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(48, 20);
            this.Page.Text = "1/1";
            this.Page.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelOpUsr
            // 
            this.labelOpUsr.Location = new System.Drawing.Point(6, 68);
            this.labelOpUsr.Name = "labelOpUsr";
            this.labelOpUsr.Size = new System.Drawing.Size(53, 20);
            this.labelOpUsr.Text = "操作人:";
            // 
            // cbOpUsr
            // 
            this.cbOpUsr.Location = new System.Drawing.Point(53, 65);
            this.cbOpUsr.Name = "cbOpUsr";
            this.cbOpUsr.Size = new System.Drawing.Size(73, 22);
            this.cbOpUsr.TabIndex = 4;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy-MM-dd";
            this.dtpFrom.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(53, 37);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(73, 20);
            this.dtpFrom.TabIndex = 2;
            this.dtpFrom.Value = new System.DateTime(2012, 10, 1, 0, 0, 0, 0);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy-MM-dd";
            this.dtpTo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(160, 37);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(73, 20);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.Value = new System.DateTime(2012, 12, 31, 0, 0, 0, 0);
            // 
            // tbBC
            // 
            this.tbBC.Location = new System.Drawing.Point(53, 10);
            this.tbBC.Name = "tbBC";
            this.tbBC.Size = new System.Drawing.Size(73, 21);
            this.tbBC.TabIndex = 0;
            this.tbBC.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbBC_KeyUp);
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
            this.dgHeader.Location = new System.Drawing.Point(0, 94);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 6;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // FormPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.tbBC);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.cbOpUsr);
            this.Controls.Add(this.labelOpUsr);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.labelBC);
            this.Controls.Add(this.cbState);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.dgTable);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormPO";
            this.Text = "PO收货";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPO_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ComboBox cbState;
        private System.Windows.Forms.Label labelBC;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menu02;
        private System.Windows.Forms.MenuItem menu03;
        private System.Windows.Forms.Label labelOpUsr;
        private System.Windows.Forms.ComboBox cbOpUsr;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.TextBox tbBC;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.MenuItem menu04;
        private System.Windows.Forms.DataGrid dgHeader;
    }
}