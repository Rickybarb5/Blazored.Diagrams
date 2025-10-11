using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Behaviours;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public partial class DiagramService : IDiagramService
{
    /// <inheritdoc />
    public IEventAggregator Events { get; set; }

    /// <inheritdoc />
    public IBehaviourContainer Behaviours { get; set; }

    /// <inheritdoc />
    public ISerializationContainer Serialize { get; set; }

    /// <inheritdoc />
    public IAddContainer Add { get; set; }
    
    /// <inheritdoc />
    public IDeleteContainer Remove { get; set; }

    /// <inheritdoc />
    public IDiagram Diagram { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DiagramService()
    {
        InitializeDiagram(new Diagram());
    }

    private void InitializeDiagram(IDiagram diagram)
    {
        Diagram = diagram;
        InitializeServices();
        InitializeOptions();
        InitializeBehaviours();
    }

    private void InitializeServices()
    {
        Behaviours = new BehaviourContainer(this);
        Add = new AddContainer(Diagram);
        Remove = new DeleteContainer(Diagram);
        Events = new EventAggregator(this);
        Serialize = new SerializationContainer(this);
    }

    private void InitializeBehaviours()
    {
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
        Behaviours.RegisterBehaviour(new SelectBehaviour(this));
        Behaviours.RegisterBehaviour(new EventLoggingBehavior(this));
    }
    
    private void InitializeOptions()
    {
        Diagram.Options.BehaviourOptions =
        [

            new DefaultGroupBehaviourOptions(),
            new DefaultLayerBehaviourOptions(),
            new DefaultLinkBehaviourOptions(),
            new DefaultPortBehaviourOptions(),
            new DeleteBehaviourOptions(),
            new DeleteWithKeyBehaviourOptions(),
            new DrawLinkBehaviourOptions(),
            new MoveBehaviourOptions(),
            new PanBehaviourOptions(),
            new RedrawBehaviourOptions(),
            new SelectBehaviourOptions(),
            new ZoomBehaviourOptions(),
            new LoggingBehaviourOptions(),
        ];
    }

    void IDiagramService.SwitchDiagram(IDiagram diagram)
    {
        Dispose();
        InitializeDiagram(diagram);
        Events.Publish(new DiagramRedrawEvent(Diagram));
       
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Behaviours.Dispose();
        Events.Dispose();
    }
}

