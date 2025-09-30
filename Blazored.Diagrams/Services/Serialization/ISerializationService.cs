using Blazored.Diagrams.Diagrams;

namespace Blazored.Diagrams.Services.Serialization;

public interface ISerializationService
{
    string ToJson<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram;

    TDiagram FromJson<TDiagram>(string json)
        where TDiagram : IDiagram;
}