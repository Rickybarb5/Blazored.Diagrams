using Blazor.Flows.Behaviours;

namespace Blazor.Flows.Options.Behaviours;

/// <summary>
/// Options for <see cref="ZIndexBehaviour"/>.
/// </summary>
public class ZIndexBehaviourOptions : BaseBehaviourOptions
{
    /// <summary>
    /// The offset added to the Z-Index calculation for <c>Group</c> elements.
    /// Groups typically have the lowest Z-Index within a nesting level (default: 0).
    /// </summary>
    public int GroupOffset { get; set; } = 0;

    /// <summary>
    /// The offset added to the Z-Index calculation for <c>Node</c> elements.
    /// Nodes are rendered above Groups within a nesting level (default: 20).
    /// </summary>
    public int NodeOffset { get; set; } = 20;

    /// <summary>
    /// The offset added to the Z-Index calculation for <c>Port</c> elements.
    /// Ports typically have the highest Z-Index within a nesting level (default: 40).
    /// </summary>
    public int PortOffset { get; set; } = 40;

    /// <summary>
    /// The multiplier used to create significant Z-Index separation between different
    /// nesting levels (e.g., elements inside a group vs. elements on the layer).
    /// (default: 100).
    /// </summary>
    public int NestingMultiplier { get; set; } = 100;
}