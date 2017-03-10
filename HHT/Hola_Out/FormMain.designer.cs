using System;
namespace Hola_Out
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
            this.btnReturn = new System.Windows.Forms.Button();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuNew = new System.Windows.Forms.MenuItem();
            this.menuModify = new System.Windows.Forms.MenuItem();
            this.normalReserve = new System.Windows.Forms.MenuItem();
            this.otherReserve = new System.Windows.Forms.MenuItem();
            this.queryReserve = new System.Windows.Forms.MenuItem();
            this.ObjOutPrint = new System.Windows.Forms.MenuItem();
            this.normalObjOut = new System.Windows.Forms.MenuItem();
            this.otherObjOut = new System.Windows.Forms.MenuItem();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.btnPurchase = new HolaCore.ImageButton();
            this.btnShop = new HolaCore.ImageButton();
            this.btnWarehouse = new HolaCore.ImageButton();
            this.btnManufacturer = new HolaCore.ImageButton();
            this.btnReserve = new HolaCore.ImageButton();
            this.btnObjOut = new HolaCore.ImageButton();
            this.SuspendLayout();
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.btnReturn.ForeColor = System.Drawing.Color.Sienna;
            this.btnReturn.Location = new System.Drawing.Point(0, 274);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(72, 20);
            this.btnReturn.TabIndex = 31;
            this.btnReturn.Text = "返 回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.Add(this.menuNew);
            this.contextMenu.MenuItems.Add(this.menuModify);
            // 
            // menuNew
            // 
            this.menuNew.Text = "新建";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuModify
            // 
            this.menuModify.Text = "修改";
            this.menuModify.Click += new System.EventHandler(this.menuModify_Click);
            // 
            // normalReserve
            // 
            this.normalReserve.Text = "正常品预约";
            this.normalReserve.Click += new System.EventHandler(this.normalReserve_Click);
            // 
            // otherReserve
            // 
            this.otherReserve.Text = "其他类预约";
            this.otherReserve.Click += new System.EventHandler(this.otherReserve_Click);
            // 
            // queryReserve
            // 
            this.queryReserve.Text = "预约查询";
            this.queryReserve.Click += new System.EventHandler(this.queryReserve_Click);
            // 
            // ObjOutPrint
            // 
            this.ObjOutPrint.Text = "实物出货列印";
            this.ObjOutPrint.Click += new System.EventHandler(this.ObjOutPrint_Click);
            // 
            // normalObjOut
            // 
            this.normalObjOut.Text = "正常品出货";
            this.normalObjOut.Click += new System.EventHandler(this.normalObjOut_Click);
            // 
            // otherObjOut
            // 
            this.otherObjOut.Text = "其他类出货";
            this.otherObjOut.Click += new System.EventHandler(this.otherObjOut_Click);
            // 
            // pbBar
            // 
            this.pbBar.Image = ((System.Drawing.Image)(resources.GetObject("pbBar.Image")));
            this.pbBar.Location = new System.Drawing.Point(0, 269);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // btnPurchase
            // 
            this.btnPurchase.Location = new System.Drawing.Point(21, 94);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.ShowFrame = true;
            this.btnPurchase.ShowText = true;
            this.btnPurchase.Size = new System.Drawing.Size(80, 85);
            this.btnPurchase.TabIndex = 4;
            this.btnPurchase.Text = "采购出货";
            this.btnPurchase.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnPurchase.Click += new System.EventHandler(this.btnPurchase_Click);
            // 
            // btnShop
            // 
            this.btnShop.Location = new System.Drawing.Point(140, 3);
            this.btnShop.Name = "btnShop";
            this.btnShop.ShowFrame = true;
            this.btnShop.ShowText = true;
            this.btnShop.Size = new System.Drawing.Size(80, 85);
            this.btnShop.TabIndex = 3;
            this.btnShop.Text = "调拨-门店";
            this.btnShop.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnShop.Click += new System.EventHandler(this.btnShop_Click);
            // 
            // btnWarehouse
            // 
            this.btnWarehouse.Location = new System.Drawing.Point(140, 3);
            this.btnWarehouse.Name = "btnWarehouse";
            this.btnWarehouse.ShowFrame = true;
            this.btnWarehouse.ShowText = true;
            this.btnWarehouse.Size = new System.Drawing.Size(80, 85);
            this.btnWarehouse.TabIndex = 2;
            this.btnWarehouse.Text = "大仓";
            this.btnWarehouse.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnWarehouse.Visible = false;
            this.btnWarehouse.Click += new System.EventHandler(this.btnWarehouse_Click);
            // 
            // btnManufacturer
            // 
            this.btnManufacturer.Location = new System.Drawing.Point(21, 3);
            this.btnManufacturer.Name = "btnManufacturer";
            this.btnManufacturer.ShowFrame = true;
            this.btnManufacturer.ShowText = true;
            this.btnManufacturer.Size = new System.Drawing.Size(80, 85);
            this.btnManufacturer.TabIndex = 1;
            this.btnManufacturer.Text = "厂商/DC";
            this.btnManufacturer.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnManufacturer.Click += new System.EventHandler(this.btnManufacturer_Click);
            // 
            // btnReserve
            // 
            this.btnReserve.Location = new System.Drawing.Point(140, 94);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.ShowFrame = true;
            this.btnReserve.ShowText = true;
            this.btnReserve.Size = new System.Drawing.Size(80, 85);
            this.btnReserve.TabIndex = 32;
            this.btnReserve.Text = "预约-门店";
            this.btnReserve.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // btnObjOut
            // 
            this.btnObjOut.Location = new System.Drawing.Point(21, 183);
            this.btnObjOut.Name = "btnObjOut";
            this.btnObjOut.ShowFrame = true;
            this.btnObjOut.ShowText = true;
            this.btnObjOut.Size = new System.Drawing.Size(80, 85);
            this.btnObjOut.TabIndex = 33;
            this.btnObjOut.Text = "实物出货";
            this.btnObjOut.TextFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.btnObjOut.Click += new System.EventHandler(this.btnObjOut_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.btnObjOut);
            this.Controls.Add(this.btnReserve);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnPurchase);
            this.Controls.Add(this.btnShop);
            this.Controls.Add(this.btnWarehouse);
            this.Controls.Add(this.btnManufacturer);
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private HolaCore.ImageButton btnManufacturer;
        private HolaCore.ImageButton btnWarehouse;
        private HolaCore.ImageButton btnShop;
        private HolaCore.ImageButton btnPurchase;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuNew;
        private System.Windows.Forms.MenuItem menuModify;

        private System.Windows.Forms.MenuItem normalReserve;
        private System.Windows.Forms.MenuItem otherReserve;
        private System.Windows.Forms.MenuItem queryReserve;

        private System.Windows.Forms.MenuItem normalObjOut;
        private System.Windows.Forms.MenuItem otherObjOut;
        private System.Windows.Forms.MenuItem ObjOutPrint;

        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.PictureBox pbBar;
        private HolaCore.ImageButton btnReserve;
        private HolaCore.ImageButton btnObjOut;
    }
}