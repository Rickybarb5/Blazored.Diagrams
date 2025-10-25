using Blazored.Diagrams.Groups;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Groups;

public class CustomGroup : Group, IHasComponent<CustomGroupComponent>
{
    public string Title { get; set; } = "I'm a group with a title!";

    public CustomGroup()
    {
        Padding = 30;
    }
}