using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShardsReceivedView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private Image shardIcon;
    [SerializeField] private TextMeshProUGUI shardTitle;
    [SerializeField] private TextMeshProUGUI shardValue;

    [Space(10)]

    [Header("Menu")]
    [SerializeField] private CollectibleLevelUpView collectibleLevelUpView;

    //Variables
    private CollectibleReference currentCollectibleReference;

    //Getters
    public override Menu Type => Menu.ShardsReceived;

    public void CheckIfCollectibleLeveledUp()
    {
        if(currentCollectibleReference.Collectible.CurrentLevel > currentCollectibleReference.LevelWhenReceivingShards)
        {
            OpenLevelUpScreen();
        }
        else
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        CanvasManager.Instance.ReturnMenu();
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        currentCollectibleReference = new CollectibleReference(setupOptions.collectible, setupOptions.shards);

        collectibleLevelUpView.CloseView();
        UpdateVariables();
    }

    protected override void OnClose()
    {
        currentCollectibleReference = default;
        collectibleLevelUpView.OnClose -= CloseMenu;

        ResetVariables();
    }

    private void OpenLevelUpScreen()
    {
        collectibleLevelUpView.Init(currentCollectibleReference.Collectible);
        collectibleLevelUpView.OnClose += CloseMenu;
    }

    private void UpdateVariables()
    {
        shardIcon.sprite = ProjectAssetsDatabase.Instance.GetCollectibleShardIcon(currentCollectibleReference.Collectible.Data.Type);
        shardTitle.text = $"{currentCollectibleReference.Collectible.Data.Name} Shards";
        shardValue.text = $"+{currentCollectibleReference.ShardsReceived}";
    }

    private void ResetVariables()
    {
        shardIcon.sprite = null;
        shardTitle.text = string.Empty;
        shardValue.text = string.Empty;
    }

    public struct CollectibleReference
    {
        //Variables
        private Collectible collectible;
        private int levelWhenReceivingShards;
        private int shardsReceived;

        //Getters
        public Collectible Collectible => collectible;
        public int LevelWhenReceivingShards => levelWhenReceivingShards;
        public int ShardsReceived => shardsReceived;

        public CollectibleReference(Collectible collectible, int shardsReceived)
        {
            this.collectible = collectible;
            levelWhenReceivingShards = collectible.CurrentLevel;
            this.shardsReceived = shardsReceived;
        }
    }
}
