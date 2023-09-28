using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum AudioType
    {
        Music,
        SFX
    }

    public string AudioName;
    public AudioClip Clip;
    public AudioType Type;
    [Range(0f, 1f)]
    public float Volume;
    [Range(-1f, 1f)]
    public float Pitch;
    public bool Loop;

    [HideInInspector]public AudioSource Source;
}

