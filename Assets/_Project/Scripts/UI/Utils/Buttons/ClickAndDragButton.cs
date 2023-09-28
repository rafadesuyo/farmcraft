using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAndDragButton : Selectable, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Events
    [Header("Events")]
    [SerializeField] private UnityEvent onPointerDownEvent = new UnityEvent();
    [SerializeField] private UnityEvent onPointerUpEvent = new UnityEvent();
    [SerializeField] private UnityEvent<PointerEventData> onBeginDragEvent = new UnityEvent<PointerEventData>();
    [SerializeField] private UnityEvent<PointerEventData> onDragEvent = new UnityEvent<PointerEventData>();
    [SerializeField] private UnityEvent<PointerEventData> onEndDragEvent = new UnityEvent<PointerEventData>();

    //Variables
    private bool selected;

    //Getters
    public UnityEvent OnPointerDownEvent => onPointerDownEvent;
    public UnityEvent OnPointerUpEvent => onPointerUpEvent;
    public UnityEvent<PointerEventData> OnBeginDragEvent => onBeginDragEvent;
    public UnityEvent<PointerEventData> OnDragEvent => onDragEvent;
    public UnityEvent<PointerEventData> OnEndDragEvent => onEndDragEvent;

    protected override void Awake()
    {
        selected = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        selected = true;

        if(interactable == true)
        {
            onPointerDownEvent?.Invoke();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if(selected == true)
        {
            if (interactable == true)
            {
                onPointerUpEvent?.Invoke();
            }
        }

        selected = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        selected = false;

        if (interactable == true)
        {
            onBeginDragEvent?.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (interactable == true)
        {
            OnDragEvent?.Invoke(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (interactable == true)
        {
            onEndDragEvent?.Invoke(eventData);
        }
    }
}
