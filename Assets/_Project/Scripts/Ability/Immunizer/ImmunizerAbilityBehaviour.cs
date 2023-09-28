using DreamQuiz.Player;

namespace DreamQuiz
{
    public class ImmunizerAbilityBehaviour : BaseAbilityBehaviour
    {
        private float poisonReduceAmount = 0.25f;
        private int moveCountToDeactivate = 1;
        private int currentMoveCount = 0;
        private SleepingTimeData sleepingTimeData;
        private bool isActive = false;

        public override bool CanUseAbility()
        {
            return isActive == false;
        }

        public override AbilityId GetAbilityId()
        {
            return AbilityId.Immunizer;
        }

        public override void OnInitialize()
        {
            sleepingTimeData = PlayerStageData.SleepingTime;
            PlayerStageData.OnNodeMove += PlayerStageData_OnNodeMove;
        }

        private void PlayerStageData_OnNodeMove(NodeBase node)
        {
            currentMoveCount++;

            if (currentMoveCount >= moveCountToDeactivate)
            {
                Deactivate();
            }
        }

        public override void UseAbility()
        {
            currentMoveCount = 0;
            sleepingTimeData.AddModifierNerf(SleepingTimeData.SleepingTimeModifier.Poison, poisonReduceAmount);
            ConsumeUsePerStage();
        }

        private void Deactivate()
        {
            if (sleepingTimeData.HasModifier(SleepingTimeData.SleepingTimeModifier.Poison))
            {
                sleepingTimeData.RemoveModifierNerf(SleepingTimeData.SleepingTimeModifier.Poison, poisonReduceAmount);
            }
        }

        protected override void ApplyEnhancements(AbilityEnhancementSO abilityEnhancementSO)
        {
            var immunizerAbilityEnhancementSO = GetEnhancementFromBase<ImmunizerAbilityEnhancementSO>(abilityEnhancementSO);
            poisonReduceAmount = immunizerAbilityEnhancementSO.PoisonReduceAmount;
            moveCountToDeactivate = immunizerAbilityEnhancementSO.MoveCountToDeactivate;
        }
    }
}