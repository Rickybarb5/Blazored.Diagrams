namespace Blazor.Flows.Interfaces;

/// <summary>
///     Properties that define the visibility of the model.
/// </summary>
public interface IVisible
{
    /// <summary>
    ///     Gets a value indicating whether the model is visible or not.
    /// </summary>
    public bool IsVisible { get; set; }
}