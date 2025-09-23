using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Events;

internal partial class EventPropagator
{
    private void SubscribeToEvents(LinkAddedEvent e)
    {
        SubscribeToEvents(e.Model);
    }

    private void SubscribeToEvents(ILink link)
    {
        link.OnVisibilityChanged += PublishVisibilityEvent;
        link.OnSelectionChanged += PublishSelectionEvent;
        link.OnSizeChanged += PublishSizeChangedEvent;
        link.OnSourcePortChanged += PublishSourcePortEvent;
        link.OnTargetPortChanged += PublishTargetPortEvent;
        link.OnTargetPositionChanged += PublishTargetPositionEvent;
    }

    private void PublishTargetPositionEvent(ILink link, int oldX, int oldY, int positionX, int positionY)
    {
        _service.Events.Publish(new LinkTargetPositionChangedEvent(link, oldX, oldY, positionX, positionY));
    }

    private void PublishTargetPortEvent(ILink link, IPort? oldTargetPort, IPort? newTargetPort)
    {
        _service.Events.Publish(new LinkTargetPortChangedEvent(link, oldTargetPort, newTargetPort));
    }

    private void PublishSourcePortEvent(ILink link, IPort oldPort, IPort newPort)
    {
        _service.Events.Publish(new LinkSourcePortChangedEvent(link, oldPort, newPort));
    }

    private void PublishSizeChangedEvent(ILink link, int oldWidth, int oldHeight, int newWidth, int newHeight)
    {
        _service.Events.Publish(new LinkSizeChangedEvent(link, oldWidth, oldHeight, newWidth, newHeight));
    }

    private void PublishSelectionEvent(ILink obj)
    {
        _service.Events.Publish(new LinkSelectionChangedEvent(obj));
    }

    private void PublishVisibilityEvent(ILink obj)
    {
        _service.Events.Publish(new LinkVisibilityChangedEvent(obj));
    }

    private void UnsubscribeFromEvents(LinkRemovedEvent e)
    {
        UnsubscribeFromEvents(e.Model);
    }

    private void UnsubscribeFromEvents(ILink link)
    {
        link.OnVisibilityChanged -= PublishVisibilityEvent;
        link.OnSelectionChanged -= PublishSelectionEvent;
        link.OnSizeChanged -= PublishSizeChangedEvent;
        link.OnSourcePortChanged -= PublishSourcePortEvent;
        link.OnSourcePortChanged -= PublishTargetPortEvent;
        link.OnTargetPositionChanged -= PublishTargetPositionEvent;
    }
}