using UnityEngine;

[CreateAssetMenu(menuName = "Daily Missions/Container Color")]
public class DailyMissionContainerColorSO : ScriptableObject
{
    //Variables
    [SerializeField] private Color missionNameAndProgressTextColor;
    [SerializeField] private Color containerColor;
    [SerializeField] private Color containerOutlineColor;

    //Getters
    public Color MissionNameAndProgressTextColor => missionNameAndProgressTextColor;
    public Color ContainerColor => containerColor;
    public Color ContainerOutlineColor => containerOutlineColor;
}
