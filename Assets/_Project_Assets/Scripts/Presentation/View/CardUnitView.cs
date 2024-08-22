using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Presentation.View
{
    public class CardUnitView : MonoBehaviour
    {
        [SerializeField] private Button cardBtn;
        [SerializeField] private Image cardImage;

        public void SetImage(Sprite sprite) => 
            cardImage.sprite = sprite;
    }
}