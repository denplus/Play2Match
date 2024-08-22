using UnityEngine;

namespace Scripts.Data.Signals
{
    public class StartGameSignal
    {
        public Vector2Int GridSize;

        public StartGameSignal(Vector2Int gridSize)
        {
            GridSize = gridSize;
        }
    }
}