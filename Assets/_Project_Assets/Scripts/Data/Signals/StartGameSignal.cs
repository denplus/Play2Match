using UnityEngine;

namespace Scripts.Data.Signals
{
    public class StartGameSignal
    {
        public Vector2 GridSize;

        public StartGameSignal(Vector2 gridSize)
        {
            GridSize = gridSize;
        }
    }
}