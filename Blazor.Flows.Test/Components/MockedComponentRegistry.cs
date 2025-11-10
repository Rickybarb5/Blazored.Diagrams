using Blazor.Flows.Components.Models;
using Blazor.Flows.Groups;
using Blazor.Flows.Links;
using Blazor.Flows.Nodes;
using Blazor.Flows.Ports;
using Blazor.Flows.Services.Registry;
using Moq;

namespace Blazor.Flows.Test.Components;

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