using System.Diagnostics.CodeAnalysis;

namespace Blazored.Diagrams.Services.Serialization;

/// <summary>
/// Object used to describe a json reference that hasn't been found yet.
/// </summary>
[ExcludeFromCodeCoverage]
public class UnresolvedReference
{
    /// <summary>
    /// Reference Id of an object.
    /// </summary>
    public string ReferenceId { get; }

    /// <summary>
    /// Initializes a <see cref="UnresolvedReference"/> object.
    /// </summary>
    /// <param name="referenceId"></param>
    public UnresolvedReference(string referenceId)
    {
        ReferenceId = referenceId;
    }
}