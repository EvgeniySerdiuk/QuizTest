using Project.Scripts.Cell;
using Project.Scripts.EventBuss;
using Project.Scripts.Match;
using Project.Scripts.SpecificStates;
using Project.Scripts.StateMachine;
using Unity.Mathematics;
using UnityEngine;
using VContainer.Unity;

namespace Project.Scripts.Services
{
    public struct CellViewsIsSpawned
    {
        public CellView[] CellViews;
    }

    public class CellViewSpawnService : IInitializable, IListener<LevelDataIsGenerated>,
        IListener<ExitState<MatchState>>
    {
        private readonly MatchConfig _matchConfig;
        private readonly IEventBuss _eventBuss;

        private CellView[] _cellViews;

        public CellViewSpawnService(MatchConfig matchConfig, IEventBuss eventBuss)
        {
            _eventBuss = eventBuss;
            _matchConfig = matchConfig;
        }

        public void Initialize()
        {
            _eventBuss.AddListener<ExitState<MatchState>>(this);
            _eventBuss.AddListener<LevelDataIsGenerated>(this);
        }

        public void Execute(ExitState<MatchState> value)
        {
            foreach (var cellView in _cellViews)
            {
                cellView.InactiveClick();
            }
        }

        public void Execute(LevelDataIsGenerated value)
        {
            DeleteOldCells();

            var cellsData = value.CellsData;
            var cellSize = _matchConfig.LevelConfigs[value.LevelIndex].CellSize;

            _cellViews = new CellView[cellsData.Length];

            for (int i = 0; i < cellsData.Length; i++)
            {
                _cellViews[i] = Object.Instantiate(_matchConfig.CellViewPrefab, value.CellPositions[i],
                    quaternion.identity);
                _cellViews[i].Construct(cellsData[i], cellSize);

                if (value.LevelIndex == 0)
                {
                    _cellViews[i].ShowEnableEffect();
                }
            }

            _eventBuss.Execute(new CellViewsIsSpawned
            {
                CellViews = _cellViews
            });
        }

        public void Restart()
        {
            DeleteOldCells();
        }

        private void DeleteOldCells()
        {
            if (_cellViews != null)
            {
                for (int i = 0; i < _cellViews.Length; i++)
                {
                    Object.Destroy(_cellViews[i].gameObject);
                }

                _cellViews = null;
            }
        }
    }
}