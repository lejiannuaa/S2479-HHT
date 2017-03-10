namespace Hola_Business
{
    partial class FormInventoryInc
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        //private System.Windows.Forms.MainMenu mainMenu1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInventoryInc));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SKUName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GoodsNO = new System.Windows.Forms.Label();
            this.Native = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SDTP = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.Diff = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.trueKC = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.inv_no = new System.Windows.Forms.Label();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.Text = "SKU新增:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 19);
            this.label2.Text = "品名:";
            // 
            // SKUName
            // 
            this.SKUName.Location = new System.Drawing.Point(53, 33);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(165, 20);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 18);
            this.label4.Text = "货号:";
            // 
            // GoodsNO
            // 
            this.GoodsNO.Location = new System.Drawing.Point(53, 58);
            this.GoodsNO.Name = "GoodsNO";
            this.GoodsNO.Size = new System.Drawing.Size(91, 18);
            // 
            // Native
            // 
            this.Native.ForeColor = System.Drawing.Color.Black;
            this.Native.Location = new System.Drawing.Point(8, 78);
            this.Native.Name = "Native";
            this.Native.Size = new System.Drawing.Size(47, 18);
            this.Native.Text = "S-DPT:";
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(4, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 18);
            this.label8.Text = "实际库存量:";
            // 
            // SDTP
            // 
            this.SDTP.Location = new System.Drawing.Point(53, 77);
            this.SDTP.Name = "SDTP";
            this.SDTP.Size = new System.Drawing.Size(178, 17);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.HotPink;
            this.label10.Location = new System.Drawing.Point(112, 122);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 18);
            this.label10.Text = "库存差异数量:";
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(66, 8);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(67, 21);
            this.SKUNO.TabIndex = 0;
            this.SKUNO.TextChanged += new System.EventHandler(this.SKUNO_TextChanged);
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // Diff
            // 
            this.Diff.Enabled = false;
            this.Diff.Location = new System.Drawing.Point(203, 119);
            this.Diff.Name = "Diff";
            this.Diff.Size = new System.Drawing.Size(34, 21);
            this.Diff.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.NavajoWhite;
            this.button1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button1.ForeColor = System.Drawing.Color.Sienna;
            this.button1.Location = new System.Drawing.Point(142, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 20);
            this.button1.TabIndex = 11;
            this.button1.Text = "查询";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.NavajoWhite;
            this.button2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button2.ForeColor = System.Drawing.Color.Sienna;
            this.button2.Location = new System.Drawing.Point(191, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 21);
            this.button2.TabIndex = 22;
            this.button2.Text = "清除";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.NavajoWhite;
            this.button3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button3.ForeColor = System.Drawing.Color.Sienna;
            this.button3.Location = new System.Drawing.Point(171, 239);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 23);
            this.button3.TabIndex = 102;
            this.button3.Text = "库调清单";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.NavajoWhite;
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button4.ForeColor = System.Drawing.Color.Sienna;
            this.button4.Location = new System.Drawing.Point(2, 274);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 20);
            this.button4.TabIndex = 103;
            this.button4.Text = "添加";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.NavajoWhite;
            this.button5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button5.ForeColor = System.Drawing.Color.Sienna;
            this.button5.Location = new System.Drawing.Point(171, 274);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(60, 20);
            this.button5.TabIndex = 104;
            this.button5.Text = "返回";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label3.Location = new System.Drawing.Point(5, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 17);
            this.label3.Text = "*库存差异数量=实际库存量-系统库存量";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 22);
            this.label5.Text = "库存调整原因:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBox1.Enabled = false;
            this.comboBox1.Items.Add("1. shiqie");
            this.comboBox1.Location = new System.Drawing.Point(8, 168);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(210, 22);
            this.comboBox1.TabIndex = 115;
            this.comboBox1.Tag = "打发";
            this.comboBox1.LostFocus += new System.EventHandler(this.comboBox1_LostFocus);
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.GotFocus += new System.EventHandler(this.comboBox1_GotFocus);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 99);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 18);
            this.label14.Text = "状态:";
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(52, 98);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(53, 18);
            // 
            // trueKC
            // 
            this.trueKC.Location = new System.Drawing.Point(78, 119);
            this.trueKC.Name = "trueKC";
            this.trueKC.Size = new System.Drawing.Size(35, 21);
            this.trueKC.TabIndex = 129;
            this.trueKC.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.trueKC.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trueKC_KeyUp);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(112, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 19);
            this.label6.Text = "系统库存:";
            // 
            // inv_no
            // 
            this.inv_no.Location = new System.Drawing.Point(179, 97);
            this.inv_no.Name = "inv_no";
            this.inv_no.Size = new System.Drawing.Size(51, 19);
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 265);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // FormInventoryInc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.pbBarI);
            this.Controls.Add(this.inv_no);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trueKC);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Diff);
            this.Controls.Add(this.SKUNO);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.SDTP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Native);
            this.Controls.Add(this.GoodsNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SKUName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "FormInventoryInc";
            this.Text = "库存调整新增";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormInventoryInc_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SKUName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label GoodsNO;
        private System.Windows.Forms.Label Native;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label SDTP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox SKUNO;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Diff;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.TextBox trueKC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label inv_no;
        private System.Windows.Forms.PictureBox pbBarI;
    }
}