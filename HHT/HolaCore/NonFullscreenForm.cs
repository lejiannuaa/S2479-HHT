using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.Status;
using Microsoft.WindowsCE.Forms;

namespace HolaCore
{
  public class NonFullscreenForm : Form
  {
    private SystemState displayRotationState = new SystemState(SystemProperty.DisplayRotation);
    private bool centered = true;

    #region Native Platform Invoke

    [DllImport("coredll.dll")]
    private static extern UInt32 SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

    [DllImport("aygshell.dll")]
    private static extern int SHDoneButton(IntPtr hwndRequester, UInt32 dwState);

    private readonly int GWL_STYLE = (-16);

    private readonly UInt32 WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
    private readonly UInt32 WS_BORDER = 0x00800000;
    private readonly UInt32 WS_POPUP = 0x80000000; 

    private readonly UInt32 SHDB_SHOW = 0x0001;
    private readonly UInt32 SHDB_HIDE = 0x0002;

    #endregion

    public NonFullscreenForm()
    {
      displayRotationState.Changed += new ChangeEventHandler(displayRotationState_Changed);
    }

    public bool CenterFormOnScreen
    {
      get
      {
        return centered;
      }
      set
      {
        centered = value;

        if (centered)
        {
          CenterWithinScreen();
        }
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      // By default if you set a form's size within
      // the Visual Studio form designer it won't
      // take into account the additional height of
      // the caption, so we'll add that height here...
      this.Height += SystemInformation.MenuHeight;

      base.OnLoad(e);

      // Add the border and caption we removed from the form
      // when we set the Form's FormBorderStyle property to None.
      // We do this at the Win32 API level, which causes the .NET
      // Compact Framework wrapper to get out of sync.
      uint style = WS_BORDER | WS_CAPTION | WS_POPUP;
      SetWindowLong(Handle, GWL_STYLE, style);

      // Add/Remove an [OK] button from the dialog's
      // caption bar as required
      SHDoneButton(Handle, ControlBox ? SHDB_SHOW : SHDB_HIDE);

      // Center the form if requested
      if (centered)
      {
        CenterWithinScreen();
      }
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      // If the dialog changes size and we want to be
      // centered we may need to move the dialog to
      // keep it centered.
      if (centered)
      {
        CenterWithinScreen();
      }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);

      // If we have an [OK] button in the caption pressing
      // Return or Escape should close the dialog
      if (this.ControlBox)
      {
        if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Escape)
        {
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void displayRotationState_Changed(object sender, ChangeEventArgs args)
    {
      // If the orientation has changed and the CenterFormOnScreen
      // property is set re-center the form
      if (centered)
      {
        CenterWithinScreen();
      }
    }

    private void CenterWithinScreen()
    {
      // Move the position of this form to center it within the
      // working area of the desktop
      int x = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
      int y = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;

      this.Location = new Point(x, y);
    }
  }
}
