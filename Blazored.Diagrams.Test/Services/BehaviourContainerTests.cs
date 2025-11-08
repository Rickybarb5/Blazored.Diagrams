using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Options.Behaviours;
using Blazored.Diagrams.Services.Behaviours;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Test.Services;

public class BehaviourContainerTests
{
    private BehaviourContainer Service;
    private IDiagramService diagramService;

    private class TestBehaviourOptions : BaseBehaviourOptions;
    private class TestBehaviour : BaseBehaviour;

    public BehaviourContainerTests()
    {
        diagramService = new DiagramService();
        Service = new BehaviourContainer(diagramService);
    }

    [Fact]
    public void RegisterBehaviourOptions_Works()
    {
        // Arrange
        var options = new TestBehaviourOptions();
        
        // Act
        Service.RegisterBehaviourOptions(options);
        
        // Assert
        Assert.Contains(options, diagramService.Diagram.Options.BehaviourOptions);
    }
    
    [Fact]
    public void RegisterBehaviourOptions_Throws_Exception_On_Double_Registration()
    {
        // Arrange
        var options = new TestBehaviourOptions();
        Service.RegisterBehaviourOptions(options);
        
        // Act
        var act = () => Service.RegisterBehaviourOptions(options);
        
        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }
    
    [Fact]
    public void EnableBehaviour_Works()
    {
        // Arrange
        var options = new TestBehaviourOptions
        {
            IsEnabled = false,
        };
        var eventCount = 0;
        options.OnEnabledChanged.Subscribe(e=>eventCount++);
        Service.RegisterBehaviourOptions(options);
        
        // Act
        Service.EnableBehaviour<TestBehaviourOptions>();
        
        // Assert
        Assert.Equal(1,eventCount);
        Assert.True(options.IsEnabled);
        
    }
    
    [Fact]
    public void DisableBehaviour_Works()
    {
        // Arrange
        var options = new TestBehaviourOptions
        {
            IsEnabled = true,
        };
        var eventCount = 0;
        options.OnEnabledChanged.Subscribe(e=>eventCount++);
        Service.RegisterBehaviourOptions(options);
        
        // Act
        Service.DisableBehaviour<TestBehaviourOptions>();
        
        // Assert
        Assert.Equal(1,eventCount);
        Assert.False(options.IsEnabled);
        
    }
    
    [Fact]
    public void RegisterBehaviour_Works()
    {
        // Arrange
        var behaviour = new TestBehaviour();
        
        // Act
        Service.RegisterBehaviour(behaviour);
        
        // Assert
        Assert.Contains(behaviour, Service._behaviours);

    }
    
    [Fact]
    public void RegisterBehaviour_Twice_Throws_Exception()
    {
        // Arrange
        var behaviour = new TestBehaviour();
        Service.RegisterBehaviour(behaviour);
        
        // Act
        var act = ()=>Service.RegisterBehaviour(behaviour);
        
        // Assert
        Assert.Throws<InvalidOperationException>(act);

    }
}