namespace HolaCore
{
    partial class FormPing
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
            this.label1 = new System.Windows.Forms.Label();
            this.IPServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IPLocal = new System.Windows.Forms.TextBox();
            this.PingMS = new System.Windows.Forms.Label();
            this.Goback = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.show = new System.Windows.Forms.Label();
            this.lbMiss = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.Text = "服务器IP地址:";
            // 
            // IPServer
            // 
            this.IPServer.Enabled = false;
            this.IPServer.Location = new System.Drawing.Point(98, 47);
            this.IPServer.Name = "IPServer";
            this.IPServer.Size = new System.Drawing.Size(112, 21);
            this.IPServer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.Text = "本机IP地址:";
            // 
            // IPLocal
            // 
            this.IPLocal.Enabled = false;
            this.IPLocal.Location = new System.Drawing.Point(98, 80);
            this.IPLocal.Name = "IPLocal";
            this.IPLocal.Size = new System.Drawing.Size(112, 21);
            this.IPLocal.TabIndex = 4;
            // 
            // PingMS
            // 
            this.PingMS.Location = new System.Drawing.Point(14, 162);
            this.PingMS.Name = "PingMS";
            this.PingMS.Size = new System.Drawing.Size(196, 20);
            // 
            // Goback
            // 
            this.Goback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.Goback.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.Goback.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.Goback.Location = new System.Drawing.Point(0, 272);
            this.Goback.Name = "Goback";
            this.Goback.Size = new System.Drawing.Size(82, 22);
            this.Goback.TabIndex = 9;
            this.Goback.Text = "返 回";
            this.Goback.Click += new System.EventHandler(this.Goback_Click);
            // 
            // test
            // 
            this.test.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.test.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.test.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.test.Location = new System.Drawing.Point(158, 272);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(82, 22);
            this.test.TabIndex = 14;
            this.test.Text = "测 试";
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // show
            // 
            this.show.Location = new System.Drawing.Point(14, 130);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(196, 20);
            // 
            // lbMiss
            // 
            this.lbMiss.Location = new System.Drawing.Point(14, 198);
            this.lbMiss.Name = "lbMiss";
            this.lbMiss.Size = new System.Drawing.Size(196, 20);
            // 
            // FormPing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.lbMiss);
            this.Controls.Add(this.show);
            this.Controls.Add(this.test);
            this.Controls.Add(this.Goback);
            this.Controls.Add(this.PingMS);
            this.Controls.Add(this.IPLocal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IPServer);
            this.Controls.Add(this.label1);
            this.Name = "FormPing";
            this.Text = "网络查看";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormPing_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IPServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IPLocal;
        private System.Windows.Forms.Label PingMS;
        private System.Windows.Forms.Button Goback;
        private System.Windows.Forms.Button test;
        private System.Windows.Forms.Label show;
        private System.Windows.Forms.Label lbMiss;
    }
}