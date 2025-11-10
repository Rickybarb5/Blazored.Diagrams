using Blazor.Flows.Behaviours;

namespace Blazor.Flows.Options.Behaviours;

/// <summary>
///     Options for <see cref="EventLoggingBehavior"/>
///     Disabled by default.
/// </summary>
public class LoggingBehaviourOptions : BaseBehaviourOptions
{
    /// <summary>
    /// When enabled, logs pointer events.
    /// When disabled, those events are ignored.
    /// </summary>
    public bool LogPointerEvents { get; set; } = false;
    
    /// <summary>
    /// Instantiates a new Logging behaviour options
    /// </summary>
    public LoggingBehaviourOptions()
    {
        IsEnabled = false;
    }
}