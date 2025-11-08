using Blazored.Diagrams.Extensions;

namespace Blazored.Diagrams.Groups;

public partial class Group
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
            OnPositionChanged.Publish(new(this, oldX, oldY, _positionX, _positionY));
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
            OnSizeChanged.Publish(new(this, oldWidth, oldHeight, _width, _height));
        }
    }

    /// <inheritdoc />
    void IGroup.SetPositionInternal(int x, int y)
    {
        _positionX = x;
        _positionY = y;
    }

    /// <inheritdoc />
    void IGroup.SetSizeInternal(int width, int height)
    {
        _width = width;
        _height = height;
    }


    /// <inheritdoc />
    public virtual void UnselectAll()
    {
        AllNodes.ForEach(node => node.IsSelected = false);
        AllGroups.ForEach(group => group.IsSelected = false);
    }

    /// <inheritdoc />
    public virtual void SelectAll()
    {
        AllNodes.ForEach(node => node.IsSelected = true);
        AllGroups.ForEach(group => group.IsSelected = true);
    }
}