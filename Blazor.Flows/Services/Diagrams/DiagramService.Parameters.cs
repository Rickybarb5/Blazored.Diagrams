using Blazor.Flows.Interfaces;

namespace Blazor.Flows.Services.Diagrams;


/// <summary>
/// Parameters to use in the <see cref="IDiagramService.CenterInViewport{TModel}"/> method.
/// </summary>
/// <param name="Model">Model to center.</param>
/// <typeparam name="TModel">Type of the model.</typeparam>
public record CenterInViewportParameters<TModel>(TModel Model);
/// <summary>
///  Parameter to use in the <see cref="IDiagramService.ZoomToModel{TModel}"/>
/// </summary>
/// <param name="Model">Model to zoom into.</param>
/// <typeparam name="TModel">Type of the model.</typeparam>
public record ZoomToModelParameters<TModel>(TModel Model);

/// <summary>
/// Parameter to use in the <see cref="IDiagramService.CenterIn{TContainer,TModel}"/>.
/// </summary>
/// <param name="Model">Model to be centered.</param>
/// <param name="Container">Model to center on.</param>
/// <typeparam name="TModel">Type of the Model.</typeparam>
/// <typeparam name="TContainer">Type of the container.</typeparam>
public record CenterInParameters<TModel, TContainer>(
    TModel Model,
    TContainer Container)
    where TModel : ISize, IPosition
    where TContainer : ISize, IPosition;