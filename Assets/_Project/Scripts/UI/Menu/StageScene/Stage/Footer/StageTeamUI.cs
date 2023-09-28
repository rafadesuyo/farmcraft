using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz.Player
{
    public class StageTeamUI : PlayerUIElement
    {
        [SerializeField] private StageCollectibleItem[] collectibleItems = null;

        protected override void InitializeUI()
        {
            SetupStageCollectibles(playerStageInstance.PlayerStageData.Team);
        }

        private void SetupStageCollectibles(List<Collectible> collectibles)
        {
            var team = collectibles;

            for (int i = 0; i < collectibleItems.Length; i++)
            {
                if (i >= team.Count)
                {
                    collectibleItems[i].gameObject.SetActive(false);
                    continue;
                }

                collectibleItems[i].gameObject.SetActive(true);
                collectibleItems[i].Setup(team[i], playerStageInstance.PlayerStageAbility);
            }
        }
    }
}