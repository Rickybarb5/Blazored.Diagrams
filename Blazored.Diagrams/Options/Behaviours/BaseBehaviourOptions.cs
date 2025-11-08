using Blazored.Diagrams.Events;
using Blazored.Diagrams.Interfaces;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Options.Behaviours;

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