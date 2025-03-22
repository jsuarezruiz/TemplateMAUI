namespace TemplateMAUI.Gallery.Views;

public partial class SignatureViewGallery : ContentPage
{
	public SignatureViewGallery()
	{
		InitializeComponent();
	}

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        await SignatureView.SaveAsync("mySignature");
    }

    void OnClearButtonClicked(object sender, EventArgs e)
    {
        SignatureView.Clear();
    }
}