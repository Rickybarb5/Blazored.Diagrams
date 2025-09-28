namespace Blazored.Diagrams.Interfaces;

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
    public event Action<bool>? OnEnabledChanged;
}