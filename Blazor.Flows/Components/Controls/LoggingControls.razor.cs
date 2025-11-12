using System.Diagnostics.CodeAnalysis;
using Blazor.Flows.Options.Behaviours;
using Blazor.Flows.Services.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Blazor.Flows.Components.Controls;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class LoggingControls
{
    private LoggingBehaviourOptions Options => DiagramService.Behaviours.GetBehaviourOptions<LoggingBehaviourOptions>();
    [CascadingParameter] private IDiagramService DiagramService { get; set; } = null!;

    /// <summary>
    /// Shows/Hides the control
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; }
}