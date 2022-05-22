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
            handlersCollection.AddTransient(typeof(AvatarView), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(BadgeView), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Controls.CarouselView), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(ChatBubble), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(CircleProgressBar), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(ComparerView), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Divider), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(GridSplitter), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Marquee), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(PinBox), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Controls.ProgressBar), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Rate), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(SegmentedControl), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Shield), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Controls.Slider), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(SnackBar), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(Tag), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(ToggleSwitch), typeof(LayoutHandler));
            handlersCollection.AddTransient(typeof(TreeView), typeof(LayoutHandler));

            return handlersCollection;
        }
    }
}