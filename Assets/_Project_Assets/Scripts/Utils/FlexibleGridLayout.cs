using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Utils
{
    public class FlexibleGridLayout : GridLayoutGroup
    {
        // to change click 3 dots and select debug mode render from inspector
        [field: SerializeField] public int AmountX { get; set; }
        [field: SerializeField] public int AmountY { get; set; }

        public override void SetLayoutHorizontal()
        {
            UpdateCellSize();
            base.SetLayoutHorizontal();
        }

        public override void SetLayoutVertical()
        {
            UpdateCellSize();
            base.SetLayoutVertical();
        }

        private void UpdateCellSize()
        {
            constraint = Constraint.FixedColumnCount;
            constraintCount = AmountX;
            float x = (rectTransform.rect.size.x - padding.horizontal - spacing.x * (constraintCount - 1)) / constraintCount;
            float y = (rectTransform.rect.size.y - padding.vertical - spacing.y * (AmountY - 1)) / AmountY;
            cellSize = new Vector2(x, y);
        }
    }
}