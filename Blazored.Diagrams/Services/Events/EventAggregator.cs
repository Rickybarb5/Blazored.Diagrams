using System.Reflection;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Services.Events;

/// <inheritdoc />
public class EventAggregator : IEventAggregator
{
    private readonly List<IDisposable> _autoSubscriptions = [];
    private readonly Dictionary<Type, List<Subscription>> _subscriptions = [];
    /// <summary>
    /// Automatically registered subscriptions
    /// </summary>
    private readonly Dictionary<object, List<object>> _typedEventSubscriptions = [];
    private long _subscriptionCount;

    public EventAggregator(IDiagramService  diagramService)
    {
        InitializeEventPropagation(diagramService);
    }
    private void InitializeEventPropagation(IDiagramService diagramService)
    {
        var Diagram = diagramService.Diagram;
        // Auto-propagate events from the diagram
        _autoSubscriptions.Add(Propagate(Diagram));
        
        // Auto-propagate from all existing models
        _autoSubscriptions.Add(PropagateMany(Diagram.Layers));
        _autoSubscriptions.Add(PropagateMany(Diagram.AllNodes));
        _autoSubscriptions.Add(PropagateMany(Diagram.AllGroups));
        _autoSubscriptions.Add(PropagateMany(Diagram.AllPorts));
        _autoSubscriptions.Add(PropagateMany(Diagram.AllLinks));
        
        // Subscribe to add/remove events to handle new models
        //Also subscribe to their children, they may have already been set before being added to the diagram.
        _autoSubscriptions.Add(SubscribeTo<NodeAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            _autoSubscriptions.Add(PropagateMany(e.Model.Ports));
        }));
        _autoSubscriptions.Add(SubscribeTo<GroupAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            _autoSubscriptions.Add(PropagateMany(e.Model.Nodes));
            _autoSubscriptions.Add(PropagateMany(e.Model.Groups));
            _autoSubscriptions.Add(PropagateMany(e.Model.Ports));
        }));
        _autoSubscriptions.Add(SubscribeTo<PortAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            _autoSubscriptions.Add(PropagateMany(e.Model.OutgoingLinks));
        }));
        _autoSubscriptions.Add(SubscribeTo<LinkAddedEvent>(e => 
            _autoSubscriptions.Add(Propagate(e.Model))));
        _autoSubscriptions.Add(SubscribeTo<LayerAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            _autoSubscriptions.Add(PropagateMany(e.Model.AllNodes));
            _autoSubscriptions.Add(PropagateMany(e.Model.AllGroups));
        }));
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

    /// <summary>
    /// Automatically subscribes to all ITypedEvent fields on an object and propagates them through this aggregator
    /// </summary>
    /// <param name="source">The object containing ITypedEvent fields</param>
    /// <returns>A disposable that will unsubscribe when disposed</returns>
    public IDisposable Propagate(object source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (_typedEventSubscriptions.ContainsKey(source))
            return new EventSubscription(() => { }); // Already subscribed

        var handlers = SubscribeToTypedEvents(source);
        
        if (!handlers.Any())
            return new EventSubscription(() => { }); // No events found

        _typedEventSubscriptions[source] = handlers;
        
        return new EventSubscription(() => StopAutoPropagation(source));
    }

    /// <summary>
    /// Automatically subscribes to multiple objects
    /// </summary>
     public IDisposable PropagateMany(IEnumerable<object> sources)
    {
        var disposables = sources.Select(Propagate).ToList();
        return new EventSubscription(() =>
        {
            foreach (var disposable in disposables)
                disposable.Dispose();
        });
    }

    /// <summary>
    /// Auto register any events that may exist within the object.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private List<object> SubscribeToTypedEvents(object source)
    {
        var type = source.GetType();
        var eventFields = type
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(f => f.FieldType.IsGenericType && 
                       f.FieldType.GetGenericTypeDefinition() == typeof(ITypedEvent<>))
            .ToList();

        var handlers = new List<object>();

        foreach (var field in eventFields)
        {
            var eventInstance = field.GetValue(source);
            if (eventInstance == null) continue;

            var eventType = field.FieldType.GetGenericArguments()[0];
            var subscribeMethod = field.FieldType.GetMethod("Subscribe")!;
            
            // Create a delegate that publishes to this aggregator
            var publishMethod = typeof(EventAggregator)
                .GetMethod(nameof(PublishFromTypedEvent), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(eventType);

            var handlerDelegate = Delegate.CreateDelegate(
                typeof(Action<>).MakeGenericType(eventType),
                this,
                publishMethod);

            subscribeMethod.Invoke(eventInstance, new[] { handlerDelegate });
            handlers.Add(new { Field = field, Handler = handlerDelegate, Instance = eventInstance });
        }

        return handlers;
    }

    private void StopAutoPropagation(object source)
    {
        if (!_typedEventSubscriptions.TryGetValue(source, out var handlers))
            return;

        foreach (dynamic handlerInfo in handlers)
        {
            var unsubscribeMethod = handlerInfo.Instance.GetType().GetMethod("Unsubscribe");
            unsubscribeMethod?.Invoke(handlerInfo.Instance, new[] { handlerInfo.Handler });
        }

        _typedEventSubscriptions.Remove(source);
    }

    private void PublishFromTypedEvent<TEvent>(TEvent evt) where TEvent : IEvent
    {
        Publish(evt);
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
    }
}