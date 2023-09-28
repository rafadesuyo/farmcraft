using DG.Tweening;
using UnityEngine;

public class UIControllerAnimated : UIController
{
    //Enums
    public enum TweenDirection { Center, Up, Right, Down, Left }

    //Variables
    [Space(10)]

    [SerializeField] protected float openAnimationDuration = 0.2f;
    [SerializeField] protected Ease openTweenType = Ease.Linear;

    [Space(10)]

    [SerializeField] protected float closeAnimationDuration = 0.2f;
    [SerializeField] protected Ease closeTweenType = Ease.Linear;

    [Space(10)]

    [SerializeField] protected TweenDirection tweenDirection = TweenDirection.Center;
    [SerializeField] protected float tweenOffSet;

    [Space(10)]

    [SerializeField] protected bool popup;

    [Space(10)]

    [SerializeField] protected bool fade;
    [SerializeField] protected bool blockerBackgroundFade;

    protected override void PlayOpenAnimation()
    {
        OnAnimationStarted();

        content.anchoredPosition = GetAnimationAchoredPosition();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(content.DOAnchorPos(initialAnchoredPosition, openAnimationDuration).SetEase(openTweenType));

        if(popup == true)
        {
            content.localScale = Vector3.zero;

            sequence.Join(content.DOScale(1, openAnimationDuration).SetEase(openTweenType));
        }

        if(fade == true)
        {
            contentCanvasGroup.alpha = 0;

            sequence.Join(contentCanvasGroup.DOFade(1, openAnimationDuration).SetEase(openTweenType));
        }

        sequence.AppendCallback(OnOpenAnimationEnded);
        
        if (blockerBackgroundFade == true && blockerBackground.gameObject.activeSelf == true)
        {
            blockerBackgroundCanvasGroup.alpha = 0;

            blockerBackgroundCanvasGroup.DOFade(1, openAnimationDuration).SetEase(openTweenType);
        }
    }

    protected override void PlayCloseAnimation()
    {
        OnAnimationStarted();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(content.DOAnchorPos(GetAnimationAchoredPosition(), closeAnimationDuration).SetEase(closeTweenType));

        if (popup == true)
        {
            sequence.Join(content.DOScale(Vector3.zero, closeAnimationDuration).SetEase(closeTweenType));
        }

        if (fade == true)
        {
            sequence.Join(contentCanvasGroup.DOFade(0, closeAnimationDuration).SetEase(closeTweenType));
        }

        sequence.AppendCallback(OnCloseAnimationEnded);

        if (blockerBackgroundFade == true && blockerBackground.gameObject.activeSelf == true)
        {
            blockerBackgroundCanvasGroup.DOFade(0, closeAnimationDuration).SetEase(closeTweenType);
        }
    }

    protected Vector2 GetAnimationAchoredPosition()
    {
        switch(tweenDirection)
        {
            case TweenDirection.Up:
                return content.anchoredPosition + (Vector2.up * tweenOffSet);

            case TweenDirection.Right:
                return content.anchoredPosition + (Vector2.right * tweenOffSet);

            case TweenDirection.Down:
                return content.anchoredPosition + (Vector2.down * tweenOffSet);

            case TweenDirection.Left:
                return content.anchoredPosition + (Vector2.left * tweenOffSet);

            default:
                return content.anchoredPosition;
        }
    }
}
