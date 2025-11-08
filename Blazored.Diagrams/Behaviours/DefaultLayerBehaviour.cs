using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Handles the default behaviour of a layer
/// </summary>
public class DefaultLayerBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly DefaultLayerBehaviourOptions _behaviourOptions;

    /// <summary>
    /// Instantiates a new <see cref="DefaultLayerBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultLayerBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<DefaultLayerBehaviourOptions>()!;
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
    
    /// <summary>
    /// Subscribe to mLayer relevant events.
    /// </summary>
    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            _service.Events.SubscribeTo<CurrentLayerChangedEvent>(HandleCurrentLayerChanged),
            _service.Events.SubscribeTo<LayerRemovedEvent>(HandleLayerRemoved),
        ];
    }

    /// <summary>
    /// Handles the <see cref="CurrentLayerChangedEvent"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="InvalidOperationException"></exception>
    protected virtual void HandleCurrentLayerChanged(CurrentLayerChangedEvent obj)
    {
        if (obj.NewLayer is null)
        {
            throw new InvalidOperationException("Current layer cannot be null.");
        }

        if (!_service.Diagram.Layers.Contains(obj.NewLayer))
        {
            _service.Diagram.Layers.AddInternal(obj.NewLayer);
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
        if (_service.Diagram.CurrentLayer.Id == obj.Model.Id)
        {
            _service.Diagram.CurrentLayer = _service.Diagram.Layers[0];
        }
    }
}