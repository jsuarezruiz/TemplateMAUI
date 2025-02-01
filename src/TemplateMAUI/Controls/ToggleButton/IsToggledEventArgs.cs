namespace TemplateMAUI.Controls
{
    public class IsToggledEventArgs : EventArgs
    {
        public IsToggledEventArgs(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }
    }
}