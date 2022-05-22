using System.Diagnostics;
using TemplateMAUI.Controls;

namespace TemplateMAUI.Gallery.Views
{
    public partial class TreeViewGallery : TabbedPage
    {
        public TreeViewGallery()
        {
            InitializeComponent();
        }

        void OnTreeViewItemTapped(object sender, NodeTappedEventArgs e)
        {
            Debug.WriteLine($"Item Tapped: {e.Node.Text}");
        }

        void OnTreeViewItemDoubleTapped(object sender, NodeDoubleTappedEventArgs e)
        {
            Debug.WriteLine($"Item DoubleTapped: {e.Node.Text}");
        }
    }
}