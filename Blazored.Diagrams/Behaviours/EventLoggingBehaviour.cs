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
    private readonly LoggingBehaviourOptions _behaviourOptions;
    private readonly IDiagramService _diagramService;

    /// <summary>
    /// Instantiates a new <see cref="EventLoggingBehavior"/>
    /// </summary>
    /// <param name="diagramService">The diagram service</param>
    public EventLoggingBehavior(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        _behaviourOptions = _diagramService.Behaviours.GetBehaviourOptions<LoggingBehaviourOptions>();
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
        try
        {
            var eventType = e.GetType();
            bool isModelInputEvent = InheritsFromGenericType(eventType, typeof(ModelInputEvent<>));

            if (isModelInputEvent && !_behaviourOptions.LogPointerEvents)
                return;

            var properties = eventType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var logMessage = new StringBuilder();

            logMessage.AppendLine($"--- Event Log: {eventType.Name} ---");

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
                        var value = prop.GetValue(e);
                        logMessage.AppendLine($"  {prop.Name}: {value ?? "null"}");

                        if (prop.Name == "Args" && value != null)
                        {
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
        catch
        {
            // Here to avoid crashes from this behaviour.
        }

    }
    
    // Utility method to check for inheritance from a generic base type
    private static bool InheritsFromGenericType(Type? type, Type genericBase)
    {
        while (type != null && type != typeof(object))
        {
            var current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (current == genericBase)
                return true;
            type = type.BaseType!;
        }
        return false;
    }
}