using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public class DeleteContainer : IDeleteContainer
{
    private IDiagram Diagram;
    public DeleteContainer(IDiagram diagram)
    {
        Diagram = diagram;
    }
    /// <inheritdoc />
    public virtual bool Node(INode nodeToRemove)
    {
        if (Diagram.Layers.Select(layer => layer.Nodes.Remove(nodeToRemove)).Any(removed => removed))
        {
            return true;
        }

        return Diagram.Layers
            .SelectMany(x => x.AllGroups)
            .Select(group => group.Nodes.Remove(nodeToRemove))
            .Any(removed => removed);
    }

    /// <inheritdoc />
    public virtual bool Group(IGroup groupToRemove)
    {
        foreach (var layer in Diagram.Layers)
        {
            var removed = layer.Groups.Remove(groupToRemove);
            if (removed) return true;
        }

        return Diagram.Layers
            .SelectMany(x => x.AllGroups)
            .Select(group => group.Groups.Remove(groupToRemove))
            .Any(removed => removed);
    }

    /// <inheritdoc />
    public virtual bool Layer(ILayer layer)
    {
        return Diagram.Layers.Remove(layer);
    }

    /// <inheritdoc />
    public bool Remove(IPort port)
    {
        var success = Diagram.Layers
            .SelectMany(x => x.AllNodes)
            .Select(node => node.Ports.Remove(port))
            .Any(removed => removed);
        if (!success)
        {
            success = Diagram.Layers
                .SelectMany(x => x.AllGroups)
                .Select(group => group.Ports.Remove(port))
                .Any(removed => removed);
        }

        return success;
    }

    /// <inheritdoc />
    public virtual bool Link(ILink linkToRemove)
    {
        return linkToRemove.SourcePort.OutgoingLinks.Remove(linkToRemove) || linkToRemove.TargetPort is null ||
               linkToRemove.TargetPort.IncomingLinks.Remove(linkToRemove);
    }
}