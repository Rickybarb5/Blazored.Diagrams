using Blazored.Diagrams.Links;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Links;

/// <summary>
/// A link model that uses the ExtendedLinkComponent, which inherits from DefaultLinkComponent.
/// This allows for easy path customization while keeping built-in events and styling.
/// </summary>
public class ExtendedLink : Link, IHasComponent<ExtendedLinkComponent>;