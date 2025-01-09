namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The ISnackBarAnimation interface defines the methods required to implement animations for snack bars. 
    /// This interface allows for custom animations to be applied when a snack bar opens and closes.
    /// </summary>
    public interface ISnackBarAnimation
    {
        Task OnOpen(View snackBar);
        Task OnClose(View snackBar);
    }
}