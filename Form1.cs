using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ColorLoupe
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

        private const int ZoomPanelSize = 110;
        private const int ZoomScale = 10;
        private const int ZoomCaptureSize = ZoomPanelSize / ZoomScale + 1; // 11 — odd for clean center pixel

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
        private IntPtr hMouseHook;

        /// <summary>
        ///
        /// </summary>
        private IntPtr hKeyboardHook;

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
        private Color originalColor = Color.White;

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
            this.copyHexButton.Text = "";
            this.copyRgbButton.Text = "";
            this.copyHslButton.Text = "";
            this.brush = new SolidBrush(Color.White);
            this.zoom.Size = new Size(ZoomPanelSize, ZoomPanelSize);
            this.colorSample.Size = new Size(ZoomPanelSize, ZoomPanelSize);
            this.bitmap = new Bitmap(ZoomCaptureSize, ZoomCaptureSize);
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
        private IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (this.capturing)
            {
                var hookStruct = (NativeMethods.MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.MOUSEHOOKSTRUCT));
                Color color = this.UpdateBitmap(hookStruct.pt.x, hookStruct.pt.y);
                this.originalColor = color;
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
        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((int)wParam == NativeMethods.WM_KEYDOWN)
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
                    this.revertColorButton.Enabled = !this.capturing;
                    this.pressSpaceMessageBox.Text = this.capturing ? freezeText : wanderText;
                }
            }
            else if ((int)wParam == NativeMethods.WM_KEYUP)
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
            this.hexValue.Text = string.Format("#{0:X02}{1:X02}{2:X02}", color.R, color.G, color.B);
            this.rgbValue.Text = string.Format("{0,3},{1,3},{2,3}", color.R, color.G, color.B);
            this.hslValue.Text = string.Format("{0}°, {1}%, {2}%",
                (int)color.GetHue(),
                (int)Math.Round(color.GetSaturation() * 100),
                (int)Math.Round(color.GetBrightness() * 100));
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
        private Color UpdateBitmap(int x, int y)
        {
            if (this.zoomOn.Checked)
            {
                return UpdateBitmap(this.bitmap, x, y);
            }
            else
            {
                using(Bitmap tmpBitmap = new Bitmap(this.bitmap.Size.Width, this.bitmap.Size.Height))
                {
                    return UpdateBitmap(tmpBitmap, x, y);
                }
            }
        }

        private static Color UpdateBitmap(Bitmap bitmap, int x, int y)
        {
            using (Graphics g = Graphics.FromImage(bitmap as Image))
            {
                int half_w = bitmap.Size.Width / 2;
                int half_h = bitmap.Size.Height / 2;
                g.CopyFromScreen(x - half_w, y - half_h, 0, 0, bitmap.Size);
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
            const float panelCx = ZoomPanelSize / 2.0f;
            const float panelCy = ZoomPanelSize / 2.0f;
            const float scale = ZoomScale;

            // image fills panel exactly: ZoomCaptureSize * ZoomScale = ZoomPanelSize
            // cursor pixel (bitmap center) lands at panel center
            float imgX = panelCx - (ZoomCaptureSize / 2.0f) * scale;
            float imgY = panelCy - (ZoomCaptureSize / 2.0f) * scale;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(bitmap as Image, new RectangleF(imgX, imgY, this.bitmap.Width * scale, this.bitmap.Height * scale));

            Color center = this.bitmap.GetPixel(this.bitmap.Width / 2, this.bitmap.Height / 2);
            Pen crosshairPen = center.GetBrightness() > 0.5f ? Pens.Black : Pens.White;

            float cx = panelCx - scale / 2;
            float cy = panelCy - scale / 2;

            // horizontal and vertical lines with gap around center square
            e.Graphics.DrawLine(crosshairPen, 0, panelCy, cx - 1, panelCy);
            e.Graphics.DrawLine(crosshairPen, cx + scale, panelCy, ZoomPanelSize, panelCy);
            e.Graphics.DrawLine(crosshairPen, panelCx, 0, panelCx, cy - 1);
            e.Graphics.DrawLine(crosshairPen, panelCx, cy + scale, panelCx, ZoomPanelSize);

            // square outline around center pixel
            e.Graphics.DrawRectangle(crosshairPen, cx, cy, scale - 1, scale - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void colorSample_Paint(object sender, PaintEventArgs e)
        {
            var ctrl = (Control)sender;
            int half = ctrl.Width / 2;
            using (var ob = new SolidBrush(this.originalColor))
                e.Graphics.FillRectangle(ob, 0, 0, half, ctrl.Height);
            e.Graphics.FillRectangle(this.brush, half, 0, ctrl.Width - half, ctrl.Height);
        }

        private void zoomOn_CheckedChanged(object sender, EventArgs e)
        {
            this.zoom.Invalidate();
            this.label1.Focus();
        }

        private void revertColorButton_Click(object sender, EventArgs e)
        {
            this.SetColor(this.originalColor);
        }

        private void copyHexButton_Click(object sender, EventArgs e)
        {
            CopyToClipboard(this.hexValue, this.hexValue.Text);
        }

        private void copyRgbButton_Click(object sender, EventArgs e)
        {
            CopyToClipboard(this.rgbValue, this.rgbValue.Text.Trim());
        }

        private void copyHslButton_Click(object sender, EventArgs e)
        {
            CopyToClipboard(this.hslValue, this.hslValue.Text.Trim());
        }

        private async void CopyToClipboard(TextBox box, string value)
        {
            Clipboard.SetText(value);
            string original = box.Text;
            box.Text = "Copied!";
            await Task.Delay(800);
            box.Text = original;
        }
    }
}