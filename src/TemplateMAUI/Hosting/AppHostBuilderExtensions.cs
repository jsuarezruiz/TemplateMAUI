using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Handlers;
using TemplateMAUI.Controls;

namespace TemplateMAUI.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder ConfigureTemplateMAUI(this MauiAppBuilder builder)
        {
            builder
                .UseMauiCompatibility()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddTemplateMAUIControlsHandlers();
                });

            return builder;
        }

        public static IMauiHandlersCollection AddTemplateMAUIControlsHandlers(this IMauiHandlersCollection handlersCollection)
        {
            // TODO:
            handlersCollection.AddTransient(typeof(Rate), typeof(LayoutHandler));

            return handlersCollection;
        }
    }
}