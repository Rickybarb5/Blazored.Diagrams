using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Links;

namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Describes a model that has link connected to it.
/// </summary>
public interface ILinkContainer : IId
{
    /// <summary>
    ///     List of links that originate from this port.
    /// </summary>
    ObservableList<ILink> OutgoingLinks { get; set; }

    /// <summary>
    ///     List of links that originate from another port.
    /// </summary>
    ObservableList<ILink> IncomingLinks { get; set; }
}