namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The ExpanderAnimation class implements the IExpanderAnimation interface, providing custom animations for expander views. 
    /// This class defines the behavior for expanding and collapsing animations using scaling effects on the Y-axis.
    /// </summary>
    public class ExpanderAnimation : IExpanderAnimation
    {
        protected uint AnimationLength { get; } = 150;

        public async Task OnCollapse(View view)
        {
            await view.ScaleYTo(0, AnimationLength, Easing.CubicOut);
        }

        public async Task OnExpand(View view)
        {
            await view.ScaleYTo(1, AnimationLength, Easing.CubicIn);
        }
    }
}