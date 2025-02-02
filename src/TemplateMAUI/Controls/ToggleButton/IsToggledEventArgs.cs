namespace TemplateMAUI.Controls
{
    /// <summary>
    /// Provides data for the toggled event.
    /// </summary>
    public class IsToggledEventArgs : EventArgs
    {
        public IsToggledEventArgs(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }
    }
}