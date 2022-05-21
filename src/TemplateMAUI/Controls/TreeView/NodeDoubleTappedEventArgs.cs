namespace TemplateMAUI.Controls
{
    public class NodeDoubleTappedEventArgs : EventArgs
    {
        public NodeDoubleTappedEventArgs(TreeViewNode treeViewNode)
        {
            Node = treeViewNode;
        }

        public TreeViewNode Node { get; set; }
    }
}