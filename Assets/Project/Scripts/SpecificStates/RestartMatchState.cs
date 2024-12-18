using System;
using System.Threading;
using System.Threading.Tasks;
using Project.Scripts.EventBuss;
using Project.Scripts.Services;
using Project.Scripts.StateMachine;
using Project.Scripts.UI;

namespace Project.Scripts.SpecificStates
{
    public class RestartMatchState : State, IListener<RestartButtonOnClick>
    {
        public override Type NextState => typeof(StartMatchState);

        private readonly LevelDataGenerationService _levelDataGenerationService;
        private readonly CellViewSpawnService _cellViewSpawnService;

        public RestartMatchState(IEventBuss eventBuss, LevelDataGenerationService levelDataGenerationService,
            CellViewSpawnService cellViewSpawnService) : base(eventBuss)
        {
            _levelDataGenerationService = levelDataGenerationService;
            _cellViewSpawnService = cellViewSpawnService;
        }

        public override Task Enter(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EventBuss.AddListener(this);
            return Task.CompletedTask;
        }

        public override Task Exit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EventBuss.RemoveListener(this);

            _levelDataGenerationService.Restart();
            _cellViewSpawnService.Restart();

            return Task.CompletedTask;
        }

        public void Execute(RestartButtonOnClick value)
        {
            Complete();
        }
    }
}