using System.Collections;
using Blazored.Diagrams.Events;
using Blazored.Diagrams.Extensions;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Services.Serialization;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Helpers;

/// <summary>
/// Custom list implementation with add and remove events that enforces
/// access control by making mutation methods internal or explicitly implemented.
/// </summary>
/// <typeparam name="TModel"></typeparam>
[JsonConverter(typeof(ObservableListConverter))]
public class ObservableList<TModel> : IList<TModel> where TModel : IId
{
    private readonly Dictionary<string, TModel> _internalIDictionary = [];
    private const string MutationError = "Modification must go through the DiagramService to ensure state consistency.";

    // --- Events ---
    internal readonly ITypedEvent<ItemAddedEvent<TModel>> OnItemAdded = new TypedEvent<ItemAddedEvent<TModel>>();
    internal readonly ITypedEvent<ItemRemovedEvent<TModel>> OnItemRemoved = new TypedEvent<ItemRemovedEvent<TModel>>();


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
            
            // Use internal methods for mutation and event publishing
            RemoveInternal(oldItem); 
            AddInternal(value);
        }
    }
    
    [JsonProperty("Items")]
    internal Dictionary<string, TModel> InternalDictionary => _internalIDictionary;

    /// <summary>
    /// Adds an item, if it doesn't already exist. (Internal method)
    /// </summary>
    internal void AddInternal(TModel item)
    {
        if (_internalIDictionary.TryAdd(item.Id, item))
        {
            OnItemAdded.Publish(new(item));
        }
    }

    /// <summary>
    /// Adds a collection of items to the list. (Internal method)
    /// </summary>
    internal void AddRangeInternal(IEnumerable<TModel> collection)
    {
        collection.ForEach(AddInternal);
    }

    /// <summary>
    /// Removes an item, if it exists. (Internal method)
    /// </summary>
    internal bool RemoveInternal(TModel item)
    {
        if (_internalIDictionary.Remove(item.Id))
        {
            OnItemRemoved.Publish(new(item));
            return true;
        }

        return false;
    }

    /// <summary>
    /// Clears the list.
    /// </summary>
    internal void ClearInternal()
    {
        var itemsToRemove = _internalIDictionary.Values.ToList();
        _internalIDictionary.Clear();
        itemsToRemove.ForEach(x => OnItemRemoved.Publish(new(x)));
    }
    
    internal void InsertInternal(int index, TModel item)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));
            
        // Implementation logic for inserting at a specific index in a Dictionary-backed list is complex.
        // For simplicity and to enforce the intended use (Add/Remove), we use the existing logic:
        var removedElement = _internalIDictionary.Values.ElementAt(index);
        RemoveInternal(removedElement); 
        AddInternal(item);             
    }
    
    internal void RemoveAtInternal(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        RemoveInternal(this[index]);
    }

    // ------------------------------------------------------------------------------------------
    // --- PUBLIC READ/QUERY METHODS (Unchanged to support LINQ and public accessors) ---
    // ------------------------------------------------------------------------------------------

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
    public IEnumerator<TModel> GetEnumerator()
    {
        return _internalIDictionary.Values.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    // -------------------------------------------------------------------------------------------
    // --- EXPLICIT INTERFACE IMPLEMENTATION (Hiding mutation methods from public access) ---
    // -------------------------------------------------------------------------------------------

    /// <inheritdoc />
    void ICollection<TModel>.Add(TModel item) =>
        throw new NotSupportedException(MutationError);

    /// <inheritdoc />
    bool ICollection<TModel>.Remove(TModel item) =>
        throw new NotSupportedException(MutationError);

    /// <inheritdoc />
    void ICollection<TModel>.Clear() =>
        throw new NotSupportedException(MutationError);
    
    /// <inheritdoc />
    void IList<TModel>.Insert(int index, TModel item) =>
        throw new NotSupportedException(MutationError);
    
    /// <inheritdoc />
    void IList<TModel>.RemoveAt(int index) =>
        throw new NotSupportedException(MutationError);
}