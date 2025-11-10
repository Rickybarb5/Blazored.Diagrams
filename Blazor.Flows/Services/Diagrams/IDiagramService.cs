using Blazor.Flows.Diagrams;
using Blazor.Flows.Groups;
using Blazor.Flows.Interfaces;
using Blazor.Flows.Layers;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Behaviours;
using Blazor.Flows.Services.Events;

namespace Blazor.Flows.Services.Diagrams;

/// <summary>
/// Manages diagram events, behaviours and child models.
/// </summary>
public interface IDiagramService : IDisposable
{
    /// <summary>
    /// Diagram instance.
    /// </summary>
    IDiagram Diagram { get; }

    /// <summary>
    /// Allows access to event subscription and publish.
    /// </summary>
    IEventAggregator Events { get; set; }

    /// <summary>
    /// Allows behaviour customization.
    /// </summary>
    IBehaviourContainer Behaviours { get; set; }

    /// <summary>
    /// Customize diagram options.
    /// </summary>
    IOptionsContainer Options { get; set; }

    /// <summary>
    /// Replaces the diagram instance with another.
    /// </summary>
    /// <param name="diagram">A diagram instance</param>
    void UseDiagram(IDiagram diagram);

    /// <summary>
    ///     Returns the center point of a model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    (int CenterX, int CenterY) GetCenterCoordinates<T>(T model) where T : ISize, IPosition;


    /// <summary>
    /// Centers a model in another model.
    /// </summary>
    /// <param name="parameters">Parameters to center the model.</param>
    /// <typeparam name="TModel">Any diagram model that implements <see cref="ISize"/>, <see cref="IPosition"/>.</typeparam>
    /// <typeparam name="TContainer">Any diagram model that implements <see cref="ISize"/>, <see cref="IPosition"/></typeparam>
    void CenterIn<TContainer, TModel>(CenterInParameters<TContainer,TModel> parameters)
        where TModel : ISize, IPosition
        where TContainer : ISize, IPosition;

    /// <summary>
    ///     Changes the position of a model to be in the center of the viewport, accounting for pan and zoom.
    /// </summary>
    /// <param name="parameters">Parameters to center the model.</param>
    /// <typeparam name="TModel">Type of the model to be centered.</typeparam>
    public void CenterInViewport<TModel>(CenterInViewportParameters<TModel> parameters)
        where TModel : IPosition, ISize;
    
    /// <summary>
    /// Sets a layer as the current layer.
    /// </summary>
    /// <param name="layer">Layer to be used.</param>
    void UseLayer(ILayer layer);
    
    /// <summary>
    /// Removes a node from the diagram.
    /// </summary>
    /// <param name="nodeToRemove"></param>
    /// <returns></returns>
    IDiagramService RemoveNode(INode nodeToRemove);

    /// <summary>
    /// Removes a group from the diagram.
    /// </summary>
    /// <param name="groupToRemove"></param>
    /// <returns></returns>
    IDiagramService RemoveGroup(IGroup groupToRemove);

    /// <summary>
    /// Removes a layer from the diagram.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    IDiagramService RemoveLayer(ILayer layer);

    /// <summary>
    /// Removes a port.
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    IDiagramService RemovePort(IPort port);

    /// <summary>
    /// Removes a link
    /// </summary>
    /// <param name="linkToRemove"></param>
    /// <returns></returns>
    IDiagramService RemoveLink(ILink linkToRemove);

    /// <summary>
    /// Creates a group in another group.
    /// </summary>
    /// <param name="parent">Container to which the new group will be added to.</param>
    /// <param name="group">Group to be added.</param>
    /// <returns></returns>
    IDiagramService AddGroupTo(IGroupContainer parent, IGroup group);

    /// <summary>
    /// Adds a node to a node container.
    /// </summary>
    /// <param name="nodeContainer">Group to which the node will be added to.</param>
    /// <param name="node">Node to be added to the container.</param>
    /// <returns></returns>
    IDiagramService AddNodeTo(INodeContainer nodeContainer, INode node);

    /// <summary>
    /// Creates a port.
    /// </summary>
    /// <param name="parent">Model to which the port will be added to.</param>
    /// <param name="port">Port that will be added.</param>
    /// <returns></returns> 
    IDiagramService AddPortTo(IPortContainer parent, IPort port);

    /// <summary>
    /// Creates a link.
    /// </summary>
    /// <param name="sourcePort">Source port of the link.</param>
    /// <param name="targetPort">Target port of the link.</param>
    /// <param name="link">Link that will be added to the container</param>
    /// <returns></returns>
    IDiagramService AddLinkTo(ILinkContainer sourcePort, ILinkContainer? targetPort, ILink link);

    /// <summary>
    /// Adds a node to the diagram.
    /// </summary>
    /// <param name="node">Node to be added.</param>
    IDiagramService AddNode(INode node);

    /// <summary>
    /// Adds a group to the current layer.
    /// </summary>
    /// <param name="group">Group to be added.</param>
    IDiagramService AddGroup(IGroup group);

    /// <summary>
    /// Adds a layer to the diagram.
    /// </summary>
    /// <param name="layer">Layer to be added.</param>
    IDiagramService AddLayer(ILayer layer);

    /// <summary>
    /// Saves the current diagram instance to json.
    /// </summary>
    /// <param name="diagram"></param>
    /// <typeparam name="TDiagram"></typeparam>
    /// <returns></returns>
    string Save<TDiagram>(TDiagram diagram)
        where TDiagram : IDiagram;

    /// <summary>
    /// Loads a JSON and uses the deserialization as the new diagram for the diagram service.
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="TDiagram"></typeparam>
    /// <returns></returns>
    void LoadAndUse<TDiagram>(string json)
        where TDiagram : IDiagram;

    /// <summary>
    /// Loads a JSON and uses the deserialization as the new diagram for the diagram service.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    void LoadAndUse(string json);

    /// <summary>
    /// Loads a JSON and returns a diagram object;
    /// </summary>
    /// <param name="json">The Json string.</param>
    /// <typeparam name="TDiagram">The Diagram type.</typeparam>
    /// <returns></returns>
    TDiagram Load<TDiagram>(string json)
        where TDiagram : IDiagram;

    /// <summary>
    /// Loads a JSON and returns the  <see cref="IDiagram"/> object;
    /// </summary>
    /// <param name="json">The Json string.</param>
    /// <returns></returns>
    IDiagram Load(string json);

    /// <summary>
    /// Zooms into an object
    /// </summary>
    /// <param name="parameters">Parameters to zoom the model.</param>
    void ZoomToModel<TModel>(ZoomToModelParameters<TModel> parameters)
        where TModel : IPosition, ISize;
    
    /// <summary>
    /// Unselects all models in the diagram.
    /// </summary>
    void UnselectAll();

    /// <summary>
    /// Selects all models in the diagram.
    /// </summary>
    void SelectAll();

    /// <summary>
    ///  Sets the diagram zoom.
    /// </summary>
    /// <param name="zoom">Desired zoom value.</param>
    void SetZoom(double zoom);
}