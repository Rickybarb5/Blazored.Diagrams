using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Behaviour that removes objects from the diagram if they are selected and the delete key is pressed.
/// </summary>
public class DeleteWithKeyBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DeleteWithKeyOptions _options;
    private IDisposable _eventSubscription;


    /// <summary>
    /// Instantiates a new <see cref="DeleteWithKeyBehaviour"/>.
    /// </summary>
    /// <param name="service"></param>
    public DeleteWithKeyBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<DeleteWithKeyOptions>()!;
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

    private void DisposeSubscriptions()
    {
        _eventSubscription.Dispose();
    }

    private void SubscribeToEvents()
    {
        _eventSubscription = _service.Events.SubscribeTo<DiagramKeyDownEvent>(e => OnKeyDown(e.Args));
    }

    private void OnKeyDown(KeyboardEventArgs obj)
    {
        if (obj.Code != _options.DeleteKeyCode ||
            !_options.IsEnabled) return;

        _service.Diagram.AllGroups.Where(x => x.IsSelected).ForEach(group =>_service.Remove(group));
        _service.Diagram.AllLinks.Where(x => x.IsSelected).ForEach(link =>_service.Remove(link));
        _service.Diagram.AllNodes.Where(x => x.IsSelected).ForEach(node =>_service.Remove(node));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
        _options.OnEnabledChanged -= OnEnabledChanged;
    }
}