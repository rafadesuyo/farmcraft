using System;
using UnityEngine;

[Serializable]
public class StageStatusDataPair
{
    [SerializeField] private StageStatus type;
    [SerializeField] private Sprite icon;
    [SerializeField] private string title;
    [SerializeField][TextArea] private string description;

    public StageStatus Type => type;
    public Sprite Icon => icon;
    public string Title => title;
    public string Description => description;
}