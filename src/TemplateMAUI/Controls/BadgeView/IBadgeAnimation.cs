namespace TemplateMAUI.Controls
{
    public interface IBadgeAnimation
    {
        Task OnAppearing(View badgeView);
        Task OnDisappering(View badgeView);
    }
}