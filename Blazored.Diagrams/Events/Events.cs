using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Events;

/// <summary>
///     Base Model for a model event.
/// </summary>
/// <param name="Model"></param>
/// <typeparam name="TModel"></typeparam>
public abstract record ModelEventBase<TModel>(TModel Model) : IModelEvent<TModel> where TModel : IId;

/// <summary>
/// Base class for an input event
/// </summary>
public abstract record InputEventBase : IEvent;

/// <summary>
/// Base class for an input event, originated from a model.
/// </summary>
/// <param name="Model"></param>
/// <typeparam name="TModel"></typeparam>
public abstract record ModelInputEvent<TModel>(TModel Model) : InputEventBase;