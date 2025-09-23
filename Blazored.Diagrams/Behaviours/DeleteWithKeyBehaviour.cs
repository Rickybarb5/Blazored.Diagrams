using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;
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

        var selectedGroups = _service.Diagram.AllGroups.Where(x => x.IsSelected);
        var selectedLinks = _service.Diagram.AllLinks.Where(x => x.IsSelected);
        var selectedNodes = _service.Diagram.AllNodes.Where(x => x.IsSelected);


        foreach (var link in selectedLinks)
        {
            _service.Remove(link);
        }

        foreach (var group in selectedGroups)
        {
            _service.Remove(group);
        }

        foreach (var node in selectedNodes)
        {
            _service.Remove(node);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
        _options.OnEnabledChanged -= OnEnabledChanged;
    }
}