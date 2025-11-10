using Blazor.Flows.Links;
using Blazor.Flows.Services.Registry;

namespace Blazor.Flows.Sandbox.Pages.Links;

/// <summary>
/// A link model that uses the ExtendedLinkComponent, which inherits from DefaultLinkComponent.
/// This allows for easy path customization while keeping built-in events and styling.
/// </summary>
public class ExtendedLink : Link, IHasComponent<ExtendedLinkComponent>;