using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaSetter : MonoBehaviour
{
    //Components
    private Canvas canvas;
    private RectTransform panelSafeArea;

    //Variables
    private Rect currentSafeArea = new Rect();
    private ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        panelSafeArea = GetComponent<RectTransform>();

        currentOrientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;

        ApplySafeArea();
    }

    private void Update()
    {
        if ((currentOrientation != Screen.orientation) || (currentSafeArea != Screen.safeArea))
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        if (panelSafeArea == null)
        {
            Debug.LogError("The object doesnt have a RectTranform to be used!");
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;

        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        panelSafeArea.anchorMin = anchorMin;
        panelSafeArea.anchorMax = anchorMax;

        currentOrientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;
    }
}
