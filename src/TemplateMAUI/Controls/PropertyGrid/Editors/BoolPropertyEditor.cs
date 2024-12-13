namespace TemplateMAUI.Controls
{
    public class BoolPropertyEditor : PropertyEditorBase
    {
        public override View CreateView(PropertyItem propertyItem) => new Switch();
        
        public override BindableProperty GetBindableProperty() => Switch.IsToggledProperty;
    }
}
