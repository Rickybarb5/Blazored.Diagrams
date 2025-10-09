using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services.Providers;

/// <summary>
/// Provides services related to diagrams.
/// </summary>
public interface IDiagramServiceProvider
{
    /// <summary>
    /// Gets 
    /// This service can perform high level operations on a diagram.
    /// </summary>
    /// <param name="diagram">The diagram that the service will operate onto to.</param>
    /// <typeparam name="TDiagram"></typeparam>
    /// <returns><see cref="DiagramService"/>.</returns>
    IDiagramService GetDiagramService(IDiagram diagram);
}