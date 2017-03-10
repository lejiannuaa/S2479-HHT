namespace Hola_Excel
{
    partial class FormInWave
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
            this.tbWave = new System.Windows.Forms.TextBox();
            this.labelWave = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btn01 = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.tbTotal = new System.Windows.Forms.TextBox();
            this.labelTotal = new System.Windows.Forms.Label();
            this.tbOK = new System.Windows.Forms.TextBox();
            this.labelOK = new System.Windows.Forms.Label();
            this.tbReady = new System.Windows.Forms.TextBox();
            this.labelReady = new System.Windows.Forms.Label();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.lbState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbWave
            // 
            this.tbWave.Location = new System.Drawing.Point(40, 3);
            this.tbWave.Name = "tbWave";
            this.tbWave.Size = new System.Drawing.Size(72, 21);
            this.tbWave.TabIndex = 0;
            this.tbWave.TextChanged += new System.EventHandler(this.tbWave_TextChanged);
            this.tbWave.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbWave_KeyPress);
            // 
            // labelWave
            // 
            this.labelWave.Location = new System.Drawing.Point(3, 3);
            this.labelWave.Name = "labelWave";
            this.labelWave.Size = new System.Drawing.Size(43, 18);
            this.labelWave.Text = "波次:";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(118, 4);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(40, 21);
            this.labelType.Text = "类型:";
            // 
            // cbType
            // 
            this.cbType.Items.Add("TRF");
            this.cbType.Items.Add("PO");
            this.cbType.Location = new System.Drawing.Point(156, 3);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(72, 22);
            this.cbType.TabIndex = 6;
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
            this.dgHeader.Location = new System.Drawing.Point(0, 87);
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
            this.dgTable.Location = new System.Drawing.Point(0, 105);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 139);
            this.dgTable.TabIndex = 9;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // tbTotal
            // 
            this.tbTotal.Location = new System.Drawing.Point(40, 30);
            this.tbTotal.Name = "tbTotal";
            this.tbTotal.ReadOnly = true;
            this.tbTotal.Size = new System.Drawing.Size(72, 21);
            this.tbTotal.TabIndex = 36;
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(3, 30);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(43, 18);
            this.labelTotal.Text = "总数:";
            // 
            // tbOK
            // 
            this.tbOK.Location = new System.Drawing.Point(155, 57);
            this.tbOK.Name = "tbOK";
            this.tbOK.ReadOnly = true;
            this.tbOK.Size = new System.Drawing.Size(72, 21);
            this.tbOK.TabIndex = 39;
            // 
            // labelOK
            // 
            this.labelOK.Location = new System.Drawing.Point(118, 57);
            this.labelOK.Name = "labelOK";
            this.labelOK.Size = new System.Drawing.Size(43, 18);
            this.labelOK.Text = "已收:";
            // 
            // tbReady
            // 
            this.tbReady.Location = new System.Drawing.Point(40, 57);
            this.tbReady.Name = "tbReady";
            this.tbReady.ReadOnly = true;
            this.tbReady.Size = new System.Drawing.Size(72, 21);
            this.tbReady.TabIndex = 42;
            // 
            // labelReady
            // 
            this.labelReady.Location = new System.Drawing.Point(3, 57);
            this.labelReady.Name = "labelReady";
            this.labelReady.Size = new System.Drawing.Size(43, 18);
            this.labelReady.Text = "未收:";
            // 
            // cbState
            // 
            this.cbState.Items.Add("全部");
            this.cbState.Items.Add("已收");
            this.cbState.Items.Add("未收");
            this.cbState.Location = new System.Drawing.Point(156, 29);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(72, 22);
            this.cbState.TabIndex = 51;
            // 
            // lbState
            // 
            this.lbState.Location = new System.Drawing.Point(118, 30);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(40, 21);
            this.lbState.Text = "状态:";
            // 
            // FormInWave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.cbState);
            this.Controls.Add(this.lbState);
            this.Controls.Add(this.tbReady);
            this.Controls.Add(this.labelReady);
            this.Controls.Add(this.tbOK);
            this.Controls.Add(this.labelOK);
            this.Controls.Add(this.tbTotal);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.tbWave);
            this.Controls.Add(this.labelWave);
            this.KeyPreview = true;
            this.Name = "FormInWave";
            this.Text = "收货波次";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbWave;
        private System.Windows.Forms.Label labelWave;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.TextBox tbTotal;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.TextBox tbOK;
        private System.Windows.Forms.Label labelOK;
        private System.Windows.Forms.TextBox tbReady;
        private System.Windows.Forms.Label labelReady;
        private System.Windows.Forms.ComboBox cbState;
        private System.Windows.Forms.Label lbState;
    }
}