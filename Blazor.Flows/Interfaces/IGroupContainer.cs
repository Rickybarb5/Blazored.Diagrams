using Blazor.Flows.Groups;
using Blazor.Flows.Helpers;

namespace Blazor.Flows.Interfaces;

/// <summary>
///     Interface that describes a model that contains groups.
/// </summary>
public interface IGroupContainer : IId
{
    /// <summary>
    ///     List of groups.
    /// </summary>
    ObservableList<IGroup> Groups { get; set; }
}