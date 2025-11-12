using Blazor.Flows.Events;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Blazor.Flows.Sandbox.Pages.Nodes;

public partial class CustomNodeOverview
{
    [Inject] public IDiagramService Service { get; set; } = null!;
    private int _nodeCount;
    private MyCustomNode? _selectedNode;
    private IDisposable? _subscription;

    protected override Task OnInitializedAsync()
    {
        AddCustomNode();

        _subscription = Service.Events.SubscribeTo<NodeSelectionChangedEvent>(HandleNodeSelectionChanged);

        return base.OnInitializedAsync();
    }

    private void HandleNodeSelectionChanged(NodeSelectionChangedEvent e)
    {
        var node = e.Model as MyCustomNode;

        if (node != null)
        {
            if (node.IsSelected)
            {
                _selectedNode = node;
            }
            else if (_selectedNode == node)
            {
                _selectedNode = null;
            }
        }
        StateHasChanged();
    }

    private void AddCustomNode()
    {
        _nodeCount++;
        var node = new MyCustomNode { Name = $"Node #{_nodeCount}" };
        node.SetPosition(100 + _nodeCount % 5 * 50, 100 + _nodeCount / 5 * 50);

        Service.AddNode(node);
    }

    private void RemoveSelectedNode()
    {
        if (_selectedNode != null)
        {
            Service.RemoveNode(_selectedNode);
            _selectedNode = null;
        }
    }

    private void AddPortToSelectedNode()
    {
        if (_selectedNode == null)
        {
            return;
        }

        var sides = new List<PortAlignment>
            { PortAlignment.Left, PortAlignment.Top, PortAlignment.Right, PortAlignment.Bottom };
        var nextSide = sides.FirstOrDefault(side => _selectedNode.Ports.All(p => p.Alignment != side));

        if (_selectedNode.Ports.Count != 4)
        {
            Service.AddPortTo(_selectedNode, new Port { Alignment = nextSide });
        }
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }
}