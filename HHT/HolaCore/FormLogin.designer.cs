namespace HolaCore
{
    partial class FormLogin
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
            this.picFormBG = new System.Windows.Forms.PictureBox();
            this.tbUsr = new System.Windows.Forms.TextBox();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.labelUsr = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // picFormBG
            // 
            this.picFormBG.Location = new System.Drawing.Point(0, 0);
            this.picFormBG.Name = "picFormBG";
            this.picFormBG.Size = new System.Drawing.Size(240, 294);
            // 
            // tbUsr
            // 
            this.tbUsr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.tbUsr.Location = new System.Drawing.Point(81, 97);
            this.tbUsr.MaxLength = 10;
            this.tbUsr.Name = "tbUsr";
            this.tbUsr.Size = new System.Drawing.Size(131, 21);
            this.tbUsr.TabIndex = 1;
            this.tbUsr.TextChanged += new System.EventHandler(this.tbUsr_TextChanged);
            this.tbUsr.GotFocus += new System.EventHandler(this.tbUsr_GotFocus);
            this.tbUsr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUsr_KeyDown);
            this.tbUsr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUsr_KeyPress);
            this.tbUsr.LostFocus += new System.EventHandler(this.tbUsr_LostFocus);
            // 
            // tbPwd
            // 
            this.tbPwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.tbPwd.Location = new System.Drawing.Point(81, 124);
            this.tbPwd.MaxLength = 10;
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.PasswordChar = '*';
            this.tbPwd.Size = new System.Drawing.Size(131, 21);
            this.tbPwd.TabIndex = 2;
            this.tbPwd.TextChanged += new System.EventHandler(this.tbPwd_TextChanged);
            this.tbPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPwd_KeyDown);
            this.tbPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPwd_KeyPress);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.btnLogin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.btnLogin.Location = new System.Drawing.Point(81, 151);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(62, 22);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "系统登录";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.btnExit.Location = new System.Drawing.Point(150, 151);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(62, 22);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "系统退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // labelUsr
            // 
            this.labelUsr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.labelUsr.Location = new System.Drawing.Point(40, 98);
            this.labelUsr.Name = "labelUsr";
            this.labelUsr.Size = new System.Drawing.Size(40, 20);
            this.labelUsr.Text = "帐号:";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.label2.Location = new System.Drawing.Point(40, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.Text = "密码:";
            // 
            // lbVersion
            // 
            this.lbVersion.BackColor = System.Drawing.Color.Transparent;
            this.lbVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.lbVersion.Location = new System.Drawing.Point(150, 40);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(90, 20);
            this.lbVersion.Text = "版本:";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelUsr);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.tbPwd);
            this.Controls.Add(this.tbUsr);
            this.Controls.Add(this.picFormBG);
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picFormBG;
        private System.Windows.Forms.TextBox tbUsr;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label labelUsr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbVersion;
    }
}