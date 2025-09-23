using Blazored.Diagrams.Events;

namespace Blazored.Diagrams.Services.Events;

/// <summary>
/// Describes an event aggregator.
/// </summary>
public interface IEventAggregator : IDisposable
{
    /// <summary>
    /// Subscribes to an event.
    /// </summary>
    /// <param name="handler"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    IDisposable SubscribeTo<TEvent>(Action<TEvent> handler)
        where TEvent : IEvent;

    /// <summary>
    /// Subscribes to an event, but is only triggered when a specific condition is met.
    /// </summary>
    /// <param name="predicate">Condition that filters the events</param>
    /// <param name="handler">Handler of the event</param>
    /// <typeparam name="TEvent">Event type.</typeparam>
    /// <returns></returns>
    IDisposable SubscribeWhere<TEvent>(Func<TEvent, bool> predicate, Action<TEvent> handler)
        where TEvent : IEvent;

    /// <summary>
    /// Publishes an event.
    /// </summary>
    /// <param name="event">Event</param>
    /// <typeparam name="TEvent">Event Type</typeparam>
    void Publish<TEvent>(TEvent @event)
        where TEvent : IEvent;

    /// <summary>
    /// Clears all subscriptions.
    /// </summary>
    void Clear();
}