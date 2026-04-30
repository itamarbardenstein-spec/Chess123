using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Chess.Models
{
    /// Represents a generic message used to transfer data between system components via Messenger
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {

    }
}