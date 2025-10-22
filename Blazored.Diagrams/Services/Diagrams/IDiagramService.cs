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
    IEventAggregator Events { get; set; }
    
    /// <summary>
    /// Allows behaviour customization.
    /// </summary>
    IBehaviourContainer Behaviours { get; set; }
    
    /// <summary>
    /// Allows diagram serialization/deserialization.
    /// </summary>
    ISerializationContainer Storage { get; set; }
    
    /// <summary>
    /// Allows adding models to the diagram.
    /// </summary>
    public IAddContainer Add { get; set; }
    
    /// <summary>
    /// Allows deleting models from the diagram.
    /// </summary>
    public IDeleteContainer Remove { get; set; }

    /// <summary>
    /// Customize diagram options.
    /// </summary>
    IOptionsContainer Options { get; set; }

    /// <summary>
    /// Replaces the diagram instance with another.
    /// </summary>
    /// <param name="diagram">A diagram instance</param>
    void UseDiagram(IDiagram diagram);
}