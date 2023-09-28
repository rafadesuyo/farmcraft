using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrialCurrenciesView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pillowCountText;

    private void OnEnable()
    {
        PillowManager.OnPillowAmountChanged += UpdatePillowCount;
    }

    private void UpdatePillowCount(int currentPillowCount, int maxPillowCount)
    {
        pillowCountText.text = $"{currentPillowCount}/{maxPillowCount}";
    }
}
