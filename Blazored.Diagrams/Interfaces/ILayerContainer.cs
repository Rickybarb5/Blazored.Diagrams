using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Layers;

namespace Blazored.Diagrams.Interfaces;

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