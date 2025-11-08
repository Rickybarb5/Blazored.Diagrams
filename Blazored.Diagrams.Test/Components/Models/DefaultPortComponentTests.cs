using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public class DefaultPortComponentTests : TestContext
{
    private readonly Mock<IDiagramService> _diagramService = new ();
    private readonly Port _testPort;

    public DefaultPortComponentTests()
    {
        Services.AddSingleton(_diagramService);
        _testPort = new Port();
    }

    [Fact]
    public void InitialRender_NoLinks_HasDefaultClass()
    {
        // Act
        var cut = RenderComponent<DefaultPortComponent>(
            p => p.Add(x => x.Port, _testPort)
                .AddCascadingValue(_diagramService.Object)
        );

        // Assert
        Assert.Equal("default-port", cut.Find("div").ClassName);
    }

    [Fact]
    public void Render_WithOutgoingLinks_HasLinkedClass()
    {
        // Arrange
        var cut = RenderComponent<DefaultPortComponent>(
            p => p.Add(x => x.Port, _testPort)
                .AddCascadingValue(_diagramService.Object)
        );
        _testPort.OutgoingLinks.AddInternal(new LineLink {});

        // Act
        cut.Render();

        // Assert
        Assert.Equal("default-port linked", cut.Find("div").ClassName);
    }
    
    [Fact]
    public void Render_WithIncomingLinks_HasLinkedClass()
    {
        // Arrange
        var cut = RenderComponent<DefaultPortComponent>(
            p => p.Add(x => x.Port, _testPort)
                .AddCascadingValue(_diagramService.Object)
        );
        _testPort.IncomingLinks.AddInternal(new LineLink {});

        // Act
        cut.Render();

        // Assert
        Assert.Equal("default-port linked", cut.Find("div").ClassName);
    }

}