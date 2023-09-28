using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeIconButton : MonoBehaviour
{
    //Components
    private GameModeIcon gameModeIcon;
    private RectTransform rectTransform;
    private Canvas canvas;

    //Variables
    [Header("Variables")]
    [SerializeField] private float distanceToMoveIcon = 300;

    private Vector3 initialPosition;

    public void Init(GameModeIcon gameModeIcon)
    {
        canvas = GetComponentInParent<Canvas>();

        ClickAndDragButton clickAndDragButton = GetComponent<ClickAndDragButton>();

        clickAndDragButton.OnPointerUpEvent.AddListener(SelectIcon);
        clickAndDragButton.OnBeginDragEvent.AddListener(OnBeginDrag);
        clickAndDragButton.OnDragEvent.AddListener(OnDrag);
        clickAndDragButton.OnEndDragEvent.AddListener(OnEndDrag);

        this.gameModeIcon = gameModeIcon;
        rectTransform = this.gameModeIcon.IconsHolder;
    }

    private void SelectIcon()
    {
        gameModeIcon.SelectIcon();
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.anchoredPosition;
    }

    private void OnDrag(PointerEventData eventData)
    {
        Vector2 movement = eventData.delta / canvas.scaleFactor;
        Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x + movement.x, rectTransform.anchoredPosition.y);

        rectTransform.anchoredPosition = newPosition;
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        float distance = rectTransform.anchoredPosition.x - initialPosition.x;

        if(distance > distanceToMoveIcon)
        {
            gameModeIcon.IconMovedToRight();
        }
        else if(distance < (distanceToMoveIcon * -1))
        {
            gameModeIcon.IconMovedToLeft();
        }

        gameModeIcon.IconEndDrag();
    }
}
