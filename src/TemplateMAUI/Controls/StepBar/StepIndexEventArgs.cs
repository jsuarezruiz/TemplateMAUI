namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The StepIndexEventArgs class provides data for the event that occurs when the step index changes in a step-based control.
    /// </summary>
    public class StepIndexEventArgs : EventArgs
    {
        public StepIndexEventArgs(int stepIndex)
        {
            StepIndex = stepIndex;
        }

        public int StepIndex { get; set; }
    }
}