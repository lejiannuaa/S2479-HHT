namespace Hola_Inventory
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("2.初盘");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("3.复盘");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("一般复盘");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("课阶复盘");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("4.财务部抽盘");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("5.离开");

            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("1.盘点检核");

            this.pbBarI = new System.Windows.Forms.PictureBox();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.FunChoose = new System.Windows.Forms.TreeView();
            this.btnReturn = new System.Windows.Forms.Button();
            this.ShopNO = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbBarI
            // 
            this.pbBarI.Image = ((System.Drawing.Image)(resources.GetObject("pbBarI.Image")));
            this.pbBarI.Location = new System.Drawing.Point(0, 267);
            this.pbBarI.Name = "pbBarI";
            this.pbBarI.Size = new System.Drawing.Size(240, 2);
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
            this.FunChoose.Location = new System.Drawing.Point(40, 60);
            this.FunChoose.Name = "FunChoose";
            treeNode1.ForeColor = System.Drawing.Color.Blue;
            treeNode1.Text = "2.初盘";
            treeNode2.ForeColor = System.Drawing.Color.Blue;
            treeNode3.ForeColor = System.Drawing.Color.Blue;
            treeNode3.Text = "一般复盘";
            treeNode4.ForeColor = System.Drawing.Color.Blue;
            treeNode4.Text = "课阶复盘";
            treeNode2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            treeNode2.Text = "3.复盘";
            treeNode5.ForeColor = System.Drawing.Color.Blue;
            treeNode5.Text = "4.财务部抽盘";
            treeNode6.ForeColor = System.Drawing.Color.Blue;
            treeNode6.Text = "5.离开";
            treeNode7.ForeColor = System.Drawing.Color.Blue;
            treeNode7.Text = "1盘点检核";
            this.FunChoose.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
                            treeNode7,
            treeNode1,
            treeNode2,
            treeNode5,
            treeNode6,

            });
            this.FunChoose.Size = new System.Drawing.Size(160, 200);
            this.FunChoose.TabIndex = 10;
            this.FunChoose.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FunChoose_AfterSelect);
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 21;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // ShopNO
            // 
            this.ShopNO.Location = new System.Drawing.Point(62, 9);
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
            this.Controls.Add(this.FunChoose);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.pbBarI);
            this.Name = "FormMain";
            this.Text = "菜单选择";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBarI;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.TreeView FunChoose;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label ShopNO;
        private System.Windows.Forms.Label label6;
    }
}

