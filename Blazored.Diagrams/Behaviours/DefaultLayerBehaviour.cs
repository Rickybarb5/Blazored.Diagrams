using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Handles the default behaviour of a layer:
/// <see cref="HandleLayerAdded"/>
/// <see cref="HandleLayerSwitch"/>
/// <see cref="HandleLayerAdded"/>
/// </summary>
public class DefaultLayerBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DefaultLayerOptions _options;
    private List<IDisposable> _subscriptions;

    /// <summary>
    /// Instantiates a new <see cref="DefaultLayerBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultLayerBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<DefaultLayerOptions>()!;
        _options.OnEnabledChanged += OnEnabledChanged;
        OnEnabledChanged(_options.IsEnabled);
    }

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

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
    }

    /// <summary>
    /// Disposes of all subscriptions.
    /// </summary>
    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    /// <summary>
    /// Subscribe to mLayer relevant events.
    /// </summary>
    private void SubscribeToEvents()
    {
        _subscriptions =
        [
            _service.Events.SubscribeTo<LayerRemovedEvent>(HandleLayerRemoved),
            _service.Events.SubscribeTo<LayerAddedEvent>(HandleLayerAdded),
            _service.Events.SubscribeTo<IsCurrentLayerChangedEvent>(HandleLayerStateChange),
            _service.Events.SubscribeTo<LayerSwitchEvent>(HandleLayerSwitch),
        ];
    }

    /// <summary>
    /// Handles the <see cref="LayerAddedEvent"/>
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void HandleLayerAdded(LayerAddedEvent obj)
    {
        // If this is the first layer being added, it's the active layer.
        if (_service.Diagram.Layers.Count == 1)
        {
            _service.Diagram.Layers.First().IsCurrentLayer = true;
        }
        else
        {
            // If a new layer is created with IsCurrentLayer = true, we have to trigger the event manually. 
            if (obj.Model.IsCurrentLayer)
            {
                _service.Events.Publish(new IsCurrentLayerChangedEvent(obj.Model));
            }
        }
    }

    /// <summary>
    /// Handles the <see cref="LayerSwitchEvent"/>.
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void HandleLayerSwitch(LayerSwitchEvent obj)
    {
        // Use the new layer.
        obj.NewCurrentLayer.IsCurrentLayer = true;
        _service.Diagram.Layers
            .Where(x => x.Id != obj.NewCurrentLayer.Id)
            .ForEach(x =>
            {
                    x.IsCurrentLayer = false;
            });
    }

    /// <summary>
    /// Handles the <see cref="IsCurrentLayerChangedEvent"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="InvalidOperationException"></exception>
    protected virtual void HandleLayerStateChange(IsCurrentLayerChangedEvent obj)
    {
        if (obj.Model.IsCurrentLayer)
        {
            _service.Diagram.Layers
                .Where(x => x.Id != obj.Model.Id)
                .ForEach(x => { x.IsCurrentLayer = false; });
        }
        else
        {
            if (_service.Diagram.Layers.Count(x => x.IsCurrentLayer) != 1)
            {
                throw new InvalidOperationException(
                    $"There must be always be only one layer with {nameof(ILayer.IsCurrentLayer)} = true at all times.");
            }
        }
    }

    /// <summary>
    /// Handles the <see cref="LayerRemovedEvent"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="InvalidOperationException"></exception>
    protected virtual void HandleLayerRemoved(LayerRemovedEvent obj)
    {
        if (_service.Diagram.Layers.Count == 0)
        {
            throw new InvalidOperationException("Cannot remove the last layer.");
        }

        // If the current layer is removed the first one will turn into the current one.
        if (!_service.Diagram.Layers.Any(x => x.IsCurrentLayer))
        {
            _service.Diagram.UseLayer(_service.Diagram.Layers.First());
        }
    }
}