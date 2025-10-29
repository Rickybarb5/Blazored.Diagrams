using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

public partial class DiagramService
{
    /// <summary>
    /// Removes a node from the diagram.
    /// </summary>
    /// <param name="nodeToRemove"></param>
    /// <returns></returns>
    public virtual IDiagramService RemoveNode(INode nodeToRemove)
    {
        if (Diagram.Layers.Select(layer => layer.Nodes.RemoveInternal(nodeToRemove)).Any(removed => removed))
        {
            return this;
        }

        Diagram.Layers
            .SelectMany(x => x.AllGroups)
            .ForEach(group => group.Nodes.RemoveInternal(nodeToRemove));

        return this;
    }

    /// <summary>
    /// Removes a group from the diagram.
    /// </summary>
    /// <param name="groupToRemove"></param>
    /// <returns></returns>
    public virtual IDiagramService RemoveGroup(IGroup groupToRemove)
    {
        foreach (var layer in Diagram.Layers)
        {
            var removed = layer.Groups.RemoveInternal(groupToRemove);
            if (removed)
                return this;
        }

        Diagram.Layers
            .SelectMany(x => x.AllGroups)
            .ForEach(group => group.Groups.RemoveInternal(groupToRemove));

        return this;
    }

    /// <summary>
    /// Removes a layer from the diagram.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public virtual IDiagramService RemoveLayer(ILayer layer)
    {
        Diagram.Layers.RemoveInternal(layer);
        return this;
    }

    /// <summary>
    /// Removes a port.
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public IDiagramService RemovePort(IPort port)
    {
        port.Parent.Ports.RemoveInternal(port);

        return this;
    }

    /// <summary>
    /// Removes a link
    /// </summary>
    /// <param name="linkToRemove"></param>
    /// <returns></returns>
    public virtual IDiagramService RemoveLink(ILink linkToRemove)
    {
        linkToRemove.SourcePort.OutgoingLinks.RemoveInternal(linkToRemove);
        linkToRemove.TargetPort?.IncomingLinks.RemoveInternal(linkToRemove);

        return this;
    }
}