namespace Hola_Excel
{
    partial class FormOutModify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOutModify));
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.tbBC = new System.Windows.Forms.TextBox();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.tbCountNeed = new System.Windows.Forms.TextBox();
            this.labelCountNeed = new System.Windows.Forms.Label();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelRTV = new System.Windows.Forms.Label();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 268);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // tbBC
            // 
            this.tbBC.Enabled = false;
            this.tbBC.Location = new System.Drawing.Point(44, 10);
            this.tbBC.Name = "tbBC";
            this.tbBC.Size = new System.Drawing.Size(68, 21);
            this.tbBC.TabIndex = 8;
            // 
            // tbCount
            // 
            this.tbCount.Enabled = false;
            this.tbCount.Location = new System.Drawing.Point(163, 38);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(68, 21);
            this.tbCount.TabIndex = 11;
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(121, 30);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(36, 36);
            this.labelCount.Text = "实退数量:";
            // 
            // tbCountNeed
            // 
            this.tbCountNeed.Enabled = false;
            this.tbCountNeed.Location = new System.Drawing.Point(44, 38);
            this.tbCountNeed.Name = "tbCountNeed";
            this.tbCountNeed.Size = new System.Drawing.Size(68, 21);
            this.tbCountNeed.TabIndex = 10;
            // 
            // labelCountNeed
            // 
            this.labelCountNeed.Location = new System.Drawing.Point(3, 30);
            this.labelCountNeed.Name = "labelCountNeed";
            this.labelCountNeed.Size = new System.Drawing.Size(39, 29);
            this.labelCountNeed.Text = "要求数量:";
            // 
            // tbFrom
            // 
            this.tbFrom.Enabled = false;
            this.tbFrom.Location = new System.Drawing.Point(163, 10);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(68, 21);
            this.tbFrom.TabIndex = 9;
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(112, 10);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(51, 20);
            this.labelFrom.Text = "调入地:";
            // 
            // labelRTV
            // 
            this.labelRTV.Location = new System.Drawing.Point(3, 11);
            this.labelRTV.Name = "labelRTV";
            this.labelRTV.Size = new System.Drawing.Size(39, 20);
            this.labelRTV.Text = "单号:";
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 73);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 16;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 91);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 149);
            this.dgTable.TabIndex = 17;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
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
            this.NextPage.TabIndex = 20;
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
            this.PrePage.TabIndex = 19;
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
            this.btn02.TabIndex = 23;
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
            this.btnReturn.TabIndex = 22;
            this.btnReturn.Text = "返  回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // FormOutModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.tbBC);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.tbCountNeed);
            this.Controls.Add(this.labelCountNeed);
            this.Controls.Add(this.tbFrom);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.labelRTV);
            this.Controls.Add(this.pbBar);
            this.Name = "FormOutModify";
            this.Text = "出货调整";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.TextBox tbBC;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox tbCountNeed;
        private System.Windows.Forms.Label labelCountNeed;
        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelRTV;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnReturn;
    }
}