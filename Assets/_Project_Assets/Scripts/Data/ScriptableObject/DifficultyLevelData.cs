using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data.ScriptableObject
{
    [CreateAssetMenu(fileName = "DifficultyLevel", menuName = "Data/DifficultyLevel", order = 0)]
    public class DifficultyLevelData : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public List<Vector2Int> GridSizes { get; private set; }
    }
}