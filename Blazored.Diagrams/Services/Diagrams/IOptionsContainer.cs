using Blazored.Diagrams.Options.Diagram;

namespace Blazored.Diagrams.Services.Diagrams;

public interface IOptionsContainer
{
    IDiagramStyleOptions Styling { get; }
    IVirtualizationOptions Virtualization { get; }
}