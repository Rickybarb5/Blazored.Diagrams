using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Events;

internal partial class EventPropagator
{
    private void SubscribeToEvents(PortAddedEvent e)
    {
        SubscribeToEvents(e.Model);
    }

    private void SubscribeToEvents(IPort port)
    {
        port.OnVisibilityChanged += PublishVisibilityEvent;
        port.OnSizeChanged += PublishSizeChangedEvent;
        port.OnPositionChanged += PublishPositionChangedEvent;
        port.OnPortAlignmentChanged += PublishPortAlignmentChangedEvent;
        port.OnPortJustificationChanged += PublishPortJustificationChangedEvent;
        port.OnPortParentChanged += PublishParentChangedEvent;
        port.OnIncomingLinkAdded += HandleIncomingLinkAdded;
        port.OnIncomingLinkRemoved += HandleIncomingLinkRemoved;
        port.OnOutgoingLinkAdded += HandleOutgoingLinkAdded;
        port.OnOutgoingLinkRemoved += HandleOutgoingLinkRemoved;
        port.IncomingLinks.ForEach(SubscribeToEvents);
        port.OutgoingLinks.ForEach(SubscribeToEvents);

        port.IncomingLinks.ForEach(l => HandleIncomingLinkAdded(port, l));
        port.OutgoingLinks.ForEach(l => HandleOutgoingLinkAdded(port, l));
    }

    private void PublishPortJustificationChangedEvent(IPort arg1, PortJustification arg2, PortJustification arg3)
    {
        _service.Events.Publish(new PortJustificationChangedEvent(arg1, arg2, arg3));
    }

    private void PublishPortAlignmentChangedEvent(IPort arg1, PortAlignment arg2, PortAlignment arg3)
    {
        _service.Events.Publish(new PortAlignmentChangedEvent(arg1, arg2, arg3));
    }

    private void HandleOutgoingLinkRemoved(IPort arg1, ILink arg2)
    {
        _service.Events.Publish(new OutgoingLinkRemovedEvent(arg1, arg2));
        _service.Events.Publish(new LinkRemovedEvent(arg2));
    }

    private void HandleOutgoingLinkAdded(IPort arg1, ILink arg2)
    {
        _service.Events.Publish(new OutgoingLinkAddedEvent(arg1, arg2));
        _service.Events.Publish(new LinkAddedEvent(arg2));
    }

    private void HandleIncomingLinkRemoved(IPort arg1, ILink arg2)
    {
        _service.Events.Publish(new IncomingLinkRemovedEvent(arg1, arg2));
        _service.Events.Publish(new LinkRemovedEvent(arg2));
    }

    private void HandleIncomingLinkAdded(IPort arg1, ILink arg2)
    {
        _service.Events.Publish(new IncomingLinkAddedEvent(arg1, arg2));
        _service.Events.Publish(new LinkAddedEvent(arg2));
    }

    private void PublishParentChangedEvent(IPort port, IPortContainer oldParent, IPortContainer newParent)
    {
        _service.Events.Publish(new PortParentChangedEvent(port, oldParent, newParent));
    }

    private void PublishPositionChangedEvent(IPort port, int oldX, int oldY, int newX, int newY)
    {
        _service.Events.Publish(new PortPositionChangedEvent(port, oldX, oldY, newX, newY));
    }

    private void PublishSizeChangedEvent(IPort obj, int oldWidth, int oldHeight, int newWidth, int newHeight)
    {
        _service.Events.Publish(new PortSizeChangedEvent(obj, oldWidth, oldHeight, newWidth, newHeight));
    }

    private void PublishVisibilityEvent(IPort arg1)
    {
        _service.Events.Publish(new PortVisibilityChangedEvent(arg1));
    }

    private void UnsubscribeFromEvents(PortRemovedEvent e)
    {
        UnsubscribeFromEvents(e.Model);
    }

    private void UnsubscribeFromEvents(IPort port)
    {
        port.OnVisibilityChanged -= PublishVisibilityEvent;
        port.OnSizeChanged -= PublishSizeChangedEvent;
        port.OnPositionChanged -= PublishPositionChangedEvent;
        port.OnPortAlignmentChanged -= PublishPortAlignmentChangedEvent;
        port.OnPortJustificationChanged -= PublishPortJustificationChangedEvent;
        port.OnPortParentChanged -= PublishParentChangedEvent;
        port.OnIncomingLinkAdded -= HandleIncomingLinkAdded;
        port.OnIncomingLinkRemoved -= HandleIncomingLinkRemoved;
        port.OnOutgoingLinkAdded -= HandleOutgoingLinkAdded;
        port.OnOutgoingLinkRemoved -= HandleOutgoingLinkRemoved;

        port.IncomingLinks.ForEach(UnsubscribeFromEvents);
        port.OutgoingLinks.ForEach(UnsubscribeFromEvents);
    }
}