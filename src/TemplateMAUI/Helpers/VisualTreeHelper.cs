namespace TemplateMAUI.Helpers
{
    /// <summary>
    /// The VisualTreeHelper class is a utility class that provides static methods for visual tree operations, such as traversing and querying elements within a visual tree. 
    /// It is commonly used for tasks like finding parent or child elements, hit testing, and retrieving visual properties.
    /// </summary>
    // TODO: Complete adding methods to get childrens, etc.
    public static class VisualTreeHelper
    {
        public static T GetParent<T>(this Element element) where T : Element
        {
            if (element is T)
                return element as T;
            else
            {
                if (element.Parent != null)
                    return element.Parent.GetParent<T>();

                return default;
            }
        }
    }
}