namespace Blazored.Diagrams.Interfaces;

public interface IDiagramOptions
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