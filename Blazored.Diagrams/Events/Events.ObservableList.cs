namespace Blazored.Diagrams.Events;

    /// <summary>
    /// Event triggered when an item is added to an observable list.
    /// </summary>
    /// <param name="Item">The item that was added.</param>
    /// <typeparam name="T">Type of the collection object.</typeparam>
    internal record ItemAddedEvent<T>(T Item) : IEvent;
    
    /// <summary>
    /// Event triggered when an item is removed from an observable list.
    /// </summary>
    /// <param name="Item">The item that was removed.</param>
    /// <typeparam name="T">Type of the collection object.</typeparam>
    internal record ItemRemovedEvent<T>(T Item) : IEvent;