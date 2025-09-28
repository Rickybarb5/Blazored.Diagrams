using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Behaviours;

namespace Blazored.Diagrams.Options.Diagram;

public class DiagramOptions
{
    /// <inheritdoc />
    public DiagramStyleOptions Style { get; set; } = new();

    /// <inheritdoc />
    public VirtualizationOptions Virtualization { get; init; } = new();

    /// <inheritdoc />
    public IReadOnlyList<IBehaviourOptions> BehaviourOptions => _behaviourOptions.AsReadOnly();
  
    
    internal List<IBehaviourOptions> _behaviourOptions=  
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