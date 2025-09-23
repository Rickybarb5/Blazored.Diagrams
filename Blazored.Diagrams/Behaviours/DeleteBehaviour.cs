using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Performs cleanup actions when something is deleted from the diagram.
/// </summary>
public class DeleteBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DeleteOptions _options;
    private List<IDisposable> _subscriptions = [];


    private void OnEnabledChanged(bool enabled)
    {
        if (enabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
        }
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
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
        _options = _service.Diagram.Options.Get<DeleteOptions>()!;
        _options.OnEnabledChanged += OnEnabledChanged;
        OnEnabledChanged(_options.IsEnabled);
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

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
        _options.OnEnabledChanged -= OnEnabledChanged;
    }
}