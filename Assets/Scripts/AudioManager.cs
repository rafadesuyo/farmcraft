using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip CollectItem;
    public AudioClip Error;
    public AudioClip UpgradeHouse;
    public AudioClip WoodHit;
    public AudioClip WoodFall;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}