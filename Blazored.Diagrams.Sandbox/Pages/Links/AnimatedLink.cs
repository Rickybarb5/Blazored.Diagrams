using Blazored.Diagrams.Links;
using Blazored.Diagrams.Services.Registry;

namespace Blazored.Diagrams.Sandbox.Pages.Links;

/// <summary>
/// A link model that uses the AnimatedLinkComponent, a totally custom component.
/// This is for full control over rendering, including SVG animation effects.
/// </summary>
public class AnimatedLink : Link, IHasComponent<AnimatedLinkComponent>
{
}