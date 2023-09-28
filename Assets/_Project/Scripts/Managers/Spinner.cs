using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : PersistentSingleton<Spinner>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float inTransitionDuration = .1f;
    [SerializeField] private float outTransitionDuration = .4f;
    [SerializeField] private float outerSpinnerLoopTime = 6;
    [SerializeField] private float innerSpinnerLoopTime = 4;
    [SerializeField] private GameObject outerSpinnerContainer;
    [SerializeField] private GameObject innerSpinnerContainer;

    private void Start()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    public IEnumerator TransitionIn()
    {
        outerSpinnerContainer.transform.DOLocalRotate(new Vector3(0, 0, 360), outerSpinnerLoopTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);
        innerSpinnerContainer.transform.DOLocalRotate(new Vector3(0, 0, 360), innerSpinnerLoopTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, inTransitionDuration);
        yield return new WaitForSeconds(inTransitionDuration);
        canvasGroup.alpha = 1;
    }

    public IEnumerator TransitionOut()
    {
        canvasGroup.DOFade(0, outTransitionDuration);
        yield return new WaitForSeconds(outTransitionDuration);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;

        DOTween.Kill(outerSpinnerContainer);
        DOTween.Kill(innerSpinnerContainer);
    }
}
