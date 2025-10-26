using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Interfaces;
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

    /// <summary>
    ///     Returns the center point of a model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    (int CenterX, int CenterY) GetCenterCoordinates<T>(T model) where T : ISize, IPosition;

    /// <summary>
    /// Centers a model in another model
    /// </summary>
    /// <param name="toCenter"></param>
    /// <param name="containerWidth"></param>
    /// <param name="containerHeight"></param>
    /// <param name="containerPositionX"></param>
    /// <param name="containerPositionY"></param>
    /// <param name="padding"></param>
    /// <typeparam name="ToCenter"></typeparam>
    void CenterTo<ToCenter>(
        ToCenter toCenter,
        int containerWidth,
        int containerHeight,
        int containerPositionX,
        int containerPositionY,
        int padding = 0)
        where ToCenter : ISize, IPosition;

    /// <summary>
    ///     Changes the position of a model to be in the center of the diagram, accounting for pan and zoom.
    /// </summary>
    /// <param name="toCenter">Model to be centered.</param>
    /// <typeparam name="ToCenter">Type of the model to be centered.</typeparam>
    void CenterInViewport<ToCenter>(ToCenter toCenter)
        where ToCenter : IPosition, ISize;
}