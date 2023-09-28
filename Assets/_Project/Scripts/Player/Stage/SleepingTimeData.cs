using System;
using System.Collections.Generic;
using UnityEngine;
namespace DreamQuiz.Player
{
    public class SleepingTimeData
    {
        public enum SleepingTimeModifier
        {
            Poison,
            Armor
        }

        private int currentValue;
        private int maxValue;
        private Dictionary<SleepingTimeModifier, float> modifierDict;
        private Dictionary<SleepingTimeModifier, float> nerfModifierDict;
        private float calculatedModifier;
        private float overrideModifier;
        private bool modifierIsOverrided;

        public event Action<int> OnSleepingTimeChanged;

        public int CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        public int MaxValue
        {
            get
            {
                return maxValue;
            }
        }

        public SleepingTimeData(int initialValue, int maxValue)
        {
            currentValue = initialValue;
            this.maxValue = maxValue;
            modifierDict = new Dictionary<SleepingTimeModifier, float>();
            nerfModifierDict = new Dictionary<SleepingTimeModifier, float>();
            calculatedModifier = 1;
        }

        private void UpdateModifier()
        {
            float modifier = 1;

            if (modifierIsOverrided)
            {
                modifier = overrideModifier;
            }
            else if (modifierDict.Count > 0)
            {
                modifier = CalculateAllModifiers();
            }

            calculatedModifier = modifier;
        }

        private float CalculateAllModifiers()
        {
            float modifier = 0;

            foreach (var keyValuePair in modifierDict)
            {
                float currentModifier = keyValuePair.Value;

                if (nerfModifierDict.TryGetValue(keyValuePair.Key, out float currentNerf))
                {
                    currentModifier -= currentNerf;
                }

                modifier += currentModifier;
            }

            return modifier / modifierDict.Count; 
        }

        private int GetModifiedValue(int value)
        {
            return Mathf.RoundToInt(value * calculatedModifier);
        }

        public bool HasModifier(SleepingTimeModifier sleepingTimeModifier)
        {
            return modifierDict.ContainsKey(sleepingTimeModifier);
        }

        public void AddModifierOverride(float value)
        {
            overrideModifier = value;
            modifierIsOverrided = true;

            UpdateModifier();
        }

        public void RemoveModifierOverride()
        {
            modifierIsOverrided = false;

            UpdateModifier();
        }

        public void AddModifier(SleepingTimeModifier sleepingTimeModifier, float value)
        {
            if (modifierDict.ContainsKey(sleepingTimeModifier))
            {
                modifierDict[sleepingTimeModifier] = value;
            }
            else
            {
                modifierDict.Add(sleepingTimeModifier, value);
            }

            UpdateModifier();
        }

        public void RemoveModifier(SleepingTimeModifier sleepingTimeModifier)
        {
            if (modifierDict.ContainsKey(sleepingTimeModifier))
            {
                modifierDict.Remove(sleepingTimeModifier);
                UpdateModifier();
            }
        }

        public void AddModifierNerf(SleepingTimeModifier sleepingTimeModifier, float value)
        {
            if (nerfModifierDict.ContainsKey(sleepingTimeModifier))
            {
                nerfModifierDict[sleepingTimeModifier] += value;
            }
            else
            {
                nerfModifierDict.Add(sleepingTimeModifier, value);
            }

            UpdateModifier();
        }

        public void RemoveModifierNerf(SleepingTimeModifier sleepingTimeModifier, float value)
        {
            if (nerfModifierDict.ContainsKey(sleepingTimeModifier))
            {
                nerfModifierDict[sleepingTimeModifier] -= value;
                UpdateModifier();
            }
        }

        public bool HasSleepingTimeToMove(int sleepingTimeNeeded)
        {
            int modifiedValue = GetModifiedValue(sleepingTimeNeeded);
            return CurrentValue >= modifiedValue;
        }

        public void SetValue(int value)
        {
            if (currentValue == value)
            {
                return;
            }

            currentValue = value;
            OnSleepingTimeChanged?.Invoke(CurrentValue);
        }

        public void Use(int value)
        {
            int modifiedValue = GetModifiedValue(value);
            SetValue(currentValue - modifiedValue);
        }

        public void Add(int value)
        {
            SetValue(currentValue + value);
        }
    }
}