using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStats", menuName = "Gameplay/Player Stat")]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField] private int baseSleepingTime = 0;
    [SerializeField] private int baseDreamEnergy = 0;

    public int BaseSleepingTime { get => baseSleepingTime; }
    public int BaseDreamEnergy { get => baseDreamEnergy; }
}
