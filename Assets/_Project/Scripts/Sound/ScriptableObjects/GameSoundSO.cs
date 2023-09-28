using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Gameplay/Sound")]
public class GameSoundSO : ScriptableObject
{
    public AudioClip audioClip = null;
    public SoundManager.AudioType audioType;
}
