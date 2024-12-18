using System;
using UnityEngine;

namespace Project.Scripts.Cell
{
    [Serializable]
    public class CellData
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Id { get; private set; }
    }

    [CreateAssetMenu(menuName = "Game/" + nameof(CellsDataConfig))]
    public class CellsDataConfig : ScriptableObject
    {
        [field: SerializeField] public CellData[] CellData { get; private set; }
    }
}