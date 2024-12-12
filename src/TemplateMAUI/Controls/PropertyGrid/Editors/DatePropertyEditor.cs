namespace TemplateMAUI.Controls
{
    public class DatePropertyEditor : PropertyEditorBase
    {
        public override View CreateView(PropertyItem propertyItem) => new DatePicker();

        public override BindableProperty GetBindableProperty() => DatePicker.DateProperty;
    }
}