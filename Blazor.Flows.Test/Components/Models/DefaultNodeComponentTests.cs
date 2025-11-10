using Blazor.Flows.Components.Models;
using Blazor.Flows.Nodes;
using Blazor.Flows.Services.Diagrams;
using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Flows.Test.Components;

public class DefaultNodeComponentTests : TestContext
{
    private const string DefaultCssClass = "default-node";

    // Mocks for dependencies
    private readonly IDiagramService _diagramService = new DiagramService();
    private readonly Node _testNode;

    public DefaultNodeComponentTests()
    {
        Services.AddSingleton(_diagramService);
        _testNode = new Node();
        _diagramService.AddNode(_testNode);
    }

    [Fact]
    public void Component_Renders_With_Default_Class_And_NoContent()
    {
        // Arrange
        var cut = RenderComponent<DefaultNodeComponent>(parameters => parameters
            .Add(p => p.Node, _testNode)
            .AddCascadingValue(_diagramService)
        );

        // Act
        // Assert
        cut.Find("div").MarkupMatches($@"<div class=""{DefaultCssClass}""></div>");
    }

    [Fact]
    public void Component_Renders_Content_When_Provided()
    {
        // Arrange
        var expectedContent = "<span>Custom Node Content</span>";

        // Act
        var cut = RenderComponent<DefaultNodeComponent>(parameters => parameters
            .Add(p => p.Node, _testNode)
            .AddCascadingValue(_diagramService)
            .Add(p => p.Content, (builder) => builder.AddMarkupContent(0, expectedContent))
        );

        // Assert
        cut.Find("div").MarkupMatches($@"<div class=""{DefaultCssClass}"">{expectedContent}</div>");
    }

    [Fact]
    public void Component_Applies_Selected_Class_On_NodeSelectionChangedEvent()
    {
        // Arrange
        _testNode.IsSelected = false; // Initial state

        var cut = RenderComponent<DefaultNodeComponent>(parameters => parameters
            .Add(p => p.Node, _testNode)
            .AddCascadingValue(_diagramService)
        );

        _testNode.IsSelected = true;
        cut.Render();

        // Assert
        var expectedClass = $"{DefaultCssClass} selected";
        cut.Find("div").MarkupMatches($@"<div class=""{expectedClass}""></div>");
    }

    [Fact]
    public void Component_Removes_Selected_Class_On_NodeDeselected()
    {
        // Arrange
        _testNode.IsSelected = true;

        var cut = RenderComponent<DefaultNodeComponent>(parameters => parameters
            .Add(p => p.Node, _testNode)
            .AddCascadingValue(_diagramService)
        );

        // Act
        _testNode.IsSelected = false;
        cut.Render();

        // Assert
        // The class should revert to just the default class.
        var expectedClass = DefaultCssClass;
        cut.Find("div").MarkupMatches($@"<div class=""{expectedClass}""></div>");
    }
}