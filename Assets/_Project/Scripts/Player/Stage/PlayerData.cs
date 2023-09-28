using System.Collections.Generic;

namespace DreamQuiz.Player
{
    public class PlayerData
    {
        public int Id;
        public PlayerPawn PlayerPawnPrefab;
        public int MaxSleepingTime;
        public List<Collectible> Team;

        public List<CollectibleAbility> GetTeamAbilities()
        {
            List<CollectibleAbility> collectibleAbilities = new List<CollectibleAbility>();

            foreach (var collectible in Team)
            {
                collectibleAbilities.AddRange(collectible.CollectibleAbilities);
            }

            return collectibleAbilities;
        }
    }
}
