using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SDOAQNet.Tool;

namespace SDOAQNet.Component
{
    public partial class SdoLabel : Label
    {
        [Category(""), Description("Border Color")]
        private Color _borderColor = Color.Transparent;
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor == value) return;

                _borderColor = value;
                Invalidate();
            }
        }

        [Category(""), Description("Border Width")]
        private int _borderWidth = 0;
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                if (_borderWidth == value) return;

                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        private const int AUTO_FONT_SIZE_MAX = 1000;
        private const int AUTO_FONT_SIZE_MIN = 8;
        private const int AUTO_FONT_SIZE_MARGIN = 5;
        public SdoLabel()
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
