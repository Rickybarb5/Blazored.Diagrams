using Blazored.Diagrams.Links;
using Blazored.Diagrams.Services.Events;

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
    public virtual bool CanConnectTo(IPort port)
    {
        var canConnect =
            Id != port.Id &&
            Parent.Id != port.Parent.Id;
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

    /// <inheritdoc />
    public virtual (int PositionX, int PositionY) CalculatePosition()
    {
        var (x, y) = (Position: Alignment, Alignment: Justification) switch
        {
            (PortAlignment.Left, PortJustification.Start) => (Parent.PositionX - Width / 2,
                Parent.PositionY - Height / 2),
            (PortAlignment.Left, PortJustification.Center) => (Parent.PositionX - Width / 2,
                Parent.PositionY + Parent.Height / 2 - Height / 2),
            (PortAlignment.Left, PortJustification.End) => (Parent.PositionX - Width / 2,
                Parent.PositionY + Parent.Height - Height / 2),

            (PortAlignment.Right, PortJustification.Start) => (
                Parent.PositionX + Parent.Width - Width / 2,
                Parent.PositionY - Height / 2),
            (PortAlignment.Right, PortJustification.Center) => (
                Parent.PositionX + Parent.Width - Width / 2,
                Parent.PositionY + Parent.Height / 2 - Height / 2),
            (PortAlignment.Right, PortJustification.End) => (Parent.PositionX + Parent.Width - Width / 2,
                Parent.PositionY + Parent.Height - Height / 2),

            (PortAlignment.Top, PortJustification.Start) =>
                (Parent.PositionX - Width / 2, Parent.PositionY - Height / 2),
            (PortAlignment.Top, PortJustification.Center) => (
                Parent.PositionX + Parent.Width / 2 - Width / 2,
                Parent.PositionY - Height / 2),
            (PortAlignment.Top, PortJustification.End) => (Parent.PositionX + Parent.Width - Width / 2,
                Parent.PositionY - Height / 2),

            (PortAlignment.Bottom, PortJustification.Start) => (Parent.PositionX - Width / 2,
                Parent.PositionY + Parent.Height - Height / 2),
            (PortAlignment.Bottom, PortJustification.Center) => (
                Parent.PositionX + Parent.Width / 2 - Width / 2,
                Parent.PositionY + Parent.Height - Height / 2),
            (PortAlignment.Bottom, PortJustification.End) => (
                Parent.PositionX + Parent.Width - Width / 2,
                Parent.PositionY + Parent.Height - Height / 2),
            (PortAlignment.Custom, _) => (PositionX, PositionY),
            (PortAlignment.CenterParent, _) => (
                Parent.PositionX + (Parent.Width / 2) - (Width / 2),
                Parent.PositionY + (Parent.Height / 2) - (Height / 2)
            ),
            _ => (PositionX, PositionY)
        };
        return (x, y);
    }

    /// <inheritdoc />
    public virtual void RefreshPositionCoordinates()
    {
        var newPosition = CalculatePosition();
        SetPosition(newPosition.PositionX, newPosition.PositionY);
    }
}