using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Tilemap sizeReferenceTilemap;
    [SerializeField] private ParticleSystem cloudsCenter;

    //Variables
    [Header("Variables")]
    [SerializeField] private Vector3 cloudsCenterBoxSize = new Vector3(10, 20, 0);
    [SerializeField] private int maxCloudsParticles = 120;

    [Header("Options")]
    [SerializeField] private float cloudsCenterPositionOffset = 1;

    private List<StageButton> stageButtons = new List<StageButton>();

    private bool isWorldUnlocked;

    //Getters
    public Tilemap SizeReferenceTilemap => sizeReferenceTilemap;
    public List<StageButton> StageButtons => stageButtons;
    public bool IsWorldUnlocked => isWorldUnlocked;

    public void Init()
    {
        CreateStageButtonsList();
    }

    private void CreateStageButtonsList()
    {
        foreach (StageButton stageButton in transform.GetComponentsInChildren<StageButton>(true))
        {
            stageButtons.Add(stageButton);
        }
    }

    public void SetIsWorldUnlocked()
    {
        isWorldUnlocked = false;

        foreach (StageButton stageButton in stageButtons)
        {
            if (stageButton.State != StageButton.StageState.Inactive)
            {
                isWorldUnlocked = true;

                return;
            }
        }
    }

    public void UpdateClouds()
    {
        if (AllStagesUnlocked(out StageButton lastStageUnlocked) == true)
        {
            DisableClouds();
        }
        else
        {
            if (lastStageUnlocked == null)
            {
                EnableAllClouds();
            }
            else
            {
                float offSet = lastStageUnlocked.transform.localPosition.y + (cloudsCenterBoxSize.y / 2);

                if (offSet <= 0)
                {
                    EnableAllClouds();
                }
                else if (offSet >= cloudsCenterBoxSize.y)
                {
                    DisableClouds();
                }
                else
                {
                    SetClouds(offSet);
                }
            }
        }
    }

    private bool AllStagesUnlocked(out StageButton lastStageUnlocked)
    {
        lastStageUnlocked = null;

        foreach (StageButton stageButton in stageButtons)
        {
            if (stageButton.State == StageButton.StageState.Inactive)
            {
                return false;
            }

            lastStageUnlocked = stageButton;
        }

        return true;
    }

    private void SetClouds(float offSet, bool ignoreCloudsCenterPositionOffSet = false)
    {
        cloudsCenter.gameObject.SetActive(true);

        //Size and Position
        float sizeOffSet = offSet;

        if (ignoreCloudsCenterPositionOffSet == false)
        {
            sizeOffSet += cloudsCenterPositionOffset;
        }

        float sizeY = cloudsCenterBoxSize.y - sizeOffSet;

        ParticleSystem.ShapeModule shape = cloudsCenter.shape;

        Vector3 size = new Vector3(cloudsCenterBoxSize.x, sizeY, cloudsCenterBoxSize.z);
        Vector3 position = new Vector3(0, (size.y / 2) * -1, 0);

        shape.scale = size;
        shape.position = position;

        //Max Particles
        float maxParticlesFloat = maxCloudsParticles * (sizeY / cloudsCenterBoxSize.y);
        int maxParticles = Mathf.Clamp((int)maxParticlesFloat, 0, maxCloudsParticles);

        ParticleSystem.MainModule main = cloudsCenter.main;
        main.maxParticles = maxParticles;

        cloudsCenter.Clear();
        cloudsCenter.Play();
    }

    private void EnableAllClouds()
    {
        SetClouds(0, true);
    }

    private void DisableClouds()
    {
        cloudsCenter.gameObject.SetActive(false);
    }
}
