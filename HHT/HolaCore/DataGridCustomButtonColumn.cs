using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HolaCore
{
    public class DataGridCustomButtonColumn : DataGridCustomColumnBase
    {
        string _nullValue = "详细";

        public override object NullValue
        {
            get { return _nullValue; }
            set
            {
                if (!(value is string))
                {
                    throw new ArgumentException("Value sould be of type Decimal for this property.");
                }

                if ((string)value != _nullValue)
                {
                    _nullValue = (string)value;
                    this.Owner.Invalidate();
                }
            }
        }

        EventHandler _clickHandler = null;

        public DataGridCustomButtonColumn(EventHandler handler)
        {
            _clickHandler = handler;
        }

        public virtual Button Button
        {
            get { return this.HostedControl as Button; }
        }

        protected override Control CreateHostedControl()
        {
            Button btn = new Button();
            btn.Text = _nullValue;
            btn.Click += _clickHandler;

            return btn;
        }

        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
        {
            Object cellData;                                                    // Object to show in the cell 

            //DrawBackground(g, bounds, rowNum, backBrush);                       // Draw cell background

            cellData = this.PropertyDescriptor.GetValue(source.List[rowNum]);   // Get data for this cell from data source.

            DrawButton(g, bounds, foreBrush, cellData != null ? cellData as string : _nullValue);

            this.updateHostedControl();                                         // Have to do that.
        }

        private void DrawButton(Graphics g, Rectangle bounds, Brush foreBrush, string txt)
        {
            Rectangle rc = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height);
            rc.X += 1;
            rc.Width -= 2;
            rc.Y += 1;
            rc.Height -= 2;
            g.FillRectangle(new SolidBrush(Color.LightGray), rc);

            Font font = new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);

            StringFormat format = new StringFormat();
            format.Alignment = System.Drawing.StringAlignment.Center;

            RectangleF rcF = new RectangleF(bounds.Left, bounds.Top, bounds.Width, bounds.Height);

            g.DrawString(txt, font, foreBrush, rcF, format);
        }
    }
}
