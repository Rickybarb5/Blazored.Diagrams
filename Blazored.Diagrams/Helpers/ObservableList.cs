using System.Collections;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Services.Events;
using Blazored.Diagrams.Services.Serialization;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Helpers;

/// <summary>
/// Custom list implementation with add and remove events.
/// </summary>
/// <typeparam name="TModel"></typeparam>

[JsonConverter(typeof(ObservableListConverter))]
public class ObservableList<TModel> : IList<TModel> where TModel : IId
{
    private readonly Dictionary<string, TModel> _internalIDictionary = [];

    /// <summary>
    /// Event triggered when an item is added.
    /// </summary>
    internal ITypedEvent<ItemAddedEvent<TModel>> OnItemAdded = new TypedEvent<ItemAddedEvent<TModel>>();

    /// <summary>
    /// Event triggered when an item is removed.
    /// </summary>
    internal ITypedEvent<ItemRemovedEvent<TModel>> OnItemRemoved = new TypedEvent<ItemRemovedEvent<TModel>>();

    /// <inheritdoc />
    [JsonIgnore]
    public int Count => _internalIDictionary.Count;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsReadOnly { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public TModel this[int index]
    {
        get => _internalIDictionary.Values.ElementAt(index);
        set
        {
            var oldItem = _internalIDictionary.Values.ElementAt(index);
            _internalIDictionary.Remove(oldItem.Id);
            OnItemRemoved?.Publish(new(oldItem));
            _internalIDictionary[value.Id] = value;
            OnItemAdded?.Publish(new(value));
        }
    }

    /// <summary>
    /// Internal property for serialization - gets/sets the dictionary directly
    /// </summary>
    [JsonProperty("Items")]
    internal Dictionary<string, TModel> InternalDictionary => _internalIDictionary;

    /// <summary>
    /// Adds an item, if it doesn't already exist.
    /// </summary>
    /// <param name="item"></param>
    public void Add(TModel item)
    {
        if (_internalIDictionary.TryAdd(item.Id, item))
        {
            OnItemAdded?.Publish(new(item));
        }
    }

    /// <summary>
    /// Adds a collection of items to the list.
    /// </summary>
    /// <param name="collection"></param>
    public void AddRange(IEnumerable<TModel> collection)
    {
        collection.ForEach(Add);
    }

    /// <summary>
    /// Removes an item, if it exists.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(TModel item)
    {
        if (_internalIDictionary.Remove(item.Id))
        {
            OnItemRemoved?.Publish(new(item));
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public void Clear()
    {
        var itemsToRemove = _internalIDictionary.Values.ToList();
        _internalIDictionary.Clear();
        itemsToRemove.ForEach(x => OnItemRemoved?.Publish(new(x)));
    }

    /// <inheritdoc />
    public bool Contains(TModel item) => _internalIDictionary.ContainsKey(item.Id);

    /// <inheritdoc />
    public void CopyTo(TModel[] array, int arrayIndex)
    {
        _internalIDictionary.Values.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public int IndexOf(TModel item)
    {
        return _internalIDictionary.Values.ToList().IndexOf(item);
    }

    /// <inheritdoc />
    public void Insert(int index, TModel item)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        var removedElement = _internalIDictionary.Values.ElementAt(index);
        Remove(removedElement);
        Add(item);
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        Remove(this[index]);
    }

    /// <inheritdoc />
    public IEnumerator<TModel> GetEnumerator()
    {
        return _internalIDictionary.Values.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}