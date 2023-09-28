using UnityEngine.EventSystems;
using UnityEngine;

public static class ClickHelper
{
    public static bool IsPointerOverUI()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
#else
         if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return true;
        }
#endif
        return false;
    }
}