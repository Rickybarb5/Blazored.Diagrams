using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Behaviour for logging diagram events to the static logger.
/// </summary>
[ExcludeFromCodeCoverage]
public class EventLoggingBehavior : BaseBehaviour
{
    private readonly IDiagramService _diagramService;
    private readonly LoggingBehaviourOptions _behaviourOptions;

    /// <summary>
    /// Instantiates a new <see cref="EventLoggingBehavior"/>
    /// </summary>
    /// <param name="diagramService">The diagram service</param>
    public EventLoggingBehavior(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        _behaviourOptions = _diagramService.Behaviours.GetBehaviourOptions<LoggingBehaviourOptions>()!;
        _behaviourOptions.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(_behaviourOptions.IsEnabled);
    }

    private void OnEnabledChanged(BehaviourEnabledEvent ev)
    {
        OnEnabledChanged(ev.IsEnabled);
    }
    
    private void OnEnabledChanged(bool isEnabled)
    {
        if (isEnabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
        }
    }

    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            _diagramService.Events.SubscribeTo<IEvent>(Log),
        ];
        
    }

    private void Log(IEvent e)
    {
            var eventType = e.GetType();
            var properties = eventType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var logMessage = new StringBuilder();
    
            // Header for the event
            logMessage.AppendLine($"--- Event Log: {eventType.Name} ---");

            // Check if the event has any public properties to log
            if (properties.Length == 0)
            {
                logMessage.AppendLine("  No public parameters found.");
            }
            else
            {
                foreach (var prop in properties)
                {
                    try
                    {
                        // Get the value of the property from the event instance
                        var value = prop.GetValue(e);

                        // Use the property name and its value in the log message
                        logMessage.AppendLine($"  {prop.Name}: {value ?? "null"}");

                        // Special handling for nested complex types (like Args) is optional but helpful
                        if (prop.Name == "Args" && value != null)
                        {
                            // If it's a Blazor EventArgs, log a few key details
                            var argsType = value.GetType();
                            if (argsType.Name.Contains("EventArgs"))
                            {
                                var clientX = argsType.GetProperty("ClientX")?.GetValue(value);
                                var clientY = argsType.GetProperty("ClientY")?.GetValue(value);
                        
                                if (clientX != null && clientY != null)
                                {
                                    logMessage.AppendLine($"    - Client Coordinates: ({clientX}, {clientY})");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logMessage.AppendLine($"  {prop.Name}: ERROR retrieving value ({ex.Message})");
                    }
                }
            }

            Console.WriteLine(logMessage.ToString());
        
    }
}