namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The RippleEffect class implements the IDrawable interface to provide a ripple effect that can be used as visual feedback for touch interactions. 
    /// The ripple effect typically originates from a touch point and spreads outwards, creating a visually appealing animation.
    /// </summary>
    public class RippleEffect : IDrawable
    {
        public Paint Background { get; set; }
        public Paint RippleColor { get; set; }
        public Point TouchPoint { get; set; }

        internal float Diameter { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Antialias = true;
            DrawBackground(canvas, dirtyRect);
            DrawRippleEffect(canvas, dirtyRect);
        }

        void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = Colors.Transparent;
            canvas.FillRectangle(dirtyRect);

            canvas.RestoreState();
        }

        void DrawRippleEffect(ICanvas canvas, RectF dirtyRect)
        {    
            if (RippleColor is not null && dirtyRect.Contains(TouchPoint))
            {
                canvas.SaveState();

                var x = dirtyRect.X;
                var y = dirtyRect.Y;
                var width = dirtyRect.Width;
                var height = dirtyRect.Height;

                var clippingPath = new PathF();
                clippingPath.AppendRectangle(x, y, width, height);

                canvas.ClipPath(clippingPath);

                canvas.FillColor = RippleColor.ToColor().WithAlpha(0.75f);

                canvas.Alpha = 0.25f;

                canvas.FillCircle((float)TouchPoint.X, (float)TouchPoint.Y, Diameter);

                canvas.RestoreState();
            }
        }
    }
}