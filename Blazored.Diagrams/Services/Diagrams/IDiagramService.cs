using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Behaviours;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services.Diagrams;

/// <summary>
/// Manages diagram events, behaviours and child models.
/// </summary>
public interface IDiagramService : IDisposable
{
    /// <summary>
    /// Diagram instance.
    /// </summary>
    IDiagram Diagram { get; }

    /// <summary>
    /// Allows access to event subscription and publish.
    /// </summary>
    IEventAggregator Events { get; init; }
    
    /// <summary>
    /// Allows behaviour customization.
    /// </summary>
    IBehaviourContainer Behaviours { get; init; }
    
    /// <summary>
    /// Allows adding models to the diagram.
    /// </summary>
    public IAddContainer Add { get; init; }
    
    /// <summary>
    /// Allows deleting models from the diagram.
    /// </summary>
    public IDeleteContainer Remove { get; init; }
}