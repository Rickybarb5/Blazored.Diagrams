using Blazored.Diagrams.Diagrams;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Services.Serialization;

/// <summary>
/// Handles saving and loading diagram objects to various formats.
/// </summary>
public interface ISerializationService
{
    /// <summary>
    /// Saves a diagram to Json format
    /// </summary>
    /// <param name="diagram">Diagram to serialize.</param>
    /// <typeparam name="TDiagram">Diagram Type./</typeparam>
    /// <returns></returns>
    string ToJson<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram;

    /// <summary>
    /// Loads a diagram from a json file.
    /// </summary>
    /// <param name="json">The json string.</param>
    /// <typeparam name="TDiagram">Diagram Type to convert to./</typeparam>
    /// <returns></returns>
    TDiagram FromJson<TDiagram>(string json)
        where TDiagram : IDiagram;

    /// <summary>
    /// Loads a diagram from a json file.
    /// </summary>
    /// <param name="json">The json string.</param>
    /// <returns></returns>
    IDiagram FromJson(string json);

    /// <summary>
    /// Creates the <see cref="JsonSerializerSettings"/> used to serialize and deserialize the diagram.
    /// </summary>
    /// <returns></returns>
    JsonSerializerSettings CreateSettings();
}