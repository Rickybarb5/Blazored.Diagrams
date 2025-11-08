using Blazored.Diagrams.Events;

namespace Blazored.Diagrams.Nodes;

public partial class Node
{
    /// <inheritdoc />
    public virtual void SetPosition(int x, int y)
    {
        var stateChanged = x != _positionX || y != _positionY;
        if (stateChanged)
        {
            var oldX = _positionX;
            var oldY = _positionY;
            _positionX = x;
            _positionY = y;
            OnPositionChanged.Publish(new NodePositionChangedEvent(this, oldX, oldY, _positionX, _positionY));
        }
    }

    /// <inheritdoc />
    public virtual void SetSize(int width, int height)
    {
        var stateChanged = width != _width || _height != height;
        if (stateChanged)
        {
            var oldWidth = _width;
            var oldHeight = _height;
            _width = width;
            _height = height;
            OnSizeChanged.Publish(new NodeSizeChangedEvent(this, oldWidth, oldHeight, _width, _height));
        }
    }

    /// <inheritdoc />
    void INode.SetPositionInternal(int x, int y)
    {
        _positionX = x;
        _positionY = y;
    }

    /// <inheritdoc />
    void INode.SetSizeInternal(int width, int height)
    {
        _width = width;
        _height = height;
    }
}