using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Helpers;

namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Interface that describes a model that contains groups.
/// </summary>
public interface IGroupContainer 
{
    /// <summary>
    ///     List of groups.
    /// </summary>
    ObservableList<IGroup> Groups { get; set; }
}