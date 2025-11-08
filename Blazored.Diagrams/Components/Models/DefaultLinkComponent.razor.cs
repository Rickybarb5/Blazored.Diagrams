using Blazored.Diagrams.Components.Containers;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazored.Diagrams.Components.Models;

/// <summary>
/// Basic functionality for a link component
/// </summary>
public abstract partial class DefaultLinkComponent
{
    /// <summary>
    /// The stroke color that will be used on the SVG path.
    /// </summary>
    protected virtual string Stroke => Link.IsSelected ? SelectedStrokeColor : StrokeColor;

    /// <summary>
    ///     Link to be rendered.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ILink Link { get; set; } = null!;

    /// <summary>
    /// Service cascaded through the <see cref="LinkContainer"/>
    /// </summary>
    [CascadingParameter]
    public required IDiagramService DiagramService { get; init; } = null!;

    [Inject] private IJSRuntime Js { get; init; } = null!;

    /// <summary>
    ///     The <see cref="LinkContainer"/> that renders the component.
    /// </summary>
    [CascadingParameter]
    public required LinkContainer Container { get; init; }

    /// <summary>
    ///     Color of the link path. Default is black.
    /// </summary>
    protected abstract string StrokeColor { get; set; }

    /// <summary>
    /// Color of the link path when it's selected. Default is gray.
    /// </summary>
    protected abstract string SelectedStrokeColor { get; set; }

    /// <summary>
    ///     Width of the path. Default is 4.
    /// </summary>
    protected abstract int StrokeWidth { get; set; }
    

    /// <summary>
    ///     ID of the element.
    /// </summary>
    public virtual string PathId => $"link-path-{Link.Id}";

    /// <summary>
    /// Handles the @onclick event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandleClick(MouseEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkClickedEvent(Link, e));
        }
    }

    /// <summary>
    /// Handles the @ondbclick event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandleDoubleClick(MouseEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkDoubleClickedEvent(Link, e));
        }
    }

    /// <summary>
    /// Handles the @onpointerdown event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandlePointerDown(PointerEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkPointerDownEvent(Link, e));
        }
    }
    
    /// <summary>
    /// Handles the @onpointerup event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandlePointerUp(PointerEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkPointerUpEvent(Link, e));
        }
    }

    /// <summary>
    /// Handles the @onpointerleave event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandlePointerLeave(PointerEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkPointerLeaveEvent(Link, e));
        }
    }

    /// <summary>
    /// Handles the @onpointerenter event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandlePointerEnter(PointerEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkPointerEnterEvent(Link, e));
        }
    }

    /// <summary>
    /// Handles the @onpointermove event.
    /// </summary>
    /// <param name="e">Mouse event args.</param>
    protected virtual async Task HandlePointerMove(PointerEventArgs e)
    {
        if (await Js.IsClickOnPath(e, PathId))
        {
            DiagramService.Events.Publish(new LinkPointerMoveEvent(Link, e));
        }
    }

    /// <summary>
    /// Relative start X coordinate of the link.
    /// </summary>
    protected virtual int RelativeX1 => DiagramService.GetCenterCoordinates(Link.SourcePort).CenterX - Container.PositionX;

    /// <summary>
    /// Relative start Y coordinate of the link.
    /// </summary>
    protected virtual int RelativeY1 => DiagramService.GetCenterCoordinates(Link.SourcePort).CenterY - Container.PositionY;

    /// <summary>
    /// Relative end X coordinate of the link.
    /// </summary>
    protected virtual int RelativeX2 => Container.TargetX - Container.PositionX;

    /// <summary>
    /// Relative start Y coordinate of the link.
    /// </summary>
    protected virtual int RelativeY2 => Container.TargetY - Container.PositionY;

    /// <summary>
    /// Gets the path string that is drawn on the SVG.
    /// </summary>
    /// <returns></returns>
    protected abstract string GetPath();

}