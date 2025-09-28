using Blazored.Diagrams.Behaviours;
using Blazored.Diagrams.Interfaces;

namespace Blazored.Diagrams.Services.Behaviours;

public interface IBehaviourContainer
{
    IBehaviourContainer RegisterBehaviour<TBehaviour>(TBehaviour behaviour)
        where TBehaviour : IBehaviour;

    IBehaviourContainer RegisterBehaviour<TBehaviour, TBehaviourOptions>(TBehaviour behaviour,
        TBehaviourOptions options)
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