namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The IBadgeAnimation interface defines the methods required to implement animations for badge views. 
    /// This interface allows for custom animations to be applied when a badge appears and disappears.
    /// </summary>
    public interface IBadgeAnimation
    {
        Task OnAppearing(View badgeView);
        Task OnDisappering(View badgeView);
    }
}