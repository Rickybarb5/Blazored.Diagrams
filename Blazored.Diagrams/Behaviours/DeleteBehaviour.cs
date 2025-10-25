using Blazored.Diagrams.Events;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Performs cleanup actions when something is deleted from the diagram.
/// </summary>
public class DeleteBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly DeleteBehaviourOptions _behaviourOptions;
    
    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            _service.Events.SubscribeTo<NodeRemovedEvent>(e => CleanupNodeDependencies(e.Model)),
            _service.Events.SubscribeTo<LayerRemovedEvent>(e => CleanupLayerDependencies(e.Model)),
            _service.Events.SubscribeTo<GroupRemovedEvent>(e => CleanupGroupDependencies(e.Model)),
            _service.Events.SubscribeTo<PortRemovedEvent>(e => CleanupPortDependencies(e.Model)),
            _service.Events.SubscribeTo<LinkRemovedEvent>(e => CleanupLinkDependencies(e.Model)),
        ];
    }

    /// <summary>
    /// Instantiates a new <see cref="DeleteBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DeleteBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<DeleteBehaviourOptions>()!;
        _behaviourOptions.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(_behaviourOptions.IsEnabled);
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

    private void CleanupLayerDependencies(ILayer obj)
    {
        obj.Dispose();
    }

    private void CleanupLinkDependencies(ILink obj)
    {
        obj.Dispose();
    }

    private void CleanupGroupDependencies(IGroup obj)
    {
        obj.Dispose();
    }

    private void CleanupNodeDependencies(INode obj)
    {
        obj.Dispose();
    }

    private void CleanupPortDependencies(IPort port)
    {
        port.Dispose();
    }
}