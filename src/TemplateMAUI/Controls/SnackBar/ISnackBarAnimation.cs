namespace TemplateMAUI.Controls
{
    public interface ISnackBarAnimation
    {
        Task OnOpen(View snackBar);
        Task OnClose(View snackBar);
    }
}