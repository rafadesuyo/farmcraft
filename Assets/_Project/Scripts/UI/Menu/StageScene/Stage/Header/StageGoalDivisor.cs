using UnityEngine;

public class StageGoalDivisor : MonoBehaviour
{
    private void OnDisable()
    {
        this.ReleaseItem();
    }
}
