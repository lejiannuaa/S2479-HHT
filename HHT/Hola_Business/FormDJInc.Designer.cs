namespace Hola_Business
{
    partial class FormDJInc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDJInc));
            this.label1 = new System.Windows.Forms.Label();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnReturn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DJName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn01 = new System.Windows.Forms.Button();
            this.DJId = new System.Windows.Forms.TextBox();
            this.DJDateFrom = new System.Windows.Forms.DateTimePicker();
            this.DJDateTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.Text = "端架Group新增";
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 32);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.Text = "端架GroupID:";
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
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
            this.btnReturn.TabIndex = 61;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.Text = "端架有效期间:";
            // 
            // DJName
            // 
            this.DJName.Location = new System.Drawing.Point(111, 88);
            this.DJName.Name = "DJName";
            this.DJName.Size = new System.Drawing.Size(124, 21);
            this.DJName.TabIndex = 69;
            this.DJName.GotFocus += new System.EventHandler(this.DJName_GotFocus);
            this.DJName.LostFocus += new System.EventHandler(this.DJName_LostFocus);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(20, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 20);
            this.label5.Text = "端架名称描述:";
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(0, 274);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(72, 20);
            this.btn01.TabIndex = 74;
            this.btn01.Text = "确 定";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // DJId
            // 
            this.DJId.Location = new System.Drawing.Point(111, 60);
            this.DJId.Name = "DJId";
            this.DJId.ReadOnly = true;
            this.DJId.Size = new System.Drawing.Size(124, 21);
            this.DJId.TabIndex = 81;
            // 
            // DJDateFrom
            // 
            this.DJDateFrom.CustomFormat = "";
            this.DJDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DJDateFrom.Location = new System.Drawing.Point(23, 147);
            this.DJDateFrom.MinDate = new System.DateTime(2014, 3, 20, 10, 30, 30, 176);
            this.DJDateFrom.Name = "DJDateFrom";
            this.DJDateFrom.Size = new System.Drawing.Size(83, 22);
            this.DJDateFrom.TabIndex = 88;
            this.DJDateFrom.Value = new System.DateTime(2014, 3, 20, 10, 30, 30, 176);
            // 
            // DJDateTo
            // 
            this.DJDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DJDateTo.Location = new System.Drawing.Point(152, 147);
            this.DJDateTo.MinDate = new System.DateTime(2014, 3, 20, 10, 30, 30, 269);
            this.DJDateTo.Name = "DJDateTo";
            this.DJDateTo.Size = new System.Drawing.Size(83, 22);
            this.DJDateTo.TabIndex = 89;
            this.DJDateTo.Value = new System.DateTime(2014, 3, 20, 10, 30, 30, 269);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(117, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 20);
            this.label2.Text = "至:";
            // 
            // FormDJInc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DJDateTo);
            this.Controls.Add(this.DJDateFrom);
            this.Controls.Add(this.DJId);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DJName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbBarI);
            this.KeyPreview = true;
            this.Name = "FormDJInc";
            this.Text = "端架新增";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DJName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.TextBox DJId;
        private System.Windows.Forms.DateTimePicker DJDateFrom;
        private System.Windows.Forms.DateTimePicker DJDateTo;
        private System.Windows.Forms.Label label2;
    }
}