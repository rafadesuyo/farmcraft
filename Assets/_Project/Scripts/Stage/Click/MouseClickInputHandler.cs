using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickInputHandler : IClickInputHandler
{
    public bool CheckPointerDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool CheckPointerUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public void UpdateObjectPointerState(GameObject currentObject, GameObject previousObject)
    {
        if (currentObject != previousObject)
        {
            if (previousObject != null)
            {
                IClickablePointerExit clickablePointerExit = previousObject.GetComponent<IClickablePointerExit>();
                clickablePointerExit?.OnPointerExit();
            }

            if (currentObject != null)
            {
                IClickablePointerEnter clickablePointerEnter = currentObject.GetComponent<IClickablePointerEnter>();
                clickablePointerEnter?.OnPointerEnter();
            }
        }
    }
}
