using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Virtualization;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class VirtualizationService : IVirtualizationService
{
    /// <inheritdoc />
    public IEnumerable<T> Virtualize<T>(IDiagram diagram, IEnumerable<T> items) where T : IPosition, ISize
    {
        if (diagram?.Options.Virtualization.Enabled != true)
        {
            foreach (var item in items)
            {
                yield return item;
            }

            yield break;
        }

        // Get the current zoom level
        var zoom = diagram.Zoom;

        // Buffer size in pixels, constant and independent of zoom
        var bufferSizeInPixels = diagram.Options.Virtualization.BufferSize;

        // Viewport in screen space (pixels), without applying zoom to pan and viewport size
        var viewportLeft = -diagram.PanX;
        var viewportTop = -diagram.PanY;
        var viewportRight = viewportLeft + diagram.Width;
        var viewportBottom = viewportTop + diagram.Height;

        // Expand the viewport by the buffer size (in pixels) on all sides
        var extendedViewportLeft = viewportLeft - bufferSizeInPixels;
        var extendedViewportTop = viewportTop - bufferSizeInPixels;
        var extendedViewportRight = viewportRight + bufferSizeInPixels;
        var extendedViewportBottom = viewportBottom + bufferSizeInPixels;

        // Iterate over all items
        foreach (var item in items)
        {
            // Convert item world coordinates to screen coordinates by applying the zoom factor
            var itemLeft = item.PositionX * zoom;
            var itemTop = item.PositionY * zoom;
            var itemRight = (item.PositionX + item.Width) * zoom;
            var itemBottom = (item.PositionY + item.Height) * zoom;

            // Check if the item is within the extended viewport (buffered area)
            if (itemLeft < extendedViewportRight &&
                itemRight > extendedViewportLeft &&
                itemTop < extendedViewportBottom &&
                itemBottom > extendedViewportTop)
            {
                yield return item;
            }
        }
    }
}