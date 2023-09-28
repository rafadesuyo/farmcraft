using System.Collections.Generic;
using UnityEngine;
public class WorldMap : MonoBehaviour
{
    //Instance
    private static WorldMap instance;

    //Components
    [Header("Components")]
    [SerializeField] private Transform worldsHolder;
    [SerializeField] private BoxCollider2D cameraBounds;

    [Space(10)]
    [SerializeField] private GameObject player;
    [SerializeField] private CameraController cameraController;

    //Events
    public delegate void StageInfoEvent(StageInfoSO stageInfo);

    public event StageInfoEvent OnStageSelected;
    public event StageInfoEvent OnNeedCollectibleToPlayStage;

    //Variables
    [Header("Variables")]
    [SerializeField] private WorldMapInfoSO worldMapInfo;

    private List<World> worlds = new List<World>();

    //Getters
    public static WorldMap Instance => instance;
    public List<World> Worlds => worlds;

    //TODO: This is only for the MVP, delete it later or move to a Scriptable Object
    [Space(20)]
    [SerializeField] CollectibleType[] collectiblesToUnlockLastStage;

    public void Open()
    {
        if (gameObject.activeSelf == true)
        {
            return;
        }

        gameObject.SetActive(true);

        OnOpen();
    }

    public void OnOpen()
    {
        cameraController.AllowMovement = true;

        SetCameraToFollowIncompleteStage();
        SetCurrentStageToLastCompletedStage();
    }

    public void Close()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }

        OnClose();

        gameObject.SetActive(false);
    }

    public void OnClose()
    {
        if (cameraController != null)
        {
            cameraController.AllowMovement = false;
            cameraController.ResetPosition();
        }
    }

    public void Init()
    {
        //Singleton instance
        instance = this;

        gameObject.SetActive(true);
        CreateWorlds();
        gameObject.SetActive(false);
    }

    private void CreateWorlds()
    {
        float offSet = 0;
        int stageNumber = 0;

        for (int i = 0; i < worldMapInfo.Worlds.Length; i++)
        {
            //World
            World world = Instantiate(worldMapInfo.Worlds[i].WorldPrefab, worldsHolder).GetComponent<World>();
            world.gameObject.SetActive(true);

            world.Init();
            world.transform.position = new Vector3(0, offSet, 0);

            offSet += world.SizeReferenceTilemap.cellBounds.size.y;

            SetStagesStates(world, stageNumber);

            stageNumber += world.StageButtons.Count;

            world.UpdateClouds();

            worlds.Add(world);
        }

        SetAreWorldsUnlocked();
        SetCameraBounds();
    }

    private void SetStagesStates(World world, int stageNumber)
    {
        foreach (StageButton stageButton in world.StageButtons)
        {
            stageButton.OnStageButtonSelected += StageButtonSelected;

            int _stageNumber = stageNumber;

            if (stageButton.UnlockInSpecificNumberOfStagesCompleted == true)
            {
                _stageNumber = stageButton.NumberOfStagesToUnlock;
            }

            if (PlayerProgress.IsStageBeat(_stageNumber))
            {
                stageButton.Init(StageButton.StageState.Completed);
            }
            else if (PlayerProgress.CanPlayStage(_stageNumber))
            {
                stageButton.Init(StageButton.StageState.Incompleted);
            }
            else
            {
                stageButton.Init(StageButton.StageState.Inactive);
            }

            stageNumber++;
        }
    }
    private void SetAreWorldsUnlocked()
    {
        foreach (World world in worlds)
        {
            world.SetIsWorldUnlocked();
        }
    }
    private void SetCameraBounds()
    {
        float sizeY = 0;
        float positionY = 0;

        foreach (World world in worlds)
        {
            sizeY += world.SizeReferenceTilemap.cellBounds.size.y;
        }

        positionY = sizeY / 2;
        positionY += (worlds[0].SizeReferenceTilemap.cellBounds.yMin);

        cameraBounds.size = new Vector2(cameraBounds.size.x, sizeY - 2);
        cameraBounds.transform.position = new Vector3(0, positionY, 0);

        Physics2D.SyncTransforms();

        cameraController.SetCameraBounds(cameraBounds);
    }

    private void StageButtonSelected(StageButton stageButton)
    {
        if (stageButton.State != StageButton.StageState.Inactive)
        {
            if (CheckIfStageCanBePlayed(stageButton.StageInfo) == true)
            {
                SetCurrentStage(stageButton);
                CanvasManager.Instance.OpenMenu(Menu.StageInfo, new MenuSetupOptions(stageButton.StageInfo));
            }
            else
            {
                OnNeedCollectibleToPlayStage?.Invoke(stageButton.StageInfo);
            }
        }
        else
        {
            //TODO: add a feedback to show that the stage is locked
            Debug.Log("This stage wasn't unlocked yet!");
        }
    }

    private void SetCameraToFollowIncompleteStage()
    {
        Transform targetToFollow = null;

        foreach (World world in worlds)
        {
            foreach (StageButton stageButton in world.StageButtons)
            {
                targetToFollow = stageButton.transform;

                if (stageButton.State == StageButton.StageState.Incompleted)
                {
                    cameraController.TargetToFollow = targetToFollow;
                    return;
                }
            }
        }

        cameraController.TargetToFollow = targetToFollow;
    }
    public bool CheckIfCameraIsInUnlockedWorld()
    {
        float cameraPosY = cameraController.transform.position.y;

        foreach (World world in worlds)
        {
            float worldPosToCompare = world.transform.position.y - (world.SizeReferenceTilemap.cellBounds.size.y / 4);

            if (cameraPosY < worldPosToCompare)
            {
                return true;
            }

            if (world.IsWorldUnlocked == false)
            {
                return false;
            }
        }

        return true;
    }

    private void SetCurrentStageToLastCompletedStage()
    {
        SetCurrentStage(GetLastCompletedStage());
    }

    private StageButton GetLastCompletedStage()
    {
        StageButton stageToTeleportTo = null;

        foreach (World world in worlds)
        {
            foreach (StageButton stageButton in world.StageButtons)
            {
                if (stageToTeleportTo == null)
                {
                    stageToTeleportTo = stageButton;
                }

                if (stageButton.State != StageButton.StageState.Completed)
                {
                    return stageToTeleportTo;
                }

                stageToTeleportTo = stageButton;
            }
        }

        return stageToTeleportTo;
    }

    public void SetCurrentStage(StageButton stage)
    {
        player.transform.position = stage.transform.position;

        OnStageSelected?.Invoke(stage.StageInfo);
    }

    private bool CheckIfStageCanBePlayed(StageInfoSO stageInfo)
    {
        if (stageInfo.UnlockWith9Collectibles == false)
        {
            return IsCollectibleRequirimentFulfilled(stageInfo);
        }
        else
        {
            return IsCollectibleRequirementToUnlockLastStageFullfilled();
        }
    }

    private bool IsCollectibleRequirimentFulfilled(StageInfoSO stageInfo)
    {
        if (stageInfo.CollectibleNeededToPlay == CollectibleType.None)
        {
            return true;
        }

        return CollectibleManager.Instance.IsCollectibleUnlocked(stageInfo.CollectibleNeededToPlay);
    }

    private bool IsCollectibleRequirementToUnlockLastStageFullfilled()
    {
        foreach (CollectibleType collectible in collectiblesToUnlockLastStage)
        {
            if (CollectibleManager.Instance.IsCollectibleUnlocked(collectible) == false)
            {
                return false;
            }
        }

        return true;
    }
}