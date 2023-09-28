using System;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    //Variables
    [Header("Components")]
    [SerializeField] private Camera mainCamera;

    [Header("Variables")]
    [SerializeField] private LayerMask clickableLayers;

    [Space(10)]

    [SerializeField] private float maxRaycastDistance = 100f;

    [Header("Debug")]
    [SerializeField] private bool printClicksBlockedByTheUI = false;

    private GameObject currentObject = null;
    private GameObject previousObject = null;

    private IClickInputHandler clickInputHandler;
    bool initialized = false;
    bool isRedirectingClick = false;
    Action<GameObject> redirectClickCallback;

    private void Awake()
    {
#if UNITY_EDITOR
        clickInputHandler = new MouseClickInputHandler();
#else
        clickInputHandler = new TouchClickInputHandler();
#endif
        if (clickInputHandler == null)
        {
            Debug.LogError("[ClickManager] missing IClickInputHandler reference");
            return;
        }

        initialized = true;
    }

    private void Update()
    {
        if (initialized == false)
        {
            return;
        }

        currentObject = SendRaycast(Input.mousePosition);

        if (currentObject != null)
        {
            CheckPointerDown();
            CheckPointerUp();
        }

        UpdateObjectPointerState();

        previousObject = currentObject;
    }

    private void CheckPointerDown()
    {
        if (clickInputHandler.CheckPointerDown() == false)
        {
            return;
        }

        if (isRedirectingClick)
        {
            return;
        }

        IClickablePointerDown clickablePointerDown = currentObject.GetComponent<IClickablePointerDown>();
        clickablePointerDown?.OnPointerDown();
    }

    private void CheckPointerUp()
    {
        if (clickInputHandler.CheckPointerUp() == false)
        {
            return;
        }

        if (isRedirectingClick)
        {
            redirectClickCallback?.Invoke(currentObject);

            currentObject = null;
            isRedirectingClick = false;
            return;
        }

        IClickablePointerUp clickablePointerUp = currentObject.GetComponent<IClickablePointerUp>();
        clickablePointerUp?.OnPointerUp();
    }

    private void UpdateObjectPointerState()
    {
        clickInputHandler.UpdateObjectPointerState(currentObject, previousObject);
    }

    private GameObject SendRaycast(Vector3 clickPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(clickPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, maxRaycastDistance, clickableLayers);

        if (hit.collider != null)
        {
            return SelectObject(hit.collider.gameObject);
        }

        return null;
    }

    private GameObject SelectObject(GameObject selectedGameObject)
    {
        if (ClickHelper.IsPointerOverUI() == true)
        {
            if (printClicksBlockedByTheUI == true)
            {
                Debug.LogWarning($"Can't click on the object \"{selectedGameObject}\" because the mouse is pointing at the UI.");
            }

            return null;
        }

        return selectedGameObject;
    }

    public void RedirectClick(Action<GameObject> redirectClickCallback)
    {
        this.redirectClickCallback = redirectClickCallback;
        isRedirectingClick = true;
    }
}