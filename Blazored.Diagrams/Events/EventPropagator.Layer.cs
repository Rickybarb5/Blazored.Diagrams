using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Events;

internal partial class EventPropagator
{
    private void SubscribeToEvents(LayerAddedEvent e)
    {
        SubscribeToEvents(e.Model);
    }

    private void SubscribeToEvents(ILayer layer)
    {
        layer.OnVisibilityChanged += PublishVisibilityEvent;
        layer.OnLayerUsageChanged += PublishLayerChangeEvent;

        layer.OnNodeAdded += PublishNodeAdded;
        layer.OnNodeRemoved += PublishNodeRemoved;

        layer.OnGroupAdded += PublishGroupAdded;
        layer.OnGroupRemoved += PublishGroupRemoved;

        layer.Nodes.ForEach(SubscribeToEvents);
        layer.Groups.ForEach(SubscribeToEvents);
        layer.Nodes.ForEach(n => PublishNodeAdded(layer, n));
        layer.Groups.ForEach(g => PublishGroupAdded(layer, g));
    }

    private void PublishLayerChangeEvent(ILayer obj)
    {
        _service.Events.Publish(new IsCurrentLayerChangedEvent(obj));
    }

    private void PublishGroupRemoved(ILayer parent, IGroup obj)
    {
        _service.Events.Publish(new GroupRemovedFromLayerEvent(parent, obj));
        _service.Events.Publish(new GroupRemovedEvent(obj));
    }

    private void PublishGroupAdded(ILayer parent, IGroup obj)
    {
        _service.Events.Publish(new GroupAddedToLayerEvent(parent, obj));
        _service.Events.Publish(new GroupAddedEvent(obj));
    }

    private void PublishNodeRemoved(ILayer parent, INode arg2)
    {
        _service.Events.Publish(new NodeRemovedFromLayerEvent(parent, arg2));
        _service.Events.Publish(new NodeRemovedEvent(arg2));
    }

    private void PublishNodeAdded(ILayer parent, INode arg2)
    {
        _service.Events.Publish(new NodeAddedToLayerEvent(parent, arg2));
        _service.Events.Publish(new NodeAddedEvent(arg2));
    }

    private void PublishVisibilityEvent(ILayer arg1)
    {
        _service.Events.Publish(new LayerVisibilityChangedEvent(arg1));
        //TODO: Add Generic event
    }

    private void UnsubscribeFromEvents(LayerRemovedEvent e)
    {
        UnsubscribeFromEvents(e.Model);
    }

    private void UnsubscribeFromEvents(ILayer layer)
    {
        layer.Nodes.ForEach(UnsubscribeFromEvents);
        layer.Groups.ForEach(UnsubscribeFromEvents);
        layer.OnVisibilityChanged -= PublishVisibilityEvent;
        layer.OnLayerUsageChanged -= PublishLayerChangeEvent;


        layer.OnNodeAdded -= PublishNodeAdded;
        layer.OnNodeRemoved -= PublishNodeRemoved;

        layer.OnGroupAdded -= PublishGroupAdded;
        layer.OnGroupRemoved -= PublishGroupRemoved;
    }
}