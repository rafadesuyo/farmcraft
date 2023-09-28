using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Misc_Bird : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private ParticleSystem notes;
    [SerializeField] private ParticleSystem feathers;

    public void PlayNotesPartycles()
    {
        notes.Play();
    }

    public void PlayFeathersPartycles()
    {
        feathers.Play();
    }
}
