namespace Hola_Inventory
{
    partial class FormCPI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCPI));
            this.label1 = new System.Windows.Forms.Label();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.CBNO = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.pCode = new System.Windows.Forms.ComboBox();
            this.ShopNO = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCpiCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.Text = "盘点代号:";
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 32);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // CBNO
            // 
            this.CBNO.Location = new System.Drawing.Point(82, 105);
            this.CBNO.Name = "CBNO";
            this.CBNO.Size = new System.Drawing.Size(138, 21);
            this.CBNO.TabIndex = 35;
            this.CBNO.TextChanged += new System.EventHandler(this.CBNO_TextChanged);
            this.CBNO.GotFocus += new System.EventHandler(this.CBNO_GotFocus);
            this.CBNO.LostFocus += new System.EventHandler(this.CBNO_LostFocus);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.Text = "抽盘柜号:";
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 268);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnConfirm.ForeColor = System.Drawing.Color.Sienna;
            this.btnConfirm.Location = new System.Drawing.Point(0, 274);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(72, 20);
            this.btnConfirm.TabIndex = 22;
            this.btnConfirm.Text = "确 定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 23;
            this.btnReturn.Text = "离 开";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pCode
            // 
            this.pCode.Location = new System.Drawing.Point(82, 74);
            this.pCode.Name = "pCode";
            this.pCode.Size = new System.Drawing.Size(138, 22);
            this.pCode.TabIndex = 30;
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 9);
            this.ShopNO.Name = "ShopNO";
            this.ShopNO.Size = new System.Drawing.Size(39, 20);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.Text = "店号:";
            // 
            // btnCpiCode
            // 
            this.btnCpiCode.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCpiCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnCpiCode.Location = new System.Drawing.Point(82, 74);
            this.btnCpiCode.Name = "btnCpiCode";
            this.btnCpiCode.Size = new System.Drawing.Size(138, 22);
            this.btnCpiCode.TabIndex = 18;
            this.btnCpiCode.Text = "请选择";
            this.btnCpiCode.Click += new System.EventHandler(this.btnCpiCode_Click);
            // 
            // FormCPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnCpiCode);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pCode);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.CBNO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbBarI);
            this.Name = "FormCPI";
            this.Text = "抽盘";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.TextBox CBNO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.ComboBox pCode;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCpiCode;
    }
}