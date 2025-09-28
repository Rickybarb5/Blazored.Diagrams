using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

/// <summary>
/// Manages adding models to the diagram.
/// </summary>
public interface IAddContainer
{
    /// <summary>
    /// Creates a group in another group.
    /// </summary>
    /// <param name="parent">Container to which the new group will be added to.</param>
    /// <param name="group">Group to be added.</param>
    /// <typeparam name="TGroup">Group Type</typeparam>
    /// <returns></returns>
    void AddGroupTo<TGroup>(IGroupContainer parent, TGroup group)
        where TGroup : IGroup;

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="parentGroup">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <typeparam name="TNode">Node Type</typeparam>
    /// <returns></returns>
    void AddNodeTo<TNode>(INodeContainer parentGroup, TNode node)
        where TNode : INode;

    /// <summary>
    /// Creates a port.
    /// </summary>
    /// <param name="parent">Model to which the port will be added to.</param>
    /// <param name="port">Port that will be added.</param>
    /// <typeparam name="TPort">Port Type</typeparam>
    /// <returns></returns> 
    void AddPortTo<TPort>(IPortContainer parent, TPort port)
        where TPort : IPort;

    /// <summary>
    /// Creates a link.
    /// </summary>
    /// <param name="sourcePort">Source port of the link.</param>
    /// <param name="targetPort">Target port of the link.</param>
    /// <param name="link">Link that will be added to the container</param>
    /// <typeparam name="TLink">Link Type</typeparam>
    /// <returns></returns>
    void AddLinkTo<TLink>(ILinkContainer sourcePort, ILinkContainer? targetPort, TLink link)
        where TLink : ILink;

    /// <summary>
    /// Creates a layer.
    /// </summary>
    /// <param name="layer">Layer to be added.</param>
    /// <typeparam name="TLayer">Link Type</typeparam>
    /// <returns></returns>
    void Layer<TLayer>(TLayer layer)
        where TLayer : ILayer;

    /// <inheritdoc />
    void Node(INode node);

    /// <inheritdoc />
    void Group(IGroup group);
}