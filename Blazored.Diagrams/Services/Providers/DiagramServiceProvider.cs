using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services.Providers;

/// <inheritdoc />
public class DiagramServiceProvider : IDiagramServiceProvider
{
    private readonly Dictionary<string, IDiagramService> _diagramServices = [];

    /// <inheritdoc />
    public IDiagramService GetDiagramService(IDiagram diagram)
    {
        if (!_diagramServices.TryGetValue(diagram.Id, out IDiagramService? service))
        {
            service = new DiagramService(diagram, this);
            _diagramServices.Add(diagram.Id, service);
        }

        return service;
    }
}