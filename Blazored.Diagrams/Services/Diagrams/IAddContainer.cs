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
    IAddContainer AddGroupTo<TGroup>(IGroupContainer parent, TGroup group)
        where TGroup : IGroup;

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="nodeContainer">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <typeparam name="TNode">Node Type</typeparam>
    IAddContainer NodeTo<TNode>(INodeContainer nodeContainer, TNode node)
        where TNode : INode;

    /// <summary>
    /// Creates a port.
    /// </summary>
    /// <param name="parent">Model to which the port will be added to.</param>
    /// <param name="port">Port that will be added.</param>
    /// <typeparam name="TPort">Port Type</typeparam>
    IAddContainer PortTo<TPort>(IPortContainer parent, TPort port)
        where TPort : IPort;

    /// <summary>
    /// Creates a link.
    /// </summary>
    /// <param name="sourcePort">Source port of the link.</param>
    /// <param name="targetPort">Target port of the link.</param>
    /// <param name="link">Link that will be added to the container</param>
    /// <typeparam name="TLink">Link Type</typeparam>
    IAddContainer AddLinkTo<TLink>(ILinkContainer sourcePort, ILinkContainer? targetPort, TLink link)
        where TLink : ILink;

    /// <summary>
    /// Creates a layer.
    /// </summary>
    /// <param name="layer">Layer to be added.</param>
    /// <typeparam name="TLayer">Link Type</typeparam>
    IAddContainer Layer<TLayer>(TLayer layer)
        where TLayer : ILayer;

    /// <summary>
    /// Adds a node to the diagram.
    /// </summary>
    /// <param name="node">Node to be added.</param>
    IAddContainer Node(INode node);

    /// <summary>
    /// Adds a group to the current layer.
    /// </summary>
    /// <param name="group">Group to be added.</param>
    IAddContainer Group(IGroup group);

    /// <summary>
    /// Adds a layer to the diagram.
    /// </summary>
    /// <param name="layer">Layer to be added.</param>
    IAddContainer AddLayer(ILayer layer);
}