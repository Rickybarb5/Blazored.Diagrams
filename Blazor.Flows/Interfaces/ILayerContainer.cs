using Blazor.Flows.Helpers;
using Blazor.Flows.Layers;

namespace Blazor.Flows.Interfaces;

/// <summary>
/// Contains features for a model that has layers.
/// </summary>
public interface ILayerContainer
{
    /// <summary>
    /// Layers that belong to the diagram.
    /// </summary>
    public ObservableList<ILayer> Layers { get; }
}