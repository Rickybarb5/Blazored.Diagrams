using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services;

public partial class DiagramService
{
    private readonly List<IBehaviour> _behaviours = [];

    /// <inheritdoc />
    public void AddBehaviour<TBehaviour, TOptions>(TBehaviour behaviour, TOptions options)
        where TBehaviour : IBehaviour
        where TOptions : IDiagramOptions
    {
        if (!Diagram.Options.Behaviours.OfType<TOptions>().Any())
        {
            Diagram.Options._behaviours.Add(options);
        }

        if (_behaviours.All(x => x is not TBehaviour)) _behaviours.Add(behaviour);
    }

    public IReadOnlyList<IBehaviour> Behaviours { get; }

    /// <inheritdoc />
    public TBehaviour? GetBehaviour<TBehaviour>() where TBehaviour : class, IBehaviour
    {
        var behaviour = _behaviours.FirstOrDefault(x => x.GetType() == typeof(TBehaviour));
        return behaviour as TBehaviour;
    }

    /// <inheritdoc />
    public bool TryGetBehaviour<TBehaviour>(out TBehaviour behaviour) where TBehaviour : class, IBehaviour
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        behaviour = GetBehaviour<TBehaviour>();
#pragma warning restore CS8601 // Possible null reference assignment.
        return behaviour is not null;
    }
}