using Blazored.Diagrams.Extensions;
using Newtonsoft.Json;

namespace Blazored.Diagrams.Interfaces;

public interface IBounds
{
    /// <summary>
    /// 
    /// </summary>
    [JsonIgnore]
    Rect Bounds { get; }
}