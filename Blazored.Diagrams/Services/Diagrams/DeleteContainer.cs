using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public class DeleteContainer : IDeleteContainer
{
    private readonly IDiagramService service;
    
    /// <summary>
    /// Instantiates a new <see cref="DeleteContainer"/> 
    /// </summary>
    /// <param name="service"></param>
    public DeleteContainer(IDiagramService service)
    {
        this.service = service;
    }
    /// <inheritdoc />
    public virtual IDeleteContainer Node(INode nodeToRemove)
    {
        if (service.Diagram.Layers.Select(layer => layer.Nodes.Remove(nodeToRemove)).Any(removed => removed))
        {
            return this;
        }

        service.Diagram.Layers
            .SelectMany(x => x.AllGroups)
            .ForEach(group => group.Nodes.Remove(nodeToRemove));

        return this;
    }

    /// <inheritdoc />
    public virtual IDeleteContainer Group(IGroup groupToRemove)
    {
        foreach (var layer in service.Diagram.Layers)
        {
            var removed = layer.Groups.Remove(groupToRemove);
            if (removed) 
                return this;
        }

        service.Diagram.Layers
            .SelectMany(x => x.AllGroups)
            .ForEach(group => group.Groups.Remove(groupToRemove));
        
        return this;
    }

    /// <inheritdoc />
    public virtual IDeleteContainer Layer(ILayer layer)
    {
        service.Diagram.Layers.Remove(layer);
        return this;
    }

    /// <inheritdoc />
    public IDeleteContainer Remove(IPort port)
    {
        service.Diagram.Layers
            .SelectMany(x => x.AllNodes)
            .ForEach(node => node.Ports.Remove(port));

        return this;
    }

    /// <inheritdoc />
    public virtual IDeleteContainer Link(ILink linkToRemove)
    {
        linkToRemove.SourcePort.OutgoingLinks.Remove(linkToRemove);
        linkToRemove.TargetPort?.IncomingLinks.Remove(linkToRemove);
        
        return this;
    }
}