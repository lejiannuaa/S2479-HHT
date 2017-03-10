namespace Hola_Out
{
    partial class FormPurchase2Ex
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPurchase2Ex));
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menu03 = new System.Windows.Forms.MenuItem();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.tbCountNeed = new System.Windows.Forms.TextBox();
            this.labelCountNeed = new System.Windows.Forms.Label();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.labelRTV = new System.Windows.Forms.Label();
            this.tbRTV = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 90);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 149);
            this.dgTable.TabIndex = 5;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menu03);
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menu03
            // 
            this.menu03.Text = "列印";
            this.menu03.Click += new System.EventHandler(this.menu03_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 243);
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
            this.NextPage.Location = new System.Drawing.Point(156, 243);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 7;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 243);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 6;
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
            this.btn02.TabIndex = 9;
            this.btn02.Text = "确  认";
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
            this.btnReturn.Text = "返  回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // tbCount
            // 
            this.tbCount.Enabled = false;
            this.tbCount.Location = new System.Drawing.Point(160, 35);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(68, 21);
            this.tbCount.TabIndex = 3;
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(124, 31);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(39, 37);
            this.labelCount.Text = "实退数量:";
            // 
            // tbCountNeed
            // 
            this.tbCountNeed.Enabled = false;
            this.tbCountNeed.Location = new System.Drawing.Point(41, 35);
            this.tbCountNeed.Name = "tbCountNeed";
            this.tbCountNeed.Size = new System.Drawing.Size(68, 21);
            this.tbCountNeed.TabIndex = 2;
            // 
            // labelCountNeed
            // 
            this.labelCountNeed.Location = new System.Drawing.Point(6, 31);
            this.labelCountNeed.Name = "labelCountNeed";
            this.labelCountNeed.Size = new System.Drawing.Size(39, 37);
            this.labelCountNeed.Text = "要求数量:";
            // 
            // tbFrom
            // 
            this.tbFrom.Enabled = false;
            this.tbFrom.Location = new System.Drawing.Point(160, 7);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(68, 21);
            this.tbFrom.TabIndex = 1;
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(109, 9);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(51, 20);
            this.labelFrom.Text = "调入店:";
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 72);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 4;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // labelRTV
            // 
            this.labelRTV.Location = new System.Drawing.Point(6, 1);
            this.labelRTV.Name = "labelRTV";
            this.labelRTV.Size = new System.Drawing.Size(37, 30);
            this.labelRTV.Text = "出货单号:";
            // 
            // tbRTV
            // 
            this.tbRTV.Enabled = false;
            this.tbRTV.Location = new System.Drawing.Point(41, 7);
            this.tbRTV.Name = "tbRTV";
            this.tbRTV.Size = new System.Drawing.Size(68, 21);
            this.tbRTV.TabIndex = 13;
            // 
            // FormPurchase2Ex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.tbRTV);
            this.Controls.Add(this.labelRTV);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.tbCountNeed);
            this.Controls.Add(this.labelCountNeed);
            this.Controls.Add(this.tbFrom);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.dgTable);
            this.MinimizeBox = false;
            this.Name = "FormPurchase2Ex";
            this.Text = "采购出货调整";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menu03;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox tbCountNeed;
        private System.Windows.Forms.Label labelCountNeed;
        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Label labelRTV;
        private System.Windows.Forms.TextBox tbRTV;
    }
}