using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Describes a model that contains nodes.
/// </summary>
public interface INodeContainer
{
    /// <summary>
    ///     Nodes that belong to the model.
    /// </summary>
    ObservableList<INode> Nodes { get; init; }
}