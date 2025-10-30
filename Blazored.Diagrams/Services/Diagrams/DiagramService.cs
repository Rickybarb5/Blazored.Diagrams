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
    /// <summary>
    /// Initializes a new instance of <see cref="DiagramService"/>.
    /// </summary>
    public DiagramService()
    {
        Diagram = new Diagram();
        Behaviours = new BehaviourContainer(this);
        Options = new OptionsContainer(this);
        Events = new EventAggregator(this);
        InitializeOptions();
        InitializeBehaviours();
    }

    /// <inheritdoc />
    public IEventAggregator Events { get; set; }

    /// <inheritdoc />
    public IBehaviourContainer Behaviours { get; set; }


    /// <inheritdoc />
    public IOptionsContainer Options { get; set; }

    /// <inheritdoc />
    public IDiagram Diagram { get; private set; }

    /// <inheritdoc />
    public void UseDiagram(IDiagram diagram)
    {
        Behaviours.Dispose();
        Diagram.Dispose();

        var oldDiagram = Diagram;
        Diagram = diagram;
        // We don't reset the entire event service
        // This way, custom behaviours keep working!
        ((EventAggregator)Events).RewireSubscriptions();
        Behaviours = new BehaviourContainer(this);
        Options = new OptionsContainer(this);
        InitializeOptions();
        InitializeBehaviours();
        Events.Publish(new DiagramSwitchEvent(oldDiagram, diagram));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Behaviours.Dispose();
        Events.Dispose();
        Diagram.Dispose();
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
    
    /// <inheritdoc />
    public virtual void SetZoom(double zoom)
    {
        Diagram.SetZoom(zoom);
    }
}