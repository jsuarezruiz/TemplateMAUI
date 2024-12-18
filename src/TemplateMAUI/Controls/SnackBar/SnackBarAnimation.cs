namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The SnackBarAnimation class implements the ISnackBarAnimation interface, providing custom animations for snack bar views. 
    /// This class defines the behavior for opening and closing animations using translation effects.
    /// </summary>
    public class SnackBarAnimation : ISnackBarAnimation
    {
        const uint OpenAnimationDuration = 250;
        const uint CloseAnimationDuration = 150;

        public async Task OnClose(View snackBar)
        {
            var height = snackBar.Height > 0 ? (snackBar.Height + snackBar.Margin.Bottom) : 1000;
            await snackBar.TranslateTo(0, height, CloseAnimationDuration);
        }

        public async Task OnOpen(View snackBar)
        {
            await snackBar.TranslateTo(0, 0, OpenAnimationDuration);
        }
    }
}