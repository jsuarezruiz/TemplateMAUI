namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The IPinAnimation interface defines the methods required to implement animations for PIN items. 
    /// This interface allows for custom animations to be applied when a PIN item gains or loses focus.
    /// </summary>
    public interface IPinAnimation
    {
        Task OnFocus(PinItem pinItem);
        Task OnUnfocus(PinItem pinItem);
    }
}