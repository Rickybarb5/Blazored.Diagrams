using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;

namespace Blazored.Diagrams.Sandbox.Pages.Examples.DragAndDrop;

public class DragAndDropBehaviour : BaseBehaviour
{
    private readonly IDiagramService _service;
    private bool _isDragging;
    private bool _isEnabled = true;
    private bool _isDragActive;
    private List<IDisposable> _subscriptions;

    public IPosition? Model;

    public DragAndDropBehaviour(IDiagramService service)
    {
        _service = service;
        SubscribeToEvents();
    }

    public new void Dispose()
    {
        DisposeSubscriptions();
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (value != _isEnabled)
            {
                _isEnabled = value;
                if (value)
                    SubscribeToEvents();
                else
                    DisposeSubscriptions();
            }
        }
    }

    private void SubscribeToEvents()
    {
        _subscriptions =
        [
            _service.Events.SubscribeTo<DiagramPointerMoveEvent>(OnPointerMove),
            _service.Events.SubscribeTo<DiagramPointerUpEvent>(OnPointerUp),
        ];
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.DisposeAll();
    }

    public void StartDrag()
    {
        _isDragActive = true;
    }

    public void EndDrag()
    {
        _isDragActive = false;
        Model = null;
    }

    private void OnPointerMove(DiagramPointerMoveEvent e)
    {
        if (!_isDragActive || Model is null) return;

        _isDragging = true;
        Model.PositionX = (int)e.Args.OffsetX;
        Model.PositionY = (int)e.Args.OffsetY;
    }

    private void OnPointerUp(DiagramPointerUpEvent e)
    {
        if (_isDragging && _isDragActive && Model is not null)
        {
            if (Model is INode node)
            {
                _service.Add.Node(node);
            }
        }

        _isDragging = false;
        _isDragActive = false;
        Model = null;
    }
}