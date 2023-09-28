using UnityEngine;
using DG.Tweening;

public class WorldMapObjectAnimation : MonoBehaviour
{
    [SerializeField] private float xPosition = 26;
    [SerializeField] private float minAnimationDuration = 180f;
    [SerializeField] private float maxAnimationDuration = 300f;
    [SerializeField] private float minHeightPosition = -15f;
    [SerializeField] private float maxHeightPosition = 15f;

    private Vector3 zRotation = new(0f, 0f, 360f);
    private float animationDuration;
    private float yPosition;

    private void Start()
    {
        animationDuration = Random.Range(minAnimationDuration, maxAnimationDuration);
        yPosition = Random.Range(minHeightPosition, maxHeightPosition);
        StartAnimation();
    }

    private void StartAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(zRotation, animationDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        sequence.Join(transform.DOMoveX(xPosition, animationDuration).SetEase(Ease.Linear));
        sequence.Join(transform.DOMoveY(yPosition, animationDuration).SetEase(Ease.Linear));
        sequence.SetLoops(-1, LoopType.Restart);
    }
}
