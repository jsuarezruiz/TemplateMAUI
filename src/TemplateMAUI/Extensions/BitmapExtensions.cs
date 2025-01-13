#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS
namespace TemplateMAUI.Platforms
{
    public static class BitmapExtensions
    {
        public static Task<object> ToImage(this IView view)
        {
            throw new NotImplementedException();
        }

        public static Task<Color> ColorAtPoint(this IView view, double x, double y, bool includeAlpha = false)
        {
            // Implement the logic to get the color at the specified point
            // This is a placeholder implementation and should be replaced with actual logic
            return Task.FromResult(Colors.Transparent);
        }

        public static Task<Color> ColorAtPoint(this object view, double x, double y, bool includeAlpha = false)
        {
            return null;
        }
    }
}
#endif