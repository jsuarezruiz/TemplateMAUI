namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The IExpanderAnimation interface defines the methods required to implement animations for expander views. 
    /// This interface allows for custom animations to be applied when an expander view expands and collapses.
    /// </summary>
    public interface IExpanderAnimation
    {
        Task OnExpand(View expanderView);
        Task OnCollapse(View expanderView);
    }
}