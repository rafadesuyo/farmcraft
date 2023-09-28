using UnityEngine;
using TMPro;
using DreamQuiz;

public class NodeInfoUI : MonoBehaviour
{
    [SerializeField] private float minVerticalDistanceFromFinger = 50f;
    [SerializeField] private float minHorizontalDistanceFromFinger = 50f;
    [SerializeField] private TextMeshProUGUI descriptionTxt = null;
    private float minHorizontalDistance = 0;
    private float minVerticalDistance = 0;
    private RectTransform rectTransform = null;

    private void Awake()
    {
        ConfigureViewValues();
    }

    public void Setup(NodeBase nodeBase)
    {
        PositionView(nodeBase.transform.position);

        descriptionTxt.text = nodeBase.GetNodeDescription();
    }

    private void ConfigureViewValues()
    {
        rectTransform = GetComponent<RectTransform>();
        minHorizontalDistance = (rectTransform.rect.width + minHorizontalDistanceFromFinger) / 2;
        minVerticalDistance = (rectTransform.rect.height + minVerticalDistanceFromFinger) / 2;
    }

    private void PositionView(Vector2 nodePosition)
    {
        Vector2 nodeScreenPosition = Camera.main.WorldToScreenPoint(nodePosition);

        Vector2 finalPosition = Vector2.zero;
        finalPosition.x = nodeScreenPosition.x + minHorizontalDistance;
        finalPosition.y = nodeScreenPosition.y + minVerticalDistance;

        // Can't place right
        if (finalPosition.x > Screen.width - minHorizontalDistance)
        {
            finalPosition.x = nodeScreenPosition.x - minHorizontalDistance;
        }

        // Can't place top
        if (finalPosition.y > Screen.height - minVerticalDistance)
        {
            finalPosition.y = nodeScreenPosition.y - minVerticalDistance;
        }

        rectTransform.position = finalPosition;
    }
}
