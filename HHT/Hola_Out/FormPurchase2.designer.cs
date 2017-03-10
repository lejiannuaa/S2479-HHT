namespace Hola_Out
{
    partial class FormPurchase2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPurchase2));
            this.labelTo = new System.Windows.Forms.Label();
            this.tbToName = new System.Windows.Forms.TextBox();
            this.labelCountNeed = new System.Windows.Forms.Label();
            this.tbCountNeed = new System.Windows.Forms.TextBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.btn03 = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuModify = new System.Windows.Forms.MenuItem();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btn01 = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.tbToID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTo
            // 
            this.labelTo.Location = new System.Drawing.Point(5, 33);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(50, 20);
            this.labelTo.Text = "调入地:";
            // 
            // tbToName
            // 
            this.tbToName.Enabled = false;
            this.tbToName.Location = new System.Drawing.Point(127, 32);
            this.tbToName.Name = "tbToName";
            this.tbToName.Size = new System.Drawing.Size(107, 21);
            this.tbToName.TabIndex = 3;
            // 
            // labelCountNeed
            // 
            this.labelCountNeed.Location = new System.Drawing.Point(5, 50);
            this.labelCountNeed.Name = "labelCountNeed";
            this.labelCountNeed.Size = new System.Drawing.Size(36, 30);
            this.labelCountNeed.Text = "要求数量:";
            // 
            // tbCountNeed
            // 
            this.tbCountNeed.Enabled = false;
            this.tbCountNeed.Location = new System.Drawing.Point(52, 56);
            this.tbCountNeed.Name = "tbCountNeed";
            this.tbCountNeed.Size = new System.Drawing.Size(68, 21);
            this.tbCountNeed.TabIndex = 4;
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(129, 53);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(36, 30);
            this.labelCount.Text = "实退数量:";
            // 
            // tbCount
            // 
            this.tbCount.Enabled = false;
            this.tbCount.Location = new System.Drawing.Point(166, 56);
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(68, 21);
            this.tbCount.TabIndex = 5;
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(156, 86);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(72, 20);
            this.btn03.TabIndex = 6;
            this.btn03.Text = "列 印";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 130);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 111);
            this.dgTable.TabIndex = 8;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menuModify);
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuModify
            // 
            this.menuModify.Text = "调整";
            this.menuModify.Click += new System.EventHandler(this.menuModify_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 244);
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
            this.NextPage.Location = new System.Drawing.Point(156, 244);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 10;
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
            this.PrePage.TabIndex = 9;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.White;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.Location = new System.Drawing.Point(52, 8);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(68, 21);
            this.btn01.TabIndex = 0;
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(168, 274);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 20);
            this.btn02.TabIndex = 12;
            this.btn02.Text = "提  交";
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
            this.btnReturn.TabIndex = 11;
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
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 112);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 7;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // tbToID
            // 
            this.tbToID.Enabled = false;
            this.tbToID.Location = new System.Drawing.Point(52, 32);
            this.tbToID.Name = "tbToID";
            this.tbToID.Size = new System.Drawing.Size(68, 21);
            this.tbToID.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 30);
            this.label1.Text = "出货单号:";
            // 
            // FormPurchase2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.tbCountNeed);
            this.Controls.Add(this.labelCountNeed);
            this.Controls.Add(this.tbToName);
            this.Controls.Add(this.tbToID);
            this.Controls.Add(this.labelTo);
            this.MinimizeBox = false;
            this.Name = "FormPurchase2";
            this.Text = "采购出货修改";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.TextBox tbToName;
        private System.Windows.Forms.Label labelCountNeed;
        private System.Windows.Forms.TextBox tbCountNeed;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuModify;
        public System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.TextBox tbToID;
        private System.Windows.Forms.Label label1;
    }
}