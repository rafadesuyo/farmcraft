using UnityEngine;

public class SoundManager : LocalSingleton<SoundManager>
{
    public enum AudioType
    {
        Music,
        SFX
    }

    // FIXME: Change to multiple sources if necessary after sound definition
    // https://ocarinastudios.atlassian.net/browse/DQG-851?atlOrigin=eyJpIjoiZTJjMWM2YmU2NGY5NDcyZmJkMzU0MjRkYzU0N2QwM2UiLCJwIjoiaiJ9
    [SerializeField] private AudioSource musicAudioListener = null;
    [SerializeField] private AudioSource SFXAudioListener = null;
    private bool isGlobalAudioMuted = false;
    private bool isMusicMuted = false;
    private float globalMusicVolume = 0f;
    private bool isSFXMuted = false;
    private float globalSFXVolume = 0f;

    public bool IsGlobalAudioMuted => isGlobalAudioMuted;

    private void ChangeMusicMuteState(bool isMute)
    {
        isMusicMuted = isMute;
        musicAudioListener.mute = isMute;
    }

    private void ChangeSFXMuteState(bool isMute)
    {
        isSFXMuted = isMute;
        SFXAudioListener.mute = isMute;
    }

    public void ToggleGlobalMuteState()
    {
        isGlobalAudioMuted = !isGlobalAudioMuted;
        AudioListener.pause = isGlobalAudioMuted;
    }

    public void ToggleMusicMuteState()
    {
        ChangeMusicMuteState(!isMusicMuted);
    }

    public void ToggleAudioMuteState()
    {
        ChangeMusicMuteState(!isSFXMuted);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioListener.clip = clip;
        musicAudioListener.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXAudioListener.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicAudioListener.Stop();
    }
}
