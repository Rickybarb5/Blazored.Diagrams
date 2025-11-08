
using System.Text.Json.Serialization;
using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Links;

namespace Blazored.Diagrams.Options.Behaviours;

/// <summary>
/// Options for <see cref="DrawLinkBehavior"/>
/// </summary>
public partial class DrawLinkBehaviourOptions : BaseBehaviourOptions
{
    [JsonInclude]
    private string TypeName = typeof(CurvedLink).FullName;
    
    /// <summary>
    ///     Type of link that the behaviour will create.
    /// Default is <see cref="Link"/>
    /// </summary>
    [JsonIgnore]
    public Type LinkType
    {
        get
        {
            var type = Type.GetType(TypeName);
            ArgumentNullException.ThrowIfNull(type);
            return type;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!typeof(ILink).IsAssignableFrom(value))
            {
                throw new ArgumentException($"{nameof(LinkType)} must implement the ILink interface.");
            }
            TypeName = value.FullName ?? throw new InvalidOperationException($"It was not possible to get type name for {value}");
        }
    }

}