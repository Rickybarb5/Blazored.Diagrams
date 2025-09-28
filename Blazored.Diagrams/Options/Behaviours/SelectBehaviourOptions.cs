using Blazored.Diagrams.Behaviours;

namespace Blazored.Diagrams.Options.Behaviours;

/// <summary>
/// Options for <see cref="SelectBehaviour"/>
/// </summary>
public class SelectBehaviourOptions : BaseBehaviourOptions
{
    /// <summary>
    ///     Enables or disables multi selection
    /// </summary>
    public bool MultiSelectEnabled { get; set; } = true;

    /// <summary>
    ///     Enables or disables model selection.
    /// </summary>
    public bool SelectionEnabled { get; set; } = true;
}