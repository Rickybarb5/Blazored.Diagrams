using System.Collections;
using System.Text.Json.Serialization;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Helpers;

/// <summary>
/// Custom list implementation with add and remove events.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class ObservableList<TModel> : IList<TModel> where TModel : IId
{
    private readonly Dictionary<Guid, TModel> _internalIDictionary = [];

    /// <summary>
    /// Event triggered when an item is added.
    /// </summary>
    public event Action<TModel>? OnItemAdded;

    /// <summary>
    /// Event triggered when an item is removed.
    /// </summary>
    public event Action<TModel>? OnItemRemoved;

    /// <inheritdoc />
    [JsonIgnore]
    public int Count => _internalIDictionary.Count;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsReadOnly => false;

    /// <inheritdoc />
    [JsonIgnore]
    public TModel this[int index]
    {
        get => _internalIDictionary.Values.ElementAt(index);
        set
        {
            var oldItem = _internalIDictionary.Values.ElementAt(index);
            _internalIDictionary.Remove(oldItem.Id);
            OnItemRemoved?.Invoke(oldItem);
            _internalIDictionary[value.Id] = value;
            OnItemAdded?.Invoke(value);
        }
    }

    /// <summary>
    /// Adds an item, if it doesn't already exist.
    /// </summary>
    /// <param name="item"></param>
    public void Add(TModel item)
    {
        if (_internalIDictionary.TryAdd(item.Id, item))
        {
            OnItemAdded?.Invoke(item);
        }
    }

    /// <summary>
    /// Adds an collection if items to the list.
    /// </summary>
    /// <param name="collection"></param>
    public void AddRange(IEnumerable<TModel> collection)
    {
        foreach (var item in collection)
        {
            Add(item);
        }
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
            OnItemRemoved?.Invoke(item);
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public void Clear()
    {
        var itemsToRemove = _internalIDictionary.Values.ToList();
        _internalIDictionary.Clear();
        foreach (var item in itemsToRemove)
        {
            OnItemRemoved?.Invoke(item);
        }
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

    /// <summary>
    /// Applies an action to every element.
    /// </summary>
    /// <param name="action"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void ForEach(Action<TModel> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        foreach (var item in _internalIDictionary.Values)
        {
            action(item);
        }
    }
}