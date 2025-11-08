using Blazored.Diagrams.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Blazored.Diagrams.Services.Registry;

/// <summary>
/// Component registry implementation.
/// </summary>
public class ComponentRegistry : IComponentRegistry
{
    private readonly Dictionary<Type, Type> _componentTypes = [];

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
                var componentInterface = GetClosestHasComponentInterface(type);

                if (componentInterface != null)
                {
                    var componentType = componentInterface.GetGenericArguments()[0];
                    RegisterComponent(componentType, type);
                }
            }
        }
    }
    
    private static Type? GetClosestHasComponentInterface(Type? type)
    {
        while (type != null && type != typeof(object))
        {
            // Only interfaces introduced on this type, not inherited
            var declaredInterfaces = type.GetInterfaces()
                .Except(type.BaseType?.GetInterfaces() ?? []);

            var match = declaredInterfaces
                .FirstOrDefault(i => i.IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(IHasComponent<>));

            if (match != null)
                return match;

            type = type.BaseType;
        }

        return null;
    }

    private bool HasComponentInterface(Type type)
    {
        return type.GetInterfaces()
            .Any(i => i.IsGenericType &&
                      i.GetGenericTypeDefinition() == typeof(IHasComponent<>));
    }

    /// <inheritdoc />
    public void RegisterComponent(Type componentType, Type modelType)
    {
        if (!typeof(IComponent).IsAssignableFrom(componentType))
            throw new ArgumentException($"Type {componentType.Name} must be a component");

        _componentTypes[modelType] = componentType;
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