namespace Hola_Business
{
    partial class FormInv
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInv));
            this.btnReturn = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btn01 = new System.Windows.Forms.Button();
            this.SKUNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SKUName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.GoodsNO = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SDPT = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OrgPrice = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.PrePrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Status = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AllowOrder = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LastOrderDate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Inventory = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.DBInTransit = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TWeekSales = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TMAvg = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Items = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.POInTransit = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.DBNeed = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.LWeekSales = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.EMAvg = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btn02 = new System.Windows.Forms.Button();
            this.btn03 = new System.Windows.Forms.Button();
            this.btn04 = new System.Windows.Forms.Button();
            this.Bvano = new System.Windows.Forms.TextBox();
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
            this.btnReturn.TabIndex = 75;
            this.btnReturn.Text = "退 出";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 268);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnDelete.ForeColor = System.Drawing.Color.Sienna;
            this.btnDelete.Location = new System.Drawing.Point(177, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 20);
            this.btnDelete.TabIndex = 80;
            this.btnDelete.Text = "清 除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btn01
            // 
            this.btn01.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn01.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn01.ForeColor = System.Drawing.Color.Sienna;
            this.btn01.Location = new System.Drawing.Point(113, 3);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(60, 20);
            this.btn01.TabIndex = 79;
            this.btn01.Text = "查 询";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // SKUNO
            // 
            this.SKUNO.Location = new System.Drawing.Point(34, 3);
            this.SKUNO.Name = "SKUNO";
            this.SKUNO.Size = new System.Drawing.Size(74, 21);
            this.SKUNO.TabIndex = 0;
            this.SKUNO.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SKUNO_KeyUp);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.Text = "SKU:";
            // 
            // SKUName
            // 
            this.SKUName.Enabled = false;
            this.SKUName.Location = new System.Drawing.Point(34, 28);
            this.SKUName.Name = "SKUName";
            this.SKUName.Size = new System.Drawing.Size(139, 21);
            this.SKUName.TabIndex = 85;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(2, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.Text = "品名:";
            // 
            // GoodsNO
            // 
            this.GoodsNO.Enabled = false;
            this.GoodsNO.Location = new System.Drawing.Point(42, 76);
            this.GoodsNO.Name = "GoodsNO";
            this.GoodsNO.Size = new System.Drawing.Size(131, 21);
            this.GoodsNO.TabIndex = 84;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.Text = "货号:";
            // 
            // SDPT
            // 
            this.SDPT.Enabled = false;
            this.SDPT.Location = new System.Drawing.Point(42, 52);
            this.SDPT.Name = "SDPT";
            this.SDPT.Size = new System.Drawing.Size(131, 21);
            this.SDPT.TabIndex = 89;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.Text = "S-DPT:";
            // 
            // OrgPrice
            // 
            this.OrgPrice.Enabled = false;
            this.OrgPrice.Location = new System.Drawing.Point(37, 124);
            this.OrgPrice.Name = "OrgPrice";
            this.OrgPrice.Size = new System.Drawing.Size(56, 21);
            this.OrgPrice.TabIndex = 94;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(2, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 20);
            this.label7.Text = "原价:";
            // 
            // PrePrice
            // 
            this.PrePrice.Enabled = false;
            this.PrePrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.PrePrice.Location = new System.Drawing.Point(37, 100);
            this.PrePrice.Name = "PrePrice";
            this.PrePrice.ReadOnly = true;
            this.PrePrice.Size = new System.Drawing.Size(56, 20);
            this.PrePrice.TabIndex = 93;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label6.Location = new System.Drawing.Point(3, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 20);
            this.label6.Text = "现价:";
            // 
            // Status
            // 
            this.Status.Enabled = false;
            this.Status.Location = new System.Drawing.Point(65, 243);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(43, 21);
            this.Status.TabIndex = 100;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.Text = "状态:";
            // 
            // AllowOrder
            // 
            this.AllowOrder.Enabled = false;
            this.AllowOrder.Location = new System.Drawing.Point(170, 100);
            this.AllowOrder.Name = "AllowOrder";
            this.AllowOrder.Size = new System.Drawing.Size(67, 21);
            this.AllowOrder.TabIndex = 99;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(97, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 20);
            this.label8.Text = "是否上Hold:";
            // 
            // LastOrderDate
            // 
            this.LastOrderDate.Enabled = false;
            this.LastOrderDate.Location = new System.Drawing.Point(170, 124);
            this.LastOrderDate.Name = "LastOrderDate";
            this.LastOrderDate.Size = new System.Drawing.Size(67, 21);
            this.LastOrderDate.TabIndex = 104;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(97, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 20);
            this.label9.Text = "最后下单日:";
            // 
            // Inventory
            // 
            this.Inventory.Enabled = false;
            this.Inventory.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.Inventory.Location = new System.Drawing.Point(42, 148);
            this.Inventory.Name = "Inventory";
            this.Inventory.ReadOnly = true;
            this.Inventory.Size = new System.Drawing.Size(66, 20);
            this.Inventory.TabIndex = 107;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label10.Location = new System.Drawing.Point(2, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 20);
            this.label10.Text = "库存:";
            // 
            // DBInTransit
            // 
            this.DBInTransit.Enabled = false;
            this.DBInTransit.Location = new System.Drawing.Point(65, 172);
            this.DBInTransit.Name = "DBInTransit";
            this.DBInTransit.Size = new System.Drawing.Size(43, 21);
            this.DBInTransit.TabIndex = 110;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(2, 174);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 20);
            this.label11.Text = "调拨在途:";
            // 
            // TWeekSales
            // 
            this.TWeekSales.Enabled = false;
            this.TWeekSales.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.TWeekSales.Location = new System.Drawing.Point(65, 195);
            this.TWeekSales.Name = "TWeekSales";
            this.TWeekSales.ReadOnly = true;
            this.TWeekSales.Size = new System.Drawing.Size(43, 20);
            this.TWeekSales.TabIndex = 113;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label12.Location = new System.Drawing.Point(2, 197);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 20);
            this.label12.Text = "本周销量:";
            // 
            // TMAvg
            // 
            this.TMAvg.Enabled = false;
            this.TMAvg.Location = new System.Drawing.Point(65, 218);
            this.TMAvg.Name = "TMAvg";
            this.TMAvg.Size = new System.Drawing.Size(43, 21);
            this.TMAvg.TabIndex = 116;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(2, 220);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 20);
            this.label13.Text = "三月平均:";
            // 
            // Items
            // 
            this.Items.Location = new System.Drawing.Point(65, 270);
            this.Items.Name = "Items";
            this.Items.Size = new System.Drawing.Size(43, 21);
            this.Items.TabIndex = 119;
            this.Items.Visible = false;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 274);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 20);
            this.label14.Text = "展示品数:";
            this.label14.Visible = false;
            // 
            // POInTransit
            // 
            this.POInTransit.Enabled = false;
            this.POInTransit.Location = new System.Drawing.Point(170, 148);
            this.POInTransit.Name = "POInTransit";
            this.POInTransit.Size = new System.Drawing.Size(67, 21);
            this.POInTransit.TabIndex = 122;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(120, 150);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 20);
            this.label15.Text = "PO在途:";
            // 
            // DBNeed
            // 
            this.DBNeed.Enabled = false;
            this.DBNeed.Location = new System.Drawing.Point(197, 172);
            this.DBNeed.Name = "DBNeed";
            this.DBNeed.Size = new System.Drawing.Size(40, 21);
            this.DBNeed.TabIndex = 125;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(120, 174);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(82, 20);
            this.label16.Text = "调拨需求量:";
            // 
            // LWeekSales
            // 
            this.LWeekSales.Enabled = false;
            this.LWeekSales.Location = new System.Drawing.Point(183, 195);
            this.LWeekSales.Name = "LWeekSales";
            this.LWeekSales.Size = new System.Drawing.Size(54, 21);
            this.LWeekSales.TabIndex = 128;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(120, 197);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(72, 20);
            this.label17.Text = "上周销量:";
            // 
            // EMAvg
            // 
            this.EMAvg.Enabled = false;
            this.EMAvg.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.EMAvg.Location = new System.Drawing.Point(183, 218);
            this.EMAvg.Name = "EMAvg";
            this.EMAvg.ReadOnly = true;
            this.EMAvg.Size = new System.Drawing.Size(54, 20);
            this.EMAvg.TabIndex = 131;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label18.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label18.Location = new System.Drawing.Point(120, 220);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 20);
            this.label18.Text = "八周平均:";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label19.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label19.Location = new System.Drawing.Point(121, 245);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 20);
            this.label19.Text = "是否POG:";
            // 
            // btn02
            // 
            this.btn02.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn02.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn02.ForeColor = System.Drawing.Color.Sienna;
            this.btn02.Location = new System.Drawing.Point(177, 28);
            this.btn02.Name = "btn02";
            this.btn02.Size = new System.Drawing.Size(60, 20);
            this.btn02.TabIndex = 136;
            this.btn02.Text = "在途PO";
            this.btn02.Click += new System.EventHandler(this.btn02_Click);
            // 
            // btn03
            // 
            this.btn03.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn03.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn03.ForeColor = System.Drawing.Color.Sienna;
            this.btn03.Location = new System.Drawing.Point(177, 52);
            this.btn03.Name = "btn03";
            this.btn03.Size = new System.Drawing.Size(60, 20);
            this.btn03.TabIndex = 137;
            this.btn03.Text = "在途调拨";
            this.btn03.Click += new System.EventHandler(this.btn03_Click);
            // 
            // btn04
            // 
            this.btn04.BackColor = System.Drawing.Color.NavajoWhite;
            this.btn04.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btn04.ForeColor = System.Drawing.Color.Sienna;
            this.btn04.Location = new System.Drawing.Point(177, 77);
            this.btn04.Name = "btn04";
            this.btn04.Size = new System.Drawing.Size(60, 20);
            this.btn04.TabIndex = 138;
            this.btn04.Text = "他店库存";
            this.btn04.Click += new System.EventHandler(this.btn04_Click);
            // 
            // Bvano
            // 
            this.Bvano.Enabled = false;
            this.Bvano.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.Bvano.Location = new System.Drawing.Point(197, 243);
            this.Bvano.Name = "Bvano";
            this.Bvano.ReadOnly = true;
            this.Bvano.Size = new System.Drawing.Size(40, 20);
            this.Bvano.TabIndex = 134;
            // 
            // FormInv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btn04);
            this.Controls.Add(this.btn03);
            this.Controls.Add(this.btn02);
            this.Controls.Add(this.Bvano);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.EMAvg);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.LWeekSales);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.DBNeed);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.POInTransit);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.Items);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.TMAvg);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.TWeekSales);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.DBInTransit);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Inventory);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.LastOrderDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AllowOrder);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.OrgPrice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PrePrice);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SDPT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SKUName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GoodsNO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.SKUNO);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBar);
            this.KeyPreview = true;
            this.Name = "FormInv";
            this.Text = "库存查询";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btn01;
        private System.Windows.Forms.TextBox SKUNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SKUName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox GoodsNO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SDPT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OrgPrice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox PrePrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AllowOrder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LastOrderDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Inventory;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox DBInTransit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TWeekSales;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TMAvg;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox Items;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox POInTransit;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox DBNeed;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox LWeekSales;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox EMAvg;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btn02;
        private System.Windows.Forms.Button btn03;
        private System.Windows.Forms.Button btn04;
        private System.Windows.Forms.TextBox Bvano;
    }
}