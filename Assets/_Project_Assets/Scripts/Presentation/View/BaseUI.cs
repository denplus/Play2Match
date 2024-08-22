using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Presentation.View
{
    public class BaseUI : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        protected virtual UniTask Show() // can be overridden to add some animations on show
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            return UniTask.CompletedTask;
        }

        protected virtual UniTask Hide() // can be overridden to add some animations on hide
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            return UniTask.CompletedTask;
        }
    }
}