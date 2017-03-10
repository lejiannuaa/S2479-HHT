namespace Hola_Excel
{
    partial class FormOutDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOutDetail));
            this.tbType = new System.Windows.Forms.TextBox();
            this.labelType = new System.Windows.Forms.Label();
            this.tbBC = new System.Windows.Forms.TextBox();
            this.labelBCDan = new System.Windows.Forms.Label();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.tbTo = new System.Windows.Forms.TextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelBCBox = new System.Windows.Forms.Label();
            this.tbBCBox = new System.Windows.Forms.TextBox();
            this.labelTransport = new System.Windows.Forms.Label();
            this.cbTransport = new System.Windows.Forms.ComboBox();
            this.labelDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.btn03 = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.menuPrintMark = new System.Windows.Forms.MenuItem();
            this.menuPrintTable = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // tbType
            // 
            this.tbType.Enabled = false;
            this.tbType.Location = new System.Drawing.Point(165, 7);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(68, 21);
            this.tbType.TabIndex = 1;
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(130, 10);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(47, 20);
            this.labelType.Text = "类型:";
            // 
            // tbBC
            // 
            this.tbBC.Enabled = false;
            this.tbBC.Location = new System.Drawing.Point(49, 7);
            this.tbBC.Name = "tbBC";
            this.tbBC.Size = new System.Drawing.Size(68, 21);
            this.tbBC.TabIndex = 0;
            // 
            // labelBCDan
            // 
            this.labelBCDan.Location = new System.Drawing.Point(0, 7);
            this.labelBCDan.Name = "labelBCDan";
            this.labelBCDan.Size = new System.Drawing.Size(54, 20);
            this.labelBCDan.Text = "单号:";
            // 
            // tbFrom
            // 
            this.tbFrom.Enabled = false;
            this.tbFrom.Location = new System.Drawing.Point(49, 31);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(68, 21);
            this.tbFrom.TabIndex = 2;
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(-1, 34);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(62, 20);
            this.labelFrom.Text = "调出店:";
            // 
            // tbTo
            // 
            this.tbTo.Enabled = false;
            this.tbTo.Location = new System.Drawing.Point(165, 31);
            this.tbTo.Name = "tbTo";
            this.tbTo.Size = new System.Drawing.Size(68, 21);
            this.tbTo.TabIndex = 3;
            // 
            // labelTo
            // 
            this.labelTo.Location = new System.Drawing.Point(117, 34);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(62, 20);
            this.labelTo.Text = "调入店:";
            // 
            // labelBCBox
            // 
            this.labelBCBox.Location = new System.Drawing.Point(0, 55);
            this.labelBCBox.Name = "labelBCBox";
            this.labelBCBox.Size = new System.Drawing.Size(62, 20);
            this.labelBCBox.Text = "箱号:";
            // 
            // tbBCBox
            // 
            this.tbBCBox.Enabled = false;
            this.tbBCBox.Location = new System.Drawing.Point(49, 54);
            this.tbBCBox.Name = "tbBCBox";
            this.tbBCBox.Size = new System.Drawing.Size(68, 21);
            this.tbBCBox.TabIndex = 4;
            // 
            // labelTransport
            // 
            this.labelTransport.Location = new System.Drawing.Point(129, 48);
            this.labelTransport.Name = "labelTransport";
            this.labelTransport.Size = new System.Drawing.Size(36, 33);
            this.labelTransport.Text = "运送方式:";
            // 
            // cbTransport
            // 
            this.cbTransport.Items.Add("物流");
            this.cbTransport.Items.Add("DC");
            this.cbTransport.Location = new System.Drawing.Point(165, 53);
            this.cbTransport.Name = "cbTransport";
            this.cbTransport.Size = new System.Drawing.Size(68, 22);
            this.cbTransport.TabIndex = 5;
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(0, 79);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(62, 20);
            this.labelDesc.Text = "备注:";
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(49, 78);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(112, 21);
            this.tbDesc.TabIndex = 6;
            this.tbDesc.GotFocus += new System.EventHandler(this.tbDesc_GotFocus);
            this.tbDesc.LostFocus += new System.EventHandler(this.tbDesc_LostFocus);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(165, 78);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(68, 21);
            this.btn03.TabIndex = 7;
            this.btn03.Text = "列 印";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 239);
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
            this.NextPage.Location = new System.Drawing.Point(156, 239);
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
            this.PrePage.Location = new System.Drawing.Point(11, 239);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 10;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(168, 274);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 20);
            this.btn02.TabIndex = 13;
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
            this.btnReturn.TabIndex = 12;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 265);
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
            this.dgHeader.Location = new System.Drawing.Point(0, 105);
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
            this.dgTable.Location = new System.Drawing.Point(0, 123);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 111);
            this.dgTable.TabIndex = 9;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuDetail
            // 
            this.menuDetail.Text = "详细";
            this.menuDetail.Click += new System.EventHandler(this.menuDetail_Click);
            // 
            // menuPrintMark
            // 
            this.menuPrintMark.Text = "唛头";
            this.menuPrintMark.Click += new System.EventHandler(this.menuPrintMark_Click);
            // 
            // menuPrintTable
            // 
            this.menuPrintTable.Text = "单据";
            this.menuPrintTable.Click += new System.EventHandler(this.menuPrintTable_Click);
            // 
            // FormOutDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.labelDesc);
            this.Controls.Add(this.cbTransport);
            this.Controls.Add(this.labelTransport);
            this.Controls.Add(this.tbBCBox);
            this.Controls.Add(this.labelBCBox);
            this.Controls.Add(this.tbTo);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.tbFrom);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.tbBC);
            this.Controls.Add(this.labelBCDan);
            this.Name = "FormOutDetail";
            this.Text = "出货明细";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.TextBox tbBC;
        private System.Windows.Forms.Label labelBCDan;
        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.TextBox tbTo;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelBCBox;
        private System.Windows.Forms.TextBox tbBCBox;
        private System.Windows.Forms.Label labelTransport;
        private System.Windows.Forms.ComboBox cbTransport;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
        private System.Windows.Forms.MenuItem menuPrintMark;
        private System.Windows.Forms.MenuItem menuPrintTable;
    }
}