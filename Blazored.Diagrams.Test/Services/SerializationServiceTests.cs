using System.Text.Json;
using System.Text.Json.Nodes;
using Blazored.Diagrams.Diagrams;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Diagrams;
using Blazored.Diagrams.Services.Providers;
using Blazored.Diagrams.Services.Serialization;

namespace Blazored.Diagrams.Test.Services;

public class SerializationServiceTests
{
    private SerializationService service = new();
    private IDiagramService diagramService;

    public SerializationServiceTests()
    {
        var provider = new DiagramServiceProvider();
        diagramService = provider.GetDiagramService(new Diagram());
    }

    [Fact]
    public void Save_Returns_Correct_Json()
    {
        var node = new Node();
        var group = new Group();
        var groupNode = new Node();
        var nodePort = new Port();
        var groupPort = new Port();
        var link = new Link();
        diagramService.Add.Node(node)
            .Group(group)
            .NodeTo(group, groupNode)
            .PortTo(node, nodePort)
            .PortTo(group, groupPort)
            .AddLinkTo(nodePort, groupPort, link);
        var json = service.ToJson(diagramService.Diagram);
        
        // Parse for traversal

        // Assert
        
        var jsonNode = JsonNode.Parse(json)!;
        var layerNode = jsonNode["Layers"]![0]!;
        var groupJson = layerNode["Groups"]![0];
        var nodeNodeJson = layerNode["Nodes"]![0];
        var nodePortJson = nodeNodeJson!["Ports"][0];
        var groupPortJson = groupJson!["Ports"][0];
        var incomingLinkJson = groupPortJson!["IncomingLinks"][0];
        var outgoingLinkJson = nodePortJson!["OutgoingLinks"][0];
        Assert.Equal(diagramService.Diagram.CurrentLayer.Id.ToString(),layerNode["Id"]?.ToString());
        Assert.Equal(group.Id.ToString(),groupJson!["Id"]!.ToString());
        Assert.Equal(node.Id.ToString(),nodeNodeJson!["Id"]!.ToString());
        Assert.Equal(nodePort.Id.ToString(),nodePortJson!["Id"]!.ToString());
        Assert.Equal(groupPort.Id.ToString(),groupPortJson!["Id"]!.ToString());
        Assert.Equal(link.Id.ToString(),incomingLinkJson!["Id"]!.ToString());
        Assert.Contains(link.Id.ToString(),outgoingLinkJson!.ToJsonString());
    }
}