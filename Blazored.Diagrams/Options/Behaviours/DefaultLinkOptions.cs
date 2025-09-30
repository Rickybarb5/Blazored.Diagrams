using Blazored.Diagrams.Components.Models;

namespace Blazored.Diagrams.Options.Behaviours;

public class DefaultLinkComponentOptions
{
    /// <summary>
    /// The default path used by <see cref="DefaultLinkComponent"/>.
    /// </summary>
    public DefaultLinkComponentPathType PathType { get; set; } = DefaultLinkComponentPathType.Curved;
    
}