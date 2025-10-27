using Blazored.Diagrams.Diagrams;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Services.Serialization;

public interface ISerializationService
{
    string ToJson<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram;

    TDiagram FromJson<TDiagram>(string json)
        where TDiagram : IDiagram;

    /// <summary>
    /// Creates the <see cref="JsonSerializerSettings"/> used to serialize and deserialize the diagram.
    /// </summary>
    /// <returns></returns>
    JsonSerializerSettings CreateSettings();

    Task ToImage(string selector, string filename);
    Task ToImage(ElementReference element, string filename);
}