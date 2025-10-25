using System.Diagnostics.CodeAnalysis;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Behaviours;

/// <summary>
/// Behaviour for logging diagram events to the static logger.
/// </summary>
[ExcludeFromCodeCoverage]
public class EventLoggingBehavior : BaseBehaviour
{
    private readonly IDiagramService _diagramService;
    private readonly LoggingBehaviourOptions _behaviourOptions;

    /// <summary>
    /// Instantiates a new <see cref="EventLoggingBehavior"/>
    /// </summary>
    /// <param name="diagramService">The diagram service</param>
    public EventLoggingBehavior(IDiagramService diagramService)
    {
        _diagramService = diagramService;
        _behaviourOptions = _diagramService.Behaviours.GetBehaviourOptions<LoggingBehaviourOptions>()!;
        _behaviourOptions.OnEnabledChanged.Subscribe(OnEnabledChanged);
        OnEnabledChanged(_behaviourOptions.IsEnabled);
    }

    private void OnEnabledChanged(BehaviourEnabledEvent ev)
    {
        OnEnabledChanged(ev.IsEnabled);
    }
    
    private void OnEnabledChanged(bool isEnabled)
    {
        if (isEnabled)
        {
            SubscribeToEvents();
        }
        else
        {
            DisposeSubscriptions();
        }
    }

    private void SubscribeToEvents()
    {
        Subscriptions =
        [
            // Layer events
            _diagramService.Events.SubscribeTo<LayerAddedEvent>(e =>
                Console.WriteLine($"[{nameof(LayerAddedEvent)}] Layer {e.Model.Id} added")),
            _diagramService.Events.SubscribeTo<LayerRemovedEvent>(e =>
                Console.WriteLine($"[{nameof(LayerRemovedEvent)}] Layer {e.Model.Id} removed")),
            _diagramService.Events.SubscribeTo<LayerVisibilityChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(LayerVisibilityChangedEvent)}] Layer {e.Model.Id} visibility changed to {e.Model.IsVisible}")),
            _diagramService.Events.SubscribeTo<CurrentLayerChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(CurrentLayerChangedEvent)}] Layer {e.OldLayer.Id} was replaced with {e.NewLayer.Id}.")),

            // Node events
            _diagramService.Events.SubscribeTo<NodeAddedEvent>(e =>
                Console.WriteLine($"[{nameof(NodeAddedEvent)}] Node {e.Model.Id} added")),
            _diagramService.Events.SubscribeTo<NodeRemovedEvent>(e =>
                Console.WriteLine($"[{nameof(NodeRemovedEvent)}] Node {e.Model.Id} removed")),
            _diagramService.Events.SubscribeTo<NodeAddedToLayerEvent>(e =>
                Console.WriteLine($"[{nameof(NodeAddedToLayerEvent)}] Node {e.Node.Id} added to layer {e.Model.Id}")),
            _diagramService.Events.SubscribeTo<NodeRemovedFromLayerEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(NodeRemovedFromLayerEvent)}] Node {e.Node.Id} removed from layer {e.Model.Id}")),
            _diagramService.Events.SubscribeTo<NodeVisibilityChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(NodeVisibilityChangedEvent)}] Node {e.Model.Id} visibility changed to {e.Model.IsVisible}")),
            _diagramService.Events.SubscribeTo<NodeSelectionChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(NodeSelectionChangedEvent)}] Node {e.Model.Id} selection changed to {e.Model.IsSelected}")),
            _diagramService.Events.SubscribeTo<NodePositionChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(NodePositionChangedEvent)}] Node {e.Model.Id} moved from ({e.OldX},{e.OldY}) to ({e.NewX},{e.NewY})")),
            _diagramService.Events.SubscribeTo<NodeSizeChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(NodeSizeChangedEvent)}] Node {e.Model.Id} resized from ({e.OldWidth}x{e.OldHeight}) to ({e.NewWidth}x{e.NewHeight})")),

            // Group events
            _diagramService.Events.SubscribeTo<GroupAddedEvent>(e =>
                Console.WriteLine($"[{nameof(GroupAddedEvent)}] Group {e.Model.Id} added")),
            _diagramService.Events.SubscribeTo<GroupRemovedEvent>(e =>
                Console.WriteLine($"[{nameof(GroupRemovedEvent)}] Group {e.Model.Id} removed")),
            _diagramService.Events.SubscribeTo<GroupPositionChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(GroupPositionChangedEvent)}] Group {e.Model.Id} moved from ({e.OldX},{e.OldY}) to ({e.NewX},{e.NewY})")),
            _diagramService.Events.SubscribeTo<GroupSizeChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(GroupSizeChangedEvent)}] Group {e.Model.Id} resized from ({e.OldWidth}x{e.OldHeight}) to ({e.NewWidth}x{e.NewHeight})")),
            _diagramService.Events.SubscribeTo<GroupPaddingChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(GroupPaddingChangedEvent)}] Group {e.Model.Id} padding changed from {e.OldPadding} to {e.NewPadding}")),

            // Port events
            _diagramService.Events.SubscribeTo<PortAddedEvent>(e =>
                Console.WriteLine($"[{nameof(PortAddedEvent)}] Port {e.Model.Id} added")),
            _diagramService.Events.SubscribeTo<PortRemovedEvent>(e =>
                Console.WriteLine($"[{nameof(PortRemovedEvent)}] Port {e.Model.Id} removed")),
            _diagramService.Events.SubscribeTo<PortVisibilityChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(PortVisibilityChangedEvent)}] Port {e.Model.Id} visibility changed to {e.Model.IsVisible}")),
            _diagramService.Events.SubscribeTo<PortPositionChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(PortPositionChangedEvent)}] Port {e.Model.Id} moved from ({e.OldX},{e.OldY}) to ({e.NewX},{e.NewY})")),
            _diagramService.Events.SubscribeTo<PortSizeChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(PortSizeChangedEvent)}] Port {e.Model.Id} resized from ({e.OldWidth}x{e.OldHeight}) to ({e.NewWidth}x{e.NewHeight})")),
            _diagramService.Events.SubscribeTo<PortParentChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(PortParentChangedEvent)}] Port {e.Model.Id} parent changed from {e.OldParent.Id} to {e.NewParent.Id}")),
            _diagramService.Events.SubscribeTo<PortAlignmentChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(PortAlignmentChangedEvent)}] Port {e.Model.Id} alignment from {e.OldPosition} to {e.NewPosition}")),
            _diagramService.Events.SubscribeTo<PortJustificationChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(PortJustificationChangedEvent)}] Port {e.Model.Id} parent changed from {e.OldJustification} to {e.NewJustification}")),
            _diagramService.Events.SubscribeTo<IncomingLinkAddedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(IncomingLinkAddedEvent)}] Incoming link {e.AddedLink.Id} added to port {e.Model.Id}")),
            _diagramService.Events.SubscribeTo<IncomingLinkRemovedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(IncomingLinkRemovedEvent)}] Incoming link {e.RemovedLink.Id} removed from port {e.Model.Id}")),
            _diagramService.Events.SubscribeTo<OutgoingLinkAddedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(OutgoingLinkAddedEvent)}] Outgoing link {e.AddedLink.Id} added to port {e.Model.Id}")),
            _diagramService.Events.SubscribeTo<OutgoingLinkRemovedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(OutgoingLinkRemovedEvent)}] Outgoing link {e.RemovedLink.Id} removed from port {e.Model.Id}")),

            // Link events
            _diagramService.Events.SubscribeTo<LinkAddedEvent>(e =>
                Console.WriteLine($"[{nameof(LinkAddedEvent)}] Link {e.Model.Id} added")),
            _diagramService.Events.SubscribeTo<LinkRemovedEvent>(e =>
                Console.WriteLine($"[{nameof(LinkRemovedEvent)}] Link {e.Model.Id} removed")),
            _diagramService.Events.SubscribeTo<LinkSourcePortChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(LinkSourcePortChangedEvent)}] Link {e.Model.Id} source port changed from {e.OldSourcePort.Id} to {e.NewSourcePort.Id}")),
            _diagramService.Events.SubscribeTo<LinkTargetPortChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(LinkTargetPortChangedEvent)}] Link {e.Model.Id} target port changed from {e.OldTargetPort?.Id.ToString() ?? "none"} to {e.NewTargetPort?.Id.ToString() ?? "none"}")),
            _diagramService.Events.SubscribeTo<LinkTargetPositionChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(LinkTargetPositionChangedEvent)}] Link {e.Model.Id} target position changed from ({e.OldX},{e.OldY}) to ({e.TargetPositionX},{e.TargetPositionY})")),

            // Diagram events
            _diagramService.Events.SubscribeTo<DiagramPanChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(DiagramPanChangedEvent)}] Diagram panned from ({e.OldPanX},{e.OldPanY}) to ({e.PanX},{e.PanY})")),
            _diagramService.Events.SubscribeTo<DiagramZoomChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(DiagramZoomChangedEvent)}] Diagram zoom changed from {e.OldZoom:F2} to {e.NewZoom:F2}")),
            _diagramService.Events.SubscribeTo<DiagramPositionChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(DiagramPositionChangedEvent)}] Diagram position changed from ({e.OldX},{e.OldY}) to ({e.NewX},{e.NewY})")),
            _diagramService.Events.SubscribeTo<DiagramSizeChangedEvent>(e =>
                Console.WriteLine(
                    $"[{nameof(DiagramSizeChangedEvent)}] Diagram size changed from ({e.OldWidth}x{e.OldHeight}) to ({e.NewWidth}x{e.NewHeight})"))
        ];
    }
}