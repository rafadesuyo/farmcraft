using DreamQuiz;
using UnityEngine;

public abstract class TurnAgent : MonoBehaviour
{
    [SerializeField] protected int turnPriority = 0;
    [SerializeField] protected int turnActionPointsPerRound = 1;
    protected int currentTurnActionPoints = 0;
    protected TurnSystem turnSystem;

    public abstract void ActivateAgent();
    public abstract void DeactivateAgent();
    protected abstract void OnEnterTurn();
    protected abstract void OnLeaveTurn();

    private void Awake()
    {
        TurnSystem.OnTurnSystemInitialize += TurnSystem_OnTurnSystemInitialize;
    }

    private void TurnSystem_OnTurnSystemInitialize(TurnSystem turnSystem)
    {
        TurnSystem.OnTurnSystemInitialize -= TurnSystem_OnTurnSystemInitialize;
        this.turnSystem = turnSystem;
        turnSystem.RegisterTurnAgent(this);
    }

    public virtual int GetTurnPriority()
    {
        return turnPriority;
    }

    public void EnterTurn()
    {
        currentTurnActionPoints = turnActionPointsPerRound;
        OnEnterTurn();
    }

    public void LeaveTurn()
    {
        OnLeaveTurn();
    }

    public void ConsumeTurnActionPoint(int value)
    {
        currentTurnActionPoints = Mathf.Max(0, currentTurnActionPoints - value);

        if (currentTurnActionPoints == 0)
        {
            CallEndTurn();
        }
    }

    public void CallEndTurn()
    {
        turnSystem.CycleTurn();
    }
}