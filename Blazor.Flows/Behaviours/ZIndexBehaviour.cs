using Blazor.Flows.Events;
using Blazor.Flows.Groups;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Services.Diagrams;

namespace Blazor.Flows.Behaviours;

/// <summary>
/// Behaviour that handles the Z-index of items based on their parent.
/// </summary>
public class ZIndexBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly ZIndexBehaviourOptions _options;

    /// <summary>
    /// Instantiates a new <see cref="ZIndexBehaviour"/>.
    /// </summary>
    /// <param name="service"><see cref="IDiagramService"/>.</param>
    public ZIndexBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Behaviours.GetBehaviourOptions<ZIndexBehaviourOptions>();
        _options.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(_options.IsEnabled);
    }
    
    private void OnEnabledChanged(BehaviourEnabledEvent ev)
    {
        OnEnabledChanged(ev.IsEnabled);
    }

    private void OnEnabledChanged(bool isEnabled)
    {
        if (isEnabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
        }
    }
    
    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            _service.Events.SubscribeTo<NodeAddedToLayerEvent>(HandleZindex),
            _service.Events.SubscribeTo<GroupAddedToLayerEvent>(HandleZindex),

            _service.Events.SubscribeTo<GroupAddedToGroupEvent>(HandleZindex),
            _service.Events.SubscribeTo<NodeAddedToGroupEvent>(HandleZindex),
            _service.Events.SubscribeTo<PortAddedToGroupEvent>(HandleZindex),

            _service.Events.SubscribeTo<PortAddedToNodeEvent>(HandleZindex),
            _service.Events.SubscribeTo<OutgoingLinkAddedEvent>(HandleZindex),
            _service.Events.SubscribeTo<IncomingLinkAddedEvent>(HandleZindex),
            
            
        ];
    }

    // Helper function to calculate the existing nesting level of a parent element from its ZIndex
    private int GetParentNestingLevel(IDepth model)
    {
        return model switch
        {
            IGroup group => group.ZIndex / _options.NestingMultiplier,
            INode node => (node.ZIndex - _options.NodeOffset) / _options.NestingMultiplier,
            _ => 0
        };
    }

    private void HandleZindex(GroupAddedToLayerEvent e)
    {
        var group = e.AddedGroup;
        if (group.ZIndex == 0)
        {
            group.ZIndex = e.Model.ZIndex * _options.NestingMultiplier + _options.GroupOffset;
        }

        _service.Events.Publish(new LayerRedrawEvent(e.Model));
    }

    private void HandleZindex(NodeAddedToLayerEvent e)
    {
        var node = e.Node;
        if (node.ZIndex == 0)
        {
            node.ZIndex = e.Model.ZIndex * _options.NestingMultiplier + _options.NodeOffset;
        }
    }

    private void HandleZindex(GroupAddedToGroupEvent e)
    {
        var parentGroup = e.ParentModel;
        var newGroup = e.AddedGroup;

        if (newGroup.ZIndex == 0)
        {
            var parentNestingLevel = GetParentNestingLevel(parentGroup);
            var newNestingLevel = parentNestingLevel + 1;

            newGroup.ZIndex = newNestingLevel * _options.NestingMultiplier + _options.GroupOffset;
        }
    }

    private void HandleZindex(NodeAddedToGroupEvent e)
    {
        var parentGroup = e.Model;
        var newNode = e.Node;

        if (newNode.ZIndex == 0)
        {
            var parentNestingLevel = GetParentNestingLevel(parentGroup);
            var newNestingLevel = parentNestingLevel + 1;

            newNode.ZIndex = newNestingLevel * _options.NestingMultiplier + _options.NodeOffset;
        }
    }

    private void HandleZindex(PortAddedToGroupEvent e)
    {
        var parentGroup = e.Model;
        var newPort = e.Port;

        if (newPort.ZIndex == 0)
        {
            // Port inside Group: New Nesting Level = Parent Nesting Level + 1
            var parentNestingLevel = GetParentNestingLevel(parentGroup);
            var newNestingLevel = parentNestingLevel + 1;

            newPort.ZIndex = newNestingLevel * _options.NestingMultiplier + _options.PortOffset;
        }
    }

    private void HandleZindex(PortAddedToNodeEvent e)
    {
        var parentNode = e.Node;
        var newPort = e.Port;

        if (newPort.ZIndex == 0)
        {
            // Port inside Node: Nesting Level = Parent Node's Nesting Level
            // Ports share the same base ZIndex level as their parent node.
            var nodeNestingLevel = GetParentNestingLevel(parentNode);

            newPort.ZIndex = nodeNestingLevel * _options.NestingMultiplier + _options.PortOffset;
        }
    }

    private void HandleZindex(IncomingLinkAddedEvent obj)
    {
        HandleLinkZindex(obj.AddedLink);
    }

    private void HandleZindex(OutgoingLinkAddedEvent obj)
    {
        HandleLinkZindex(obj.AddedLink);
    }

    private void HandleLinkZindex(ILink link)
    {
        var sourcePort = link.SourcePort;
        var sourceZIndex = sourcePort.ZIndex;

        // Use the target port's ZIndex if available. 
        // if Target is null (e.g., drawing a new link) -> highest ZIndex
        var targetZIndex = link.TargetPort?.ZIndex ?? 99999;
        Console.WriteLine($"Target:{targetZIndex}");

        var maxPortZIndex = Math.Max(sourceZIndex, targetZIndex);

        link.ZIndex = maxPortZIndex - 1;
    }
}