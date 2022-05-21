using System.Threading.Tasks;

namespace TemplateMAUI.Controls
{
    public interface ICarouselItemTransition
    {
        Task OnSelectionChanging(SelectionChangingArgs args);
        Task OnSelectionChanged(SelectionChangedArgs args);
    }
}