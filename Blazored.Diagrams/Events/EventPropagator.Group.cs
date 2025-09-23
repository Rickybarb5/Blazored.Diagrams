using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Events;

internal partial class EventPropagator
{
    private void SubscribeToEvents(GroupAddedEvent e)
    {
        SubscribeToEvents(e.Model);
    }

    private void SubscribeToEvents(IGroup group)
    {
        group.OnVisibilityChanged += PublishVisibilityEvent;
        group.OnSelectionChanged += PublishSelectionEvent;
        group.OnSizeChanged += PublishSizeChangedEvent;
        group.OnPositionChanged += PublishPositionChangedEvent;
        group.OnPaddingChanged += PublishPaddingChanged;

        group.OnNodeAdded += PublishNodeAdded;
        group.OnNodeRemoved += PublishNodeRemoved;


        group.OnGroupAdded += PublishGroupAdded;
        group.OnGroupRemoved += PublishGroupRemoved;

        group.OnPortAdded += PublishPortAddedEvent;
        group.OnPortRemoved += PublishPortRemovedEvent;

        group.Nodes.ForEach(SubscribeToEvents);
        group.Groups.ForEach(SubscribeToEvents);
        group.Ports.ForEach(SubscribeToEvents);
        group.Groups.ForEach(g => PublishGroupAdded(group, g));
        group.Nodes.ForEach(n => PublishNodeAdded(group, n));
        group.Ports.ForEach(p => PublishPortAddedEvent(group, p));
    }

    private void PublishPaddingChanged(IGroup obj, int oldPadding, int newPadding)
    {
        _service.Events.Publish(new GroupPaddingChangedEvent(obj, oldPadding, newPadding));
    }

    private void PublishGroupRemoved(IGroup parentGroup, IGroup obj)
    {
        _service.Events.Publish(new GroupRemovedFromGroupEvent(parentGroup, obj));
        _service.Events.Publish(new GroupRemovedEvent(obj));
    }

    private void PublishGroupAdded(IGroup parentGroup, IGroup obj)
    {
        _service.Events.Publish(new GroupAddedToGroupEvent(parentGroup, obj));
        _service.Events.Publish(new GroupAddedEvent(obj));
    }

    private void PublishNodeRemoved(IGroup arg1, INode arg2)
    {
        _service.Events.Publish(new NodeRemovedFromGroupEvent(arg1, arg2));
        _service.Events.Publish(new NodeRemovedEvent(arg2));
    }

    private void PublishNodeAdded(IGroup arg1, INode arg2)
    {
        _service.Events.Publish(new NodeAddedToGroupEvent(arg1, arg2));
        _service.Events.Publish(new NodeAddedEvent(arg2));
    }

    private void PublishPositionChangedEvent(IGroup group, int oldX, int oldY, int newX, int newY)
    {
        _service.Events.Publish(new GroupPositionChangedEvent(group, oldX, oldY, newX, newY));
    }

    private void PublishSizeChangedEvent(IGroup obj, int oldWidth, int oldHeight, int newWidth, int newHeight)
    {
        _service.Events.Publish(new GroupSizeChangedEvent(obj, oldWidth, oldHeight, newWidth, newHeight));
    }

    private void PublishPortRemovedEvent(IGroup group, IPort port)
    {
        _service.Events.Publish(new PortRemovedFromGroupEvent(group, port));
        _service.Events.Publish(new PortRemovedEvent(port));
    }

    private void PublishPortAddedEvent(IGroup group, IPort port)
    {
        _service.Events.Publish(new PortAddedToGroupEvent(group, port));
        _service.Events.Publish(new PortAddedEvent(port));
    }

    private void PublishSelectionEvent(IGroup arg1)
    {
        _service.Events.Publish(new GroupSelectionChanged(arg1));
        //TODO: Add Generic event
    }

    private void PublishVisibilityEvent(IGroup arg1)
    {
        _service.Events.Publish(new GroupVisibilityChanged(arg1));
        //TODO: Add Generic event
    }

    private void UnsubscribeFromEvents(GroupRemovedEvent e)
    {
        UnsubscribeFromEvents(e.Model);
    }

    private void UnsubscribeFromEvents(IGroup group)
    {
        group.OnVisibilityChanged -= PublishVisibilityEvent;
        group.OnSelectionChanged -= PublishSelectionEvent;
        group.OnSizeChanged -= PublishSizeChangedEvent;
        group.OnPositionChanged -= PublishPositionChangedEvent;
        group.OnPaddingChanged -= PublishPaddingChanged;

        group.OnNodeAdded -= PublishNodeAdded;
        group.OnNodeRemoved -= PublishNodeRemoved;

        group.OnGroupAdded -= PublishGroupAdded;
        group.OnGroupRemoved -= PublishGroupRemoved;

        group.OnPortAdded -= PublishPortAddedEvent;
        group.OnPortRemoved -= PublishPortRemovedEvent;

        group.Nodes.ForEach(UnsubscribeFromEvents);
        group.Groups.ForEach(UnsubscribeFromEvents);
        group.Ports.ForEach(SubscribeToEvents);

        group.Groups.ForEach(g => PublishGroupAdded(group, g));
        group.Nodes.ForEach(n => PublishNodeAdded(group, n));
        group.Ports.ForEach(p => PublishPortAddedEvent(group, p));
    }
}