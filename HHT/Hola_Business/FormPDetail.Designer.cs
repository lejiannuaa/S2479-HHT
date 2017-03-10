namespace Hola_Business
{
    partial class FormPDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPDetail));
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btn01 = new System.Windows.Forms.Button();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ActDateFrom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ActDateTo = new System.Windows.Forms.TextBox();
            this.Event = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ActType = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 81;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 267);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 32);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(118, 55);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(60, 20);
            this.btn01.TabIndex = 87;
            this.btn01.Text = "查 询";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(41, 55);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(76, 21);
            this.SKUNO.TabIndex = 0;
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.Text = "SKU:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(179, 55);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 20);
            this.btnDelete.TabIndex = 89;
            this.btnDelete.Text = "清 除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ActDateFrom
            // 
            this.ActDateFrom.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.ActDateFrom.Location = new System.Drawing.Point(60, 82);
            this.ActDateFrom.Name = "ActDateFrom";
            this.ActDateFrom.ReadOnly = true;
            this.ActDateFrom.Size = new System.Drawing.Size(73, 20);
            this.ActDateFrom.TabIndex = 91;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.Location = new System.Drawing.Point(1, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.Text = "活动期间:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label4.Location = new System.Drawing.Point(140, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 20);
            this.label4.Text = "至";
            // 
            // ActDateTo
            // 
            this.ActDateTo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.ActDateTo.Location = new System.Drawing.Point(168, 82);
            this.ActDateTo.Name = "ActDateTo";
            this.ActDateTo.ReadOnly = true;
            this.ActDateTo.Size = new System.Drawing.Size(69, 20);
            this.ActDateTo.TabIndex = 96;
            // 
            // Event
            // 
            this.Event.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.Event.Location = new System.Drawing.Point(60, 109);
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            this.Event.Size = new System.Drawing.Size(177, 20);
            this.Event.TabIndex = 98;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label5.Location = new System.Drawing.Point(1, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 20);
            this.label5.Text = "活动类型:";
            // 
            // ActType
            // 
            this.ActType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.ActType.Location = new System.Drawing.Point(60, 136);
            this.ActType.Name = "ActType";
            this.ActType.ReadOnly = true;
            this.ActType.Size = new System.Drawing.Size(177, 20);
            this.ActType.TabIndex = 101;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label6.Location = new System.Drawing.Point(1, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.Text = "活动主题:";
            // 
            // FormPDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.ActType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Event);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ActDateTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ActDateFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.SKUNO);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBarI);
            this.Controls.Add(this.pbBar);
            this.KeyPreview = true;
            this.Name = "FormPDetail";
            this.Text = "折扣详细";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.TextBox SKUNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox ActDateFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ActDateTo;
        private System.Windows.Forms.TextBox Event;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ActType;
        private System.Windows.Forms.Label label6;
    }
}