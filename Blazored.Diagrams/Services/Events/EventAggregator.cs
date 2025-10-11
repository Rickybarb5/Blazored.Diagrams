using System.Reflection;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Services.Events;

/// <inheritdoc />
public partial class EventAggregator : IEventAggregator
{
    private IDiagramService _diagramService;
    private readonly Dictionary<Type, List<Subscription>> _subscriptions = [];
    private long _subscriptionCount;

    public EventAggregator(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        InitializeEventPropagation();
    }
    private class Subscription
    {
        public long Id { get; }
        public Delegate Handler { get; }
        public Func<object, bool>? Predicate { get; }
        public object? Model { get; }

        public Subscription(long id, Delegate handler, Func<object, bool>? predicate = null, object? model = null)
        {
            Id = id;
            Handler = handler;
            Predicate = predicate;
            Model = model;
        }
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

    private void PublishToSubscriptions<TEvent>(TEvent eventData, Type eventType)
        where TEvent : notnull
    {
        if (!_subscriptions.TryGetValue(eventType, out var subs)) return;

        // Create a copy to avoid collection modified exceptions
        var subscriptionsCopy = subs.ToList();
        
        foreach (var sub in subscriptionsCopy)
        {
            if (sub.Predicate == null || sub.Predicate(eventData))
            {
                ((Action<TEvent>)sub.Handler)(eventData);
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

    /// <inheritdoc />
    public void Dispose()
    {
        // Stop all auto-propagation
        foreach (var source in _typedEventSubscriptions.Keys.ToList())
        {
            StopAutoPropagation(source);
        }
        
        _typedEventSubscriptions.Clear();
        _subscriptions.Clear();
        _autoSubscriptions.DisposeAll();
    }
}