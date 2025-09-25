using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Events;

/// <summary>
/// Represents a type-safe event associated with a model that implements IId.
/// The covariant type parameter TModel allows events with more specific model types
/// to be treated as events of their base types.
/// </summary>
/// <typeparam name="TModel">The type of model this event relates to. Must implement IId.</typeparam>
public interface IModelEvent<out TModel> : IEvent where TModel : IId
{
    /// <summary>
    /// Model that triggered the event.
    /// </summary>
    TModel Model { get; }
}