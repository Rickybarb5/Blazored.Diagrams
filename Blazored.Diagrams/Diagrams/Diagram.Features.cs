using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Options.Behaviours;

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
            OnPanChanged?.Invoke(this, oldX, oldY, _panX, _panY);
        }
    }

    /// <inheritdoc />
    public virtual void UnselectAll()
    {
        foreach (var layer in _layers) layer.UnselectAll();
    }

    /// <inheritdoc />
    public virtual void SelectAll()
    {
        foreach (var layer in _layers) layer.SelectAll();
    }
    
    /// <inheritdoc />
    public virtual void StepZoomUp()
    {
        SetZoom(Zoom + Options.Get<ZoomOptions>()!.ZoomStep);
    }

    /// <inheritdoc />
    public virtual void StepZoomDown()
    {
        SetZoom(Zoom - Options.Get<ZoomOptions>()!.ZoomStep);
    }

    /// <inheritdoc />
    public virtual void SetZoom(double zoom)
    {
        if (zoom > Options.Get<ZoomOptions>()!.MaxZoom)
        {
            zoom = Options.Get<ZoomOptions>()!.MaxZoom;
        }
        else if (zoom < Options.Get<ZoomOptions>()!.MinZoom)
        {
            zoom = Options.Get<ZoomOptions>()!.MinZoom;
        }

        if (zoom != _zoom)
        {
            var oldZoom = _zoom;
            _zoom = zoom;
            OnZoomChanged?.Invoke(this, oldZoom, _zoom);
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
            OnSizeChanged?.Invoke(this, oldWidth, oldHeight, _width, _height);
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
            OnPositionChanged?.Invoke(this, oldX, oldY, _positionX, _positionY);
        }
    }

    /// <inheritdoc />
    public virtual void UseLayer(ILayer layer)
    {
        UseLayer(layer.Id);
    }

    /// <inheritdoc />
    public virtual void UseLayer(Guid layerId)
    {
        var layer = Layers.FirstOrDefault(x => x.Id == layerId);

        if (layer is null)
        {
            throw new InvalidOperationException($"Layer {layerId} is not a part of the diagram");
        }

        layer.IsCurrentLayer = true;
    }
}