using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace TemplateMAUI.Platforms
{
    public static class BitmapExtensions
    {
        public static async Task<UIImage> ToImage(this IView view)
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

        internal static Task<UIImage> ToImage(this UIView view, IMauiContext mauiContext)
        {
            if (view.Superview is WrapperView wrapper)
                view = wrapper;

            var imageRect = new CGRect(0, 0, view.Frame.Width, view.Frame.Height);

            if (view.Frame.Width == 0 && view.Frame.Height == 0)
            {
                UIGraphicsImageRenderer renderer = new UIGraphicsImageRenderer(imageRect.Size);

                return Task.FromResult(renderer.CreateImage(c =>
                {
                    view.Layer.RenderInContext(c.CGContext);
                }));
            }

            UIGraphics.BeginImageContext(imageRect.Size);

            var context = UIGraphics.GetCurrentContext();
            view.Layer.RenderInContext(context);
            var image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return Task.FromResult(image);
        }

        internal static async Task<Microsoft.Maui.Graphics.Color> ColorAtPoint(this UIView view, double x, double y, IMauiContext mauiContext, bool includeAlpha = false)
        {
            var bitmap = await view.ToImage(mauiContext);

            return bitmap.ColorAtPoint(x, y).ToColor();
        }

        internal static UIColor ColorAtPoint(this UIImage bitmap, double x, double y)
        {
            var pixel = bitmap.GetPixel(x, y);

            var color = new UIColor(
                pixel[0] / 255.0f,
                pixel[1] / 255.0f,
                pixel[2] / 255.0f,
                pixel[3] / 255.0f);

            return color;
        }

        internal static byte[] GetPixel(this UIImage bitmap, double x, double y)
        {
            var cgImage = bitmap.CGImage!;
            var width = cgImage.Width;
            var height = cgImage.Height;
            var colorSpace = CGColorSpace.CreateDeviceRGB();
            var bitsPerComponent = 8;
            var bytesPerRow = 4 * width;
            var componentCount = 4;

            var dataBytes = new byte[width * height * componentCount];

            using var context = new CGBitmapContext(
                dataBytes,
                width, height,
                bitsPerComponent, bytesPerRow,
                colorSpace,
                CGBitmapFlags.ByteOrder32Big | CGBitmapFlags.PremultipliedLast);

            context.DrawImage(new CGRect(0, 0, width, height), cgImage);

            var pixelLocation = (bytesPerRow * y) + componentCount * x;

            var pixel = new byte[]
            {
                dataBytes[(int)pixelLocation],
                dataBytes[(int)(pixelLocation + 1)],
                dataBytes[(int)(pixelLocation + 2)],
                dataBytes[(int)(pixelLocation + 3)],
            };

            return pixel;
        }

        internal static async Task<Color> ColorAtPoint(this object obj, double x, double y, bool includeAlpha = false)
        {
            if (obj is UIImage bitmap)
            {
                var color = await bitmap.ColorAtPoint(x, y, includeAlpha);

                return color;
            }

            return null;
        }
    }
}
