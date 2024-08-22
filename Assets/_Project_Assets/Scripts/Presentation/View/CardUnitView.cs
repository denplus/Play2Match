using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Presentation.View
{
    public class CardUnitView : MonoBehaviour
    {
        [SerializeField] private Button cardBtn;
        [SerializeField] private Image cardImage;

        public int Index { get; private set; }
        public float AnimationDuration => 0.25f;
        public event Action<CardUnitView> OnFlipCard = delegate { };

        private RectTransform _rectTransform;
        private RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();

        private void Start() => 
            cardBtn.onClick.AddListener(CardClick);

        private void OnEnable() => 
            RectTransform.rotation = Quaternion.Euler(Vector3.zero);

        public void SetData(int index)
        {
            Index = index;
            RectTransform.rotation = Quaternion.Euler(Vector3.zero);
            cardBtn.interactable = true;
        }

        private void CardClick()
        {
            AnimateFlip(true);
            OnFlipCard?.Invoke(this);
        }

        public void AnimateFlip(bool showCard)
        {
            cardBtn.interactable = !showCard;
            Vector3 rotate = Vector3.up * (showCard ? 90f : 0f); 
            RectTransform.DORotate(rotate, AnimationDuration);
        }

        public void SetImage(Sprite sprite) =>
            cardImage.sprite = sprite;

        public void ResetAllSubscriptions() =>
            OnFlipCard = null;

        private void OnDestroy() =>
            cardBtn.onClick.RemoveListener(CardClick);
    }
}