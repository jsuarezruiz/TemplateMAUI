using Microsoft.Graphics.Canvas;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.DirectX;
using WColor = Windows.UI.Color;

namespace TemplateMAUI.Platforms
{
    public static class BitmapExtensions
    {
        public static async Task<CanvasBitmap> ToImage(this IView view)
        {
            IMauiContext mauiContext = view.Handler.MauiContext;
            var platformView = view.ToPlatform(mauiContext);
            var bitmap = await platformView.ToImage(mauiContext);

            return bitmap;
        }

        public static async Task<Color> ColorAtPoint(this IView view, double x, double y, bool includeAlpha = false)
        {
            IMauiContext mauiContext = view.Handler.MauiContext;
            var platformView = view.ToPlatform(mauiContext);
            var color = await platformView.ColorAtPoint(x, y, mauiContext, includeAlpha);

            return color;
        }

        internal static async Task<CanvasBitmap> ToImage(this FrameworkElement view, IMauiContext mauiContext)
        {
            if (view.Parent is Microsoft.UI.Xaml.Controls.Border wrapper)
                view = wrapper;

           var platformWindow = mauiContext.Services.GetRequiredService<Microsoft.UI.Xaml.Window>();

            var device = CanvasDevice.GetSharedDevice();
            using var windowBitmap = await CaptureHelper.RenderAsync(platformWindow, device);
            var bmp = new RenderTargetBitmap();
            await bmp.RenderAsync(view);
            var pixels = await bmp.GetPixelsAsync();
            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;

            return CanvasBitmap.CreateFromBytes(device, pixels, width, height, DirectXPixelFormat.B8G8R8A8UIntNormalized);
        }

        internal static async Task<Color> ColorAtPoint(this FrameworkElement view, double x, double y, IMauiContext mauiContext, bool includeAlpha = false)
        {
            var bitmap = await view.ToImage(mauiContext);

            return bitmap.ColorAtPoint(x, y).ToColor();
        }

        internal static WColor ColorAtPoint(this CanvasBitmap bitmap, double x, double y, bool includeAlpha = false)
        {
            var pixel = bitmap.GetPixelColors((int)x, (int)y, 1, 1)[0];

            return includeAlpha
                ? pixel
                : WColor.FromArgb(255, pixel.R, pixel.G, pixel.B);
        }
    }
}
