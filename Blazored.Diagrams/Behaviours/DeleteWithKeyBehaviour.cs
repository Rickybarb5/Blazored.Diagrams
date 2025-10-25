using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;
using Microsoft.AspNetCore.Components.Web;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Behaviour that removes objects from the diagram if they are selected and the delete key is pressed.
/// </summary>
public class DeleteWithKeyBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private readonly DeleteWithKeyBehaviourOptions _behaviourOptions;


    /// <summary>
    /// Instantiates a new <see cref="DeleteWithKeyBehaviour"/>.
    /// </summary>
    /// <param name="service"></param>
    public DeleteWithKeyBehaviour(IDiagramService service)
    {
        _service = service;
        _behaviourOptions = _service.Behaviours.GetBehaviourOptions<DeleteWithKeyBehaviourOptions>()!;
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
    private void SubscribeToEvents()
    {
        Subscriptions = [_service.Events.SubscribeTo<DiagramKeyDownEvent>(e => OnKeyDown(e.Args))];
    }

    private void OnKeyDown(KeyboardEventArgs obj)
    {
        if (obj.Code != _behaviourOptions.DeleteKeyCode ||
            !_behaviourOptions.IsEnabled) return;

        _service.Diagram.AllGroups.Where(x => x.IsSelected).ForEach(group =>_service.Remove.Group(group));
        _service.Diagram.AllLinks.Where(x => x.IsSelected).ForEach(link =>_service.Remove.Link(link));
        _service.Diagram.AllNodes.Where(x => x.IsSelected).ForEach(node =>_service.Remove.Node(node));
    }
}