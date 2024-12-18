namespace TemplateMAUI.Controls
{
    /// <summary>
    /// Represents the types of chat bubbles in a chat interface. 
    /// It is used to differentiate between the messages sent by the user and the messages received from others.
    /// </summary>    
    public enum ChatBubbleType
    {
        Sender, // Indicates that the chat bubble is for messages sent by the user.
        Receiver // Indicates that the chat bubble is for messages received from others.
    }
}