using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DesktopColorSampler
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private const string freezeText = "Ctrl + / to freeze...";

        /// <summary>
        /// 
        /// </summary>
        private const string wanderText = "Ctrl + / to unfreeze...";

        /// <summary>
        /// 
        /// </summary>
        private bool capturing = true;

        /// <summary>
        /// 
        /// </summary>
        private bool ctrl = false;

        /// <summary>
        /// 
        /// </summary>
        private int hMouseHook;

        /// <summary>
        /// 
        /// </summary>
        private int hKeyboardHook;

        /// <summary>
        /// 
        /// </summary>
        private NativeMethods.HookProcDelegate MouseHook { set; get; }

        /// <summary>
        /// 
        /// </summary>
        private NativeMethods.HookProcDelegate KeyboardHook { set; get; }

        /// <summary>
        /// 
        /// </summary>
        private Bitmap bitmap = null;

        /// <summary>
        /// 
        /// </summary>
        private SolidBrush brush = null;

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.brush = new SolidBrush(Color.White);
            this.bitmap = new Bitmap(this.zoom.Width / 10, this.zoom.Height / 10);
            using(var g = Graphics.FromImage(this.bitmap as Image))
            {
                g.FillRectangle(this.brush, 0, 0, this.bitmap.Width, this.bitmap.Height);
            }

            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;

            // Loading User32.dll since SetWindowHookEx has trouble finding it
            IntPtr user32Hahdle = NativeMethods.LoadLibrary("user32.dll");

            this.MouseHook = new NativeMethods.HookProcDelegate(this.MouseHookProc);
            this.hMouseHook = NativeMethods.SetWindowsHookEx(
                NativeMethods.WH_MOUSE_LL,
                this.MouseHook,
                user32Hahdle, 
                0);

            this.KeyboardHook = new NativeMethods.HookProcDelegate(this.KeyboardHookProc);
            this.hKeyboardHook = NativeMethods.SetWindowsHookEx(
                NativeMethods.WH_KEYBOARD_LL,
                this.KeyboardHook,
                user32Hahdle,
                0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (this.capturing)
            {
                Color color = this.UpdateBitmap();
                this.SetColor(color);
                this.pressSpaceMessageBox.Text = freezeText;
            }

            return NativeMethods.CallNextHookEx(this.hMouseHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (wParam == NativeMethods.WM_KEYDOWN)
            {
                // Getting mouse data from a Win32 structure
                var keyboardInfo = (NativeMethods.KEYBOARDHOOKSTRUCT)Marshal.PtrToStructure(
                    lParam,
                    typeof(NativeMethods.KEYBOARDHOOKSTRUCT));

                if (keyboardInfo.vkCode == NativeMethods.VK_CONTROL)
                {
                    this.ctrl = true;
                }
                else if (keyboardInfo.vkCode == (int)Keys.OemQuestion && this.ctrl)
                {
                    this.capturing = !this.capturing;
                    this.editColorButton.Enabled = !this.capturing;
                    this.pressSpaceMessageBox.Text = this.capturing ? freezeText : wanderText;
                }
            }
            else if (wParam == NativeMethods.WM_KEYUP)
            {
                // Getting mouse data from a Win32 structure
                var keyboardInfo = (NativeMethods.KEYBOARDHOOKSTRUCT)Marshal.PtrToStructure(
                    lParam,
                    typeof(NativeMethods.KEYBOARDHOOKSTRUCT));

                if (keyboardInfo.vkCode == NativeMethods.VK_CONTROL)
                {
                    this.ctrl = false;
                }
            }

            return NativeMethods.CallNextHookEx(this.hKeyboardHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            NativeMethods.UnhookWindowsHookEx(this.hMouseHook);
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditColor()
        {
            if (!this.capturing)
            {
                this.colorDialog1.Color = this.GetColor();
                if(this.colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.SetColor(this.colorDialog1.Color);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(Color color)
        {
            this.hexValue.Text = string.Format("{0:X02}{1:X02}{2:X02}", color.R, color.G, color.B);
            this.rgbValue.Text = string.Format("{0,3},{1,3},{2,3}", color.R, color.G, color.B);
            this.brush = new SolidBrush(color);
            this.colorSample.Invalidate();
            this.zoom.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Color GetColor()
        {
            return this.brush.Color;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.EditColor();
        }

        /// <summary>
        /// 
        /// </summary>
        private Color UpdateBitmap()
        {
            if (this.zoomOn.Checked)
            {
                return UpdateBitmap(this.bitmap);
            }
            else
            {
                using(Bitmap tmpBitmap = new Bitmap(this.bitmap.Size.Width, this.bitmap.Size.Height))
                {
                    return UpdateBitmap(tmpBitmap);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static Color UpdateBitmap(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap as Image))
            {
                int half_w = bitmap.Size.Width / 2;
                int half_h = bitmap.Size.Height / 2;
                g.CopyFromScreen(MousePosition.X - half_w, MousePosition.Y - half_h, 0, 0, bitmap.Size);
                return bitmap.GetPixel(half_w, half_h);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoom_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(bitmap as Image, new Rectangle(0, 0, this.zoom.Width + 3, this.zoom.Height + 3));
            if (this.zoomOn.Checked)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                e.Graphics.DrawLine(Pens.Black, this.zoom.Width / 2 + 1, 0, this.zoom.Width / 2 + 1, this.zoom.Height);
                e.Graphics.DrawLine(Pens.Black, 0, this.zoom.Height / 2 + 1, this.zoom.Width, this.zoom.Height / 2 + 1);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void colorSample_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(this.brush, e.ClipRectangle);
        }

        private void zoomOn_CheckedChanged(object sender, EventArgs e)
        {
            this.zoom.Invalidate();
            this.label1.Focus();
        }
    }
}