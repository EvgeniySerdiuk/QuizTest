using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Cell;
using Project.Scripts.EventBuss;
using UnityEngine;
using VContainer.Unity;

namespace Project.Scripts.Services
{
    public struct WinConditionIsDone{}
    public struct UniqueValuesEnded{}
    public struct ChooseWinConditionId
    {
        public string ConditionId;
    }

    public class WinConditionHandlerService : IInitializable,
        IListener<LevelDataIsGenerated>,
        IListener<CellViewsIsSpawned>
    {
        private readonly IEventBuss _eventBuss;
        private readonly List<string> _unusedId;

        private string _currentConditionId;

        public WinConditionHandlerService(IEventBuss eventBuss)
        {
            _eventBuss = eventBuss;
            _unusedId = new List<string>();
        }

        public void Initialize()
        {
            _eventBuss.AddListener<CellViewsIsSpawned>(this);
            _eventBuss.AddListener<LevelDataIsGenerated>(this);
        }

        public void Execute(LevelDataIsGenerated value)
        {
            var randomData = value.CellsData
                .Where(x => !_unusedId.Contains(x.Id))
                .OrderBy(_ => Random.value)
                .FirstOrDefault();

            if (randomData == null)
            {
                _eventBuss.Execute(new UniqueValuesEnded());
            }

            _currentConditionId = randomData.Id;
            _unusedId.Add(randomData.Id);

            Debug.Log($"<color=green>Choose win condition</color>: {_currentConditionId}");
            _eventBuss.Execute(new ChooseWinConditionId { ConditionId = _currentConditionId });
        }

        public void Execute(CellViewsIsSpawned value)
        {
            foreach (var cellView in value.CellViews)
            {
                cellView.OnCellClicked += CheckWinConditions;
            }
        }

        private void CheckWinConditions(string id, CellView cellView)
        {
            if (_currentConditionId.Equals(id))
            {
                cellView.ShowTrueCellEffect(() =>
                {
                    Debug.Log($"<color=green>Win condition is done!</color>");
                    _eventBuss.Execute(new WinConditionIsDone());
                });
            }
            else
            {
                cellView.ShowFalseCellEffect();
            }
        }
    }
}