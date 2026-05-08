using CommunityToolkit.Mvvm.Messaging;

namespace FitGames.app.Services.Interfaces;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}