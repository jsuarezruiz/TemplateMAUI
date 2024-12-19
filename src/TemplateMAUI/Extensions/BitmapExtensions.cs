#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS
namespace TemplateMAUI.Platforms
{
    public static class BitmapExtensions
    {
        public static async Task<Color> ColorAtPoint(this IView view, double x, double y, bool includeAlpha = false)
        {
            // Implement the logic to get the color at the specified point
            // This is a placeholder implementation and should be replaced with actual logic
            return Colors.Transparent;
        }
    }
}
#endif