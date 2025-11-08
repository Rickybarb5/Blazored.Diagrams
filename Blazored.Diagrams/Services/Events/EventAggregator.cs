using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Services.Events;

/// <inheritdoc />
public partial class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, List<Subscription>> _subscriptions = [];
    private IDiagramService _diagramService;
    private long _subscriptionCount;

    /// <summary>
    /// Instantiates an new <see cref="EventAggregator"/>.
    /// </summary>
    /// <param name="diagramService"></param>
    public EventAggregator(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        InitializeEventPropagation();
    }

    /// <inheritdoc />
    public IDisposable SubscribeTo<TEvent>(Action<TEvent> handler)
        where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        EnsureSubscriptionsList(eventType);

        var subscription = new Subscription(GetNextId(), handler);
        _subscriptions[eventType].Add(subscription);

        return new EventSubscription(() => RemoveSubscription(eventType, subscription.Id));
    }

    /// <inheritdoc />
    public IDisposable SubscribeWhere<TEvent>(Func<TEvent, bool> predicate, Action<TEvent> handler)
        where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        EnsureSubscriptionsList(eventType);

        var subscription = new Subscription(GetNextId(), handler, PredicateWrapper);
        _subscriptions[eventType].Add(subscription);

        return new EventSubscription(() => RemoveSubscription(eventType, subscription.Id));

        bool PredicateWrapper(object e) => predicate((TEvent)e);
    }

    /// <inheritdoc />
    public void Publish<TEvent>(TEvent @event)
        where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        PublishToSubscriptions(@event, eventType);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeAutoPropagation();
        _subscriptions.Clear();
    }

    private void PublishToSubscriptions<TEvent>(TEvent eventData, Type eventType)
        where TEvent : notnull
    {
        // 1. Get the list of types/interfaces to check for subscriptions
        var typesToCheck = new List<Type> { eventType };
        // Add all interfaces that the event implements (IEvent, IModelEvent<TModel>, etc.)
        typesToCheck.AddRange(eventType.GetInterfaces());

        // Use Distinct() to handle cases where an interface might be listed multiple times
        foreach (var t in typesToCheck.Distinct())
        {
            if (!_subscriptions.TryGetValue(t, out var subs)) continue;

            // Create a copy to avoid collection modified exceptions
            var subscriptionsCopy = subs.ToList();

            foreach (var sub in subscriptionsCopy)
            {
                if (sub.Predicate == null || sub.Predicate(eventData))
                {
                    // If 't' is the exact type (TEvent), we can use the fast direct cast.
                    // If 't' is a base interface (like IEvent), we must use DynamicInvoke 
                    // because we cannot cast Action<IEvent> to Action<TEvent>.
                    if (t == eventType)
                    {
                        ((Action<TEvent>)sub.Handler)(eventData);
                    }
                    else
                    {
                        // This covers subscriptions to base types/interfaces like IEvent
                        // 'eventData' is guaranteed to be assignable to the type 't'.
                        sub.Handler.DynamicInvoke(eventData);
                    }
                }
            }
        }
    }

    private long GetNextId() => Interlocked.Increment(ref _subscriptionCount);

    private void EnsureSubscriptionsList(Type eventType)
    {
        if (!_subscriptions.ContainsKey(eventType))
        {
            _subscriptions[eventType] = [];
        }
    }

    private void RemoveSubscription(Type eventType, long subscriptionId)
    {
        if (!_subscriptions.TryGetValue(eventType, out var subs)) return;

        var subscription = subs.FirstOrDefault(s => s.Id == subscriptionId);
        if (subscription != null)
        {
            subs.Remove(subscription);
            if (subs.Count == 0)
            {
                _subscriptions.Remove(eventType);
            }
        }
    }

    // Resets the aut event propagation.
    internal void RewireSubscriptions()
    {
        DisposeAutoPropagation();
        InitializeEventPropagation();
    }

    private void DisposeAutoPropagation()
    {
        foreach (var source in _typedEventSubscriptions.Keys.ToList())
        {
            StopAutoPropagation(source);
        }

        _typedEventSubscriptions.Clear();
        _autoSubscriptions.DisposeAll();
    }

    private class Subscription
    {
        public Subscription(long id, Delegate handler, Func<object, bool>? predicate = null, object? model = null)
        {
            Id = id;
            Handler = handler;
            Predicate = predicate;
            Model = model;
        }

        public long Id { get; }
        public Delegate Handler { get; }
        public Func<object, bool>? Predicate { get; }
        public object? Model { get; }
    }
}