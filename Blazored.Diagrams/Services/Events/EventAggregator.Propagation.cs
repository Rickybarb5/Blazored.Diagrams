using System.Reflection;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Events;

public partial class EventAggregator
{
    private readonly List<IDisposable> _autoSubscriptions = [];
    /// <summary>
    /// Automatically registered subscriptions
    /// </summary>
    private readonly Dictionary<object, List<object>> _typedEventSubscriptions = [];

    /// <summary>
    /// Propagates events for existing/future diagram components.
    /// </summary>
    public void InitializeEventPropagation()
    {
        // Auto-propagate events from the diagram
        _autoSubscriptions.Add(Propagate(_diagramService.Diagram));
        _autoSubscriptions.Add(PropagateMany(_diagramService.Diagram.Layers));
        _autoSubscriptions.Add(PropagateMany(_diagramService.Diagram.AllNodes));
        _autoSubscriptions.Add(PropagateMany(_diagramService.Diagram.AllGroups));
        _autoSubscriptions.Add(PropagateMany(_diagramService.Diagram.AllPorts));
        _autoSubscriptions.Add(PropagateMany(_diagramService.Diagram.AllLinks));
        
        // recently added models may already have nested models.
        _autoSubscriptions.Add(SubscribeTo<LayerAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            PropagateNodes(e.Model);
            PropagateGroups(e.Model);
        }));
        _autoSubscriptions.Add(SubscribeTo<NodeAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            PropagatePorts(e.Model);
        }));
        
        
        _autoSubscriptions.Add(SubscribeTo<GroupAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            PropagateNodes(e.Model);
            PropagateGroups(e.Model);
            PropagatePorts(e.Model);
        }));
        
        _autoSubscriptions.Add(SubscribeTo<PortAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
            PropagateLinks(e.Model);
        }));
        
        _autoSubscriptions.Add(SubscribeTo<LinkAddedEvent>(e =>
        {
            _autoSubscriptions.Add(Propagate(e.Model));
        }));
    }

    private void PropagateLinks(IPort port)
    {
        port.OutgoingLinks.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new OutgoingLinkAddedEvent(port, p));
            Publish(new LinkAddedEvent(p));
        });
        
        port.IncomingLinks.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new IncomingLinkAddedEvent(port, p));
        });
    }

    private void PropagateGroups(ILayer layer)
    {
        layer.Groups.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new GroupAddedToLayerEvent(layer, p));
            Publish(new GroupAddedEvent(p));
        });
    }

    private void PropagateNodes(ILayer layer)
    {
        layer.Nodes.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new NodeAddedToLayerEvent(layer, p));
            Publish(new NodeAddedEvent(p));
            
        });
    }

    private void PropagateNodes(IGroup group)
    {
        group.Nodes.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new NodeAddedToGroupEvent(group, p));
            Publish(new NodeAddedEvent(p));
            
        });
    }
    
    private void PropagateGroups(IGroup group)
    {
        group.Groups.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new GroupAddedToGroupEvent(group, p));
            Publish(new GroupAddedEvent(p));
            
        });
    }

    private void PropagatePorts(INode node)
    {
        node.Ports.ForEach(p=>
        {
           _autoSubscriptions.Add(Propagate(p));
           Publish(new PortAddedToNodeEvent(node, p));
           Publish(new PortAddedEvent(p));
            
        });
    }
    private void PropagatePorts(IGroup group)
    {
        group.Ports.ForEach(p=>
        {
            _autoSubscriptions.Add(Propagate(p));
            Publish(new PortAddedToGroupEvent(group, p));
            Publish(new PortAddedEvent(p));
            
        });
    }
    
    /// <summary>
    /// Automatically subscribes to all ITypedEvent fields on an object and propagates them through this aggregator
    /// </summary>
    /// <param name="source">The object containing ITypedEvent fields</param>
    /// <returns>A disposable that will unsubscribe when disposed</returns>
    private IDisposable Propagate(object source)
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
    var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

    // 1. Get properties and fields that implement ITypedEvent<>
    var eventMembers = type
        .GetMembers(bindingFlags)
        .Where(m =>
        {
            // Only consider Fields or Properties
            if (m is FieldInfo field)
            {
                // Exclude compiler-generated backing fields to avoid duplicates of properties
                if (field.Name.Contains("<") && field.Name.Contains(">k__BackingField")) 
                    return false;
                
                return field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(ITypedEvent<>);
            }
            else if (m is PropertyInfo property && property.CanRead)
            {
                return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ITypedEvent<>);
            }
            return false;
        })
        .Select(m =>
        {
            // Determine the Type of the member
            var memberType = m is FieldInfo field ? field.FieldType : 
                              m is PropertyInfo property ? property.PropertyType : null;
            
            return new { Member = m, MemberType = memberType };
        })
        .ToList();

    var handlers = new List<object>();

    foreach (var memberInfo in eventMembers)
    {
        object? eventInstance = null;

        // Retrieve the value of the member (the actual ITypedEvent instance)
        if (memberInfo.Member is FieldInfo field)
        {
            eventInstance = field.GetValue(source);
        }
        else if (memberInfo.Member is PropertyInfo property)
        {
            eventInstance = property.GetValue(source);
        }

        if (eventInstance == null) continue;

        var eventType = memberInfo.MemberType?.GetGenericArguments()[0];
        var subscribeMethod = memberInfo.MemberType?.GetMethod(nameof(ITypedEvent<IEvent>.Subscribe))!;

        if (eventType is not null)
        {
            // Create a delegate that publishes to this aggregator
            var publishMethod = typeof(EventAggregator)
                .GetMethod(nameof(PublishFromTypedEvent), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(eventType);

            var handlerDelegate = Delegate.CreateDelegate(
                typeof(Action<>).MakeGenericType(eventType),
                this,
                publishMethod);

            subscribeMethod.Invoke(eventInstance, [handlerDelegate]);
        
            // Store the information needed for unsubscription later
            handlers.Add(new { Member = memberInfo.Member, Handler = handlerDelegate, Instance = eventInstance });
        }
    }

    return handlers;
}
    
    private void PublishFromTypedEvent<TEvent>(TEvent evt) where TEvent : IEvent
    {
        Publish(evt);
    }

    private void StopAutoPropagation(object source)
    {
        if (!_typedEventSubscriptions.TryGetValue(source, out var handlers))
            return;

        foreach (dynamic handlerInfo in handlers)
        {
            var unsubscribeMethod = handlerInfo.Instance.GetType().GetMethod(nameof(ITypedEvent<IEvent>.Unsubscribe));
            unsubscribeMethod?.Invoke(handlerInfo.Instance, new[] { handlerInfo.Handler });
        }

        _typedEventSubscriptions.Remove(source);
    }
}