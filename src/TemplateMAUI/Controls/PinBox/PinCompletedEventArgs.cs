namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The PinCompletedEventArgs class provides data for the event that occurs when a PIN entry is completed.
    /// </summary>
    public class PinCompletedEventArgs : EventArgs
    {
        public PinCompletedEventArgs(string password)
        {
            Password = password;
        }

        public string Password { get; set; }
    }
}