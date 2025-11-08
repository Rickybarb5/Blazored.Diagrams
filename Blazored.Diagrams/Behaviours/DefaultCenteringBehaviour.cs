using Blazored.Diagrams.Events;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// This behaviour centers a model in the viewport if it's position is 0,0.
/// It also centers models within other models if the position is 0,0
/// </summary>
public class DefaultCenteringBehaviour : BaseBehaviour
{
    private readonly IDiagramService diagramService;

    /// <summary>
    /// Instantieates a new <see cref="DefaultCenteringBehaviour"/>
    /// </summary>
    /// <param name="service"></param>
    public DefaultCenteringBehaviour(IDiagramService service)
    {
        diagramService = service;
        var options = service.Behaviours.GetBehaviourOptions<DefaultCenteringBehaviourOptions>();
        options.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(options.IsEnabled);
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
            diagramService.Events.SubscribeTo<NodeAddedEvent>(Handle),
            diagramService.Events.SubscribeTo<GroupAddedEvent>(Handle),
            diagramService.Events.SubscribeTo<NodeAddedToGroupEvent>(Handle),
            diagramService.Events.SubscribeTo<GroupAddedToGroupEvent>(Handle),
        ];
    }

    private void Handle(GroupAddedToGroupEvent obj)
    {
        CenterInModel(obj.AddedGroup, obj.ParentModel);
    }

    private void Handle(NodeAddedToGroupEvent obj)
    {
        CenterInModel(obj.Node, obj.Model);
    }

    private void Handle(GroupAddedEvent obj)
    {
        CenterInViewPort(obj.Model);
    }

    private void Handle(NodeAddedEvent obj)
    {
        CenterInViewPort(obj.Model);
    }

    private void CenterInViewPort<T>(T model)
        where T: ISize, IPosition
    {
        if (model.PositionX == 0 && model.PositionY == 0)
        {
            diagramService.CenterInViewport(new CenterInViewportParameters<T>(model));
        }
    }

    private void CenterInModel<TAdded, TParent>(TAdded added, TParent parent)
        where TAdded : ISize, IPosition
        where TParent : ISize, IPosition
    {
        if (added.PositionX == 0 && added.PositionY == 0)
        {
            var padding = parent is IPadding pad ? pad.Padding : 0; 
            diagramService.CenterIn(new CenterInParameters<TAdded, TParent>(added, parent));
        }
    }
    
}