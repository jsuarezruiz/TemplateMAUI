namespace TemplateMAUI.Controls
{
    public class StringPropertyEditor : PropertyEditorBase
    {
        public override View CreateView(PropertyItem propertyItem) => new Entry();

        public override BindableProperty GetBindableProperty() => Entry.TextProperty;
    }
}