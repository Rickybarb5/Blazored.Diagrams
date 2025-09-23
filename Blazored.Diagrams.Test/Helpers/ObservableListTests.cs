using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Nodes;

namespace Blazored.Diagrams.Test.Helpers;

public class ObservableListTests
{
    [Fact]
    public void Add_Item_Triggers_Event()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var eventTriggered = false;
        list.OnItemAdded += ctx => { eventTriggered = true; };
        // Act
        list.Add(new Node());

        // Assert
        Assert.Single(list);
        Assert.True(eventTriggered);
    }

    [Fact]
    public void Insert_Item_Triggers_Event()
    {
        // Arrange
        var list = new ObservableList<INode>();
        list.Add(new Node());
        var addEventTriggered = false;
        var removeEventTriggered = false;
        list.OnItemAdded += ctx => { addEventTriggered = true; };
        list.OnItemRemoved += ctx => { removeEventTriggered = true; };
        // Act
        list.Insert(0, new Node());

        // Assert
        Assert.Single(list);
        Assert.True(addEventTriggered);
        Assert.True(removeEventTriggered);
    }

    [Fact]
    public void Remove_Item_Triggers_Event()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var eventTriggered = false;
        list.OnItemRemoved += ctx => { eventTriggered = true; };
        var node = new Node();
        list.Add(node);

        // Act
        list.Remove(node);

        // Assert
        Assert.Empty(list);
        Assert.True(eventTriggered);
    }

    [Fact]
    public void RemoveAt_Triggers_Event()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var eventTriggered = false;
        list.OnItemRemoved += ctx => { eventTriggered = true; };
        var node = new Node();
        list.Add(node);

        // Act
        list.RemoveAt(0);

        // Assert
        Assert.Empty(list);
        Assert.True(eventTriggered);
    }

    [Fact]
    public void Add_Range_Triggers_Events()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var eventTriggered = false;
        int eventCount = default;
        list.OnItemAdded += ctx =>
        {
            eventTriggered = true;
            eventCount++;
        };

        // Act
        list.AddRange([new Node(), new Node(), new Node()]);

        // Assert
        Assert.True(eventTriggered);
        Assert.Equal(3, eventCount);
    }

    [Fact]
    public void Replace_Triggers_Event()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var addEventTriggered = false;
        var removeEventTriggered = false;
        var node1 = new Node();
        var node2 = new Node();
        list.Add(node1);
        list.OnItemAdded += ctx => { addEventTriggered = true; };
        list.OnItemRemoved += ctx => { removeEventTriggered = true; };

        // Act
        list[0] = node2;

        // Assert
        Assert.Single(list);
        Assert.Equal(list[0], node2);
        Assert.True(addEventTriggered);
        Assert.True(removeEventTriggered);
    }

    [Fact]
    public void Clear_Triggers_Events()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var eventTriggered = false;
        int eventCount = default;
        list.OnItemRemoved += ctx =>
        {
            eventTriggered = true;
            eventCount++;
        };
        list.AddRange([new Node(), new Node(), new Node()]);

        // Act
        list.Clear();

        // Assert
        Assert.Empty(list);
        Assert.True(eventTriggered);
        Assert.Equal(3, eventCount);
    }

    [Fact]
    public void Foreach_Executes_Action()
    {
        // Arrange
        var list = new ObservableList<INode>();
        int eventCount = default;
        list.AddRange([new Node(), new Node(), new Node()]);

        // Act
        list.ForEach(ctx => eventCount++);

        // Assert
        Assert.Equal(list.Count, eventCount);
    }

    [Fact]
    public void IndexOf_Returns_Correct_Index()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var node = new Node();
        list.AddRange([new Node(), node, new Node()]);

        // Act
        var result = list.IndexOf(node);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Contains_Returns_True()
    {
        // Arrange
        var list = new ObservableList<INode>();
        var node = new Node();
        list.AddRange([new Node(), node, new Node()]);

        // Act
        var result = list.Contains(node);

        // Assert
        Assert.True(result);
    }
}