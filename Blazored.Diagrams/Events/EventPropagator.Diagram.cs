using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Layers;

namespace Blazored.Diagrams.Events;

internal partial class EventPropagator
{
    private void SubscribeToEvents(IDiagram diagram)
    {
        diagram.OnLayerAdded += PublishLayerAddedEvent;
        diagram.OnLayerRemoved += PublishLayerRemoved;
        diagram.OnPanChanged += PublishPanChanged;
        diagram.OnZoomChanged += PublishZoomChanged;
        diagram.OnPositionChanged += PublishPositionChanged;
        diagram.OnSizeChanged += PublishSizeChanged;
    }

    private void PublishSizeChanged(IDiagram diagram, int arg1, int arg2, int arg3, int arg4)
    {
        _service.Events.Publish(new DiagramSizeChangedEvent(diagram, arg1, arg2, arg3, arg4));
    }

    private void PublishPositionChanged(IDiagram diagram, int arg1, int arg2, int arg3, int arg4)
    {
        _service.Events.Publish(new DiagramPositionChangedEvent(diagram, arg1, arg2, arg3, arg4));
    }

    private void PublishZoomChanged(IDiagram diagram, double arg1, double arg2)
    {
        _service.Events.Publish(new DiagramZoomChangedEvent(diagram, arg1, arg2));
    }

    private void PublishPanChanged(IDiagram diagram, int arg1, int arg2, int arg3, int arg4)
    {
        _service.Events.Publish(new DiagramPanChangedEvent(diagram, arg1, arg2, arg3, arg4));
    }

    private void PublishLayerRemoved(IDiagram arg1, ILayer arg2)
    {
        _service.Events.Publish(new LayerRemovedEvent(arg2));
    }

    private void PublishLayerAddedEvent(IDiagram arg1, ILayer arg2)
    {
        _service.Events.Publish(new LayerAddedEvent(arg2));
    }
}