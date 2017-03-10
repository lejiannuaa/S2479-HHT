namespace HolaCore
{
    partial class FormCamera
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SCbutton = new System.Windows.Forms.Button();
            this.DYbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn
            // 
            this.btn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn.ForeColor = System.Drawing.Color.Sienna;
            this.btn.Location = new System.Drawing.Point(7, 3);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(66, 50);
            this.btn.TabIndex = 0;
            this.btn.Text = "照相";
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.NavajoWhite;
            this.button1.ForeColor = System.Drawing.Color.Sienna;
            this.button1.Location = new System.Drawing.Point(156, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 22);
            this.button1.TabIndex = 1;
            this.button1.Text = "退出";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(25, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(191, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(25, 195);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(191, 130);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.DoubleClick += new System.EventHandler(this.pictureBox2_DoubleClick);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(25, 331);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(191, 130);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.DoubleClick += new System.EventHandler(this.pictureBox3_DoubleClick);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.NavajoWhite;
            this.button2.ForeColor = System.Drawing.Color.Sienna;
            this.button2.Location = new System.Drawing.Point(156, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 22);
            this.button2.TabIndex = 3;
            this.button2.Text = "全部删除";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SCbutton
            // 
            this.SCbutton.BackColor = System.Drawing.Color.NavajoWhite;
            this.SCbutton.ForeColor = System.Drawing.Color.Sienna;
            this.SCbutton.Location = new System.Drawing.Point(80, 31);
            this.SCbutton.Name = "SCbutton";
            this.SCbutton.Size = new System.Drawing.Size(69, 22);
            this.SCbutton.TabIndex = 4;
            this.SCbutton.Text = "上传";
            this.SCbutton.Click += new System.EventHandler(this.SCbutton_Click);
            // 
            // DYbutton
            // 
            this.DYbutton.BackColor = System.Drawing.Color.NavajoWhite;
            this.DYbutton.ForeColor = System.Drawing.Color.Sienna;
            this.DYbutton.Location = new System.Drawing.Point(79, 3);
            this.DYbutton.Name = "DYbutton";
            this.DYbutton.Size = new System.Drawing.Size(69, 22);
            this.DYbutton.TabIndex = 5;
            this.DYbutton.Text = "打印";
            this.DYbutton.Click += new System.EventHandler(this.DYbutton_Click);
            // 
            // FormCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(253, 294);
            this.Controls.Add(this.DYbutton);
            this.Controls.Add(this.SCbutton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn);
            this.Name = "FormCamera";
            this.Text = "照相";
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button SCbutton;
        private System.Windows.Forms.Button DYbutton;
    }
}

