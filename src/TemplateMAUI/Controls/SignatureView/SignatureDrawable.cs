namespace TemplateMAUI.Controls
{
    public class SignatureDrawable : IDrawable
    {
        public SignatureDrawable()
        {
            SignatureStartPoint = PointF.Zero;
            SignaturePoints = new List<PointF>();
        }

        public Color StrokeColor { get; set; }
        public double StrokeThickness { get; set; }

        internal PointF SignatureStartPoint { get; set; }
        internal List<PointF> SignaturePoints { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawStroke(canvas, dirtyRect);
        }

        void DrawStroke(ICanvas canvas, RectF dirtyRect)
        {
            if (SignaturePoints?.Count > 0)
            {
                PathF path = new PathF();

                path.MoveTo(SignatureStartPoint.X, SignatureStartPoint.Y);
                foreach (var point in SignaturePoints)
                {
                    path.LineTo(point);
                }

                canvas.StrokeColor = StrokeColor;
                canvas.StrokeSize = (float)StrokeThickness;

                canvas.DrawPath(path);
            }
        }
    }
}