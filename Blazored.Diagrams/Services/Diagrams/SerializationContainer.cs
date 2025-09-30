using System.Text.Json;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Serialization;

namespace Blazored.Diagrams.Services.Diagrams;

/// <inheritdoc />
public class SerializationContainer : ISerializationContainer
{
    private readonly ISerializationService _serializationService;
    private readonly IDiagramService _diagramService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public SerializationContainer(IDiagramService service)
    {
        _diagramService = service;
        _serializationService = new SerializationService();
    }

    /// <inheritdoc />
    public string Save<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram
    {
        var json = _serializationService.ToJson(diagram);
        return json;
    }

    /// <inheritdoc />
    public TDiagram FromJson<TDiagram>(string json)
        where TDiagram : IDiagram
    {
        var diagram = _serializationService.FromJson<TDiagram>(json);
        _diagramService.SwitchDiagram(diagram);
        return diagram;
    }
}