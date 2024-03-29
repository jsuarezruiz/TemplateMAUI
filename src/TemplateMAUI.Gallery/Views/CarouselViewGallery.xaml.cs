﻿namespace TemplateMAUI.Gallery.Views
{
    public partial class CarouselViewGallery : TabbedPage
    {
        public CarouselViewGallery()
        {
            InitializeComponent();
        }

        async void OnScrollToZeroClicked(object sender, EventArgs e)
        {
            bool isAnimated = IsAnimatedScrollToCheckBox.IsChecked;
            await TemplateCarouselView.ScrollToAsync(0, isAnimated);
        }

        async void OnScrollToFiveClicked(object sender, EventArgs e)
        {
            bool isAnimated = IsAnimatedScrollToCheckBox.IsChecked;
            await TemplateCarouselView.ScrollToAsync(5, isAnimated);
        }
    }
}