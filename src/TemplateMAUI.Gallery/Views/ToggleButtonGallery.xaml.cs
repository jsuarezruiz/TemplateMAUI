using TemplateMAUI.Controls;

namespace TemplateMAUI.Gallery.Views;

public partial class ToggleButtonGallery : TabbedPage
{
	public ToggleButtonGallery()
	{
		InitializeComponent();
	}

    void OnIsToggledChanged(object sender, IsToggledEventArgs e)
    {
		DisplayAlert("IsToggledChanged", $"IsToggled: {e.Value}", "Ok");
    }
}