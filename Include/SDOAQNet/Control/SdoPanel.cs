using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDOAQNet.Component
{
    public partial class SdoPanel : Panel
    {
        [Category(""), Description("Border Color")]
        private Color _borderColor = Color.Transparent;
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category(""), Description("Border Width")]
        private int _borderWidth = 0;
        public int BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        public SdoPanel()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.Paint += Control_Paint;
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            if (BorderStyle == BorderStyle.None)
            {
                ControlPaint.DrawBorder(e.Graphics, new Rectangle(0, 0, this.Width, this.Height)
                                    , BorderColor, BorderWidth, ButtonBorderStyle.Solid
                                    , BorderColor, BorderWidth, ButtonBorderStyle.Solid
                                    , BorderColor, BorderWidth, ButtonBorderStyle.Solid
                                    , BorderColor, BorderWidth, ButtonBorderStyle.Solid);
            }
        }
    }
}
