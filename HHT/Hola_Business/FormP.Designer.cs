namespace Hola_Business
{
    partial class FormP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormP));
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GoodsNO = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SKUName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PresPrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.OrgPrice = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn01 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btn02 = new System.Windows.Forms.Button();
            this.btn03 = new System.Windows.Forms.Button();
            this.btn04 = new System.Windows.Forms.Button();
            this.Unit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Model = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.OrgPlace = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Specifications = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ProStatus = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.Bvano = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnReturn = new System.Windows.Forms.Button();
            this.ShopNO = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
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
            this.label3.Location = new System.Drawing.Point(5, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.Text = "SKU:";
            // 
            // GoodsNO
            // 
            this.GoodsNO.Enabled = false;
            this.GoodsNO.Location = new System.Drawing.Point(36, 96);
            this.GoodsNO.Name = "GoodsNO";
            this.GoodsNO.Size = new System.Drawing.Size(75, 21);
            this.GoodsNO.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(1, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.Text = "货号:";
            // 
            // SKUName
            // 
            this.SKUName.Enabled = false;
            this.SKUName.Location = new System.Drawing.Point(36, 71);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(139, 21);
            this.SKUName.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(1, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.Text = "品名:";
            // 
            // PresPrice
            // 
            this.PresPrice.Enabled = false;
            this.PresPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.PresPrice.Location = new System.Drawing.Point(157, 121);
            this.PresPrice.Name = "PresPrice";
            this.PresPrice.ReadOnly = true;
            this.PresPrice.Size = new System.Drawing.Size(81, 20);
            this.PresPrice.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label6.Location = new System.Drawing.Point(117, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 20);
            this.label6.Text = "现价:";
            // 
            // OrgPrice
            // 
            this.OrgPrice.Enabled = false;
            this.OrgPrice.Location = new System.Drawing.Point(36, 121);
            this.OrgPrice.Name = "OrgPrice";
            this.OrgPrice.Size = new System.Drawing.Size(75, 21);
            this.OrgPrice.TabIndex = 46;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(1, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 20);
            this.label7.Text = "原价:";
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(115, 46);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(60, 20);
            this.btn01.TabIndex = 48;
            this.btn01.Text = "查 询";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(178, 46);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 20);
            this.btnDelete.TabIndex = 49;
            this.btnDelete.Text = "清 除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(115, 95);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(60, 20);
            this.btn02.TabIndex = 50;
            this.btn02.Text = "国际条码";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Enabled = false;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(178, 72);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(60, 20);
            this.btn03.TabIndex = 51;
            this.btn03.Text = "子母商品";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // btn04
            // 
            this.btn04.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn04.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn04.ForeColor = System.Drawing.Color.Sienna;
            this.btn04.Location = new System.Drawing.Point(178, 95);
            this.btn04.Name = "btn04";
            this.btn04.Size = new System.Drawing.Size(60, 20);
            this.btn04.TabIndex = 52;
            this.btn04.Text = "折扣详细";
            this.btn04.Click += new System.EventHandler(this.btn04_Click);
            // 
            // Unit
            // 
            this.Unit.Enabled = false;
            this.Unit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.Unit.Location = new System.Drawing.Point(36, 148);
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Size = new System.Drawing.Size(75, 20);
            this.Unit.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label8.Location = new System.Drawing.Point(0, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 20);
            this.label8.Text = "单位:";
            // 
            // Model
            // 
            this.Model.Enabled = false;
            this.Model.Location = new System.Drawing.Point(145, 148);
            this.Model.Name = "Model";
            this.Model.Size = new System.Drawing.Size(93, 21);
            this.Model.TabIndex = 57;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(112, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 20);
            this.label9.Text = "型号:";
            // 
            // OrgPlace
            // 
            this.OrgPlace.Enabled = false;
            this.OrgPlace.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.OrgPlace.Location = new System.Drawing.Point(145, 175);
            this.OrgPlace.Name = "OrgPlace";
            this.OrgPlace.ReadOnly = true;
            this.OrgPlace.Size = new System.Drawing.Size(93, 20);
            this.OrgPlace.TabIndex = 62;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label10.Location = new System.Drawing.Point(112, 179);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 20);
            this.label10.Text = "产地:";
            // 
            // Specifications
            // 
            this.Specifications.Enabled = false;
            this.Specifications.Location = new System.Drawing.Point(36, 175);
            this.Specifications.Name = "Specifications";
            this.Specifications.Size = new System.Drawing.Size(75, 21);
            this.Specifications.TabIndex = 61;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(0, 179);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 20);
            this.label11.Text = "规格:";
            // 
            // ProStatus
            // 
            this.ProStatus.Enabled = false;
            this.ProStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.ProStatus.Location = new System.Drawing.Point(64, 202);
            this.ProStatus.Name = "ProStatus";
            this.ProStatus.ReadOnly = true;
            this.ProStatus.Size = new System.Drawing.Size(47, 20);
            this.ProStatus.TabIndex = 66;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label12.Location = new System.Drawing.Point(-1, 203);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 20);
            this.label12.Text = "商品状态:";
            // 
            // Bvano
            // 
            this.Bvano.Enabled = false;
            this.Bvano.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.Bvano.Location = new System.Drawing.Point(178, 202);
            this.Bvano.Name = "Bvano";
            this.Bvano.ReadOnly = true;
            this.Bvano.Size = new System.Drawing.Size(60, 20);
            this.Bvano.TabIndex = 69;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label13.Location = new System.Drawing.Point(112, 207);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 20);
            this.label13.Text = "是否POG:";
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
            this.btnReturn.TabIndex = 73;
            this.btnReturn.Text = "退 出";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 9);
            this.ShopNO.Name = "ShopNO";
            this.ShopNO.Size = new System.Drawing.Size(39, 20);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 20);
            this.label2.Text = "店号:";
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(36, 46);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(75, 21);
            this.SKUNO.TabIndex = 27;
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // FormP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.SKUNO);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.Bvano);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.ProStatus);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.OrgPlace);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Specifications);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Model);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Unit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btn04);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.OrgPrice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PresPrice);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SKUName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GoodsNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbBarI);
            this.KeyPreview = true;
            this.Name = "FormP";
            this.Text = "价格查询";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormP_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GoodsNO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SKUName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PresPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox OrgPrice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Button btn04;
        private System.Windows.Forms.TextBox Unit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Model;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox OrgPlace;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Specifications;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox ProStatus;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Bvano;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SKUNO;
    }
}