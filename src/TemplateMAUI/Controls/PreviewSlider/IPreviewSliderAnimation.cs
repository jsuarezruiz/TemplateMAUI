namespace TemplateMAUI.Controls
{
    /// <summary>
    /// Interface for handling Slider animations when a preview is appearing or disappearing.
    /// </summary>
    public interface IPreviewSliderAnimation
    {
        Task OnAppearing(View preview);
        Task OnDisappering(View preview);
    }
}