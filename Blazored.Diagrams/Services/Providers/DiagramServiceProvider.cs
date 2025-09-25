using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services.Providers;

/// <inheritdoc />
public class DiagramServiceProvider : IDiagramServiceProvider
{
    private readonly Dictionary<Guid, IDiagramService> _diagramServices = [];
    private readonly Dictionary<Guid, IEventPropagator> _diagramEventPropagators = [];
    private readonly Dictionary<Guid, IEventAggregator> _eventAggregators = [];

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
    
    /// <inheritdoc />
    public IEventPropagator GetDiagramEventPropagator(IDiagram diagram)
    {
        if (!_diagramEventPropagators.TryGetValue(diagram.Id, out IEventPropagator? service))
        {
            service = new EventPropagator(GetDiagramService(diagram));
            _diagramEventPropagators.Add(diagram.Id, service);
        }

        return service;
    }
    
    /// <inheritdoc />
    public IEventAggregator GetDiagramEventAggregator(IDiagram diagram)
    {
        if (!_eventAggregators.TryGetValue(diagram.Id, out IEventAggregator? service))
        {
            service = new EventAggregator();
            _eventAggregators.Add(diagram.Id, service);
        }

        return service;
    }
}