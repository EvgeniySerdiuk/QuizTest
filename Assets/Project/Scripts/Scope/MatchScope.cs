using Project.Scripts.Match;
using Project.Scripts.Services;
using Project.Scripts.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Scope
{
    public class MatchScope : LifetimeScope
    {
        [SerializeField] private MatchConfig matchConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseEntryPoints(epBuilder =>
            {
                epBuilder.Add<MatchStateMachine>().AsSelf();
                epBuilder.Add<WinConditionHandlerService>().AsSelf();
                epBuilder.Add<CellViewSpawnService>().AsSelf();
            });

            builder.UseComponents(cmpBuilder =>
            {
                cmpBuilder.AddInstance(matchConfig);
            });

            builder.Register<EventBuss.EventBuss>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<LevelDataGenerationService>(Lifetime.Scoped);

            StatesFactory.RegisterStates(builder);
            builder.Register<StatesFactory>(Lifetime.Scoped);
        }
    }
}