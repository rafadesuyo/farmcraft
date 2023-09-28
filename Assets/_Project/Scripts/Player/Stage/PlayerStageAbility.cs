using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DreamQuiz.Player
{
    public class PlayerStageAbility
    {
        private Dictionary<AbilityId, BaseAbilityBehaviour> abilityDict = new Dictionary<AbilityId, BaseAbilityBehaviour>();
        private PlayerStageData playerStageData;

        public event Action<BaseAbilityBehaviour> OnAbilityUse;

        public PlayerStageAbility(PlayerStageData playerStageData, List<Collectible> collectibles)
        {
            this.playerStageData = playerStageData;
            abilityDict.Clear();

            var allAbilityTypes = Assembly.GetAssembly(typeof(BaseAbilityBehaviour)).GetTypes()
               .Where(t => typeof(BaseAbilityBehaviour).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var abilityType in allAbilityTypes)
            {
                var abilityInstance = Activator.CreateInstance(abilityType) as BaseAbilityBehaviour;

                var abilityId = abilityInstance.GetAbilityId();

                if (TryGetCollectibleAndAbilityByAbilityId(abilityId, collectibles, out Collectible collectible, out CollectibleAbility collectibleAbility))
                {
                    abilityInstance.Initialize(playerStageData, collectibleAbility, collectible);
                    AddAbility(abilityInstance);
                }
            }
        }

        private bool TryGetCollectibleAndAbilityByAbilityId(AbilityId abilityId, List<Collectible> collectibles, out Collectible returnCollectible, out CollectibleAbility returnCollectibleAbility)
        {
            returnCollectible = null;
            returnCollectibleAbility = null;

            foreach (var collectible in collectibles)
            {
                var collectibleAbility = collectible.CollectibleAbilities.Find(a => a.AbilityDataSO.AbilityId == abilityId);

                if (collectibleAbility != null)
                {
                    returnCollectible = collectible;
                    returnCollectibleAbility = collectibleAbility;
                    return true;
                }
            }

            return false;
        }

        private void AddAbility(BaseAbilityBehaviour ability)
        {
            var abilityId = ability.GetAbilityId();

            if (abilityDict.ContainsKey(abilityId))
            {
                Debug.LogError($"[BaseAbility] Duplicate abilityId '{abilityId}' type implementation");
                return;
            }

            abilityDict.Add(ability.GetAbilityId(), ability);
        }

        public bool UseAbility(AbilityId abilityId)
        {
            if (abilityDict.TryGetValue(abilityId, out BaseAbilityBehaviour ability) == false)
            {
                return false;
            }
            bool hasUsedAbility = ability.TryUseAbility(AbilityUsedCallback);

            return hasUsedAbility;
        }

        public void AbilityUsedCallback(BaseAbilityBehaviour baseAbilityBehaviour)
        {
            playerStageData.RegisterAbilityUse(baseAbilityBehaviour.GetAbilityId());
            OnAbilityUse?.Invoke(baseAbilityBehaviour);
        }

        public BaseAbilityBehaviour GetAbility(AbilityId abilityId)
        {
            if (abilityDict.TryGetValue(abilityId, out BaseAbilityBehaviour ability) == false)
            {
                return null;
            }

            return ability;
        }

        public List<BaseAbilityBehaviour> GetAllAbilities()
        {
            return abilityDict.Values.ToList();
        }

        public void RegisterAbilityListener(AbilityId abilityId, Action<BaseAbilityBehaviour> onUpdate)
        {
            if (abilityDict.TryGetValue(abilityId, out BaseAbilityBehaviour ability))
            {
                ability.OnAbilityUpdate += onUpdate;
            }
        }

        public void UnregisterAbilityListener(AbilityId abilityId, Action<BaseAbilityBehaviour> onUpdate)
        {
            if (abilityDict.TryGetValue(abilityId, out BaseAbilityBehaviour ability))
            {
                ability.OnAbilityUpdate -= onUpdate;
            }
        }
    }
}