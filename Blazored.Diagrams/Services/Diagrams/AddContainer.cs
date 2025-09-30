using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public class AddContainer : IAddContainer
{
    private IDiagram _diagram;
    public AddContainer(IDiagram diagram)
    {
        _diagram = diagram;
    }
    /// <summary>
    /// Creates a group in another group.
    /// </summary>
    /// <param name="parent">Container to which the new group will be added to.</param>
    /// <param name="group">Group to be added.</param>
    /// <typeparam name="TGroup">Group Type</typeparam>
    /// <returns></returns>
    public IAddContainer AddGroupTo<TGroup>(IGroupContainer parent, TGroup group)
        where TGroup : IGroup
    {
        parent.Groups.Add(group);
        return this;
    }

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="parentGroup">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <typeparam name="TNode">Node Type</typeparam>
    /// <returns></returns>
    public IAddContainer NodeTo<TNode>(INodeContainer parentGroup, TNode node)
        where TNode : INode
    {
        parentGroup.Nodes.Add(node);
        return this;
    }

    /// <summary>
    /// Creates a port.
    /// </summary>
    /// <param name="parent">Model to which the port will be added to.</param>
    /// <param name="port">Port that will be added.</param>
    /// <typeparam name="TPort">Port Type</typeparam>
    /// <returns></returns> 
    public IAddContainer PortTo<TPort>(IPortContainer parent, TPort port)
        where TPort : IPort
    {
        parent.Ports.Add(port);
        return this;
    }

    /// <summary>
    /// Creates a link.
    /// </summary>
    /// <param name="sourcePort">Source port of the link.</param>
    /// <param name="targetPort">Target port of the link.</param>
    /// <param name="link">Link that will be added to the container</param>
    /// <typeparam name="TLink">Link Type</typeparam>
    /// <returns></returns>
    public IAddContainer AddLinkTo<TLink>(ILinkContainer sourcePort, ILinkContainer? targetPort, TLink link)
        where TLink : ILink
    {
        sourcePort.OutgoingLinks.Add(link);
        targetPort?.IncomingLinks.Add(link);
        return this;
    }

    /// <summary>
    /// Creates a layer.
    /// </summary>
    /// <param name="layer">Layer to be added.</param>
    /// <typeparam name="TLayer">Link Type</typeparam>
    /// <returns></returns>
    public IAddContainer Layer<TLayer>(TLayer layer)
        where TLayer : ILayer
    {
        _diagram.Layers.Add(layer);
        return this;
    }

    /// <inheritdoc />
    public virtual IAddContainer Node(INode node)
    {
        _diagram.CurrentLayer.Nodes.Add(node);
        return this;
    }

    /// <inheritdoc />
    public virtual IAddContainer Group(IGroup group)
    {
        _diagram.CurrentLayer.Groups.Add(group);
        return this;
    }

    /// <inheritdoc />
    public virtual IAddContainer AddLayer(ILayer layer)
    {
        _diagram.Layers.Add(layer);
        return this;
    }
}