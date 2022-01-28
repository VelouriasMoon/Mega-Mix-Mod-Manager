using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Mega_Mix_Mod_Manager.IO
{
    [DesignerCategory("Code")]
    public class Spinner : UserControl
    {
        private int next = 0;
        private Timer timer = new Timer();

        public Spinner()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(100, 100);
            BackColor = Color.Transparent;

            timer.Tick += (s, e) => Invalidate();
            if (!DesignMode)
            {
                timer.Enabled = true;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public int NodeCount { get; set; } = 8;

        [Browsable(true)]
        [Category("Appearance")]
        public int NodeRadius { get; set; } = 4;

        [Browsable(true)]
        [Category("Appearance")]
        public float NodeResizeRatio { get; set; } = 1.0f;

        [Browsable(true)]
        [Category("Appearance")]
        public Color NodeFillColor { get; set; } = Color.Black;

        [Browsable(true)]
        [Category("Appearance")]
        public Color NodeBorderColor { get; set; } = Color.White;

        [Browsable(true)]
        [Category("Appearance")]
        public int NodeBorderSize { get; set; } = 2;

        [Browsable(true)]
        [Category("Appearance")]
        public int SpinnerRadius { get; set; } = 100;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (null != Parent && (BackColor.A != 255 || BackColor == Color.Transparent))
            {
                using (Bitmap bmp = new Bitmap(Parent.Width, Parent.Height))
                {
                    foreach (Control control in GetIntersectingControls(Parent))
                    {
                        control.DrawToBitmap(bmp, control.Bounds);
                    }

                    e.Graphics.DrawImage(bmp, -Left, -Top);

                    if (BackColor != Color.Transparent)
                    {
                        using (Brush brush = new SolidBrush(BackColor))
                        {
                            e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
                        }
                    }
                }
            }

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            PointF center = new PointF(Width / 2, Height / 2);
            int bigRadius = (int)(SpinnerRadius / 2 - NodeRadius - (NodeCount - 1) * NodeResizeRatio);
            float unitAngle = 360 / NodeCount;

            if (!DesignMode)
            {
                next++;
            }

            next = next >= NodeCount ? 0 : next;

            for (int i = next, a = 0; i < next + NodeCount; i++, a++)
            {
                int factor = i % NodeCount;
                float c1X = center.X + (float)(bigRadius * Math.Cos(unitAngle * factor * Math.PI / 180));
                float c1Y = center.Y + (float)(bigRadius * Math.Sin(unitAngle * factor * Math.PI / 180));
                int currRad = (int)(NodeRadius + a * NodeResizeRatio);
                PointF c1 = new PointF(c1X - currRad, c1Y - currRad);

                using (Brush brush = new SolidBrush(NodeFillColor))
                {
                    e.Graphics.FillEllipse(brush, c1.X, c1.Y, 2 * currRad, 2 * currRad);
                }

                using (Pen pen = new Pen(Color.White, NodeBorderSize))
                {
                    e.Graphics.DrawEllipse(pen, c1.X, c1.Y, 2 * currRad, 2 * currRad);
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            timer.Enabled = Visible;
            base.OnVisibleChanged(e);
        }

        private IOrderedEnumerable<Control> GetIntersectingControls(Control parent)
        {
            return parent.Controls.Cast<Control>()
                .Where(c => parent.Controls.GetChildIndex(c) > parent.Controls.GetChildIndex(this))
                .Where(c => c.Bounds.IntersectsWith(Bounds))
                .OrderByDescending(c => parent.Controls.GetChildIndex(c));
        }
    }
}

