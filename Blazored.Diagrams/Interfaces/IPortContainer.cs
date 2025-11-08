using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Ports;

namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Describes a model that contains ports.
/// </summary>
public interface IPortContainer : ISize, IPosition, IId
{
    /// <summary>
    ///     Ports associated with this model.
    /// </summary>
    ObservableList<IPort> Ports { get; set; }
}