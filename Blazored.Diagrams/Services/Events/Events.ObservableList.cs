namespace Blazored.Diagrams.Services.Events;

    /// <summary>
    /// Event triggered when an item is added to an observable list.
    /// </summary>
    /// <param name="Item"></param>
    /// <typeparam name="T"></typeparam>
    internal record ItemAddedEvent<T>(T Item) : IEvent;
    
    /// <summary>
    /// Event triggered when an item is removed from an observable list.
    /// </summary>
    /// <param name="Item"></param>
    /// <typeparam name="T"></typeparam>
    internal record ItemRemovedEvent<T>(T Item) : IEvent;