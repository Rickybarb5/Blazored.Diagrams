using Blazored.Diagrams.Helpers;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Services.Serialization;

/// <summary>
/// Custom JSON converter for ObservableList to properly serialize/deserialize the internal dictionary
/// while preserving reference metadata ($ref, $id)
/// </summary>
public class ObservableListConverter : JsonConverter
{
    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType && 
               objectType.GetGenericTypeDefinition() == typeof(ObservableList<>);
    }

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // Get the generic type argument (e.g., INode, ILayer, etc.)
        var itemType = objectType.GetGenericArguments()[0];
        
        // Create the ObservableList<T> instance
        var listType = typeof(ObservableList<>).MakeGenericType(itemType);
        var list = Activator.CreateInstance(listType);
        
        // Get the internal dictionary property
        var dictProperty = listType.GetProperty("InternalDictionary", 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var internalDict = dictProperty?.GetValue(list) as System.Collections.IDictionary;
        
        if (internalDict == null || reader.TokenType == JsonToken.Null)
            return list;
        
        // Read the dictionary manually
        if (reader.TokenType != JsonToken.StartObject)
            throw new JsonSerializationException("Expected StartObject token");
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
                break;
                
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var key = reader.Value?.ToString();
                reader.Read(); 
                var value = serializer.Deserialize(reader, itemType);
                
                if (key != null && value != null)
                {
                    internalDict.Add(key, value);
                }
            }
        }
        
        return list;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        
        // Get the internal dictionary
        var listType = value.GetType();
        var dictProperty = listType.GetProperty("InternalDictionary", 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var internalDict = dictProperty?.GetValue(value) as System.Collections.IDictionary;
        
        if (internalDict == null)
        {
            writer.WriteNull();
            return;
        }
        
        // Manually write the dictionary without $type metadata for the dictionary itself
        writer.WriteStartObject();
        
        foreach (System.Collections.DictionaryEntry entry in internalDict)
        {
            writer.WritePropertyName(entry.Key.ToString() ?? "");
            serializer.Serialize(writer, entry.Value); // This will add type info for the values
        }
        
        writer.WriteEndObject();
    }
}