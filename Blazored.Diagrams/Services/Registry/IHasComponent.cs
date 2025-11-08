using Microsoft.AspNetCore.Components;

namespace Blazored.Diagrams.Services.Registry;

/// <summary>
/// Marker interface for data models linked to a component.
/// </summary>
/// <typeparam name="TComponent">Component type.</typeparam>
public interface IHasComponent<TComponent> where TComponent : IComponent;