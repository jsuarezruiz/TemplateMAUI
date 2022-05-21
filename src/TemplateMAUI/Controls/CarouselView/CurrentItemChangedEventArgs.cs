namespace TemplateMAUI.Controls
{
    public class CurrentItemChangedEventArgs : EventArgs
    {
        public CurrentItemChangedEventArgs(object currentItem)
        {
            CurrentItem = currentItem;
        }

        public object CurrentItem { get; set; }
    }
}