using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Interfaces;
using Blazored.Diagrams.Services.Diagrams;

namespace Blazored.Diagrams.Services.Behaviours;

/// <inheritdoc />
public class BehaviourContainer : IBehaviourContainer
{
    internal readonly List<IBehaviour> _behaviours = [];
    private readonly IDiagramService _service;
    
    /// <summary>
    /// Initializes a new <see cref="BehaviourContainer"/>.
    /// </summary>
    /// <param name="service"></param>
    public BehaviourContainer(IDiagramService service)
    {
        _service = service;
    }
    
    /// <inheritdoc />
    public IBehaviourContainer RegisterBehaviour<TBehaviour>(TBehaviour behaviour)
        where TBehaviour : IBehaviour
    {
        if (_behaviours.OfType<TBehaviour>().Any())
        {
            throw new InvalidOperationException(
                $"Behaviour of type {nameof(TBehaviour)} is already registered.");
            
        }
        _behaviours.Add(behaviour);
        return this;
    }

    /// <inheritdoc />
    public IBehaviourContainer Register<TBehaviour, TBehaviourOptions>(
        Func<TBehaviour> behaviourConfiguration, 
        Func<TBehaviourOptions> optionConfiguration) 
        where TBehaviour : IBehaviour
        where TBehaviourOptions : IBehaviourOptions
    {
        RegisterBehaviourOptions(optionConfiguration());
        RegisterBehaviour(behaviourConfiguration());
        return this;
    }

    /// <inheritdoc />
    public void EnableBehaviour<TBehaviourOptions>()
        where TBehaviourOptions : IBehaviourOptions
    {
        GetBehaviourOptions<TBehaviourOptions>().IsEnabled = true;
    }
    
    /// <inheritdoc />
    public void DisableBehaviour<TBehaviourOptions>()
        where TBehaviourOptions : IBehaviourOptions
    {
        GetBehaviourOptions<TBehaviourOptions>().IsEnabled = false;
    }
    
    /// <inheritdoc />
    public TBehaviourOptions GetBehaviourOptions<TBehaviourOptions>()
        where TBehaviourOptions : IBehaviourOptions
    {
        var options = _service.Diagram.Options.BehaviourOptions.OfType<TBehaviourOptions>().FirstOrDefault();
        if (options is null)
        {
            throw new InvalidOperationException($"No behaviour options of type {typeof(TBehaviourOptions)} exists.");
        }

        return options;
    }

    /// <inheritdoc />
    public IBehaviourContainer RegisterBehaviourOptions<TBehaviourOptions>(TBehaviourOptions options)
        where TBehaviourOptions : IBehaviourOptions
    {
        if ( _service.Diagram.Options.BehaviourOptions.OfType<TBehaviourOptions>().Any())
        {
            throw new InvalidOperationException(
                $"Behaviour options of type {nameof(TBehaviourOptions)} is already registered.");
        }

        _service.Diagram.Options.BehaviourOptions.Add(options);
        return this;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _behaviours.ForEach(b=> b.Dispose());
    }
}