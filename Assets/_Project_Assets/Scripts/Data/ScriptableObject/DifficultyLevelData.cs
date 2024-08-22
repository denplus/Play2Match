using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data.ScriptableObject
{
    [CreateAssetMenu(fileName = "DifficultyLevel", menuName = "Data/DifficultyLevel", order = 0)]
    public class DifficultyLevelData : UnityEngine.ScriptableObject
    {
        [SerializeField] private List<Vector2> gridSizes;

        public IEnumerable<Vector2> GridSizes => gridSizes;
    }
}