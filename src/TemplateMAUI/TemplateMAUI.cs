using TemplateMAUI.Themes;

namespace TemplateMAUI
{
    public class TemplateMAUI
    {
        public static void Init()
        {
            var templateMAUIDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(templateMAUIDictionary))
                Application.Current.Resources.Add(templateMAUIDictionary);
        }
    }
}