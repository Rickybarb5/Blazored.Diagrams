using System.Text.Json.Serialization;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Serialization;

/// <summary>
/// Manages references.
/// </summary>
public class CustomReferenceResolver : ReferenceResolver
{
    private readonly Dictionary<string, object> _referenceMap = new();
    private readonly Dictionary<object, string> _reverseReferenceMap = new();
    private readonly Dictionary<string, List<Action<object>>> _unresolvedReferences = new();

    /// <inheritdoc />
    public override string GetReference(object value, out bool alreadyExists)
    {
        if (_reverseReferenceMap.TryGetValue(value, out var referenceId))
        {
            alreadyExists = true;
            return referenceId;
        }

        if (value is IId iIdObject)
        {
            referenceId = $"{iIdObject.Id}";
        }
        else
        {
            referenceId = $"{Guid.NewGuid()}";
        }

        _reverseReferenceMap[value] = referenceId;
        alreadyExists = false;
        return referenceId;
    }

    /// <summary>
    /// Gets a value by reference id.
    /// </summary>
    /// <param name="referenceId"></param>
    /// <returns>Object if it exists, <see cref="UnresolvedReference"/> object otherwise.</returns>
    public override object ResolveReference(string referenceId)
    {
        if (_referenceMap.TryGetValue(referenceId, out var value))
        {
            return value;
        }

        // Return a placeholder object if the reference is not resolved yet
        return new UnresolvedReference(referenceId);
    }

    /// <summary>
    /// Adds a reference and resolves any previous missing references if necessary.
    /// </summary>
    /// <param name="referenceId"></param>
    /// <param name="value"></param>
    public override void AddReference(string referenceId, object value)
    {
        _referenceMap[referenceId] = value;
        _reverseReferenceMap[value] = referenceId;

        // Resolve any pending references
        if (_unresolvedReferences.TryGetValue(referenceId, out var actions))
        {
            foreach (var action in actions)
            {
                action(value);
            }

            _unresolvedReferences.Remove(referenceId);
        }
    }

    /// <summary>
    /// Adds an unresolved reference, and a method to resolve it.
    /// </summary>
    /// <param name="referenceId"></param>
    /// <param name="onResolve"></param>
    public void AddUnresolvedReference(string referenceId, Action<object> onResolve)
    {
        if (!_unresolvedReferences.ContainsKey(referenceId))
        {
            _unresolvedReferences[referenceId] = [];
        }

        _unresolvedReferences[referenceId].Add(onResolve);
    }

    /// <summary>
    /// Clears all references from the resolver.
    /// </summary>
    public void Reset()
    {
        _referenceMap.Clear();
        _reverseReferenceMap.Clear();
        _unresolvedReferences.Clear();
    }
}