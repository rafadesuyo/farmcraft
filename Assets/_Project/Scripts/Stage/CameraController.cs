using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Constants
    public const float minZoomSize = 5;
    public const float maxZoomSize = 30;

    //Components
    [Header("Components")]
    [SerializeField] private Camera mainCamera;

    private BoxCollider2D cameraBounds;
    //Enums
    public enum CameraMode { FollowPlayer, Free }
    //Variables
    [Header("Camera Follow")]
    [SerializeField] private Transform targetToFollow;
    [SerializeField] private Vector3 offSet = new Vector3(0, 0, -10);
    [SerializeField] [Range(0, 1000)] private float damping;

    [Header("Camera Zoom")]
    [Tooltip("The minimun size of the camera, it is always less or equal the Zoom Max.")]
    [SerializeField] [Range(minZoomSize, maxZoomSize)] private float zoomMin = 5;

    [Tooltip("The max size of the camera, it is always greater or equal the Zoom Min.")]
    [SerializeField] [Range(minZoomSize, maxZoomSize)] private float zoomMax = 12;

#if UNITY_EDITOR
    [Space(10)]
    [Tooltip("The current size of the Camera, editing it only affects the Camera in Play Mode, it is always greater or equal the Zoom Min and less or equal the Zoom Max.")]
    [SerializeField] private float currentZoom;
#endif

    [Header("Options")]
    [SerializeField] private float minimunDistanceToFreeTheCamera = 0.05f;
    [SerializeField] private float pcZoomSensitivity = 3;
    [SerializeField] private float mobileZoomSensitivity = 0.01f;
    [Space(10)]
    [SerializeField] private bool moveOnAxisX = true;
    [SerializeField] private bool moveOnAxisY = true;
    [Space(10)]
    [SerializeField] private bool allowMovement = true;
    [SerializeField] private bool allowZoom = true;
    //Control
    CameraMode currentCameraMode;
    private bool canMoveCamera;
    private Vector3 touchStart;
    //Movement
    private Vector2 cameraExtents;
    private float limitLeft;
    private float limitRight;
    private float limitUp;
    private float limitDown;
    //Getters
    public Transform TargetToFollow { get => targetToFollow; set => targetToFollow = value; }
    public bool AllowMovement { get => allowMovement; set => allowMovement = value; }
    public bool AllowZoom { get => allowZoom; set => allowZoom = value; }

    private void Awake()
    {
        canMoveCamera = false;
    }

    private void Start()
    {
        UpdateCameraExtents();
        GetCameraBoundsFromStage();
    }
    private void Update()
    {
        CameraControl();
    }
    private void LateUpdate()
    {
        CameraFollow();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        zoomMax = Mathf.Clamp(zoomMax, minZoomSize, maxZoomSize);
        zoomMin = Mathf.Clamp(zoomMin, minZoomSize, zoomMax);
        currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);

        if (Application.isPlaying == true)
        {
            SetCameraSize(currentZoom);
        }
    }
#endif

    public void SetCameraZoomVariables(StageInfoSO stageInfo)
    {
        zoomMin = stageInfo.ZoomMin;
        zoomMax = stageInfo.ZoomMax;

        SetCameraSize(stageInfo.InitialZoom);
        GetCameraBoundsFromStage();
    }

    private void UpdateCameraExtents()
    {
        float sizeX = mainCamera.aspect * 2f * mainCamera.orthographicSize;
        float sizeY = 2f * mainCamera.orthographicSize;
        cameraExtents = new Vector2(sizeX, sizeY) / 2;
    }
    public void GetCameraBoundsFromStage()
    {
        StageHolder stageHolder = FindObjectOfType<StageHolder>();
        if (stageHolder == null)
        {
            return;
        }
        SetCameraBounds(stageHolder.CameraBounds);
    }
    public void SetCameraBounds(BoxCollider2D newCameraBounds)
    {
        cameraBounds = newCameraBounds;
        UpdateCameraBounds();
    }
    private void UpdateCameraBounds()
    {
        Vector3 center = cameraBounds.bounds.center;
        Vector2 extents = Vector2.Scale(cameraBounds.size, cameraBounds.transform.lossyScale) / 2;
        limitLeft = center.x - extents.x;
        limitRight = center.x + extents.x;
        limitUp = center.y + extents.y;
        limitDown = center.y - extents.y;
    }

    public void SetCameraFollow(Transform transform)
    {
        targetToFollow = transform;
    }
    public void SetCameraMode(CameraMode cameraMode)
    {
        currentCameraMode = cameraMode;
    }
    public void SetCameraMode(int cameraMode)
    {
        SetCameraMode((CameraMode)cameraMode);
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero + offSet;
    }
    public void TeleportToTarget()
    {
        if (targetToFollow == null)
        {
            return;
        }
        transform.position = targetToFollow.transform.position + offSet;
    }
    private void CameraFollow()
    {
        if (currentCameraMode != CameraMode.FollowPlayer)
        {
            return;
        }
        if (targetToFollow == null)
        {
            return;
        }
        Vector3 movement = ((targetToFollow.position + offSet) - transform.position) / damping;
        TranslateCamera(movement);
    }
    private void TranslateCamera(Vector3 movement)
    {
        Vector3 currentPosition = transform.position;
        float posX = currentPosition.x + movement.x;
        float posY = currentPosition.y + movement.y;
        float posZ = currentPosition.z + movement.z;
        posX = Mathf.Clamp(posX, limitLeft + cameraExtents.x, limitRight - cameraExtents.x);
        posY = Mathf.Clamp(posY, limitDown + cameraExtents.y, limitUp - cameraExtents.y);
        if (moveOnAxisX == false)
        {
            posX = currentPosition.x;
        }
        if (moveOnAxisY == false)
        {
            posY = currentPosition.y;
        }
        transform.position = new Vector3(posX, posY, posZ);
    }
    private void CameraControl()
    {
        if (Input.GetMouseButton(0) == false)
        {
            canMoveCamera = false;
        }
        CameraMovement();
    }
    private void CameraMovement()
    {
        if (PlayerInput.EnableInput == false)
        {
            return;
        }
        if (allowMovement == false)
        {
            return;
        }
#if UNITY_EDITOR
        ControlCamera_PC();
#else
        ControlCamera_Mobile();
#endif
    }
    private void ControlCamera_PC()
    {
        CheckZoom_PC();
        if (Input.GetMouseButtonDown(0))
        {
            StartMovement(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            MoveCamera(Input.mousePosition);
        }
    }
    private void ControlCamera_Mobile()
    {
        if (Input.touchCount == 2)
        {
            CheckZoom_Mobile();
            canMoveCamera = false;
        }
        else
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartMovement(Input.GetTouch(0).position);
            }
            if (Input.touchCount > 0)
            {
                MoveCamera(Input.GetTouch(0).position);
            }
        }
    }
    private void StartMovement(Vector3 clickPosition)
    {
        if (ClickHelper.IsPointerOverUI() == false)
        {
            touchStart = mainCamera.ScreenToWorldPoint(clickPosition);
            canMoveCamera = true;
        }
    }
    private void MoveCamera(Vector3 clickPosition)
    {
        if (canMoveCamera == true)
        {
            Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(clickPosition);
            TranslateCamera(direction);
            if (direction.sqrMagnitude > minimunDistanceToFreeTheCamera)
            {
                SetCameraMode(CameraMode.Free);
            }
        }
    }
    private void CheckZoom_PC()
    {
        if (allowZoom == false)
        {
            return;
        }
        float zoomIncrement = Input.GetAxis("Mouse ScrollWheel") * -1;
        if (zoomIncrement != 0)
        {
            ZoomCamera(zoomIncrement * pcZoomSensitivity);
        }
    }
    private void CheckZoom_Mobile()
    {
        if (allowZoom == false)
        {
            return;
        }
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);
        Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
        Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
        float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
        float currentMagnitude = (touch0.position - touch1.position).magnitude;
        float difference = currentMagnitude - prevMagnitude;
        if (difference != 0)
        {
            ZoomCamera(difference * mobileZoomSensitivity * -1);
        }
    }
    private void ZoomCamera(float increment)
    {
        if (ClickHelper.IsPointerOverUI() == false)
        {
            UpdateCameraSize(increment);
        }
    }
    private void UpdateCameraSize(float increment)
    {
        float newSize = mainCamera.orthographicSize + increment;

        SetCameraSize(newSize);
    }

    private void SetCameraSize(float newSize)
    {
        mainCamera.orthographicSize = Mathf.Clamp(newSize, zoomMin, zoomMax);

        UpdateCameraExtents();

        TranslateCamera(Vector3.zero);

#if UNITY_EDITOR
        currentZoom = mainCamera.orthographicSize;
#endif
    }
}