using Newtonsoft.Json;
using Blazored.Diagrams.Diagrams; // Assuming IDiagram is here

namespace Blazored.Diagrams.Services.Serialization;

/// <inheritdoc />
public class SerializationService : ISerializationService
{
    private JsonSerializerSettings? _settings;
    
    /// <inheritdoc />
    public virtual string ToJson<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram
    {
        if (_settings is null)
        {
            _settings = CreateSettings();
        }
        return JsonConvert.SerializeObject(diagram, _settings);
    }

    /// <inheritdoc />
    public virtual TDiagram FromJson<TDiagram>(string json)
        where TDiagram : IDiagram
    {
        
        if (_settings is null)
        {
            _settings = CreateSettings();
        }
        return JsonConvert.DeserializeObject<TDiagram>(json, _settings)!;
    }

    /// <inheritdoc />
    public IDiagram FromJson(string json)
    {
        
        if (_settings is null)
        {
            _settings = CreateSettings();
        }
        return JsonConvert.DeserializeObject<IDiagram>(json, _settings)!;
    }

    /// <summary>
    /// Creates the <see cref="JsonSerializerSettings"/> used to serialize and deserialize the diagram.
    /// </summary>
    /// <returns></returns>
    public virtual JsonSerializerSettings CreateSettings()
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All, 
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            Formatting = Formatting.Indented,
        };

        return settings;
    }
}