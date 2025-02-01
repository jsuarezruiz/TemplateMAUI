using XmlnsPrefixAttribute = Microsoft.Maui.Controls.XmlnsPrefixAttribute;

// Custom xaml schema
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui/templatemaui", "TemplateMAUI.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/dotnet/2021/maui/templatemaui/datavisualization", "TemplateMAUI.DataVisualization")]
// Recommended prefix
[assembly: XmlnsPrefix("http://schemas.microsoft.com/dotnet/2021/maui/templatemaui", "tmaui")]