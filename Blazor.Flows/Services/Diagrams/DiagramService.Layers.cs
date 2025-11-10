using Blazor.Flows.Extensions;
using Blazor.Flows.Layers;

namespace Blazor.Flows.Services.Diagrams;

public partial class DiagramService
{
    /// <inheritdoc />
    public void UseLayer(ILayer layer)
    {
        if (!Diagram.Layers.Contains(layer))
        {
            AddLayer(layer);
        }

        Diagram.CurrentLayer = layer;
    }
    
    /// <inheritdoc />
    public virtual void UnselectAll()
    {
        Diagram.Layers.ForEach(l =>l.UnselectAll());
    }

    /// <inheritdoc />
    public virtual void SelectAll()
    {
        Diagram.Layers.ForEach(l =>l.SelectAll());
    }

}