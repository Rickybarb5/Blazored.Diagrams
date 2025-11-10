using Blazor.Flows.Diagrams;
using Blazor.Flows.Services.Serialization;

namespace Blazor.Flows.Services.Diagrams;

public partial class DiagramService
{
    private ISerializationService _serializationService = new SerializationService();

    /// <summary>
    /// Saves the current diagram instance to json.
    /// </summary>
    /// <param name="diagram"></param>
    /// <typeparam name="TDiagram"></typeparam>
    /// <returns></returns>
    public string Save<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram
    {
        var json = _serializationService.ToJson(diagram);
        return json;
    }

    /// <summary>
    /// Loads a JSON and uses the deserialization as the new diagram for the diagram service.
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="TDiagram"></typeparam>
    /// <returns></returns>
    public void LoadAndUse<TDiagram>(string json)
        where TDiagram : IDiagram
    {
        var diagram = Load<TDiagram>(json);
        UseDiagram(diagram);
    }

    /// <summary>
    /// Loads a JSON and uses the deserialization as the new diagram for the diagram service.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public void LoadAndUse(string json)
    {
        var diagram = Load(json);
        UseDiagram(diagram);
    }

    /// <summary>
    /// Loads a JSON and returns the a <see cref="IDiagram"/> object;
    /// </summary>
    /// <param name="json">The Json string.</param>
    /// <typeparam name="TDiagram">The Diagram type.</typeparam>
    /// <returns></returns>
    public TDiagram Load<TDiagram>(string json)
        where TDiagram : IDiagram
    {
        var diagram = _serializationService.FromJson<TDiagram>(json);
        return diagram;
    }

    /// <summary>
    /// Loads a JSON and returns the a <see cref="IDiagram"/> object;
    /// </summary>
    /// <param name="json">The Json string.</param>
    /// <returns></returns>
    public IDiagram Load(string json)
    {
        var diagram = _serializationService.FromJson<IDiagram>(json);
        return diagram;
    }
}