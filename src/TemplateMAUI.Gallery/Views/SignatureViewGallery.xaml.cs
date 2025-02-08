namespace TemplateMAUI.Gallery.Views;

public partial class SignatureViewGallery : ContentPage
{
	public SignatureViewGallery()
	{
		InitializeComponent();
	}

    void OnSaveButtonClicked(object sender, EventArgs e)
    {

    }

    void OnClearButtonClicked(object sender, EventArgs e)
    {
        SignatureView.Clear();
    }
}