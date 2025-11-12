namespace Blazor.Flows.Interfaces;

/// <summary>
/// Represents the depth of the model in the diagram.
/// </summary>
public interface IDepth
{
    /// <summary>
    /// Z-Index of the component.
    /// </summary>
    public int ZIndex { get; set; }
}