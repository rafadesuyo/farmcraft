using UnityEngine;
using DreamQuiz.Player;

public class PlayerTurnAgent : TurnAgent
{
    [SerializeField] private PlayerStageInstance playerStageInstance;

    private void OnEnable()
    {
        playerStageInstance.OnInitialized += PlayerStageInstance_OnInitialized;
    }

    private void OnDisable()
    {
        playerStageInstance.OnInitialized -= PlayerStageInstance_OnInitialized;
        playerStageInstance.PlayerStageData.OnCorrectAnswer -= PlayerStageData_OnCorrectAnswer;
    }

    private void PlayerStageInstance_OnInitialized()
    {
        playerStageInstance.PlayerStageData.OnCorrectAnswer += PlayerStageData_OnCorrectAnswer;
    }

    private void PlayerStageData_OnCorrectAnswer(PlayerAnswerEventArgs eventArgs)
    {
        ConsumeTurnActionPoint(1);
    }

    public override void ActivateAgent()
    {
        playerStageInstance.Activate();
    }

    public override void DeactivateAgent()
    {
        playerStageInstance.Deactivate();
    }

    protected override void OnEnterTurn()
    {
        playerStageInstance.Activate();
    }

    protected override void OnLeaveTurn()
    {
        playerStageInstance.Deactivate();
    }
}
