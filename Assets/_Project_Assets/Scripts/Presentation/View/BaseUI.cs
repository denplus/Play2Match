using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Presentation.View
{
    public class BaseUI : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        protected virtual UniTask Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            return UniTask.CompletedTask;
        }

        protected virtual UniTask Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            return UniTask.CompletedTask;
        }
    }
}