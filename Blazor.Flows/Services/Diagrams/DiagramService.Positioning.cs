using Blazor.Flows.Interfaces;

namespace Blazor.Flows.Services.Diagrams;

public partial class DiagramService
{
    /// <summary>
    ///     Returns the center point of a model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public (int CenterX, int CenterY) GetCenterCoordinates<T>(T model) where T : ISize, IPosition
    {
        return (model.PositionX + model.Width / 2, model.PositionY + model.Height / 2);
    }


    /// <inheritdoc />
    public virtual void CenterIn<TContainer, TModel>(CenterInParameters<TContainer, TModel> parameters)
        where TModel : ISize, IPosition
        where TContainer : ISize, IPosition
    {
        var padding = parameters.Container is IPadding p ? p.Padding : 0;
        
        var innerPositionX = parameters.Container.PositionX + padding;
        var innerPositionY = parameters.Container.PositionY + padding;

        
        var innerWidth = parameters.Container.Width - padding * 2;
        var innerHeight = parameters.Container.Height - padding * 2;

        
        var targetCenterX = innerPositionX + innerWidth / 2;
        var targetCenterY = innerPositionY + innerHeight / 2;

        
        var newPositionX = targetCenterX - parameters.Container.Width / 2;
        var newPositionY = targetCenterY - parameters.Container.Height / 2;
        
        parameters.Model.SetPosition(newPositionX, newPositionY);
    }


    /// <inheritdoc />
    public virtual void CenterInViewport<TModel>(CenterInViewportParameters<TModel> parameters)
        where TModel : IPosition, ISize
    {
        // Diagram might not be rendered on screen yet.
        if (Diagram.Width == 0 || Diagram.Height == 0)
        {
            return;
        }

        var viewportCenterX = (Diagram.Width / 2 - Diagram.PanX) / Diagram.Zoom;
        var viewportCenterY = (Diagram.Height / 2 - Diagram.PanY) / Diagram.Zoom;

        var newPositionX = (int)(viewportCenterX - parameters.Model.Width / 2);
        var newPositionY = (int)(viewportCenterY - parameters.Model.Height / 2);

        parameters.Model.SetPosition(newPositionX, newPositionY);
    }
    
    /// <inheritdoc />
    public virtual void ZoomToModel<TModel>(ZoomToModelParameters<TModel> parameters)
        where TModel : IPosition, ISize
    {
        if (Diagram.Width <= 0 || Diagram.Height <= 0 || parameters.Model.Width <= 0 || parameters.Model.Height <= 0)
            return;

        var worldCenterX = parameters.Model.PositionX + parameters.Model.Width / 2;
        var worldCenterY = parameters.Model.PositionY + parameters.Model.Height / 2;

        var screenCenterX = Diagram.Width / 2.0;
        var screenCenterY = Diagram.Height / 2.0;

        var newPanX = screenCenterX  - worldCenterX;
        var newPanY = screenCenterY  - worldCenterY;

        Diagram.SetZoom(1);
        Diagram.SetPan((int)newPanX, (int)newPanY);
    }
}
