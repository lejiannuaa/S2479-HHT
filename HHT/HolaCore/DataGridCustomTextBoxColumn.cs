//--------------------------------------------------------------------- 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY 
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
//PARTICULAR PURPOSE. 
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Data;
namespace HolaCore
{
    // This is our editable TextBox column.
    public class DataGridCustomTextBoxColumn : DataGridCustomColumnBase
    {
        Regex reg = new Regex(@"^\d+$");


        // Let's add this so user can access 
        public virtual TextBox TextBox
        {
            get { return this.HostedControl as TextBox; }
        }
        
        protected override string GetBoundPropertyName()
        {
            return "Text";                                                          // Need to bount to "Text" property on TextBox
        }

        protected override Control CreateHostedControl()                            
        {
            TextBox box = new TextBox();// Our hosted control is a TextBox

            Font newfont = new Font("Tahoma", (float)10.0, FontStyle.Regular);
            
            box.Font = newfont;

            box.BorderStyle = BorderStyle.None;                                     // It has no border
            box.Multiline = true;                                                  // And it's multiline
                                                              
            box.TextAlign = HorizontalAlignment.Left;//  this.Alignment;                                         // Set up aligment.
            //box.LostFocus += new EventHandler(box_LostFocus);
            //box.GotFocus += new EventHandler(box_GotFocus);
            //this.Owner.MouseDown -= new MouseEventHandler(Owner_MouseDown);
            //this.Owner.MouseDown += new MouseEventHandler(Owner_MouseDown);
            //box.Validating+=new System.ComponentModel.CancelEventHandler(box_Validating);
            return box;
        }

        //void box_LostFocus(object sender, EventArgs e)
        //{
        //    int colIndex = this.Owner.CurrentCell.ColumnNumber - 1;
        //    //this.Owner.CurrentCell = new DataGridCell(this.Owner.CurrentCell.RowNumber, colIndex < 0 ? 0 : colIndex);
        //    DataTable dt = (DataTable)this.Owner.DataSource;
        //    string Total = dt.Rows[Owner.CurrentCell.RowNumber][Owner.CurrentCell.ColumnNumber].ToString();
           
        //}
      

        //void Owner_MouseDown(object sender, MouseEventArgs e)
        //{
        //    DataGrid.HitTestInfo hitTest = this.Owner.HitTest(e.X, e.Y);
        //    DataTable dt = (DataTable)this.Owner.DataSource;
        //    if (rowFlag < 0)
        //    {
        //        if (hitTest.Type == DataGrid.HitTestType.Cell)
        //        {
        //            rowFlag = hitTest.Row;
        //            colFlag = hitTest.Column;
        //            Oldtext = dt.Rows[rowFlag][colFlag].ToString();
        //        }
        //    }
        //    else if (rowFlag >= 0 && rowFlag != hitTest.Row)
        //    {

        //        if (hitTest.Type == DataGrid.HitTestType.Cell)
        //        {
        //            Oldtext = dt.Rows[rowFlag][colFlag].ToString();
        //        }

        //    }


        //}

        void box_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (reg.IsMatch(((TextBox)sender).Text) && ((TextBox)sender).Text != "")
            {
                string text = ((TextBox)sender).Text;
                if (int.Parse(((TextBox)sender).Text) > 0)
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        string subtext = text.Substring(i, 1);
                        if (int.Parse(subtext) == 0)
                        {
                            e.Cancel = true;
                            MessageBox.Show("请不要以0开头!");
                            break;
                        }
                    }
                }

                if (int.Parse(((TextBox)sender).Text) == 0)
                {
                    for (int i = 1; i < text.Length; i++)
                    {
                        string subtext = text.Substring(i, 1);
                        if (int.Parse(subtext) == 0)
                        {
                            e.Cancel = true;
                            MessageBox.Show("请不要以0开头!");
                            break;
                        }
                    }
                }

            }
            else
            {
                e.Cancel = true;
                MessageBox.Show("请输入非负整数！");
            }
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            Object cellData;                                                    // Object to show in the cell 

            cellData = this.PropertyDescriptor.GetValue(source.List[rowNum]);   // Get data for this cell from data source.

            if (cellData == null || ((String)cellData).Length==0)
            {
                bounds.Width++;
                DrawBackground(g, bounds, rowNum, backBrush);                       // Draw cell background
            }
            else
            {
                base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
            }
        }
    }
}
