using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Default behaviour of all groups
/// </summary>
public class DefaultGroupBehaviour : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DefaultGroupOptions _options;
    private List<IDisposable> _subscriptions = [];

    /// <summary>
    /// Instantiates a new <see cref="DefaultGroupBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultGroupBehaviour(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<DefaultGroupOptions>()!;
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
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
        [
            _service.Events.SubscribeTo<GroupAddedToGroupEvent>(HandleGroupAddedToGroupEvent),
            _service.Events.SubscribeTo<GroupPositionChangedEvent>(OnGroupPositionChanged),
        ];
    }


    private void HandleGroupAddedToGroupEvent(GroupAddedToGroupEvent obj)
    {
        // A group cannot be added to itself
        if (obj.ParentModel.Id == obj.AddedGroup.Id && obj.ParentModel.AllGroups.Any(g => g.Id == obj.AddedGroup.Id))
            throw new InvalidOperationException("Cannot add group to itself.");
    }

    private void OnGroupPositionChanged(GroupPositionChangedEvent obj)
    {
        // Move child groups
        var xDiff = obj.Model.PositionX - obj.OldX;
        var yDiff = obj.Model.PositionY - obj.OldY;
        obj.Model.AllGroups.ForEach(g => { g.SetPositionInternal(g.PositionX + xDiff, g.PositionY + yDiff); });
        obj.Model.AllNodes.ForEach(n => { n.SetPositionInternal(n.PositionX + xDiff, n.PositionY + yDiff); });
        obj.Model.AllPorts.ForEach(p => { p.SetPosition(p.PositionX + xDiff, p.PositionY + yDiff); });
    }

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
    }
}