using Android.Graphics;
using Microsoft.Maui.Platform;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;

namespace TemplateMAUI.Platforms
{
    public static class BitmapExtensions
    {
        public static async Task<Bitmap> ToImage(this IView view)
        {
            IMauiContext mauiContext = view.Handler.MauiContext;
            var platformView = view.ToPlatform(mauiContext);
            var bitmap = await platformView.ToImage(mauiContext);

            return bitmap;
        }

        public static async Task<Microsoft.Maui.Graphics.Color> ColorAtPoint(this IView view, double x, double y, bool includeAlpha = false)
        {
            IMauiContext mauiContext = view.Handler.MauiContext;
            var platformView = view.ToPlatform(mauiContext);
            var color = await platformView.ColorAtPoint(x, y, mauiContext, includeAlpha);

            return color;
        }

        internal static async Task<Bitmap> ToImage(this AView view, IMauiContext mauiContext)
        {
            if (view.Parent is WrapperView wrapper)
                view = wrapper;

            var bitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888!)!;

            using (var canvas = new Canvas(bitmap))
            {
                view.Draw(canvas);
            }

            return await Task.FromResult(bitmap);
        }

        internal static async Task<Microsoft.Maui.Graphics.Color> ColorAtPoint(this AView view, double x, double y, IMauiContext mauiContext, bool includeAlpha = false)
        {
            var bitmap = await view.ToImage(mauiContext);

            var color = await bitmap.ColorAtPoint(x, y, includeAlpha);

            return color;
        }

        internal static async Task<Microsoft.Maui.Graphics.Color> ColorAtPoint(this Bitmap bitmap, double x, double y, bool includeAlpha = false)
        {
            Microsoft.Maui.Graphics.Color result;

            int pixel = bitmap.GetPixel((int)x, (int)y);

            int red = AColor.GetRedComponent(pixel);
            int blue = AColor.GetBlueComponent(pixel);
            int green = AColor.GetGreenComponent(pixel);

            if (includeAlpha)
            {
                int alpha = AColor.GetAlphaComponent(pixel);
                result = AColor.Argb(alpha, red, green, blue).ToColor();
            }
            else
            {
                result = AColor.Rgb(red, green, blue).ToColor();
            }

            return await Task.FromResult(result);
        }
    }
}
