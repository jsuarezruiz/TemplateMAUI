
namespace TemplateMAUI.Controls
{
    internal class TimePropertyEditor : PropertyEditorBase
    {
        public override View CreateView(PropertyItem propertyItem) => new TimePicker(); 
        
        public override BindableProperty GetBindableProperty() => TimePicker.TimeProperty;
    }
}