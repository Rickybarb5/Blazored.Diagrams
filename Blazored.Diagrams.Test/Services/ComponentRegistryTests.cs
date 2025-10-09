using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Options;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Registry;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Moq;

namespace Blazored.Diagrams.Test.Services;

public class ComponentRegistryTests
{
    private readonly Mock<IOptions<BlazoredDiagramsOptions>> _optionsMock;
    private readonly BlazoredDiagramsOptions _options;

    public ComponentRegistryTests()
    {
        _options = new BlazoredDiagramsOptions
        {
            Assemblies = [typeof(ComponentRegistryTests).Assembly]
        };
        _optionsMock = new Mock<IOptions<BlazoredDiagramsOptions>>();
        _optionsMock.Setup(x => x.Value).Returns(_options);
    }

    [Fact]
    public void Constructor_WithValidAssembly_RegistersComponents()
    {
        // Arrange & Act
        var registry = new ComponentRegistry(_optionsMock.Object);

        // Assert
        var componentType = registry.GetComponentType(typeof(TestNode));
        Assert.Equal(typeof(TestComponent), componentType);
    }

    [Fact]
    public void RegisterComponent_WithNonComponentType_ThrowsArgumentException()
    {
        // Arrange
        var registry = new ComponentRegistry(_optionsMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            registry.RegisterComponent(typeof(string), typeof(TestNode)));
    }

    [Fact]
    public void Constructor_WithInheritedInterface_RegistersCorrectComponent()
    {
        // Arrange & Act
        var registry = new ComponentRegistry(_optionsMock.Object);

        // Assert
        var componentType = registry.GetComponentType(typeof(InheritedTestNode));
        Assert.Equal(typeof(TestComponent), componentType);
    }

    [Fact]
    public void GetComponentType_WithUnregisteredType_ThrowsInvalidOperationException()
    {
        // Arrange
        var registry = new ComponentRegistry(_optionsMock.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            registry.GetComponentType(typeof(UnregisteredTestNode)));
    }

    [Fact]
    public void Constructor_WithDirectAndInheritedInterface_PrefersDirect()
    {
        // Arrange & Act
        var registry = new ComponentRegistry(_optionsMock.Object);

        // Assert
        var componentType = registry.GetComponentType(typeof(MultipleInterfaceTestNode));
        Assert.Equal(typeof(AnotherTestComponent), componentType);
    }

    // Test classes
    private class TestComponent : ComponentBase, IComponent;

    private class AnotherTestComponent : ComponentBase, IComponent;

    private class TestNode : Node, IHasComponent<TestComponent>;

    private class UnregisteredTestNode : INode
    {
        private ITypedEvent<NodeSizeChangedEvent> _sizeChanged;
        private ITypedEvent<NodePositionChangedEvent> _positionChanged;
        private ITypedEvent<NodeSelectionChangedEvent> _selectionChanged;
        private ITypedEvent<NodeVisibilityChangedEvent> _visibilityChanged;
        private ITypedEvent<PortAddedToNodeEvent> _portAdded;
        private ITypedEvent<PortRemovedFromNodeEvent> _portRemoved;
        private ITypedEvent<PortAddedEvent> _portAdded1;
        private ITypedEvent<PortRemovedEvent> _portRemoved1;
        public string Id { get; init; }
        public bool IsVisible { get; set; }
        public bool IsSelected { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public void SetPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public void SetSize(int width, int height)
        {
            throw new NotImplementedException();
        }

        public ObservableList<IPort> Ports { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetPositionInternal(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SetSizeInternal(int width, int height)
        {
            throw new NotImplementedException();
        }

        ITypedEvent<NodeSizeChangedEvent> INode.OnSizeChanged
        {
            get => _sizeChanged;
            init => _sizeChanged = value;
        }

        ITypedEvent<NodePositionChangedEvent> INode.OnPositionChanged
        {
            get => _positionChanged;
            init => _positionChanged = value;
        }

        ITypedEvent<NodeSelectionChangedEvent> INode.OnSelectionChanged
        {
            get => _selectionChanged;
            init => _selectionChanged = value;
        }

        ITypedEvent<NodeVisibilityChangedEvent> INode.OnVisibilityChanged
        {
            get => _visibilityChanged;
            init => _visibilityChanged = value;
        }

        ITypedEvent<PortAddedEvent> INode.OnPortAdded
        {
            get => _portAdded1;
            init => _portAdded1 = value;
        }

        ITypedEvent<PortRemovedEvent> INode.OnPortRemoved
        {
            get => _portRemoved1;
            init => _portRemoved1 = value;
        }

        ITypedEvent<PortAddedToNodeEvent> INode.OnPortAddedToNode
        {
            get => _portAdded;
            init => _portAdded = value;
        }

        public ITypedEvent<PortRemovedFromNodeEvent> OnPortRemovedFromNode { get; init; }


        public event Action<INode, int, int, int, int>? OnSizeChanged;
        public event Action<INode, int, int, int, int>? OnPositionChanged;
        public event Action<INode>? OnSelectionChanged;
        public event Action<INode>? OnVisibilityChanged;
        public event Action<INode, IPort>? OnPortAdded;
        public event Action<INode, IPort>? OnPortRemoved;
    }

    private abstract class BaseTestNode : Node, IHasComponent<TestComponent>;

    private class InheritedTestNode : BaseTestNode;

    private class MultipleInterfaceTestNode : BaseTestNode, IHasComponent<AnotherTestComponent>;
}