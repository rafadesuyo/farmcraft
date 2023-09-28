using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageStatusDatabase", menuName = "Gameplay/Stage Status")]
public class StageStatusDatabase : ScriptableObject
{
    [SerializeField] private List<StageStatusDataPair> stageStatus = new List<StageStatusDataPair>();

    public StageStatusDataPair GetStageStatusPairOfType(StageStatus type)
    {
        return stageStatus.Find(status => status.Type == type);
    }
}
