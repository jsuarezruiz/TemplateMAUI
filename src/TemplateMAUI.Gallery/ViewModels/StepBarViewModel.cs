using System.Windows.Input;

namespace TemplateMAUI.Gallery.ViewModels
{
    public class StepBarViewModel : BindableObject
    {
        int _stepIndex;

        public int StepIndex
        {
            get { return _stepIndex; }
            set
            {
                _stepIndex = value;
                OnPropertyChanged();
            }
        }

        public ICommand PreviousCommand => new Command(Previous);
        public ICommand NextCommand => new Command(Next);

        void Previous()
        {
            if (StepIndex == 0)
                return;

            StepIndex--;
        }

        void Next()
        {
            StepIndex++;
        }
    }
}
