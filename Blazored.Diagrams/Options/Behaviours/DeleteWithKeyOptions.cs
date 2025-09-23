using Blazored.Diagrams.Behaviours;

namespace Blazored.Diagrams.Options.Behaviours;

/// <summary>
/// Options for <see cref="DeleteWithKeyBehaviour"/>
/// </summary>
public class DeleteWithKeyOptions : BehaviourOptionsBase
{
    /// <summary>
    /// Key code that deletes diagram children.
    /// </summary>
    public string DeleteKeyCode { get; set; } = "Delete";
}