using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Behaviours;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public partial class DiagramService : IDiagramService
{
    /// <inheritdoc />
    public IEventAggregator Events { get; set; } = null!;

    /// <inheritdoc />
    public IBehaviourContainer Behaviours { get; set; } = null!;

    /// <inheritdoc />
    public ISerializationContainer Storage { get; set; } = null!;
    
    /// <inheritdoc />
    public IOptionsContainer Options { get; set; } = null!;

    /// <inheritdoc />
    public IAddContainer Add { get; set; } = null!;

    /// <inheritdoc />
    public IDeleteContainer Remove { get; set; } = null!;

    /// <inheritdoc />
    public IDiagram Diagram { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of <see cref="DiagramService"/>.
    /// </summary>
    public DiagramService()
    {
        Diagram = new Diagram();
        Behaviours = new BehaviourContainer(this);
        Options = new OptionsContainer(this);
        Add = new AddContainer(this);
        Remove = new DeleteContainer(this);
        Events = new EventAggregator(this);
        Storage = new SerializationContainer(this);
        InitializeOptions();
        InitializeBehaviours();
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
        Behaviours.RegisterBehaviour(new DefaultCenteringBehaviour(this));
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
            new DefaultCenteringBehaviourOptions(),
        ];
    }

    void IDiagramService.UseDiagram(IDiagram diagram)
    {
        Behaviours.Dispose();
        Diagram.Dispose();

        var oldDiagram = Diagram;
        Diagram = diagram;
        // We don't reset the entire event service
        // This way, custom behaviours keep working!
        //TODO: Maybe create another function with the rewire. This whole thing should reset the diagram.
        ((EventAggregator)Events).RewireSubscriptions();
        Behaviours = new BehaviourContainer(this);
        Options = new OptionsContainer(this);
        Add = new AddContainer(this);
        Remove = new DeleteContainer(this);
        Storage = new SerializationContainer(this);
        InitializeOptions();
        InitializeBehaviours();
        Events.Publish(new DiagramSwitchEvent(oldDiagram, diagram));
        Events.Publish(new DiagramRedrawEvent(Diagram));
       
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Behaviours.Dispose();
        Events.Dispose();
        Diagram.Dispose();
    }
}

