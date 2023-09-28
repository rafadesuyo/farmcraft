using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAndHoldButton : Selectable
{
    //Events
    [Header("Events")]
    [SerializeField] private UnityEvent onPointerDownEvent = new UnityEvent();
    [SerializeField] private UnityEvent onPointerUpEvent = new UnityEvent();

    //Getters
    public UnityEvent OnPointerDownEvent => onPointerDownEvent;
    public UnityEvent OnPointerUpEvent => onPointerUpEvent;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (!IsActive() || !IsInteractable())
        {
            return;
        }

        onPointerDownEvent?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (!IsActive() || !IsInteractable())
        {
            return;
        }

        onPointerUpEvent?.Invoke();
    }
}
