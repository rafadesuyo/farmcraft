using DG.Tweening;
using UnityEngine;

namespace DreamQuiz
{
    public class LoginErrorMessageUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup errorPanelCanvasGroup;
        [SerializeField] TMPro.TextMeshProUGUI errorMessageTextMesh;
        [SerializeField] float transitionTime = 0.2f;
        [SerializeField] float duration = 2f;

        private void Start()
        {
            LoginManager.Instance.OnLoginError += Instance_OnLoginError;
            errorPanelCanvasGroup.alpha = 0;
        }

        private void OnDestroy()
        {
            LoginManager.Instance.OnLoginError -= Instance_OnLoginError;
        }

        private void Instance_OnLoginError(string errorMessage)
        {
            errorPanelCanvasGroup.alpha = 0;
            errorMessageTextMesh.text = errorMessage;
            DOTween.Kill(errorPanelCanvasGroup);
            errorPanelCanvasGroup.DOFade(1, transitionTime);
            errorPanelCanvasGroup.DOFade(0, transitionTime).SetDelay(duration);
        }
    }
}