namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The ICarouselViewController interface defines the methods required to manage touch interactions for a carousel view. 
    /// This interface provides a way to handle touch events such as starting, changing, and ending touch gestures on a carousel.
    /// </summary>
    public interface ICarouselViewController
    {
        void TouchStarted();
        void TouchChanged(double offset);
        void TouchEnded();
    }
}