using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
///     Behaviour for creating a link when a port is clicked.
/// </summary>
public class DrawLinkBehavior : IBehaviour
{
    private readonly IDiagramService _service;
    private readonly DrawLinkOptions _options;
    private int _initialClickX;
    private int _initialClickY;
    private bool _isCreatingLink;

    private IPort? _sourcePort;
    private IPort? _targetPort;
    private List<IDisposable> _subscriptions = [];

    /// <summary>
    ///     Initializes a new instance of <see cref="DrawLinkBehavior"/>
    /// </summary>
    /// <param name="service"></param>
    public DrawLinkBehavior(IDiagramService service)
    {
        _service = service;
        _options = _service.Diagram.Options.Get<DrawLinkOptions>()!;
        _options.OnEnabledChanged += OnEnabledChanged;
        OnEnabledChanged(_options.IsEnabled);
    }

    private void OnEnabledChanged(bool enabled)
    {
        if (enabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
        }
    }

    private ILink? Link { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeSubscriptions();
        _options.OnEnabledChanged -= OnEnabledChanged;
    }


    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
        [
            _service.Events.SubscribeTo<PortPointerDownEvent>(StartLinkCreation),
            _service.Events.SubscribeTo<PortPointerUpEvent>(CreateLink),
            _service.Events.SubscribeTo<DiagramPointerMoveEvent>(UpdateTargetPosition),
            _service.Events.SubscribeTo<DiagramPointerUpEvent>(OnDiagramPointerUp),
        ];
    }

    /// <summary>
    ///     1-When pointer is down on a port, start link creation.
    /// </summary>
    /// <param name="e"></param>
    private void StartLinkCreation(PortPointerDownEvent e)
    {
        Link = null;
        _sourcePort = null;
        _targetPort = null;
        _isCreatingLink = false;
        _initialClickX = 0;
        _initialClickY = 0;
        ClearUnboundedLinks();
        if (!_options.IsEnabled || !e.Model.CanCreateLink()) return;
        _isCreatingLink = true;
        _sourcePort = e.Model;
        _initialClickX = (int)e.Args.ClientX;
        _initialClickY = (int)e.Args.ClientY;

        Link = AddLink(_sourcePort, null, _options.LinkType);
        _service.Events.Publish(new DrawLinkStartEvent(Link));
        var startCoordinates = _sourcePort.GetCenterCoordinates();
        Link.SetTargetPosition(startCoordinates.CenterX, startCoordinates.CenterY);
    }
    
    private ILink AddLink(IPort sourcePort, IPort? targetPort, Type linkType)
    {
        // Ensure the provided type implements ILink
        if (!typeof(ILink).IsAssignableFrom(linkType))
        {
            throw new ArgumentException($"The type {linkType.Name} must implement {nameof(ILink)}.",
                nameof(linkType));
        }

        // Create an instance of linkType using reflection
        var link = (ILink?)Activator.CreateInstance(linkType);
        if (link is null)
        {
            throw new InvalidOperationException(
                $"Link couldn't be created with component type: {linkType.Name}");
        }

        if (targetPort is not null)
        {
            CanLinkBeCreated(sourcePort, targetPort);
        }

        sourcePort.OutgoingLinks.Add(link);
        targetPort?.IncomingLinks.Add(link);
        return link;
    }
    
    /// <summary>
    /// Checks if a link can be created between two ports.
    /// </summary>
    /// <param name="sourcePort"></param>
    /// <param name="targetPort"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void CanLinkBeCreated(IPort sourcePort, IPort targetPort)
    {
        if (_service.Diagram.AllPorts.FirstOrDefault(x => x.Id == sourcePort.Id) is null)
        {
            throw new InvalidOperationException(
                "Source port does not belong to the diagram");
        }

        if (_service.Diagram.AllPorts.FirstOrDefault(x => x.Id == targetPort.Id) is null)
        {
            throw new InvalidOperationException(
                "Target port does not belong to the diagram.");
        }

        if (!sourcePort.CanCreateLink())
        {
            throw new InvalidOperationException(
                $"Source port does not allow link creation. Check{nameof(IPort.CanCreateLink)} on {sourcePort.GetType().Name}.");
        }

        if (!sourcePort.CanConnectTo(targetPort))
        {
            throw new InvalidOperationException(
                $"Source port cannot connect to Target Port. Check{nameof(IPort.CanConnectTo)} on {sourcePort.GetType().Name}.");
        }
    }

    /// <summary>
    ///     2-Pointer is moving.
    /// </summary>
    /// <param name="e"></param>
    private void UpdateTargetPosition(DiagramPointerMoveEvent e)
    {
        _isCreatingLink = e.Args.Buttons == 1 && _sourcePort is not null;

        if (_isCreatingLink && Link is not null)
        {
            // Calculate new target position based on the movement from the initial click
            var newX = (int)(_sourcePort!.PositionX + (e.Args.ClientX - _initialClickX) / _service.Diagram.Zoom);
            var newY = (int)(_sourcePort.PositionY + (e.Args.ClientY - _initialClickY) / _service.Diagram.Zoom);

            Link.SetTargetPosition(newX, newY);
        }
    }

    /// <summary>
    ///     3-When the pointer is released on top of a port.
    /// </summary>
    /// <param name="e"></param>
    private void CreateLink(PortPointerUpEvent e)
    {
        if (_isCreatingLink && _sourcePort is not null && Link is not null)
        {
            _targetPort = e.Model;
            OnDiagramPointerUp(null);
        }

        ClearUnboundedLinks();
    }

    /// <summary>
    ///     4-When the mouse is released.
    /// </summary>
    /// <param name="e"></param>
    private void OnDiagramPointerUp(DiagramPointerUpEvent? e)
    {
        if (CanLinkToTarget())
        {
            _targetPort!.IncomingLinks.Add(Link!);
            _service.Events.Publish(new DrawLinkCreatedEvent(Link!));
        }
        else
        {
            _service.Events.Publish(new DrawLinkCancelledEvent(Link!));
        }

        ClearUnboundedLinks();

        Link = null;
        _sourcePort = null;
        _targetPort = null;
        _isCreatingLink = false;
        _initialClickX = 0;
        _initialClickY = 0;
    }

    private bool CanLinkToTarget()
    {
        return _targetPort is not null && _sourcePort is not null && Link is not null &&
               _sourcePort.CanConnectTo(_targetPort) && _targetPort.CanConnectTo(_sourcePort);
    }

    private void ClearUnboundedLinks()
    {
        var unboundedLinks = _service.Diagram.AllLinks.Where(x => x.TargetPort is null);
        foreach (var unboundedLink in unboundedLinks) unboundedLink.Dispose();
    }
}