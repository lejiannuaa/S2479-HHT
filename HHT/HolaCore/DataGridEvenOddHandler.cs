using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

namespace HolaCore
{
	/// <summary>
	/// Summary description for DataGridEvenOddHandler.
	/// </summary>
	public class DataGridEvenOddHandler: IDisposable
	{

		private DataGrid m_grid;
		private object m_r;
		private Color[] arrClr = new Color[] { Color.Cyan, Color.LightYellow };
		private Color[] arrClrText = new Color[] { Color.Black, Color.Black };
		private SolidBrush brCell, brCellFore;
		private FieldInfo m_fiRowVisibleFirst, m_fiRowVisibleLast, m_fiColVisibleFirst, m_fiColVisibleLast;
		private FieldInfo m_fiRowDrawFirst, m_fiRowDrawLast, m_fiColDrawFirst, m_fiColDrawLast;
		private FieldInfo m_fiRows;
		private MethodInfo m_miDrawCells;
		private DataGridCell currentCell;

		private int iRowVisibleFirst, iRowVisibleLast, iColVisibleFirst, iColVisibleLast;

		private int RowIndex = -1;
		
		public DataGridEvenOddHandler(DataGrid grid)
		{
			m_grid = grid;
			HookGrid();
		}

		private void HookGrid()
		{
			m_r = typeof(DataGrid).GetField("m_renderer", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_grid);
			m_fiRowDrawFirst = m_r.GetType().GetField("m_irowDrawFirst", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiRowDrawLast = m_r.GetType().GetField("m_irowDrawLast", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiColDrawFirst = m_r.GetType().GetField("m_icolDrawFirst", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiColDrawLast = m_r.GetType().GetField("m_icolDrawLast", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiRowVisibleFirst = typeof(DataGrid).GetField("m_irowVisibleFirst", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiRowVisibleLast = typeof(DataGrid).GetField("m_irowVisibleLast", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiColVisibleFirst = typeof(DataGrid).GetField("m_icolVisibleFirst", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiColVisibleLast = typeof(DataGrid).GetField("m_icolVisibleLast", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_fiRows = typeof(DataGrid).GetField("m_rlrow", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);
			m_miDrawCells =  m_r.GetType().GetMethod("_DrawCells", BindingFlags.NonPublic|BindingFlags.Instance);

			brCell = (SolidBrush )m_r.GetType().GetField("m_brushCellBack", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_r);
			brCellFore = (SolidBrush )m_r.GetType().GetField("m_brushCellFore", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_r);
			//currentCell = new DataGridCell(-1, -1);

			VScrollBar vsb = (VScrollBar)typeof(DataGrid).GetField("m_sbVert", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_grid);
			HScrollBar hsb = (HScrollBar)typeof(DataGrid).GetField("m_sbHorz", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_grid);

			vsb.ValueChanged += new EventHandler(vsb_ValueChanged);
			hsb.ValueChanged += new EventHandler(hsb_ValueChanged);

			m_grid.Paint += new PaintEventHandler(grid_Paint);
			m_grid.CurrentCellChanged += new EventHandler(grid_CurrentCellChanged);

			currentCell = m_grid.CurrentCell;

			iRowVisibleFirst = (int)m_fiRowVisibleFirst.GetValue(m_grid);
			iRowVisibleLast = (int)m_fiRowVisibleLast.GetValue(m_grid);
			iColVisibleFirst = (int)m_fiColVisibleFirst.GetValue(m_grid);
			iColVisibleLast = (int)m_fiColVisibleLast.GetValue(m_grid);
		}

		private void UnhookGrid()
		{
			if ( m_grid == null )
				return;

			VScrollBar vsb = (VScrollBar)typeof(DataGrid).GetField("m_sbVert", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_grid);
			HScrollBar hsb = (HScrollBar)typeof(DataGrid).GetField("m_sbHorz", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance).GetValue(m_grid);

			vsb.ValueChanged -= new EventHandler(vsb_ValueChanged);
			hsb.ValueChanged -= new EventHandler(hsb_ValueChanged);

			m_grid.Paint -= new PaintEventHandler(grid_Paint);
			m_grid.CurrentCellChanged -= new EventHandler(grid_CurrentCellChanged);
		}

		public Color OddRowColor
		{
			get { return arrClr[1]; }
			set { arrClr[1] = value; /*grid_Paint(null, null);*/ }
		}

		public Color EvenRowColor
		{
			get { return arrClr[0]; }
			set { arrClr[0] = value; /*grid_Paint(null, null); */}
		}

		public Color OddRowTextColor
		{
			get { return arrClrText[1]; }
			set { arrClrText[1] = value; /*grid_Paint(null, null);*/ }
		}

		public Color EvenRowTextColor
		{
			get { return arrClrText[0]; }
			set { arrClrText[0] = value; /*grid_Paint(null, null); */}
		}

		private void ForceRepaintGridRow(Graphics g, int row)
		{
			brCell.Color = arrClr[row % 2];
			brCellFore.Color = arrClrText[row % 2];
			m_fiRowDrawFirst.SetValue(m_r, row);
			m_fiRowDrawLast.SetValue(m_r, row);
			m_miDrawCells.Invoke(m_r, new object[] { g });
		}

		private void ForceRepaintGridRows( Graphics g, int rowStart, int rowEnd )
		{
			for ( int i = rowStart; i <= rowEnd; i++ )
			{
				brCell.Color = arrClr[i % 2];
				brCellFore.Color = arrClrText[i % 2];
				m_fiRowDrawFirst.SetValue(m_r, i);
				m_fiRowDrawLast.SetValue(m_r, i);
				m_miDrawCells.Invoke(m_r, new object[] { g });
			}
		}

		private void ForceRepaintGridRows( Graphics g, int [] range )
		{
			foreach ( int i in range )
			{
				brCell.Color = arrClr[i % 2];
				brCellFore.Color = arrClrText[i % 2];
				m_fiRowDrawFirst.SetValue(m_r, i);
				m_fiRowDrawLast.SetValue(m_r, i);
				m_miDrawCells.Invoke(m_r, new object[] { g });
			}
		}

		private void vsb_ValueChanged(object sender, EventArgs e)
		{
			int iRowFirst = (int)m_fiRowVisibleFirst.GetValue(m_grid);
			int iRowLast = (int)m_fiRowVisibleLast.GetValue(m_grid);
			Graphics g = m_grid.CreateGraphics();
			if ( iRowVisibleFirst > iRowFirst ) //Scroll Up
			{
				ForceRepaintGridRows(g, iRowFirst, iRowVisibleFirst);
			}
			else
			{
				ForceRepaintGridRows(g, iRowVisibleLast, iRowLast);
			}
			iRowVisibleFirst = iRowFirst;
			iRowVisibleLast = iRowLast;
			//grid_Paint(null, null);
		}

		private void hsb_ValueChanged(object sender, EventArgs e)
		{
			grid_Paint(null, null);
		}

		#region IDisposable Members

		public void Dispose()
		{
			UnhookGrid();
		}

		#endregion

		private void grid_Paint(object sender, PaintEventArgs e)
		{
			int rowFirst, rowLast;
			rowFirst = (int)m_fiRowVisibleFirst.GetValue(m_grid);
			rowLast = (int)m_fiRowVisibleLast.GetValue(m_grid);
			rowFirst = Math.Max(rowFirst, 0);
			rowLast = Math.Max(rowLast, 0);
			Color cl = brCell.Color;
			Color clText = brCellFore.Color;
			Graphics g;
			g = m_grid.CreateGraphics();
			ForceRepaintGridRows(g, rowFirst, rowLast);
			brCell.Color = cl;
			brCellFore.Color = clText;
		}

		private void grid_CurrentCellChanged(object sender, EventArgs e)
		{
			Color cl = brCell.Color;
			Color clText = brCellFore.Color;
			Graphics g = m_grid.CreateGraphics();
			int nFirstVisibleRow = (int)m_fiRowVisibleFirst.GetValue(m_grid);
			ArrayList Rows = (ArrayList) m_fiRows.GetValue(m_grid);
			object Row;
			FieldInfo fiSelected = Rows[0].GetType().GetField("m_fSelected", BindingFlags.NonPublic|BindingFlags.GetField|BindingFlags.Instance);

            if (m_grid.TableStyles[0].GridColumnStyles[currentCell.ColumnNumber] is DataGridCustomColumnBase)
            {
                ForceRepaintGridRows(g, 0, m_grid.TableStyles.Count);
            }
            else
            {
			    if ( RowIndex > -1 )
			    {
				    Row = Rows[RowIndex];
				    if (  (bool)fiSelected.GetValue(Row) )
					    m_grid.Invalidate(_GetRowBounds(RowIndex));
				    else
					    ForceRepaintGridRow(g, RowIndex);
			    }
			    currentCell = m_grid.CurrentCell;
			    RowIndex = m_grid.CurrentRowIndex;

			    if ( RowIndex > -1 )
			    {
				    Row = Rows[RowIndex];
				    if (  (bool)fiSelected.GetValue(Row) )
					    m_grid.Invalidate(_GetRowBounds(RowIndex));
				    else
					    ForceRepaintGridRow(g, RowIndex);
			    }
            }

			brCell.Color = cl;
			brCellFore.Color = clText;
		}

		private Rectangle _GetRowBounds(int iRow)
		{
			int colFirst = (int)m_fiColVisibleFirst.GetValue(m_grid);
			int colLast = (int)m_fiColVisibleLast.GetValue(m_grid);
			Rectangle r = m_grid.GetCellBounds(iRow, colFirst);
			for ( int i = colFirst; i <= colLast; i++ )
			{
				Rectangle rNew = m_grid.GetCellBounds(iRow, i);
				r.X = Math.Min(r.X, rNew.X);
				r.Width = Math.Max(r.Right, rNew.Right) - Math.Min(r.X, rNew.X);
			}

			return r;
		}
	}
}
