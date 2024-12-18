using System;
using System.Threading;
using System.Threading.Tasks;
using Project.Scripts.EventBuss;
using Project.Scripts.Services;
using Project.Scripts.StateMachine;

namespace Project.Scripts.SpecificStates
{
    public class StartMatchState : State, IListener<CellViewsIsSpawned>
    {
        public override Type NextState => typeof(MatchState);

        private readonly LevelDataGenerationService _dataGenerationService;

        public StartMatchState(IEventBuss eventBuss, LevelDataGenerationService dataGenerationService) : base(eventBuss)
        {
            _dataGenerationService = dataGenerationService;
        }

        public override Task Enter(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventBuss.AddListener(this);
            EventBuss.Execute(new StartState<StartMatchState>());

            _dataGenerationService.Generate();
            return Task.CompletedTask;
        }

        public override Task Exit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventBuss.RemoveListener(this);
            return Task.CompletedTask;
        }

        public void Execute(CellViewsIsSpawned value)
        {
            Complete();
        }
    }
}