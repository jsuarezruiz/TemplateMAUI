namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The PinAnimation class implements the IPinAnimation interface, providing custom animations for PIN items. 
    /// This class defines the behavior for focusing and unfocusing animations using scaling effects on the visual elements.
    /// </summary>
    public class PinAnimation : IPinAnimation
    {
        public async Task OnFocus(PinItem pinItem)
        {
            if (pinItem is VisualElement visualElement)
                await visualElement.ScaleTo(1.1, 150);
        }

        public async Task OnUnfocus(PinItem pinItem)
        {
            if (pinItem is VisualElement visualElement)
                await visualElement.ScaleTo(1.0, 100);
        }
    }
}