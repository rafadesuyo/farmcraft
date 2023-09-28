using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace DreamQuiz
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Selectable : MonoBehaviour, IClickablePointerDown, IClickablePointerUp
    {
        [Serializable]
        public class SelectEvent : UnityEvent { }

        public enum SelectionState
        {
            None,
            PointerDown,
            PointerUp,
            Holding
        }

        [Header("Options")]
        [SerializeField] private float minHoldTime = 0.5f;

        [Header("Animation")]
        [SerializeField] private float clickAnimationDuration = 0.1f;
        [SerializeField] private Transform clickableModel;
        [SerializeField] private float pressingScale = 0.9f;
        [SerializeField] private float holdingScale = 0.8f;

        [Header("Events")]
        [SerializeField] private SelectEvent onPointerDownEvent;
        [SerializeField] private SelectEvent onPointerUpEvent;
        [SerializeField] private SelectEvent holdingEvent;

        private float currentHoldTime = 0;
        private SelectionState selectionState = SelectionState.None;

        private void Update()
        {
            if (selectionState != SelectionState.PointerDown)
            {
                return;
            }

            currentHoldTime += Time.deltaTime;

            if (currentHoldTime >= minHoldTime)
            {
                SetSelectionState(SelectionState.Holding);
                clickableModel.DOScale(holdingScale, clickAnimationDuration);
                holdingEvent?.Invoke();
            }
        }

        public void OnPointerDown()
        {
            currentHoldTime = 0f;
            SetSelectionState(SelectionState.PointerDown);
            clickableModel.DOScale(pressingScale, clickAnimationDuration);
            onPointerDownEvent?.Invoke();
        }

        public void OnPointerUp()
        {
            if (selectionState == SelectionState.PointerDown)
            {
                SetSelectionState(SelectionState.PointerUp);
                onPointerUpEvent?.Invoke();
            }

            clickableModel.DOScale(1, clickAnimationDuration).SetEase(Ease.OutElastic);
            SetSelectionState(SelectionState.None);
        }

        private void SetSelectionState(SelectionState selection)
        {
            selectionState = selection;
        }
    }
}