namespace Blazored.Diagrams.Interfaces;

/// <summary>
/// Interface used to describe models with an id.
/// </summary>
public interface IId
{
    /// <summary>
    ///     Id of the model.
    /// </summary>
    public string Id { get; init; }
}