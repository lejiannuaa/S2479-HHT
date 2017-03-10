namespace Hola_In
{
    partial class FormDB
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
            this.btn00102 = new System.Windows.Forms.Button();
            this.lableBC = new System.Windows.Forms.Label();
            this.btn00104 = new System.Windows.Forms.Button();
            this.btn00103 = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.btn05 = new System.Windows.Forms.Button();
            this.lableState = new System.Windows.Forms.Label();
            this.tbState = new System.Windows.Forms.TextBox();
            this.btn06 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btnBC = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.timer1 = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // btn00102
            // 
            this.btn00102.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn00102.Enabled = false;
            this.btn00102.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn00102.ForeColor = System.Drawing.Color.Sienna;
            this.btn00102.Location = new System.Drawing.Point(11, 32);
            this.btn00102.Name = "btn00102";
            this.btn00102.Size = new System.Drawing.Size(69, 20);
            this.btn00102.TabIndex = 2;
            this.btn00102.Text = "整箱收货";
            this.btn00102.Click += new System.EventHandler(this.btn02_Click);
            // 
            // lableBC
            // 
            this.lableBC.Location = new System.Drawing.Point(6, 8);
            this.lableBC.Name = "lableBC";
            this.lableBC.Size = new System.Drawing.Size(49, 20);
            this.lableBC.Text = "箱号:";
            // 
            // btn00104
            // 
            this.btn00104.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn00104.Enabled = false;
            this.btn00104.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn00104.ForeColor = System.Drawing.Color.Sienna;
            this.btn00104.Location = new System.Drawing.Point(86, 32);
            this.btn00104.Name = "btn00104";
            this.btn00104.Size = new System.Drawing.Size(69, 20);
            this.btn00104.TabIndex = 3;
            this.btn00104.Text = "整箱破损";
            this.btn00104.Click += new System.EventHandler(this.btn04_Click);
            // 
            // btn00103
            // 
            this.btn00103.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn00103.Enabled = false;
            this.btn00103.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn00103.ForeColor = System.Drawing.Color.Sienna;
            this.btn00103.Location = new System.Drawing.Point(159, 32);
            this.btn00103.Name = "btn00103";
            this.btn00103.Size = new System.Drawing.Size(69, 20);
            this.btn00103.TabIndex = 4;
            this.btn00103.Text = "无该箱";
            this.btn00103.Click += new System.EventHandler(this.btn03_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 102);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 159);
            this.dgTable.TabIndex = 8;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // btn05
            // 
            this.btn05.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn05.Enabled = false;
            this.btn05.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn05.ForeColor = System.Drawing.Color.Sienna;
            this.btn05.Location = new System.Drawing.Point(11, 58);
            this.btn05.Name = "btn05";
            this.btn05.Size = new System.Drawing.Size(69, 20);
            this.btn05.TabIndex = 5;
            this.btn05.Text = "进入收货";
            this.btn05.Click += new System.EventHandler(this.btn05_Click);
            // 
            // lableState
            // 
            this.lableState.Location = new System.Drawing.Point(124, 8);
            this.lableState.Name = "lableState";
            this.lableState.Size = new System.Drawing.Size(47, 20);
            this.lableState.Text = "状态:";
            // 
            // tbState
            // 
            this.tbState.Enabled = false;
            this.tbState.Location = new System.Drawing.Point(159, 5);
            this.tbState.Name = "tbState";
            this.tbState.Size = new System.Drawing.Size(69, 21);
            this.tbState.TabIndex = 1;
            // 
            // btn06
            // 
            this.btn06.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn06.Enabled = false;
            this.btn06.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn06.ForeColor = System.Drawing.Color.Sienna;
            this.btn06.Location = new System.Drawing.Point(86, 58);
            this.btn06.Name = "btn06";
            this.btn06.Size = new System.Drawing.Size(69, 20);
            this.btn06.TabIndex = 6;
            this.btn06.Text = "解除收货";
            this.btn06.Click += new System.EventHandler(this.btn06_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 300);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 11;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 267);
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
            this.NextPage.Location = new System.Drawing.Point(156, 267);
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
            this.PrePage.Location = new System.Drawing.Point(11, 267);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 9;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // btnBC
            // 
            this.btnBC.BackColor = System.Drawing.Color.White;
            this.btnBC.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnBC.Location = new System.Drawing.Point(41, 5);
            this.btnBC.Name = "btnBC";
            this.btnBC.Size = new System.Drawing.Size(79, 21);
            this.btnBC.TabIndex = 0;
            this.btnBC.Click += new System.EventHandler(this.btnBC_Click);
            // 
            // pbBar
            // 
            this.pbBar.Location = new System.Drawing.Point(0, 295);
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
            this.dgHeader.Location = new System.Drawing.Point(0, 84);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 7;
            this.dgHeader.TabStop = false;
            this.dgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgHeader_MouseDown);
            // 
            // FormDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnBC);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btn06);
            this.Controls.Add(this.tbState);
            this.Controls.Add(this.lableState);
            this.Controls.Add(this.btn05);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.btn00103);
            this.Controls.Add(this.btn00104);
            this.Controls.Add(this.lableBC);
            this.Controls.Add(this.btn00102);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "FormDB";
            this.Text = "FormDB";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn00102;
        private System.Windows.Forms.Label lableBC;
        private System.Windows.Forms.Button btn00104;
        private System.Windows.Forms.Button btn00103;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Button btn05;
        private System.Windows.Forms.Label lableState;
        private System.Windows.Forms.TextBox tbState;
        private System.Windows.Forms.Button btn06;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btnBC;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Timer timer1;
    }
}