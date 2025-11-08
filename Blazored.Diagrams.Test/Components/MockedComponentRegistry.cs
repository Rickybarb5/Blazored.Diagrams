using Blazored.Diagrams.Components.Models;
using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Links;
using Blazored.Diagrams.Nodes;
using Blazored.Diagrams.Ports;
using Blazored.Diagrams.Services.Registry;
using Moq;

namespace Blazored.Diagrams.Test.Components;

public class MockedComponentRegistry
{
    private Mock<IComponentRegistry> mock;
    public IComponentRegistry Object => mock.Object;

    public MockedComponentRegistry()
    {
        mock = new Mock<IComponentRegistry>();
        mock.Setup(s=>s.GetComponentType(It.Is<Type>(x=>x == typeof(Node)))).Returns(typeof(DefaultNodeComponent));
        mock.Setup(s=>s.GetComponentType(It.Is<Type>(x=>x == typeof(Group)))).Returns(typeof(DefaultGroupComponent));
        mock.Setup(s=>s.GetComponentType(It.Is<Type>(x=>x == typeof(Port)))).Returns(typeof(DefaultPortComponent));
        mock.Setup(s=>s.GetComponentType(It.Is<Type>(x=>x == typeof(CurvedLink)))).Returns(typeof(CurvedLinkComponent));
        mock.Setup(s=>s.GetComponentType(It.Is<Type>(x=>x == typeof(OrthogonalLink)))).Returns(typeof(OrthogonalLinkComponent));
        mock.Setup(s=>s.GetComponentType(It.Is<Type>(x=>x == typeof(LineLink)))).Returns(typeof(LineLinkComponent));
    }
    
}