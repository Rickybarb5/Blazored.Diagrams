using Blazor.Flows.Groups;
using Blazor.Flows.Services.Registry;

namespace Blazor.Flows.Sandbox.Pages.Groups;

public class CustomGroup : Group, IHasComponent<CustomGroupComponent>
{
    public string Title { get; set; } = "I'm a group with a title!";

    public CustomGroup()
    {
        Padding = 35;
    }
}