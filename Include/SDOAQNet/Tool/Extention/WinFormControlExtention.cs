using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SDOAQNet.Tool
{
    public static class WinFormControlExtention
    {
        public static void SetBounds(this Control ctrl, Rectangle rect)
        {
            ctrl.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rectangle ShrinkRect(this Rectangle rect, int margin)
        {
            return new Rectangle(rect.X + margin, rect.Y + margin, rect.Width - (2 * margin), rect.Height - (2 * margin));
        }

        public static Rectangle ShrinkRect(this Rectangle rect, int margin_X, int margin_Y)
        {
            return new Rectangle(rect.X + margin_X, rect.Y + margin_Y, rect.Width - (2 * margin_X), rect.Height - (2 * margin_Y));
        }

        public static List<Rectangle> DivideRect_Row(this Rectangle rect, int count, int margin = 4, float[] ratios = null)
        {
            if (ratios != null && count < ratios.Length)
            {
                throw new ArgumentException("DivideRect_Row, Row count should be equal to or greater than the ratio count.");
            }

            if (ratios == null || count > ratios.Length)
            {
                var colRatios = new float[count];

                int len = ratios == null ? 0 : ratios.Length;

                if (len > 0)
                {
                    Array.Copy(ratios, colRatios, len);
                }

                for (int i = len; i < count; i++)
                {
                    colRatios[i] = 1;
                }
                ratios = colRatios;
            }

            var totalRatios = 0f;
            foreach (var ratio in ratios)
            {
                totalRatios += ratio;
            }

            var subRects = new List<Rectangle>();
            var currentY = rect.Y;

            for (int i = 0; i < count; i++)
            {
                int modifyHeight = (int)(((rect.Height - (margin * (count - 1))) * (ratios[i] / totalRatios)) + 0.5);
                var subRect = new Rectangle(rect.X, currentY, rect.Width, modifyHeight);
                subRects.Add(subRect);

                currentY += modifyHeight + margin;
            }

            return subRects;
        }

        public static List<Rectangle> DivideRect_Col(this Rectangle rect, int count, int margin = 4, float[] ratios = null)
        {
            if (ratios != null && count < ratios.Length)
            {
                throw new ArgumentException("DivideRect_Col, Col count should be equal to or greater than the ratio count.");
            }

            if (ratios == null || count > ratios.Length)
            {
                var colRatios = new float[count];

                int len = ratios == null ? 0 : ratios.Length;

                if (len > 0)
                {
                    Array.Copy(ratios, colRatios, len);
                }

                for (int i = len; i < count; i++)
                {
                    colRatios[i] = 1;
                }
                ratios = colRatios;
            }

            var totalRatios = 0f;
            foreach (var ratio in ratios)
            {
                totalRatios += ratio;
            }

            var subRects = new List<Rectangle>();
            var currentX = rect.X;

            for (int i = 0; i < count; i++)
            {
                int modifyWidth = (int)(((rect.Width - (margin * (count - 1))) * (ratios[i] / totalRatios)) + 0.5);
                var subRect = new Rectangle(currentX, rect.Y, modifyWidth, rect.Height);
                subRects.Add(subRect);

                currentX += modifyWidth + margin;
            }

            return subRects;
        }

        public static void Invoke(this Control control, Action action)
        {
            if (IsValidContrl(control) == false)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static void BeginInvoke(this Control control, Action action)
        {
            if (IsValidContrl(control) == false)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        private static bool IsValidContrl(Control control)
        {
            if (control == null || control.IsDisposed || control.Disposing)
            {
                return false;
            }
            return true;
        }
    }
}
