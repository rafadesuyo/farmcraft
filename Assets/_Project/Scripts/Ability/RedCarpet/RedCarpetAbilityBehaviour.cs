namespace DreamQuiz
{
    public class RedCarpetAbilityBehaviour : BaseTargetableAbilityBehaviour<NodeBase>
    {
        private QuizSystemAbilityRestriction quizSystemAbilityRestriction;

        public override bool CanUseAbility()
        {
            return quizSystemAbilityRestriction.IsRestricted() == false;
        }

        public override AbilityId GetAbilityId()
        {
            return AbilityId.RedCarpet;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            var quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
            quizSystemAbilityRestriction = new QuizSystemAbilityRestriction(QuizSystemAbilityRestriction.QuizResctictionType.UseOnlyOutsideQuiz, quizSystem);
            quizSystemAbilityRestriction.OnRestrictionChange += QuizSystemAbilityRestriction_OnRestrictionChange;

            UpdateAbility();
        }

        private void QuizSystemAbilityRestriction_OnRestrictionChange(bool restriction)
        {
            UpdateAbility();
        }

        protected override bool OnTargetAcquired(NodeBase target)
        {
            bool validTarget = false;
            NodeBase currentNode = PlayerStageData.CurrentNode;

            if (NodeHelper.HasConnectionToNode(currentNode, target, out NodeConnection nodeConnection))
            {
                validTarget = true;
                nodeConnection.Path.SetSleepingTimeToTraverse(0);
            }

            return validTarget;
        }
    }
}