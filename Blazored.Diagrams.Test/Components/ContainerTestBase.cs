using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Options.Diagram;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Virtualization;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public abstract class ContainerTestBase<TComponent> : TestContext
    where TComponent : IComponent
{
    protected readonly Mock<IDiagramService> _diagramServiceMock;
    protected readonly Mock<IEventAggregator> _eventServiceMock;
    protected readonly Mock<IOptionsContainer> _optionsMock;
    protected readonly Mock<IVirtualizationService> _virtualizationServiceMock;
    protected readonly MockedComponentRegistry _componentRegistry;
    protected readonly ResizeObserverServiceMock _observerService;
    protected readonly JSRuntimeMock _jsRuntime;

    public ContainerTestBase()
    {
        _diagramServiceMock = new Mock<IDiagramService>();
        _eventServiceMock = new Mock<IEventAggregator>();
        _componentRegistry = new();
        _observerService = new ResizeObserverServiceMock();
        _optionsMock = new Mock<IOptionsContainer>();
        _jsRuntime = new JSRuntimeMock();
        _virtualizationServiceMock = new();

        _optionsMock.SetupGet(s => s.Virtualization).Returns(new VirtualizationOptions());
        _optionsMock.SetupGet(s => s.Styling).Returns(new DiagramStyleOptions());
        _diagramServiceMock.SetupGet(s => s.Events).Returns(_eventServiceMock.Object);
        _diagramServiceMock.SetupGet(s => s.Options).Returns(_optionsMock.Object);
        _diagramServiceMock.SetupGet(s => s.Diagram).Returns(new Diagram());

        Services.AddSingleton(_diagramServiceMock.Object);
        Services.AddSingleton(_componentRegistry.Object);
        Services.AddSingleton(_observerService.Object);
        Services.AddSingleton(_jsRuntime.Mock.Object);
        Services.AddSingleton(_virtualizationServiceMock.Object);
    }
}