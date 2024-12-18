using System;
using System.Threading;
using System.Threading.Tasks;
using Project.Scripts.EventBuss;
using Project.Scripts.Match;
using Project.Scripts.Services;
using Project.Scripts.StateMachine;

namespace Project.Scripts.SpecificStates
{
    public class MatchState : State, IListener<WinConditionIsDone>
    {
        public override Type NextState => typeof(RestartMatchState);

        private readonly LevelDataGenerationService _dataGenerationService;
        private readonly int _amountLevels;

        private int _currentLevel;

        public MatchState(MatchConfig config, IEventBuss eventBuss, LevelDataGenerationService dataGenerationService) :
            base(eventBuss)
        {
            _amountLevels = config.LevelConfigs.Length;
            _dataGenerationService = dataGenerationService;
        }

        public override Task Enter(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _currentLevel = 1;
            EventBuss.AddListener(this);
            return Task.CompletedTask;
        }

        public override Task Exit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EventBuss.RemoveListener(this);
            return Task.CompletedTask;
        }

        public void Execute(WinConditionIsDone value)
        {
            if (_currentLevel >= _amountLevels)
            {
                Complete();
                EventBuss.Execute(new ExitState<MatchState>());
            }
            else
            {
                _dataGenerationService.Generate();
                _currentLevel++;
            }
        }
    }
}