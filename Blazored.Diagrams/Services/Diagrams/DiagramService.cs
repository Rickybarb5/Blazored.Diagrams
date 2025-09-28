using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Behaviours;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Providers;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public partial class DiagramService : IDiagramService
{
    /// <summary>
    /// Propagates model events to the event aggregator.
    /// </summary>
    protected IEventPropagator? EventPropagator;

    /// <inheritdoc />
    public IEventAggregator Events { get; init; }

    /// <inheritdoc />
    public IBehaviourContainer Behaviours { get; init; }
    
    /// <inheritdoc />
    public IAddContainer Add { get; init; }
    
    /// <inheritdoc />
    public IDeleteContainer Remove { get; init; }

    /// <inheritdoc />
    public IDiagram Diagram { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="diagram"></param>
    /// <param name="diagramServiceProvider"></param>
    public DiagramService(IDiagram diagram, IDiagramServiceProvider diagramServiceProvider)
    {
        Diagram = diagram;
        Behaviours = new BehaviourContainer(this);
        Add = new AddContainer(Diagram);
        Remove = new DeleteContainer(Diagram);
        Events = diagramServiceProvider.GetDiagramEventAggregator(diagram);
        EventPropagator = new EventPropagator(this);
        
        //Register behaviors
        Behaviours.RegisterBehaviour(new DefaultGroupBehaviour(this));
        Behaviours.RegisterBehaviour(new DefaultLayerBehaviour(this));
        Behaviours.RegisterBehaviour(new DefaultLinkBehaviour(this));
        Behaviours.RegisterBehaviour(new DefaultPortBehaviour(this));
        Behaviours.RegisterBehaviour(new DeleteBehaviour(this));
        Behaviours.RegisterBehaviour(new DeleteWithKeyBehaviour(this));
        Behaviours.RegisterBehaviour(new DrawLinkBehavior(this));
        Behaviours.RegisterBehaviour(new PanBehaviour(this));
        Behaviours.RegisterBehaviour(new RedrawBehaviour(this));
        Behaviours.RegisterBehaviour(new MoveBehaviour(this));
        Behaviours.RegisterBehaviour(new ZoomBehavior(this));
        Behaviours.RegisterBehaviour(new EventLoggingBehavior(this));
        Behaviours.RegisterBehaviour(new SelectBehaviour(this));
        
    }
    /// <inheritdoc />
    public void Dispose()
    {
        EventPropagator?.Dispose();
        Events.Dispose();
    }
}

