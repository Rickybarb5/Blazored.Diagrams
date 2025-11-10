using Blazor.Flows.Links;
using Blazor.Flows.Services.Registry;

namespace Blazor.Flows.Sandbox.Pages.Links;

/// <summary>
/// A link model that uses the AnimatedLinkComponent, a totally custom component.
/// This is for full control over rendering, including SVG animation effects.
/// </summary>
public class AnimatedLink : Link, IHasComponent<AnimatedLinkComponent>;