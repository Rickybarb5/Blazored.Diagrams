namespace Blazor.Flows.Services.Events;

/// <summary>
/// Represents a subscription related to an event.
/// </summary>
public class EventSubscription : IDisposable
{
    private readonly Action _unsubscribeAction;
    private bool _disposed;

    /// <summary>
    /// Instantiates a new Event subscription.
    /// </summary>
    /// <param name="unsubscribeAction">What to do after the subscription has been fired.</param>
    public EventSubscription(Action unsubscribeAction)
    {
        _unsubscribeAction = unsubscribeAction;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_disposed)
        {
            _unsubscribeAction();
            _disposed = true;
        }
    }
}