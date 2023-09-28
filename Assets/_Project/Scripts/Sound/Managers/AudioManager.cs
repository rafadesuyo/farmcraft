using System.Collections.Generic;
using UnityEngine;

public class AudioManager : LocalSingleton<AudioManager> 
{
    [HideInInspector,SerializeField] private AudioSource audioSource;
    public List<Sound> sounds;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        foreach (Sound sound in sounds)
        {
            sound.Source = audioSource;
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }
    }

    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.AudioName == name);

        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        sound.Source.PlayOneShot(sound.Clip);
    }
}
