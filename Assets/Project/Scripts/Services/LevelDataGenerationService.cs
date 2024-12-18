using System;
using System.Linq;
using Project.Scripts.Cell;
using Project.Scripts.EventBuss;
using Project.Scripts.Match;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Services
{
    public struct LevelDataIsGenerated
    {
        public CellData[] CellsData;
        public Vector2[] CellPositions;
        public int LevelIndex;
    }

    public class LevelDataGenerationService
    {
        private readonly MatchConfig _matchConfig;
        private readonly IEventBuss _eventBuss;

        private int _levelIndex;

        public LevelDataGenerationService(MatchConfig matchConfig, IEventBuss eventBuss)
        {
            _matchConfig = matchConfig;
            _eventBuss = eventBuss;
        }

        public void Generate()
        {
            var levelConfig = _matchConfig.LevelConfigs[_levelIndex];
            var randomConfig = levelConfig.CellsDataConfigs[Random.Range(0, levelConfig.CellsDataConfigs.Length)];
            var amountCells = levelConfig.AmountColumn * levelConfig.AmountLine;

            var cellsId = randomConfig.CellData
                .OrderBy(x => Random.value)
                .Take(amountCells)
                .ToArray();

            _eventBuss.Execute(new LevelDataIsGenerated
            {
                CellsData = cellsId,
                CellPositions =
                    GeneratePositions(levelConfig.AmountColumn, levelConfig.AmountLine, levelConfig.CellSize),
                LevelIndex = _levelIndex
            });

            _levelIndex++;
        }

        public void Restart()
        {
            _levelIndex = 0;
        }

        private Vector2[] GeneratePositions(int amountColumn, int amountLine, Vector2 cellSize)
        {
            var positions = new Vector2[amountColumn * amountLine];
            var startX = -(amountColumn - 1) * cellSize.x / 2;
            var startY = -(amountLine - 1) * cellSize.y / 2;

            for (int i = 0, x = 0; x < amountColumn; x++)
            {
                for (int y = 0; y < amountLine; y++, i++)
                {
                    positions[i] = new Vector2(startX + x * cellSize.x, startY + y * cellSize.y);
                }
            }

            return positions;
        }
    }
}