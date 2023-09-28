using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundPlayer : MonoBehaviour
{
    [SerializeField] private LocalSound bgSound;
    private void Start()
    {
            SoundManager.Instance.PlayMusic(bgSound.gameSound.audioClip);
    }
}
