using Blazor.Flows.Events;

namespace Blazor.Flows.Interfaces;

/// <summary>
/// Describes what an 
/// </summary>
public interface IBehaviourOptions
{
    /// <summary>
    /// Indicates if a diagram option is enabled or disabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Event triggered when IsEnabled changes.
    /// </summary>
    public ITypedEvent<BehaviourEnabledEvent> OnEnabledChanged { get; init; }
}