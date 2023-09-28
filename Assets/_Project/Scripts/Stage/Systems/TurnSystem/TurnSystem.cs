using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz
{
    public class TurnSystem : BaseStageSystem
    {
        public static event Action<TurnSystem> OnTurnSystemInitialize;

        private int currentTurnAgentIndex;
        private bool hasBegun;
        public List<TurnAgent> AllTurnAgentList { get; private set; }
        public List<TurnAgent> OrderedTurnAgentList { get; private set; }
        public TurnAgent CurrentTurnAgent { get; private set; }
        public int Round { get; private set; }

        public override void Initialize()
        {
            AllTurnAgentList = new List<TurnAgent>();
            hasBegun = false;
            Round = 0;
            IsReady = true;

            OnTurnSystemInitialize?.Invoke(this);
        }

        public void RegisterTurnAgent(TurnAgent turnAgent)
        {
            AllTurnAgentList.Add(turnAgent);

            if (hasBegun == true && CurrentTurnAgent == null)
            {
                CurrentTurnAgent = OrderListAndGetCurrentTurnAgent();
                CurrentTurnAgent.EnterTurn();
            }

            turnAgent.DeactivateAgent();
        }

        public void UnregisterTurnAgent(TurnAgent turnAgent)
        {
            AllTurnAgentList.Remove(turnAgent);
        }

        public void BeginTurn()
        {
            if (CurrentTurnAgent == null)
            {
                CurrentTurnAgent = OrderListAndGetCurrentTurnAgent();
                CurrentTurnAgent?.EnterTurn();
            }

            hasBegun = true;
        }

        public void CycleTurn()
        {
            CurrentTurnAgent?.LeaveTurn();

            currentTurnAgentIndex++;

            if (currentTurnAgentIndex >= OrderedTurnAgentList.Count)
            {
                OrderList();
                currentTurnAgentIndex = 0;
                Round++;
            }

            CurrentTurnAgent = OrderedTurnAgentList[currentTurnAgentIndex];

            CurrentTurnAgent?.EnterTurn();
        }

        private TurnAgent OrderListAndGetCurrentTurnAgent()
        {
            if (AllTurnAgentList.Count == 0)
            {
                return null;
            }

            OrderList();
            return OrderedTurnAgentList[currentTurnAgentIndex];
        }

        private void OrderList()
        {
            OrderedTurnAgentList = AllTurnAgentList.OrderBy(agent => agent.GetTurnPriority()).ToList();
        }

        protected override void RegisterSystem()
        {
            StageSystemLocator.RegisterSystem(this);
        }

        protected override void UnregisterSystem()
        {
            StageSystemLocator.UnregisterSystem<TurnSystem>();
        }
    }
}