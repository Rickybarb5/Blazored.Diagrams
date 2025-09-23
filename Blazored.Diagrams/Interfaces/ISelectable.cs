namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Properties that define the selectability of a model.
/// </summary>
public interface ISelectable
{
    /// <summary>
    ///     Gets a value indicating whether the model is selected.
    /// </summary>
    public bool IsSelected { get; set; }
}