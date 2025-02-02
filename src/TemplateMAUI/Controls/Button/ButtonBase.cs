using System.Windows.Input;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The ButtonBase static class defines the bindable properties that are commonly used in button controls. 
    /// These properties include the content, visual appearance, and behavior of the button, such as the background color, border, text color, font settings, and command properties.
    /// </summary>
    static class ButtonBase
    {
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(IButton.Content), typeof(object), typeof(ButtonBase));

        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(IButton.Background), typeof(Brush), typeof(ButtonBase));

        public static readonly BindableProperty BorderBrushProperty =
            BindableProperty.Create(nameof(IButton.BorderBrush), typeof(Brush), typeof(ButtonBase));

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(IButton.BorderThickness), typeof(double), typeof(ButtonBase), 1.0d);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(IButton.TextColor), typeof(Color), typeof(ButtonBase));

        public static BindableProperty FontSizeProperty =
             BindableProperty.Create(nameof(IButton.FontSize), typeof(double), typeof(ButtonBase), 12.0);

        public static BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(IButton.FontFamily), typeof(string), typeof(ButtonBase), string.Empty);
       
        public static BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(IButton.FontAttributes), typeof(FontAttributes), typeof(ButtonBase), FontAttributes.None);

        public static readonly BindableProperty RippleColorProperty =
            BindableProperty.Create(nameof(IButton.RippleColor), typeof(Color), typeof(FeedbackView), Colors.Gray);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(IButton.Command), typeof(ICommand), typeof(ButtonBase)); 
        
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(IButton.CommandParameter), typeof(object), typeof(ButtonBase));
    }
}