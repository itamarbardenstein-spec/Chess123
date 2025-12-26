using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Chess.Models
{
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {

    }
}
