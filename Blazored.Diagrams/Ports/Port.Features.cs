using Blazored.Diagrams.Events;
using Blazored.Diagrams.Links;

namespace Blazored.Diagrams.Ports;

public partial class Port
{
    /// <inheritdoc />
    public virtual void SetPosition(int x, int y)
    {
        var stateChanged = _positionX != x || _positionY != y;
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
    public virtual bool CanCreateLink()
    {
        return true;
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
            OnSizeChanged.Publish(new(this, oldWidth, oldHeight, _width, _height));
        }
    }

    /// <inheritdoc />
    public virtual bool CanConnectTo(IPort targetPort)
    {
        var canConnect =
            Id != targetPort.Id &&
            Parent.Id != targetPort.Parent.Id;
        return canConnect;
    }

    /// <inheritdoc />
    void IPort.SetPositionInternal(int x, int y)
    {
        _positionX = x;
        _positionY = y;
    }

    /// <inheritdoc />
    void IPort.SetSizeInternal(int width, int height)
    {
        _width = width;
        _height = height;
    }

    /// <inheritdoc />
    /// <returns> <see cref="PositionX"/> and <see cref="PositionY"/>.</returns>
    public virtual (int PositionX, int PositionY) CustomPositioning()
    {
        return (PositionX, PositionY);
    }

    private void HandleOutgoingLinkRemoved(ItemRemovedEvent<ILink> obj)
    {
        OnOutgoingLinkRemoved.Publish(new(this, obj.Item));
        OnLinkRemoved.Publish(new(obj.Item));
    }

    private void HandleOutgoingLinkAdded(ItemAddedEvent<ILink> obj)
    {
        OnOutgoingLinkAdded.Publish(new(this, obj.Item));
        OnLinkAdded.Publish(new(obj.Item));
    }

    private void HandleIncomingLinkRemoved(ItemRemovedEvent<ILink> obj)
    {
        OnIncomingLinkRemoved.Publish(new(this, obj.Item));
    }

    private void HandleIncomingLinkAdded(ItemAddedEvent<ILink> obj)
    {
        OnIncomingLinkAdded.Publish(new(this, obj.Item));
    }
}