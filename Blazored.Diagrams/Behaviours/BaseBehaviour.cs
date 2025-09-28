using System.Text.Json.Serialization;
using Blazored.Diagrams.Extensions;

namespace Blazored.Diagrams.Behaviours;

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