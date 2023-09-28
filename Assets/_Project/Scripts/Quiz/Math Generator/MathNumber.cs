using UnityEngine;
using Random = UnityEngine.Random;
using System;

[Serializable]
public class MathNumber
{
    [SerializeField] private int[] digitsCountPossitiblities = null;

    public int GetNumber()
    {
        int digitCount = digitsCountPossitiblities[Random.Range(0, digitsCountPossitiblities.Length)];
        // Convert to float in order to force that the max is inclusive. Then, convert back to int.
        return (int)Random.Range(GetMinPossibleNumberOfDigitCount(digitCount), (float)GetMaxPossibleNumberOfDigitCount(digitCount));
    }

    private int GetMaxPossibleNumberOfDigitCount(int digitCount)
    {
        int maxNumber = (int)Mathf.Pow(10, digitCount) - 1;
        return maxNumber;
    }

    private int GetMinPossibleNumberOfDigitCount(int digitCount)
    {
        int maxNumber = (int)Mathf.Pow(10, digitCount - 1);
        return maxNumber;
    }
}
