using System;

namespace TemplateMAUI.Controls
{
    public class PinCompletedEventArgs : EventArgs
    {
        public PinCompletedEventArgs(string password)
        {
            Password = password;
        }

        public string Password { get; set; }
    }
}