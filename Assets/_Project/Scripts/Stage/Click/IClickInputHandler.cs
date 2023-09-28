using UnityEngine;

public interface IClickInputHandler
{
    bool CheckPointerDown();
    bool CheckPointerUp();
    void UpdateObjectPointerState(GameObject currentObject, GameObject previousObject);
}
