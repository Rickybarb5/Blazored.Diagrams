using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Links;

public partial class Link
{
    /// <inheritdoc cref="ILink.SetTargetPosition" />
    public virtual void SetTargetPosition(int x, int y)
    {
        var stateChanged = _targetPositionX != x || _targetPositionY != y;
        if (stateChanged)
        {
            var oldX = _targetPositionX;
            var oldY = _targetPositionY;
            _targetPositionX = x;
            _targetPositionY = y;
            OnTargetPositionChanged?.Invoke(this, oldX, oldY, _targetPositionX, _targetPositionY);
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
            OnSizeChanged?.Invoke(this, oldWidth, oldHeight, _width, _height);
        }
    }

    /// <summary>
    /// Sets the links target position to the center of a model
    /// </summary>
    /// TODO:Make this more customizable
    public virtual void SetTargetPosition<TModel>(TModel model) where TModel : IPosition, ISize
    {
        ArgumentNullException.ThrowIfNull(model);
        var centerCoordinates = model.GetCenterCoordinates();
        SetTargetPosition(centerCoordinates.CenterX, centerCoordinates.CenterY);
    }
}