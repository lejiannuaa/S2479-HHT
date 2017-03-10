namespace Hola_Business
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("1.价格查询");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("2.库存查询");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("3.折扣详细");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("4.他店库存");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("5.价标作业");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("6.端架管理");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("7.下单申请");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("TRF下单申请新增");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("TRF下单申请查询");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("PO下单申请新增");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("PO下单申请查询");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("8.库存调整");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("库存调整新增");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("库存调整查询");
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.FunChoose = new System.Windows.Forms.TreeView();
            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.btnReturn = new System.Windows.Forms.Button();
            this.ShopNO = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 32);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // FunChoose
            // 
            this.FunChoose.Indent = 40;
            this.FunChoose.Location = new System.Drawing.Point(20, 40);
            this.FunChoose.Name = "FunChoose";
            treeNode1.ForeColor = System.Drawing.Color.Blue;
            treeNode1.Text = "1.价格查询";
            treeNode2.ForeColor = System.Drawing.Color.Blue;
            treeNode2.Text = "2.库存查询";
            treeNode3.ForeColor = System.Drawing.Color.Blue;
            treeNode3.Text = "3.折扣详细";
            treeNode4.ForeColor = System.Drawing.Color.Blue;
            treeNode4.Text = "4.他店库存";
            treeNode5.ForeColor = System.Drawing.Color.Blue;
            treeNode5.Text = "5.价标作业";
            treeNode6.ForeColor = System.Drawing.Color.Blue;
            treeNode6.Text = "6.端架管理";
            treeNode7.ForeColor = System.Drawing.Color.Blue;
            treeNode8.ForeColor = System.Drawing.Color.Blue;
            treeNode8.Text = "TRF下单申请新增";
            treeNode9.ForeColor = System.Drawing.Color.Blue;
            treeNode9.Text = "TRF下单申请查询";
            treeNode10.ForeColor = System.Drawing.Color.Blue;
            treeNode10.Text = "PO下单申请新增";
            treeNode11.ForeColor = System.Drawing.Color.Blue;
            treeNode11.Text = "PO下单申请查询";
            treeNode7.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            treeNode7.Text = "7.下单申请";
            treeNode12.ForeColor = System.Drawing.Color.Blue;
            treeNode13.ForeColor = System.Drawing.Color.Blue;
            treeNode13.Text = "库存调整新增";
            treeNode14.ForeColor = System.Drawing.Color.Blue;
            treeNode14.Text = "库存调整查询";
            treeNode12.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14});
            treeNode12.Text = "8.库存调整";
            this.FunChoose.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode12});
            this.FunChoose.Size = new System.Drawing.Size(190, 220);
            this.FunChoose.TabIndex = 4;
            this.FunChoose.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FunChoose_AfterSelect);
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 266);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(168, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 22;
            this.btnReturn.Text = "退 出";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 10);
            this.ShopNO.Name = "ShopNO";
            this.ShopNO.Size = new System.Drawing.Size(39, 20);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(20, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 20);
            this.label6.Text = "店号:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.ShopNO);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.pbBarI);
            this.Controls.Add(this.FunChoose);
            this.Controls.Add(this.pbBar);
            this.Name = "FormMain";
            this.Text = "营业课";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.TreeView FunChoose;
        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label label6;
    }
}