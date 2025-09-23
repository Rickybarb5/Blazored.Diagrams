using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services;

/// <summary>
///     Diagram service - model creation
/// </summary>
public partial class DiagramService
{
    /// <summary>
    /// Creates a group in another group.
    /// </summary>
    /// <param name="parent">Container to which the new group will be added to.</param>
    /// <param name="group">Group to be added.</param>
    /// <typeparam name="TGroup">Group Type</typeparam>
    /// <returns></returns>
    public void AddGroupTo<TGroup>(IGroupContainer parent, TGroup group)
        where TGroup : IGroup
    {
        parent.Groups.Add(group);
    }

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="parentGroup">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <typeparam name="TNode">Node Type</typeparam>
    /// <returns></returns>
    public void AddNodeTo<TNode>(INodeContainer parentGroup, TNode node)
        where TNode : INode
    {
        parentGroup.Nodes.Add(node);
    }

    /// <summary>
    /// Creates a port.
    /// </summary>
    /// <param name="parent">Model to which the port will be added to.</param>
    /// <param name="port">Port that will be added.</param>
    /// <typeparam name="TPort">Port Type</typeparam>
    /// <returns></returns> 
    public void AddPortTo<TPort>(IPortContainer parent, TPort port)
        where TPort : IPort
    {
        parent.Ports.Add(port);
    }

    /// <summary>
    /// Creates a link.
    /// </summary>
    /// <param name="sourcePort">Source port of the link.</param>
    /// <param name="targetPort">Target port of the link.</param>
    /// <param name="link">Link that will be added to the container</param>
    /// <typeparam name="TLink">Link Type</typeparam>
    /// <returns></returns>
    public void AddLinkTo<TLink>(ILinkContainer sourcePort, ILinkContainer? targetPort, TLink link)
        where TLink : ILink
    {
        sourcePort.OutgoingLinks.Add(link);
        targetPort?.IncomingLinks.Add(link);
    }

    /// <summary>
    /// Creates a layer.
    /// </summary>
    /// <param name="layer">Layer to be added.</param>
    /// <typeparam name="TLayer">Link Type</typeparam>
    /// <returns></returns>
    public void AddLayer<TLayer>(TLayer layer)
        where TLayer : ILayer
    {
        Diagram.Layers.Add(layer);
    }

    /// <inheritdoc />
    public virtual void AddNode(INode node)
    {
        Diagram.CurrentLayer.Nodes.Add(node);
    }

    /// <inheritdoc />
    public virtual void AddGroup(IGroup group)
    {
        Diagram.CurrentLayer.Groups.Add(group);
    }

    /// <inheritdoc />
    public virtual void AddLayer(ILayer layer)
    {
        Diagram.Layers.Add(layer);
    }
}