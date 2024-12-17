namespace TemplateMAUI.Controls
{
    public class ReadOnlyPropertyEditor : PropertyEditorBase
    {
        public override View CreateView(PropertyItem propertyItem) => new Entry
        {
            IsEnabled = false
        };

        public override BindableProperty GetBindableProperty() => Entry.TextProperty;
    }
}
