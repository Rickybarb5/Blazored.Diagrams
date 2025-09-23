using Blazored.Diagrams.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Blazored.Diagrams.Services.Registry;

/// <summary>
/// Component registry implementation.
/// </summary>
public class ComponentRegistry : IComponentRegistry
{
    private readonly Dictionary<Type, Type> _componentTypes = new();

    /// <summary>
    ///     Initializes a new instance of <see cref="ComponentRegistry"/>
    /// </summary>
    /// <param name="options"></param>
    public ComponentRegistry(IOptions<BlazoredDiagramsOptions> options)
    {
        foreach (var assembly in options.Value.Assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && HasComponentInterface(t));

            foreach (var type in types)
            {
                // Get the first directly implemented IHasComponent<> interface
                var componentInterface = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(IHasComponent<>) &&
                                         IsDirectlyImplementedInterface(type, i));

                // If no direct interface is found, use the first inherited one
                componentInterface ??= type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(IHasComponent<>));

                if (componentInterface != null)
                {
                    var componentType = componentInterface.GetGenericArguments()[0];
                    RegisterComponent(componentType, type);
                }
            }
        }
    }

    /// <summary>
    /// Checks if an interface is directly implemented by the given type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="interfaceType">The interface to verify.</param>
    /// <returns>True if the interface is directly implemented by the type; otherwise, false.</returns>
    private bool IsDirectlyImplementedInterface(Type type, Type interfaceType)
    {
        // Get the directly implemented interfaces of the type
        var directlyImplementedInterfaces = type.GetInterfaces()
            .Except(type.BaseType?.GetInterfaces() ?? Enumerable.Empty<Type>());

        return directlyImplementedInterfaces.Contains(interfaceType);
    }

    private bool HasComponentInterface(Type type)
    {
        return type.GetInterfaces()
            .Any(i => i.IsGenericType &&
                      i.GetGenericTypeDefinition() == typeof(IHasComponent<>));
    }

    /// <inheritdoc />
    public void RegisterComponent(Type componentType, Type dataType)
    {
        if (!typeof(IComponent).IsAssignableFrom(componentType))
            throw new ArgumentException($"Type {componentType.Name} must be a component");

        _componentTypes[dataType] = componentType;
    }

    /// <inheritdoc />
    public Type? GetComponentType(Type modelType)
    {
        // Get the component type from the interface
        var success = _componentTypes.TryGetValue(modelType, out var componentType);
        if (!success)
        {
            throw new InvalidOperationException($"No component type registered for {modelType.Name}. Did you add the {typeof(IHasComponent<>).Name} interface to the model?");
        }

        return componentType;
    }
}