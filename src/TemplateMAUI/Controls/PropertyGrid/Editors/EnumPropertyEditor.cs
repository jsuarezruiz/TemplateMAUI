namespace TemplateMAUI.Controls
{
    public class EnumPropertyEditor : PropertyEditorBase
    {
        public override View CreateView(PropertyItem propertyItem) => new Picker()
        {
            IsEnabled = !propertyItem.IsReadOnly,
            ItemsSource = Enum.GetValues(propertyItem.PropertyType)
        };

        public override BindableProperty GetBindableProperty() => Picker.SelectedItemProperty;
    }
}
