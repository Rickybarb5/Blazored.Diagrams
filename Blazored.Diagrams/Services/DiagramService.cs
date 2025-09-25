using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Providers;

namespace Blazored.Diagrams.Services;

public partial class DiagramService : IDiagramService
{
    private readonly IEventAggregator _events;
    private readonly IDiagram _currentDiagram;
    public IEventAggregator Events => _events;
    private IEventPropagator? _eventPropagator;

    /// <inheritdoc />
    public IDiagram Diagram => _currentDiagram;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="diagram"></param>
    /// <param name="diagramServiceProvider"></param>
    public DiagramService(IDiagram diagram, IDiagramServiceProvider diagramServiceProvider)
    {
        _currentDiagram = diagram;
        AddDefaultBehaviours();
        _eventPropagator = diagramServiceProvider.GetDiagramEventPropagator(diagram);
        _events = diagramServiceProvider.GetDiagramEventAggregator(diagram);
        _events.Publish(new DiagramRedrawEvent(Diagram));
    }
    
    private void AddDefaultBehaviours()
    {
        AddBehaviour(new DefaultGroupBehaviour(this), new DefaultGroupOptions());
        AddBehaviour(new DefaultLayerBehaviour(this), new DefaultLayerOptions());
        AddBehaviour(new DefaultLinkBehaviour(this), new DefaultLinkOptions());
        AddBehaviour(new DefaultPortBehaviour(this), new DefaultPortOptions());
        AddBehaviour(new DeleteBehaviour(this), new DeleteOptions());
        AddBehaviour(new DeleteWithKeyBehaviour(this), new DeleteWithKeyOptions());
        AddBehaviour(new DrawLinkBehavior(this), new DrawLinkOptions());
        AddBehaviour(new MoveBehaviour(this), new MoveOptions());
        AddBehaviour(new PanBehaviour(this), new PanOptions());
        AddBehaviour(new RedrawBehaviour(this), new RedrawOptions());
        AddBehaviour(new SelectBehaviour(this), new SelectOptions());
        AddBehaviour(new ZoomBehavior(this), new ZoomOptions());
        AddBehaviour(new EventLoggingBehavior(this), new LoggingOptions());
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _eventPropagator?.Dispose();
        _events.Dispose();
        foreach (var behaviour in Behaviours)
        {
            behaviour.Dispose();
        }
    }
}