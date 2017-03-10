namespace Hola_Business
{
    partial class FormDJ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDJ));
            this.label1 = new System.Windows.Forms.Label();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.btn01 = new System.Windows.Forms.Button();
            this.DJId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.Text = "端架Group维护";
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 32);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(20, 120);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(72, 20);
            this.btn01.TabIndex = 54;
            this.btn01.Text = "确定新增";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // DJId
            // 
            this.DJId.Location = new System.Drawing.Point(112, 60);
            this.DJId.Name = "DJId";
            this.DJId.Size = new System.Drawing.Size(112, 21);
            this.DJId.TabIndex = 52;
            this.DJId.TextChanged += new System.EventHandler(this.DJId_TextChanged);
            this.DJId.GotFocus += new System.EventHandler(this.DJId_GotFocus);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.Text = "端架GroupID:";
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 267);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 58;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(141, 120);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(72, 20);
            this.btn02.TabIndex = 72;
            this.btn02.Text = "查询";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // FormDJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.DJId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbBarI);
            this.KeyPreview = true;
            this.Name = "FormDJ";
            this.Text = "端架";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.TextBox DJId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btn02;
    }
}