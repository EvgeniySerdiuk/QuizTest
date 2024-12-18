using Project.Scripts.Cell;
using UnityEngine;

namespace Project.Scripts.Level
{
    [CreateAssetMenu(menuName = "Game/" + nameof(LevelConfig))]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public Vector2 CellSize { get; private set; }
        [field: SerializeField] public int AmountColumn { get; private set; }
        [field: SerializeField] public int AmountLine { get; private set; }

        [field: SerializeField] public CellsDataConfig[] CellsDataConfigs { get; private set; }
    }
}