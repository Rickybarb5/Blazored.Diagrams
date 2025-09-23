using System.Text.Json.Serialization;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Options.Diagram;

namespace Blazored.Diagrams.Options;

/// <summary>
///    Configurable diagram options
/// </summary>
public class DiagramOptions
{
    internal readonly List<IDiagramOptions> _behaviours = new();

    /// <summary>
    ///     Default constructor.
    /// </summary>
    public DiagramOptions()
    {
    }

    /// <summary>
    /// Constructor used for serialization.
    /// </summary>
    /// <param name="items"></param>
    [JsonConstructor]
    public DiagramOptions(IEnumerable<IDiagramOptions> items)
    {
        _behaviours = items.ToList();
    }

    /// <summary>
    ///     Behaviour options.
    /// </summary>
    public IReadOnlyList<IDiagramOptions> Behaviours => _behaviours;

    /// <summary>
    ///     Style options.
    /// </summary>
    public DiagramStyleOptions Styles { get; init; } = new();

    /// <summary>
    /// Virtualization options.
    /// </summary>
    public VirtualizationOptions Virtualization { get; init; } = new();

    public TOptions? Get<TOptions>()
        where TOptions : IDiagramOptions
    {
        return _behaviours.OfType<TOptions>().FirstOrDefault();
    }
}