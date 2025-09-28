using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

/// <summary>
/// Manages model deletion from the diagram.
/// </summary>
public interface IDeleteContainer
{
    /// <summary>
    /// Removes a node from the diagram.
    /// </summary>
    /// <param name="nodeToRemove"></param>
    /// <returns></returns>
    bool Node(INode nodeToRemove);

    /// <summary>
    /// Removes a group from the diagram.
    /// </summary>
    /// <param name="groupToRemove"></param>
    /// <returns></returns>
    bool Group(IGroup groupToRemove);

    /// <summary>
    /// Removes a layer from the diagram.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    bool Layer(ILayer layer);

    /// <summary>
    /// Removes a port.
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    bool Remove(IPort port);

    /// <summary>
    /// Removes a link
    /// </summary>
    /// <param name="linkToRemove"></param>
    /// <returns></returns>
    bool Link(ILink linkToRemove);
}