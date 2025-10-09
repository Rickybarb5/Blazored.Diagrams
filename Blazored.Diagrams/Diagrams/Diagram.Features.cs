using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Layers;

namespace Blazored.Diagrams.Diagrams;

public partial class Diagram
{
    /// <inheritdoc />
    public virtual void SetPan(int panX, int panY)
    {
        if (_panX != panX || _panY != panY)
        {
            var oldX = _panX;
            var oldY = _panY;
            _panX = panX;
            _panY = panY;
            OnPanChanged.Publish(new(this, oldX, oldY, _panX, _panY));
        }
    }

    /// <inheritdoc />
    public virtual void UnselectAll()
    {
        _layers.ForEach(l =>l.UnselectAll());
    }

    /// <inheritdoc />
    public virtual void SelectAll()
    {
        _layers.ForEach(l =>l.SelectAll());
    }
    
    /// <inheritdoc />
    public virtual void StepZoomUp()
    {
        SetZoom(Zoom /*TODO:Get step from options*/);
    }

    /// <inheritdoc />
    public virtual void StepZoomDown()
    {
        SetZoom(Zoom /*TODO:Get step from options*/);
    }

    /// <inheritdoc />
    public virtual void SetZoom(double zoom)
    {
        if (zoom != _zoom)
        {
            var oldZoom = _zoom;
            _zoom = zoom;
            OnZoomChanged.Publish(new(this, oldZoom, _zoom));
        }
    }

    /// <inheritdoc />
    void IDiagram.SetSize(int width, int height)
    {
        var stateChanged = width != _width || _height != height;

        if (stateChanged)
        {
            var oldWidth = _width;
            var oldHeight = _height;
            _width = width;
            _height = height;
            OnSizeChanged.Publish(new(this, oldWidth, oldHeight, _width, _height));
        }
    }

    /// <inheritdoc />
    void IDiagram.SetPosition(int x, int y)
    {
        var stateChanged = x != _positionX || y != _positionY;

        if (stateChanged)
        {
            var oldX = _positionX;
            var oldY = _positionY;
            _positionX = x;
            _positionY = y;
            OnPositionChanged.Publish(new(this, oldX, oldY, _positionX, _positionY));
        }
    }

    /// <inheritdoc />
    public virtual void UseLayer(ILayer layer)
    {
        if (!Layers.Contains(layer))
        {
            Layers.Add(layer);
        }

        CurrentLayer = layer;
    }

    /// <inheritdoc />
    public virtual void UseLayer(string layerId)
    {
        var layer = Layers.FirstOrDefault(x => x.Id == layerId);

        CurrentLayer = layer ?? throw new InvalidOperationException($"Layer {layerId} is not a part of the diagram");
    }
}