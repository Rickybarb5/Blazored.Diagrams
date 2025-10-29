using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;

namespace Blazored.Diagrams.Services.Diagrams;

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

    /// <summary>
    /// Centers a model in another model
    /// </summary>
    /// <param name="toCenter">Model that will change position.</param>
    /// <param name="container">Model where the model will be centered to. </param>
    /// <typeparam name="TModel">Any diagram model that implements <see cref="ISize"/>, <see cref="IPosition"/>.</typeparam>
    /// <typeparam name="TContainer">Any diagram model that implements <see cref="ISize"/>, <see cref="IPosition"/></typeparam>
    public void CenterIn<TContainer, TModel>(
        TModel toCenter,
        TContainer container)
        where TModel : ISize, IPosition
        where TContainer : ISize, IPosition
    {
        var padding = container is IPadding p ? p.Padding : 0;
        // --- 1. Calculate the inner content area (excluding padding) ---
        // The inner content's top-left corner starts after the padding.
        var innerPositionX = container.PositionX + padding;
        var innerPositionY = container.PositionY + padding;

        // The inner content's width/height is the total minus padding on both sides.
        var innerWidth = container.Width - (padding * 2);
        var innerHeight = container.Height - (padding * 2);

        // --- 2. Calculate the center of the inner content area ---
        var targetCenterX = innerPositionX + innerWidth / 2;
        var targetCenterY = innerPositionY + innerHeight / 2;

        // --- 3. Calculate the new position for 'toCenter' to align its center with the target center ---
        // This correctly accounts for the size of the object being centered.
        var newPositionX = targetCenterX - toCenter.Width / 2;
        var newPositionY = targetCenterY - toCenter.Height / 2;

        // --- 4. Set the new position ---
        // NO padding subtraction is needed here, as it was accounted for in Step 1 & 2.
        toCenter.SetPosition(newPositionX, newPositionY);
    }

    /// <summary>
    ///     Changes the position of a model to be in the center of the diagram, accounting for pan and zoom.
    /// </summary>
    /// <param name="toCenter">Model to be centered.</param>
    /// <typeparam name="TModel">Type of the model to be centered.</typeparam>
    public void CenterInViewport<TModel>(TModel toCenter)
        where TModel : IPosition, ISize
    {
        // Diagram might not be rendered on screen yet.
        if (Diagram.Width == 0 || Diagram.Height == 0)
        {
            return;
        }

        // Calculate the center of the viewport.
        var viewportCenterX = (Diagram.Width / 2 - Diagram.PanX) / Diagram.Zoom;
        var viewportCenterY = (Diagram.Height / 2 - Diagram.PanY) / Diagram.Zoom;

        // Calculate the new position for the toCenter element to center it within the viewport
        var newPositionX = (int)(viewportCenterX - toCenter.Width / 2);
        var newPositionY = (int)(viewportCenterY - toCenter.Height / 2);

        // Set the new position of the toCenter element
        toCenter.SetPosition(newPositionX, newPositionY);
    }


    /// <inheritdoc />
    // TODO: This is bugged!
    public void FitToScreen(FitToScreenParameters parameters)
    {
        bool VisiblePredicate(IVisible x) => parameters.IncludeInvisible || x.IsVisible;

        // Collect all bounds from nodes, groups, and ports that pass visibility filter
        var allBounds = Diagram.AllNodes.Where(VisiblePredicate).Select(x => x.GetBounds())
            .Concat(Diagram.AllGroups.Where(VisiblePredicate).Select(x => x.GetBounds()))
            .Concat(Diagram.AllPorts.Where(VisiblePredicate).Select(x => x.GetBounds())).ToList();

        if (allBounds.Count == 0)
            return;

        // Compute combined bounding box
        var minX = allBounds.Min(b => b.Left);
        var minY = allBounds.Min(b => b.Top);
        var maxX = allBounds.Max(b => b.Right);
        var maxY = allBounds.Max(b => b.Bottom);

        var totalWidth = Math.Abs(maxX - minX);
        var totalHeight = Math.Abs(maxY - minY);

        if (totalWidth <= 0 || totalHeight <= 0 || Diagram.Width <= 0 || Diagram.Height <= 0)
            return;

        // Add margin
        var requiredWidth = totalWidth + parameters.Margin * 2;
        var requiredHeight = totalHeight + parameters.Margin * 2;

        // Compute zoom ratios
        double zoomX = Diagram.Width / requiredWidth;
        double zoomY = Diagram.Height / requiredHeight;

        var newZoom = Math.Min(zoomX, zoomY);
        var zoomOptions = Behaviours.GetBehaviourOptions<ZoomBehaviourOptions>();

        newZoom = Math.Min(newZoom, zoomOptions.MaxZoom);

        // Compute world center and target pan
        var worldCenterX = minX + totalWidth / 2;
        var worldCenterY = minY + totalHeight / 2;

        var screenCenterX = Diagram.Width / 2;
        var screenCenterY = Diagram.Height / 2;

        var newPanX = screenCenterX - worldCenterX * newZoom;
        var newPanY = screenCenterY - worldCenterY * newZoom;

        Diagram.SetZoom(newZoom);
        Diagram.SetPan((int)newPanX, (int)newPanY);
    }

    /// <summary>
    /// Collection of options to use on the <see cref="DiagramService.FitToScreen"/> method.
    /// </summary>
    /// <param name="Margin">Margin in pixels between the models and the edge of the diagram container.</param>
    /// <param name="IncludeInvisible">If true, invisible components will also be taken into account.</param>
    public record FitToScreenParameters(int Margin, bool IncludeInvisible);
}