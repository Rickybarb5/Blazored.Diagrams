using System.Text.Json.Serialization;
using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Layers;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Services;

public interface IDiagramService : IDisposable
{
    /// <summary>
    /// Gets a value indicating if the diagram has been initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Diagram instance
    /// </summary>
    IDiagram Diagram { get; }

    /// <summary>
    /// Event container of the diagram service.
    /// Allows you to subscribe to and publish events.
    /// </summary>
    IEventAggregator Events { get; }

    /// <summary>
    /// Creates a new diagram instance for the service.
    /// </summary>
    /// <param name="configure"></param>
    /// <typeparam name="TDiagram"></typeparam>
    /// <returns></returns>
    TDiagram Create<TDiagram>() where TDiagram : IDiagram, new();


    /// <summary>
    /// Removes a node from the diagram.
    /// </summary>
    /// <param name="node">Node to be removed</param>
    /// <returns>True if removed, false otherwise.</returns>
    bool Remove(INode node);

    /// <summary>
    /// Removes a group from the diagram.
    /// </summary>
    /// <param name="group">Group to be removed</param>
    /// <returns>True if removed, false otherwise.</returns>
    bool Remove(IGroup group);

    /// <summary>
    /// Removes a layer from the diagram.
    /// </summary>
    /// <param name="layer">Layer to be removed</param>
    /// <returns>True if removed, false otherwise.</returns>
    bool Remove(ILayer layer);

    /// <summary>
    /// Removes a port from the diagram.
    /// </summary>
    /// <param name="port">Port to be removed</param>
    /// <returns>True if removed, false otherwise.</returns>
    bool Remove(IPort port);

    /// <inheritdoc />
    bool Remove(ILink link);

    #region Behaviours

    /// <summary>
    ///     Behaviours executed by the container.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<IBehaviour> Behaviours { get; }

    /// <summary>
    /// Gets a behaviour from the container, if it exists.
    /// </summary>
    /// <typeparam name="TBehaviour"></typeparam>
    /// <returns></returns>
    public TBehaviour? GetBehaviour<TBehaviour>() where TBehaviour : class, IBehaviour;

    /// <summary>
    /// Tries to get a behaviour by its type.
    /// </summary>
    /// <param name="behaviour">Behaviour instance.</param>
    /// <typeparam name="TBehaviour">Behaviour type/</typeparam>
    /// <returns></returns>
    public bool TryGetBehaviour<TBehaviour>(out TBehaviour behaviour) where TBehaviour : class, IBehaviour;

    /// <summary>
    /// Adds a behaviour and behaviour options.
    /// </summary>
    /// <param name="behaviour"></param>
    /// <param name="options"></param>
    /// <typeparam name="TBehaviour"></typeparam>
    /// <typeparam name="TOptions"></typeparam>
    void AddBehaviour<TBehaviour, TOptions>(TBehaviour behaviour, TOptions options)
        where TBehaviour : IBehaviour
        where TOptions : IDiagramOptions;

    #endregion

    /// <summary>
    /// Creates a group in another group.
    /// </summary>
    /// <param name="parent">Container to which the new group will be added to.</param>
    /// <param name="group">Group to be added.</param>
    /// <typeparam name="TGroup">Group Type</typeparam>
    /// <returns></returns>
    void AddGroupTo<TGroup>(IGroupContainer parent, TGroup group)
        where TGroup : IGroup;

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="parentGroup">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <typeparam name="TNode">Node Type</typeparam>
    /// <returns></returns>
    void AddNodeTo<TNode>(INodeContainer parentGroup, TNode node)
        where TNode : INode;

    /// <summary>
    /// Creates a port.
    /// </summary>
    /// <param name="parent">Model to which the port will be added to.</param>
    /// <param name="port">Port that will be added.</param>
    /// <typeparam name="TPort">Port Type</typeparam>
    /// <returns></returns> 
    void AddPortTo<TPort>(IPortContainer parent, TPort port)
        where TPort : IPort;

    /// <summary>
    /// Creates a link.
    /// </summary>
    /// <param name="sourcePort">Source port of the link.</param>
    /// <param name="targetPort">Target port of the link.</param>
    /// <param name="link">Link that will be added to the container</param>
    /// <typeparam name="TLink">Link Type</typeparam>
    /// <returns></returns>
    void AddLinkTo<TLink>(ILinkContainer sourcePort, ILinkContainer? targetPort, TLink link)
        where TLink : ILink;

    /// <summary>
    /// Creates a layer.
    /// </summary>
    /// <param name="configure">Creation action.</param>
    /// <typeparam name="TLayer">Link Type</typeparam>
    /// <returns></returns>
    void AddLayer<TLayer>(TLayer layer)
        where TLayer : ILayer;

    /// <summary>
    /// Adds a node to the current layer.
    /// </summary>
    /// <param name="node"></param>
    void AddNode(INode node);

    /// <summary>
    /// Adds a group to the current layer.
    /// </summary>
    /// <param name="group"></param>
    void AddGroup(IGroup group);

    /// <summary>
    /// Adds a layer to the diagram.
    /// </summary>
    /// <param name="layer"></param>
    void AddLayer(ILayer layer);
}