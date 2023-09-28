using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DreamQuiz.Player
{
    public class PlayerUI : MonoBehaviour
    {
        public CanvasGroup canvasGroup;

        [SerializeField] PlayerStageInstance playerStageInstance;
        List<PlayerUIElement> playerUiElementList;

        private void OnEnable()
        {
            playerStageInstance.OnInitialized += PlayerStageInstanceOnInitialized;
            playerStageInstance.OnActivate += PlayerStageInstanceOnActivate;
            playerStageInstance.OnDeactivate += PlayerStageInstanceOnInactivate;
        }

        private void OnDisable()
        {
            playerStageInstance.OnInitialized -= PlayerStageInstanceOnInitialized;
            playerStageInstance.OnActivate -= PlayerStageInstanceOnActivate;
            playerStageInstance.OnDeactivate -= PlayerStageInstanceOnInactivate;
        }

        private void PlayerStageInstanceOnInitialized()
        {
            playerUiElementList = GetComponentsInChildren<PlayerUIElement>().ToList();
            foreach (var playerUiElement in playerUiElementList)
            {
                playerUiElement.SetupPlayerStageInstance(playerStageInstance);
            }
        }

        private void PlayerStageInstanceOnActivate()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        private void PlayerStageInstanceOnInactivate()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}