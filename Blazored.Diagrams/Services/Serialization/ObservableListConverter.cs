using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.Diagrams.Helpers;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Serialization;

/// <summary>
/// Creates converters for <see cref="ObservableList{T}"/>
/// </summary>
public class ObservableListConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(ObservableList<>);
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converter = (JsonConverter)Activator.CreateInstance(
            typeof(ObservableListConverterInner<>).MakeGenericType(elementType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: [options],
            culture: null)!;
        return converter;
    }

    /// <summary>
    /// Generic observable list converter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private class ObservableListConverterInner<T>(JsonSerializerOptions options) : JsonConverter<ObservableList<T>>
        where T : class, IId
    {
        public override ObservableList<T> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var resolver = (CustomReferenceResolver)options.ReferenceHandler!.CreateResolver();
            using var doc = JsonDocument.ParseValue(ref reader);
            var rootElement = doc.RootElement;

            if (rootElement.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException("Expected start of array.");
            }

            var list = new ObservableList<T>();

            var jsonArray = rootElement.EnumerateArray();
            for (var i = 0; i < jsonArray.Count(); i++)
            {
                var item = default(T);
                // Handle object if it's a reference.
                if (jsonArray.ElementAt(i).TryGetProperty(SerializationConstants.RefKey, out var refProperty))
                {
                    var referenceId = refProperty.GetString()!;
                    var objectReference = resolver.ResolveReference(referenceId);

                    // If we try to fetch a reference that doesn't exist yet, we add an action to add it to the array.
                    if (objectReference is UnresolvedReference)
                    {
                        // creates an unresolved reference and when resolved, adds it to the list.
                        var i1 = i;
                        resolver.AddUnresolvedReference(referenceId, obj => list.Insert(i1, (T)obj));
                    }
                    else
                    {
                        item = (T)objectReference;
                    }
                }
                else
                {
                    // Deserialize normally, if not a reference.
                    item = JsonSerializer.Deserialize<T>(jsonArray.ElementAt(i), options);
                }

                // Add to array if not null
                if (item is not null)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public override void Write(Utf8JsonWriter writer, ObservableList<T> value, JsonSerializerOptions options1)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                // Let other serializers do their work.
                JsonSerializer.Serialize(writer, item, typeof(T), options);
            }

            writer.WriteEndArray();
        }
    }
}