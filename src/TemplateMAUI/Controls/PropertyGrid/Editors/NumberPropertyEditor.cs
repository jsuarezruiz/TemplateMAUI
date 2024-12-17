namespace TemplateMAUI.Controls
{
    public class NumberPropertyEditor : PropertyEditorBase
    {
        public NumberPropertyEditor()
        {

        }

        public NumberPropertyEditor(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public double Minimum { get; set; }

        public double Maximum { get; set; }

        public override View CreateView(PropertyItem propertyItem) => new Stepper
        {
            IsEnabled = !propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum
        };
        
        public override BindableProperty GetBindableProperty() => Stepper.ValueProperty;
    }
}