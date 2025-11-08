using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Diagrams;

public partial class DiagramService
{
    /// <inheritdoc />
    public IDiagramService AddGroupTo(IGroupContainer parent, IGroup group)
    {
        switch (parent)
        {
            // Add parent group if not in diagram.
            case IGroup groupParent when Diagram.AllGroups.All(n => n.Id != groupParent.Id):
                AddGroup(groupParent);
                break;
            // Add layer if not in diagram.
            case ILayer layerParent when Diagram.Layers.All(n => n.Id != layerParent.Id):
                AddLayer(layerParent);
                break;
        }

        parent.Groups.AddInternal(group);
        return this;
    }


    /// <inheritdoc />
    public IDiagramService AddNodeTo(INodeContainer parent, INode node)
    {
        switch (parent)
        {
            // Add parent group if not in diagram
            case IGroup groupParent when Diagram.AllGroups.All(n => n.Id != groupParent.Id):
                AddGroup(groupParent);
                break;
            // Add parent layer if not in diagram
            case ILayer layerParent when Diagram.Layers.All(n => n.Id != layerParent.Id):
                AddLayer(layerParent);
                break;
        }

        parent.Nodes.AddInternal(node);
        return this;
    }

    /// <inheritdoc />
    public IDiagramService AddPortTo(IPortContainer parent, IPort port)
    {
        switch (parent)
        {
            // Add parent group if not in diagram
            case IGroup groupParent when Diagram.AllGroups.All(n => n.Id != groupParent.Id):
                AddGroup(groupParent);
                break;
            // Add parent node if not in diagram
            case INode nodeParent when Diagram.AllNodes.All(n => n.Id != nodeParent.Id):
                AddNode(nodeParent);
                break;
        }

        parent.Ports.AddInternal(port);
        return this;
    }

    /// <inheritdoc />
    public IDiagramService AddLinkTo(ILinkContainer sourcePort, ILinkContainer? targetPort, ILink link)
    {
        if (sourcePort is IPort sp && Diagram.AllPorts.All(n => n.Id != sp.Id))
        {
            AddPortTo(sp.Parent, sp);
        }

        if (targetPort is IPort tp && Diagram.AllPorts.All(n => n.Id != tp.Id))
        {
            AddPortTo(tp.Parent, tp);
        }

        sourcePort.OutgoingLinks.AddInternal(link);
        targetPort?.IncomingLinks.AddInternal(link);
        return this;
    }

    /// <inheritdoc />
    public virtual IDiagramService AddNode(INode node)
    {
        Diagram.CurrentLayer.Nodes.AddInternal(node);
        return this;
    }

    /// <inheritdoc />
    public virtual IDiagramService AddGroup(IGroup group)
    {
        Diagram.CurrentLayer.Groups.AddInternal(group);
        return this;
    }

    /// <inheritdoc />
    public virtual IDiagramService AddLayer(ILayer layer)
    {
        Diagram.Layers.AddInternal(layer);
        return this;
    }
}