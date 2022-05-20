using System.Threading.Tasks;

namespace TemplateMAUI.Controls
{
    public interface IPinAnimation
    {
        Task OnFocus(PinItem pinItem);
        Task OnUnfocus(PinItem pinItem);
    }
}