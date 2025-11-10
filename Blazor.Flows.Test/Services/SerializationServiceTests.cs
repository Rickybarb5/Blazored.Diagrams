using Blazor.Flows.Diagrams;
using Blazor.Flows.Groups;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Diagrams;
using Blazor.Flows.Services.Serialization;

namespace Blazor.Flows.Test.Services;

public class SerializationServiceTests
{
    private IDiagramService diagramService;
    private ISerializationService serializationService;

    public SerializationServiceTests()
    {
        diagramService = new DiagramService();
        serializationService = new SerializationService();
    }

    [Fact]
    public void Serialization_Works()
    {
        // Arrange
        var node = new Node();
        var group = new Group();
        var groupNode = new Node();
        var nodePort = new Port();
        var groupPort = new Port();
        var link = new LineLink();
        diagramService
            .AddNode(node)
            .AddGroup(group)
            .AddNodeTo(group, groupNode)
            .AddPortTo(node, nodePort)
            .AddPortTo(group, groupPort)
            .AddLinkTo(nodePort, groupPort, link);

        var expected = serializationService.ToJson(diagramService.Diagram);

        // Act
        var diagram = serializationService.FromJson<Diagram>(expected);
        var actual = serializationService.ToJson<Diagram>(diagram);

        Assert.Equal(expected, actual);
    }
}