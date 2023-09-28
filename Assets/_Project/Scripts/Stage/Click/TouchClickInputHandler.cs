using UnityEngine;
using UnityEngine.EventSystems;

public class TouchClickInputHandler : IClickInputHandler
{
    public bool CheckPointerDown()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public bool CheckPointerUp()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
    }

    public void UpdateObjectPointerState(GameObject currentObject, GameObject previousObject)
    {

    }
}