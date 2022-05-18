using TemplateMAUI.Themes;

namespace TemplateMAUI.Gallery
{
    public class TemplateMAUI
    {
        // TODO: 
        public static void Init()
        {
            var templateUIDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(templateUIDictionary))
                Application.Current.Resources.Add(templateUIDictionary);
        }
    }
}