namespace Blazored.Diagrams.Services.Events;

/// <summary>
/// Describes a typed event.
/// Typed events are equivalent to the event keyword in c# but much more extensible and manageable.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface ITypedEvent<TEvent> where TEvent : IEvent
{
    /// <summary>
    /// Adds the subscription action to the list of handlers.
    /// </summary>
    /// <param name="handler"></param>
    void Subscribe(Action<TEvent> handler);
    
    /// <summary>
    /// Removes the subscription from the list of handlers.
    /// </summary>
    /// <param name="handler"></param>
    void Unsubscribe(Action<TEvent> handler);
    
    /// <summary>
    /// Publishes the event to all those who subscribed to it.
    /// </summary>
    /// <param name="evt"></param>
    void Publish(TEvent evt);
}