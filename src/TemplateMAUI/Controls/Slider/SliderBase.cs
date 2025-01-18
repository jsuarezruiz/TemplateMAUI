namespace TemplateMAUI.Controls
{
    static class SliderBase
    {
        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(ISlider.Minimum), typeof(double), typeof(SliderBase), 0d);
       
        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(ISlider.Maximum), typeof(double), typeof(SliderBase), 10.0d);
   
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(ISlider.Value), typeof(double), typeof(SliderBase), 0d);
    }
}