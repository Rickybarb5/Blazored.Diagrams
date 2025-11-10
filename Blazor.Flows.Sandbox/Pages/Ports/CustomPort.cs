using Blazor.Flows.Ports;
using Blazor.Flows.Services.Registry;

namespace Blazor.Flows.Sandbox.Pages.Ports;

/// <summary>
/// A custom port model that specifies a custom component.
/// </summary>
public class CustomPort : Port, IHasComponent<CustomPortComponent>
{
    // You can add any custom properties you need here.
    public string CustomData { get; set; } = "My Port";
}