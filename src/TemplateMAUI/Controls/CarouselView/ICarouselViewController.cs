namespace TemplateMAUI.Controls
{
    public interface ICarouselViewController
    {
        void TouchStarted();
        void TouchChanged(double offset);
        void TouchEnded();
    }
}