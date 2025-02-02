namespace TemplateMAUI.Gallery.Views;

public partial class ButtonGallery : TabbedPage
{
	public ButtonGallery()
	{
		InitializeComponent();
	}

    void OnButtonClicked(object sender, EventArgs e)
    {
		DisplayAlert("Button Clicked", "Hello from the Button!", "OK");
    }
}