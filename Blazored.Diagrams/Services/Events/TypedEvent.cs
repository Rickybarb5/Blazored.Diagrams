namespace Blazored.Diagrams.Services.Events;

/// <inheritdoc />
public class TypedEvent<TEvent> : ITypedEvent<TEvent> where TEvent : IEvent
{
    private event Action<TEvent>? _handlers;

    /// <inheritdoc />
    public void Subscribe(Action<TEvent> handler) => _handlers += handler;

    /// <inheritdoc />
    public void Unsubscribe(Action<TEvent> handler) => _handlers -= handler;

    /// <inheritdoc />
    public void Publish(TEvent evt) => _handlers?.Invoke(evt);
}
