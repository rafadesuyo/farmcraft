using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleLevelUpView : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image collectibleIcon;
    [SerializeField] private TextMeshProUGUI collectibleLevelUpTitle;
    [SerializeField] private TextMeshProUGUI collectibleLevelUpText;

    //Events
    public event Action OnClose;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithCollectible = "{collectible}".ToUpper();
    [SerializeField] private string textToReplaceWithValue = "{value}".ToUpper();

    [Space(10)]

    [SerializeField] private string titleCollectibleLeveledUp = "Level Up!".ToUpper();
    [SerializeField] private string titleCollectibleUnlocked = "Collectible Unlocked!".ToUpper();
    [SerializeField] private string textCollectibleLeveledUp = "{collectible} leveled up to {value}!".ToUpper();
    [SerializeField] private string textCollectibleUnlocked = "You unlocked {collectible}!".ToUpper();

    public void OpenView()
    {
        gameObject.SetActive(true);
    }

    public void CloseView()
    {
        ResetVariables();

        gameObject.SetActive(false);

        OnClose?.Invoke();
    }

    public void Init(Collectible collectible)
    {
        OpenView();
        UpdateVariables(collectible);
    }

    private void UpdateVariables(Collectible collectible)
    {
        string levelUpTitle;
        string levelUpText;

        collectibleIcon.sprite = collectible.Data.Icon;

        if (collectible.CurrentLevel <= 1)
        {
            levelUpTitle = titleCollectibleUnlocked;

            levelUpText = textCollectibleUnlocked.Replace(textToReplaceWithCollectible, collectible.Data.Name);
        }
        else
        {
            levelUpTitle = titleCollectibleLeveledUp;

            levelUpText = textCollectibleLeveledUp.Replace(textToReplaceWithCollectible, collectible.Data.Name);
            levelUpText = levelUpText.Replace(textToReplaceWithValue, collectible.CurrentLevel.ToString());
        }

        collectibleLevelUpTitle.text = levelUpTitle;
        collectibleLevelUpText.text = levelUpText;
    }

    private void ResetVariables()
    {
        collectibleIcon.sprite = null;
        collectibleLevelUpText.text = string.Empty;
    }
}
