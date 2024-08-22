using Scripts.Data.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class CardUnitView : MonoBehaviour
    {
        [SerializeField] private Button cardBtn;
        [SerializeField] private Image cardImage;
        
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            cardBtn.onClick.AddListener(FlipCard);
        }

        private void FlipCard()
        {
            _signalBus.TryFire<CardFlipSignal>();
        }

        public void SetImage(Sprite sprite) => 
            cardImage.sprite = sprite;

        private void OnDestroy()
        {
            cardBtn.onClick.RemoveListener(FlipCard);
        }
    }
}