using DG.Tweening;
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
        private RectTransform _rectTransform;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            cardBtn.onClick.AddListener(FlipCard);
            _rectTransform = GetComponent<RectTransform>();
        }

        private void FlipCard()
        {
            _rectTransform.DORotate(_rectTransform.rotation.eulerAngles + new Vector3(0, 90f, 0f), 1f);
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