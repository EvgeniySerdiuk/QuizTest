using System;
using Project.Scripts.SpecificStates;
using VContainer;

namespace Project.Scripts.StateMachine
{
    public class StatesFactory
    {
        private readonly IObjectResolver _resolver;

        public StatesFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public IState CreateState<T>()
        {
            return _resolver.Resolve<T>() as IState;
        }

        public IState CreateState(Type type)
        {
            return _resolver.ResolveNonGeneric(type) as IState;
        }

        public static void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<StartMatchState>(Lifetime.Transient);
            builder.Register<MatchState>(Lifetime.Transient);
            builder.Register<RestartMatchState>(Lifetime.Transient);
        }
    }
}