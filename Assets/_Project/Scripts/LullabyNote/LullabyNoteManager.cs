using System;
using UnityEngine;

public class LullabyNoteManager : LocalSingleton<LullabyNoteManager>
{
    public readonly int MaxLullabyNoteCount = 50;
    private int currentLullabyNoteCount = 0;

    public float TimeToNextLullabyNote
    {
        get;
        private set;
    }

    public bool CanEarnFreeLullabyNote
    {
        get
        {
            return CurrentLullabyNoteCount < MaxLullabyNoteCount;
        }
    }

    public bool HasLullabyNote
    {
        get
        {
            return CurrentLullabyNoteCount > 0;
        }
    }

    public int CurrentLullabyNoteCount
    {
        get
        {
            return currentLullabyNoteCount;
        }
        private set
        {
            currentLullabyNoteCount = value;
            EventsManager.Publish(EventsManager.onLullabyNoteChange);
        }
    }

    public void AddLullabyNotes(int lullabyNoteCount, bool ignoreLullabyNoteLimit = false)
    {
        if (ignoreLullabyNoteLimit)
        {
            CurrentLullabyNoteCount += lullabyNoteCount;
        }
        else if (CurrentLullabyNoteCount + lullabyNoteCount <= MaxLullabyNoteCount)
        {
            CurrentLullabyNoteCount += lullabyNoteCount;
        }
    }

    public void UseLullabyNote()
    {
        CurrentLullabyNoteCount--;
    }

    public void SetupLullabyNotes()
    {
        if (!PlayerProgress.SaveState.hasSaveData)
        {
            CurrentLullabyNoteCount = MaxLullabyNoteCount;
        }
        else
        {
            CurrentLullabyNoteCount = PlayerProgress.SaveState.playerInfo.currentLullabyNote;
        }
    }

    private void Update()
    {
        if (!CanEarnFreeLullabyNote)
        {
            return;
        }

        TimeToNextLullabyNote -= Time.deltaTime;

        if (TimeToNextLullabyNote <= 0)
        {
            OnGetNewLullabyNote();
        }
    }

    private void OnGetNewLullabyNote()
    {
        AddLullabyNotes(1);
    }
}