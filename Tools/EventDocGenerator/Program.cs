using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using EventDocGenerator;

if (args.Length == 0)
{
    Console.Error.WriteLine("Missing output directory argument.");
    Console.Error.WriteLine("Usage: EventDocGenerator <output-directory>");
    return;
}

var outputDir = args[0];
Directory.CreateDirectory(outputDir);

// Assuming the 'Blazor.Flows' assembly is available
var assembly = typeof(Blazor.Flows.Diagrams.Diagram).Assembly;
var xmlPath = Path.ChangeExtension(assembly.Location, ".xml");

if (!File.Exists(xmlPath))
{
    Console.Error.WriteLine($"Cannot find XML documentation file at: {xmlPath}");
    return;
}

// 1. Get the events, now grouped by a key (e.g., "Layer", "Node")
var docsByGroup = EventDocScanner.GetDiagramEventDocs(assembly, xmlPath);
var totalEvents = 0;

var options = new JsonSerializerOptions 
{ 
    WriteIndented = true,
    // This encoder allows tags like < and > to pass through without escaping
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

// 2. Iterate over each group
foreach (var group in docsByGroup)
{
    var groupKey = group.Key;
    if (groupKey == "Other")
    {
        Console.Error.WriteLine($"Found {group.Count()} events with 'Other' grouping:");
        foreach(var doc in group) Console.Error.WriteLine($"   - {doc.Name}");
        continue;
    }

    // Get the list of docs for just this group
    var docsForGroup = group.ToList();
    
    // 3. Serialize this group's list
    var json = JsonSerializer.Serialize(docsForGroup, options);

    // 4. Create a unique filename for this group
    var fileName = $"events.{groupKey.ToLowerInvariant()}.json";
    var outputPath = Path.Combine(outputDir, fileName);
    
    File.WriteAllText(outputPath, json);
    Console.WriteLine($"Generated {docsForGroup.Count} event docs at {outputPath}");
    totalEvents += docsForGroup.Count;
}

Console.WriteLine($"\nTotal events generated: {totalEvents}");

namespace EventDocGenerator
{
    // --- Scanner Class ---

    public static class EventDocScanner
    {
        /// <summary>
        /// Scans the assembly for event types and groups them by their logical area.
        /// </summary>
        /// <returns>An ILookup where the key is the group name (e.g., "Layer", "Node")
        /// and the value is the list of event docs.</returns>
        public static ILookup<string, EventDoc> GetDiagramEventDocs(Assembly assembly, string xmlDocPath)
        {
            var xml = XDocument.Load(xmlDocPath);

            // 1. UPDATED: This now reads the <summary> AND <param> elements.
            // It stores them in a dictionary for lookup.
            var members = xml.Descendants("member")
                .ToDictionary(
                    e => e.Attribute("name")?.Value ?? "",
                    e => 
                    {
                        // Get Summary
                        var summaryEl = e.Element("summary");
                        string? summary = null;
                        if (summaryEl != null)
                        {
                            // Use string.Concat(Nodes()) to preserve inner tags like <see>
                            summary = string.Concat(summaryEl.Nodes()).Trim();
                        }

                        // Get Params
                        var paramDocs = e.Elements("param")
                            .ToDictionary(
                                p => p.Attribute("name")?.Value ?? "",
                                // Use string.Concat(Nodes()) here too
                                p => string.Concat(p.Nodes()).Trim()
                            );
                    
                        return new { Summary = summary, ParamDocs = paramDocs };
                    });

            var eventTypes = assembly.GetTypes()
                .Where(t =>
                    t.Namespace != null &&
                    t.Namespace.StartsWith("Blazor.Flows.Events") &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericTypeDefinition && // Exclude ItemAddedEvent<T> definition
                    t.GetInterfaces().Any(i => i.Name == "IEvent"));

            return eventTypes
                .OrderBy(t => t.Name)
                .Select(type =>
                {
                    // 2. Get constructor parameters via reflection (for Name and Type)
                    var constructor = type.GetConstructors()
                        .OrderByDescending(c => c.GetParameters().Length)
                        .FirstOrDefault(); // Get primary ctor

                    // 3. Get XML doc key
                    var key = $"T:{type.FullName}";
                    if (type.IsGenericType)
                    {
                        key = $"T:{type.FullName?.Split('`')[0]}";
                    }
                
                    // 4. Look up the doc entry for this type
                    members.TryGetValue(key, out var docEntry);
                    var summary = docEntry?.Summary;
                    var paramDocs = docEntry?.ParamDocs ?? new Dictionary<string, string>();

                    if (string.IsNullOrWhiteSpace(summary))
                    {
                        summary = "(no documentation found)";
                    }

                    // 5. Build the parameter list
                    var parameters = new List<EventParameterDoc>();
                    if (constructor != null)
                    {
                        parameters = constructor.GetParameters()
                            .Select(p => 
                            {
                                // Find the matching doc for this parameter
                                paramDocs.TryGetValue(p.Name, out var paramDoc);
                                return new EventParameterDoc(
                                    p.Name, 
                                    p.ParameterType.Name, 
                                    paramDoc ?? "(no documentation)"
                                );
                            })
                            .ToList();
                    }
                
                    return new
                    {
                        GroupKey = GetGroupKey(type),
                        Doc = new EventDoc(type.Name, summary, parameters)
                    };
                })
                .ToLookup(x => x.GroupKey, x => x.Doc);
        }

        /// <summary>
        /// Determines the logical group for an event type based on its inheritance.
        /// This maps to your file structure (e.g., NodeEvent -> "Node" -> events.node.json).
        /// </summary>
        private static string GetGroupKey(Type type)
        {
            // Handle special cases first
            if (type.Name.StartsWith("Item"))
                return "ObservableList";
            if (type.Name == "CurrentLayerChangedEvent")
                return "Layer";

            var baseType = type.BaseType;
            if (baseType == null)
                return "Other";

            // Check for simple inheritance (e.g., LayerAddedEvent : LayerEvent)
            switch (baseType.Name)
            {
                case "LayerEvent": return "Layer";
                case "NodeEvent": return "Node";
                case "GroupEvent": return "Groups";
                case "LinkEvent": return "Links";
                case "PortEvent": return "Ports";
                case "DiagramEvent": return "Diagram";
            }

            // Check for ModelInputEvent<T> inheritance
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition().Name == "ModelInputEvent`1")
            {
                var modelType = baseType.GetGenericArguments()[0];
                switch (modelType.Name)
                {
                    case "IDiagram": return "Diagram";
                    case "INode": return "Node";
                    case "IGroup": return "Groups";
                    case "ILink": return "Links";
                    case "IPort": return "Ports";
                }
            }
        
            return "Other"; // Fallback
        }
    }

// --- Documentation Records ---

    /// <summary>
    /// Documents a parameter from an event's constructor.
    /// </summary>
    /// <param name="Name">The name of the parameter (e.g., "Model").</param>
    /// <param name="TypeName">The C# type name of the parameter (e.g., "INode").</param>
    /// <param name="Documentation">The XML documentation for the parameter.</param>
    public record EventParameterDoc(string Name, string TypeName, string Documentation);

    /// <summary>
    /// Documents a single event.
    /// </summary>
    /// <param name="Name">The class name of the event (e.g., "NodeAddedEvent").</param>
    /// <param name="Summary">The XML documentation summary.</param>
    /// <param name="Parameters">A list of the event's constructor parameters.</param>
    public record EventDoc(string Name, string Summary, IEnumerable<EventParameterDoc> Parameters);
}