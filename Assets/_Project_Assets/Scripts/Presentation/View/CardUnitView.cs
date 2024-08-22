using System;
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

        public event Action<int> OnFlipCard = delegate { }; 
        
        private SignalBus _signalBus;
        private RectTransform _rectTransform;
        private Sequence _sequence;
        
        private int _index;

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

        public void SetData(int index)
        {
            _index = index;
        }

        private void FlipCard()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_rectTransform.DORotate(_rectTransform.rotation.eulerAngles + new Vector3(0, 90f, 0f), 1f));
            _sequence.AppendInterval(1f);
            _sequence.Append(_rectTransform.DORotate(_rectTransform.rotation.eulerAngles + new Vector3(0, 0f, 0f), 1f));
            _sequence.Play();
            
            OnFlipCard?.Invoke(_index);
            
            Debug.Log(_index);
        }

        public void SetImage(Sprite sprite) => 
            cardImage.sprite = sprite;
        
        public void ResetAllSubscriptions() =>
            OnFlipCard = null;

        private void OnDestroy() => 
            cardBtn.onClick.RemoveListener(FlipCard);
    }
}