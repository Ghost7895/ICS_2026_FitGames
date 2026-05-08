using CommunityToolkit.Mvvm.Messaging;
using FitGames.app.Services.Interfaces;

namespace FitGames.app.Services;

public class MessengerService : IMessengerService
{
    public IMessenger Messenger { get; }

    public MessengerService(IMessenger messenger)
    {
        Messenger = messenger;
    }

    public void Send<TMessage>(TMessage message) where TMessage : class
    {
        Messenger.Send(message);
    }
}