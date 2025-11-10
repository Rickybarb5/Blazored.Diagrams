using Blazor.Flows.Events;
using Blazor.Flows.Interfaces;
using Newtonsoft.Json;

namespace Blazor.Flows.Options.Behaviours;

/// <summary>
///     Base implementation of behaviour options.
/// </summary>
public partial class BaseBehaviourOptions : IBehaviourOptions
{
    private bool _isEnabled = true;

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (value != _isEnabled)
            {
                _isEnabled = value;
                OnEnabledChanged.Publish(new BehaviourEnabledEvent(GetType(), value));
            }
        }
    }

    /// <inheritdoc />
    [JsonIgnore]
    public ITypedEvent<BehaviourEnabledEvent> OnEnabledChanged { get; init; }
        = new TypedEvent<BehaviourEnabledEvent>();
}