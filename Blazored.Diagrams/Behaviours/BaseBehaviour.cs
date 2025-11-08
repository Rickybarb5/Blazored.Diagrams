
using Blazored.Diagrams.Extensions;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Base implementation of a behaviour.
/// </summary>
public class BaseBehaviour : IBehaviour
{
    /// <summary>
    /// Subscriptions handled by the behaviour.
    /// </summary>
    protected List<IDisposable> Subscriptions = [];

    /// <inheritdoc />
    public virtual void Dispose()
    {
        DisposeSubscriptions();
    }
    /// <summary>
    /// Disposes all the current subscriptions.
    /// </summary>
    protected void DisposeSubscriptions()
    {
        Subscriptions.DisposeAll();
    }
}