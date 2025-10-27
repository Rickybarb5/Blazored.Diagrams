using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Serialization;
using Microsoft.JSInterop;

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
    public SerializationContainer(IDiagramService service, IJSRuntime jsRuntime)
    {
        _diagramService = service;
        _serializationService = new SerializationService(jsRuntime);
    }

    /// <inheritdoc />
    public string Save<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram
    {
        var json = _serializationService.ToJson(diagram);
        return json;
    }

    /// <inheritdoc />
    public TDiagram Load<TDiagram>(string json)
        where TDiagram : IDiagram
    {
        var diagram = _serializationService.FromJson<TDiagram>(json);
        _diagramService.UseDiagram(diagram);
        return diagram;
    }
}