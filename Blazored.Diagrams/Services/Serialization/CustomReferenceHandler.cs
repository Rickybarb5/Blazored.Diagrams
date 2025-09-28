using System.Text.Json.Serialization;

namespace Blazored.Diagrams.Services.Serialization;

/// <summary>
/// Custom reference handler to manage cycles and repeated references.
/// </summary>
public class CustomReferenceHandler : ReferenceHandler
{
    public readonly CustomReferenceResolver Resolver = new();

    /// <inheritdoc />
    public override CustomReferenceResolver CreateResolver()
    {
        return Resolver;
    }
}