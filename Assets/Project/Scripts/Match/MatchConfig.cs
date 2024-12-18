using Project.Scripts.Cell;
using Project.Scripts.Level;
using UnityEngine;

namespace Project.Scripts.Match
{
    [CreateAssetMenu(menuName = "Game/" + nameof(MatchConfig))]
    public class MatchConfig : ScriptableObject
    {
        [field: SerializeField] public CellView CellViewPrefab { get; private set; }
        [field: SerializeField] public string TitleTextFormat { get; private set; }
        [field: SerializeField] public LevelConfig[] LevelConfigs { get; private set; }
    }
}