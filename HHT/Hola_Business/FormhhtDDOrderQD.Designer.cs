﻿namespace Hola_Business
{
    partial class FormhhtDDOrderQD
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
            this.dgHeader = new System.Windows.Forms.DataGrid();
            this.dgTable = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuDetail = new System.Windows.Forms.MenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.pbBar = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Page = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dgHeader
            // 
            this.dgHeader.BackColor = System.Drawing.Color.NavajoWhite;
            this.dgHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgHeader.ColumnHeadersVisible = false;
            this.dgHeader.ForeColor = System.Drawing.Color.Sienna;
            this.dgHeader.GridLineColor = System.Drawing.Color.Sienna;
            this.dgHeader.Location = new System.Drawing.Point(0, 40);
            this.dgHeader.Name = "dgHeader";
            this.dgHeader.RowHeadersVisible = false;
            this.dgHeader.SelectionBackColor = System.Drawing.Color.Sienna;
            this.dgHeader.Size = new System.Drawing.Size(240, 20);
            this.dgHeader.TabIndex = 100;
            this.dgHeader.TabStop = false;
            // 
            // dgTable
            // 
            this.dgTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgTable.ColumnHeadersVisible = false;
            this.dgTable.ContextMenu = this.contextMenu1;
            this.dgTable.Location = new System.Drawing.Point(0, 60);
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.Size = new System.Drawing.Size(240, 139);
            this.dgTable.TabIndex = 101;
            this.dgTable.TabStop = false;
            this.dgTable.CurrentCellChanged += new System.EventHandler(this.dgTable_CurrentCellChanged);
            this.dgTable.GotFocus += new System.EventHandler(this.dgTable_GotFocus);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.menuDetail);
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuDetail
            // 
            this.menuDetail.Text = "删除";
            this.menuDetail.Click += new System.EventHandler(this.menuDetail_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.NavajoWhite;
            this.button3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button3.ForeColor = System.Drawing.Color.Sienna;
            this.button3.Location = new System.Drawing.Point(2, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 21);
            this.button3.TabIndex = 102;
            this.button3.Text = "删除";
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.NavajoWhite;
            this.button4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button4.ForeColor = System.Drawing.Color.Sienna;
            this.button4.Location = new System.Drawing.Point(2, 273);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(74, 20);
            this.button4.TabIndex = 103;
            this.button4.Text = "提交/导出";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.NavajoWhite;
            this.button5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button5.ForeColor = System.Drawing.Color.Sienna;
            this.button5.Location = new System.Drawing.Point(142, 273);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(93, 20);
            this.button5.TabIndex = 104;
            this.button5.Text = "返回新增页面";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // pbBar
            // 
            this.pbBar.Location = new System.Drawing.Point(-5, 264);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(240, 2);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.NavajoWhite;
            this.button1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button1.ForeColor = System.Drawing.Color.Sienna;
            this.button1.Location = new System.Drawing.Point(15, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 21);
            this.button1.TabIndex = 105;
            this.button1.Text = "上一页";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.NavajoWhite;
            this.button2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button2.ForeColor = System.Drawing.Color.Sienna;
            this.button2.Location = new System.Drawing.Point(153, 225);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 21);
            this.button2.TabIndex = 106;
            this.button2.Text = "下一页";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Page
            // 
            this.Page.Location = new System.Drawing.Point(104, 225);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(31, 20);
            this.Page.Text = "1/1";
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.NavajoWhite;
            this.button6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.button6.ForeColor = System.Drawing.Color.Sienna;
            this.button6.Location = new System.Drawing.Point(163, 6);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(72, 21);
            this.button6.TabIndex = 109;
            this.button6.Text = "全部删除";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // FormhhtDDOrderQD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.Page);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pbBar);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dgTable);
            this.Controls.Add(this.dgHeader);
            this.KeyPreview = true;
            this.Name = "FormhhtDDOrderQD";
            this.Text = "TRF下单清单";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgHeader;
        private System.Windows.Forms.DataGrid dgTable;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.PictureBox pbBar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label Page;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuDetail;
    }
}