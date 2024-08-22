using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Utils
{
    public class FlexibleGridLayout : GridLayoutGroup
    {
        [SerializeField] private int constrains; // to change click 3 dots and select debug mode render from inspector
        
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
            constraintCount = constrains;
            float x = (rectTransform.rect.size.x - padding.horizontal - spacing.x * (constraintCount - 1)) / constraintCount;
            cellSize = new Vector2(x, cellSize.y);
        }
    }
}