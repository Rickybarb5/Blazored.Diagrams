using Blazored.Diagrams.Interfaces;

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
                OnEnabledChanged?.Invoke(value);
            }
        }
    }

    /// <inheritdoc />
    public event Action<bool>? OnEnabledChanged;
}