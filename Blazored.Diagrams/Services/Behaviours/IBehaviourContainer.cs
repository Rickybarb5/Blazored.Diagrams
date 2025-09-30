using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Behaviours;

public interface IBehaviourContainer : IDisposable
{
    /// <summary>
    /// Register a behaviour.
    /// </summary>
    /// <param name="behaviour"></param>
    /// <typeparam name="TBehaviour"></typeparam>
    /// <returns></returns>
    IBehaviourContainer RegisterBehaviour<TBehaviour>(TBehaviour behaviour)
        where TBehaviour : IBehaviour;

    IBehaviourContainer Register<TBehaviour, TBehaviourOptions>(TBehaviour behaviour,
        TBehaviourOptions options)
        where TBehaviour : IBehaviour
        where TBehaviourOptions : IBehaviourOptions;

    public IBehaviourContainer Register<TBehaviour, TBehaviourOptions>(
        Func<TBehaviour> behaviourConfiguration,
        Func<TBehaviourOptions> optionConfiguration)
        where TBehaviour : IBehaviour
        where TBehaviourOptions : IBehaviourOptions;

    void EnableBehaviour<TBehaviourOptions>()
        where TBehaviourOptions : IBehaviourOptions;

    void DisableBehaviour<TBehaviourOptions>()
        where TBehaviourOptions : IBehaviourOptions;

    TBehaviourOptions GetBehaviourOptions<TBehaviourOptions>()
        where TBehaviourOptions : IBehaviourOptions;

    IBehaviourContainer RegisterBehaviourOptions<TBehaviourOptions>(TBehaviourOptions options)
        where TBehaviourOptions : IBehaviourOptions;
}