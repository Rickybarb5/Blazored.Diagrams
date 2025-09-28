using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Services.Serialization;

/// <inheritdoc />
public class DiagramSerializationService : IDiagramSerializationService
{
    private readonly JsonSerializerOptions _options;

    public DiagramSerializationService()
    {
        _options = CreateOptions(Assembly.GetExecutingAssembly());
    }
    /// <inheritdoc />
    public string ToJson<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram
    {
        var json = JsonSerializer.Serialize(diagram, _options);
        (_options.ReferenceHandler as CustomReferenceHandler)!.Resolver.Reset();
        return json;
    }

    /// <inheritdoc />
    public TDiagram FromJson<TDiagram>(string json)
        where TDiagram : IDiagram
    {
        var diagram= JsonSerializer.Deserialize<TDiagram>(json, _options);
        return diagram;
    }

    public static JsonSerializerOptions CreateOptions(params Assembly[] assembliesToScan)
    {
        var allDerivedTypes = assembliesToScan
            .SelectMany(x => x.GetTypes())
            .Where(t => !t.IsAbstract &&
                        (typeof(INode).IsAssignableFrom(t) ||
                         typeof(IGroup).IsAssignableFrom(t) ||
                         typeof(ILink).IsAssignableFrom(t) ||
                         typeof(IPort).IsAssignableFrom(t) ||
                         typeof(IDiagram).IsAssignableFrom(t) ||
                         typeof(IBehaviourOptions).IsAssignableFrom(t)))
            .ToList();
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = new CustomReferenceHandler(),
            Converters = { new ObservableListConverter() },
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    typeInfo =>
                    {
                        if (typeInfo.Type == typeof(INode) ||
                            typeInfo.Type == typeof(IGroup) ||
                            typeInfo.Type == typeof(ILink) ||
                            typeInfo.Type == typeof(IPort) ||
                            typeInfo.Type == typeof(IDiagram) ||
                            typeInfo.Type == typeof(IBehaviourOptions))
                        {
                            typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                            {
                                TypeDiscriminatorPropertyName = "$type"
                            };

                            foreach (var type in allDerivedTypes.Where(t => typeInfo.Type.IsAssignableFrom(t)))
                            {
                                typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(type, type.Name));
                            }
                        }
                        ApplyJsonAttributes(typeInfo);
                    }
                }
            }
        };

        return options;
    }

    private static void ApplyJsonAttributes(JsonTypeInfo typeInfo)
    {
        foreach (var prop in typeInfo.Properties.ToList())
        {
            var attrs = prop.AttributeProvider?.GetCustomAttributes(inherit: true);

            if (attrs == null) continue;

            foreach (var attr in attrs)
            {
                switch (attr)
                {
                    case JsonIgnoreAttribute ignoreAttr:
                        if (ignoreAttr.Condition == JsonIgnoreCondition.Always)
                            typeInfo.Properties.Remove(prop);
                        break;

                    case JsonPropertyNameAttribute nameAttr:
                        prop.Name = nameAttr.Name;
                        break;

                    case JsonPropertyOrderAttribute orderAttr:
                        prop.Order = orderAttr.Order;
                        break;
                }
            }
        }
    }
}