using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Events;

internal partial class EventPropagator
{
    private void SubscribeToEvents(NodeAddedEvent e)
    {
        SubscribeToEvents(e.Model);
    }

    private void SubscribeToEvents(INode node)
    {
        node.OnVisibilityChanged += PublishVisibilityEvent;
        node.OnSelectionChanged += PublishSelectionEvent;
        node.OnSizeChanged += PublishSizeChangedEvent;
        node.OnPositionChanged += PublishPositionChangedEvent;

        node.OnPortAdded += PublishPortAddedEvent;
        node.OnPortRemoved += PublishPortRemovedEvent;
        node.Ports.ForEach(SubscribeToEvents);
        node.Ports.ForEach(p => PublishPortAddedEvent(node, p));
    }

    private void PublishPositionChangedEvent(INode node, int oldX, int oldY, int newX, int newY)
    {
        _service.Events.Publish(new NodePositionChangedEvent(node, oldX, oldY, newX, newY));
    }

    private void PublishSizeChangedEvent(INode obj, int oldWidth, int oldHeight, int newWidth, int newHeight)
    {
        _service.Events.Publish(new NodeSizeChangedEvent(obj, oldWidth, oldHeight, newWidth, newHeight));
    }

    private void PublishPortRemovedEvent(INode node, IPort port)
    {
        _service.Events.Publish(new PortRemovedFromNodeEvent(node, port));
        _service.Events.Publish(new PortRemovedEvent(port));
    }

    private void PublishPortAddedEvent(INode node, IPort port)
    {
        _service.Events.Publish(new PortAddedToNodeEvent(node, port));
        _service.Events.Publish(new PortAddedEvent(port));
    }

    private void PublishSelectionEvent(INode arg1)
    {
        _service.Events.Publish(new NodeSelectionChangedEvent(arg1));
        //TODO: Add Generic event
    }

    private void PublishVisibilityEvent(INode arg1)
    {
        _service.Events.Publish(new NodeVisibilityChangedEvent(arg1));
        //TODO: Add Generic event
    }

    private void UnsubscribeFromEvents(NodeRemovedEvent e)
    {
        UnsubscribeFromEvents(e.Model);
    }

    private void UnsubscribeFromEvents(INode node)
    {
        node.OnVisibilityChanged -= PublishVisibilityEvent;
        node.OnSelectionChanged -= PublishSelectionEvent;
        node.OnSizeChanged -= PublishSizeChangedEvent;
        node.OnPositionChanged -= PublishPositionChangedEvent;

        node.OnPortAdded -= PublishPortAddedEvent;
        node.OnPortRemoved -= PublishPortRemovedEvent;
        node.Ports.ForEach(UnsubscribeFromEvents);
    }
}