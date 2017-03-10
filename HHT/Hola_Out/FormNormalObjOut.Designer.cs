namespace Hola_Out
{
    partial class FormNormalObjOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNormalObjOut));
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.NextPage = new System.Windows.Forms.Button();
            this.PrePage = new System.Windows.Forms.Button();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnBC = new System.Windows.Forms.Button();
            this.Camera = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(156, 48);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(68, 20);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnAdd.Enabled = false;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnAdd.ForeColor = System.Drawing.Color.Sienna;
            this.btnAdd.Location = new System.Drawing.Point(53, 47);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 21);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "添 加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(94, 245);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(48, 20);
            this.Page.Text = "1/1";
            this.Page.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // NextPage
            // 
            this.NextPage.BackColor = System.Drawing.Color.NavajoWhite;
            this.NextPage.Enabled = false;
            this.NextPage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.NextPage.ForeColor = System.Drawing.Color.Sienna;
            this.NextPage.Location = new System.Drawing.Point(156, 245);
            this.NextPage.Name = "NextPage";
            this.NextPage.Size = new System.Drawing.Size(72, 20);
            this.NextPage.TabIndex = 13;
            this.NextPage.Text = "下一页";
            this.NextPage.Click += new System.EventHandler(this.NextPage_Click);
            // 
            // PrePage
            // 
            this.PrePage.BackColor = System.Drawing.Color.NavajoWhite;
            this.PrePage.Enabled = false;
            this.PrePage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.PrePage.ForeColor = System.Drawing.Color.Sienna;
            this.PrePage.Location = new System.Drawing.Point(11, 245);
            this.PrePage.Name = "PrePage";
            this.PrePage.Size = new System.Drawing.Size(72, 20);
            this.PrePage.TabIndex = 12;
            this.PrePage.Text = "上一页";
            this.PrePage.Click += new System.EventHandler(this.PrePage_Click);
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.Location = new System.Drawing.Point(0, 94);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 139);
            this.dgTable.TabIndex = 11;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgTable_MouseDown);
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 74);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 10;
            this.dgHeader.TabStop = false;
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 15;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirm.Enabled = false;
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnConfirm.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirm.Location = new System.Drawing.Point(0, 274);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(72, 20);
            this.btnConfirm.TabIndex = 14;
            this.btnConfirm.Text = "提交";
            this.btnConfirm.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // pbBar
            // 
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnBC
            // 
            this.btnBC.BackColor = System.Drawing.Color.White;
            this.btnBC.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnBC.Location = new System.Drawing.Point(71, 14);
            this.btnBC.Name = "btnBC";
            this.btnBC.Size = new System.Drawing.Size(139, 21);
            this.btnBC.TabIndex = 1;
            this.btnBC.Click += new System.EventHandler(this.btnBC_Click);
            // 
            // Camera
            // 
            this.Camera.Image = ((System.Drawing.Image)(resources.GetObject("Camera.Image")));
            this.Camera.Location = new System.Drawing.Point(23, 1);
            this.Camera.Name = "Camera";
            this.Camera.Size = new System.Drawing.Size(40, 42);
            this.Camera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // FormNormalObjOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.Camera);
            this.Controls.Add(this.btnBC);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dgHeader);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.NextPage);
            this.Controls.Add(this.PrePage);
            this.MinimizeBox = false;
            this.Name = "FormNormalObjOut";
            this.Text = "正常出货";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button NextPage;
        private System.Windows.Forms.Button PrePage;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnBC;
        private System.Windows.Forms.PictureBox Camera;
    }
}