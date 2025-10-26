using Blazored.Diagrams.Extensions;
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
    private readonly IDiagramService service;
    /// <summary>
    /// Instantiates a new <see cref="AddContainer"/>.
    /// </summary>
    /// <param name="service"></param>
    public AddContainer(IDiagramService service)
    {
        this.service = service;
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
        parent.Groups.AddInternal(group);
        if (group.PositionX == 0 && group.PositionY == 0 && parent is IPosition p and ISize s)
        {
            var padding = parent is IPadding pad ? pad.Padding : 0; 
            group.CenterIn(s.Width, s.Height, p.PositionX, p.PositionY, padding);
        }
        return this;
    }

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="nodeContainer">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <typeparam name="TNode">Node Type</typeparam>
    /// <returns></returns>
    public IAddContainer NodeTo<TNode>(INodeContainer nodeContainer, TNode node)
        where TNode : INode
    {
        nodeContainer.Nodes.AddInternal(node);
        if (node.PositionX == 0 && node.PositionY == 0 && nodeContainer is IPosition p and ISize s)
        {
            var padding = nodeContainer is IPadding pad ? pad.Padding : 0; 
            node.CenterIn(s.Width, s.Height, p.PositionX, p.PositionY, padding);
        }
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
        parent.Ports.AddInternal(port);
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
        sourcePort.OutgoingLinks.AddInternal(link);
        targetPort?.IncomingLinks.AddInternal(link);
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
        service.Diagram.Layers.AddInternal(layer);
        return this;
    }

    /// <inheritdoc />
    public virtual IAddContainer Node(INode node)
    {
        service.Diagram.CurrentLayer.Nodes.AddInternal(node);
        if (node is { PositionX: 0, PositionY: 0 })
        {
            node.CenterIn(service.Diagram);
        }
        return this;
    }

    /// <inheritdoc />
    public virtual IAddContainer Group(IGroup group)
    {
        service.Diagram.CurrentLayer.Groups.AddInternal(group);
        if (group is { PositionX: 0, PositionY: 0 })
        {
            group.CenterIn(service.Diagram);
        }
        return this;
    }

    /// <inheritdoc />
    public virtual IAddContainer AddLayer(ILayer layer)
    {
        service.Diagram.Layers.AddInternal(layer);
        return this;
    }
}