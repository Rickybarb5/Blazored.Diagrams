using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Options.Diagram;

namespace Blazor.Flows.Services.Diagrams;

/// <inheritdoc />
public class OptionsContainer : IOptionsContainer
{
    private readonly IDiagramService _service;
    /// <summary>
    /// Creates a new <see cref="OptionsContainer"/>.
    /// </summary>
    /// <param name="service"></param>
    public OptionsContainer(IDiagramService service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public virtual IDiagramStyleOptions Styling => _service.Diagram.Options.Style;

    /// <inheritdoc />
    public CurvedLinkOptions CurvedLinkOptions => _service.Diagram.Options.CurvedLinkOptions;

    /// <inheritdoc />
    public LineLinkOptions LineLinkOptions => _service.Diagram.Options.LineLinkOptions;

    /// <inheritdoc />
    public OrthogonalLinkOptions OrthogonalLinkOptions => _service.Diagram.Options.OrthogonalLinkOptions;
}