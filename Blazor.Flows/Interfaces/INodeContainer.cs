using Blazor.Flows.Helpers;
using Blazor.Flows.Nodes;

namespace Blazor.Flows.Interfaces;

/// <summary>
///     Describes a model that contains nodes.
/// </summary>
public interface INodeContainer: IId
{
    /// <summary>
    ///     Nodes that belong to the model.
    /// </summary>
    ObservableList<INode> Nodes { get; set; }
}