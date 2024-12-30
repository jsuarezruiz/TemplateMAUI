namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The HighlightEffect class implements the IDrawable interface to provide a visual highlighting effect. 
    /// This effect can be used to draw a highlight around or over a specific area in a user interface, allowing for emphasis and visual feedback.
    /// </summary>
    public class HightlightEffect : IDrawable
    {
        public Color HightlightColor { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Antialias = true;
            DrawBackground(canvas, dirtyRect);
            DrawHightlight(canvas, dirtyRect);
        }

        void DrawBackground(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = Colors.Transparent;
            canvas.FillRectangle(dirtyRect);

            canvas.RestoreState();
        }

        void DrawHightlight(ICanvas canvas, RectF dirtyRect)
        {
            if (HightlightColor is not null)
            {
                canvas.SaveState();

                canvas.Alpha = 0.5f;

                canvas.FillColor = HightlightColor;

                canvas.FillRectangle(dirtyRect);

                canvas.RestoreState();
            }
        }
    }
}
