using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusSlot : MonoBehaviour
{
    [SerializeField] private Image iconImg = null;
    [SerializeField] private RectTransform imgRectTransform = null;
    [SerializeField] private RectTransform test = null;

    private void OnEnable()
    {
        StatusUI.OnWheelRotate += RotateSlotImage;
    }

    private void OnDisable()
    {
        StatusUI.OnWheelRotate -= RotateSlotImage;
    }

    public void RotateSlotImage()
    {
        float zRotation = test.localRotation.eulerAngles.z;
        Vector3 angle = new Vector3(imgRectTransform.localEulerAngles.x, imgRectTransform.localEulerAngles.y, -zRotation);

        iconImg.rectTransform.localEulerAngles = angle;
    }

    public void UpdateSlotStatus(Sprite sprite)
    {
        iconImg.sprite = sprite;
    }
}
