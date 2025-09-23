namespace Blazored.Diagrams.Services.Registry;

/// <summary>
///     A component registry is where all components related to a data model are registrated.
/// </summary>
public interface IComponentRegistry
{
    /// <summary>
    ///     Register 
    /// </summary>
    /// <param name="componentType">Type of the component</param>
    /// <param name="modelType">Type of the data model.</param>
    void RegisterComponent(Type componentType, Type modelType);

    /// <summary>
    ///     Gets the component associate with a model type.
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    Type? GetComponentType(Type modelType);
}