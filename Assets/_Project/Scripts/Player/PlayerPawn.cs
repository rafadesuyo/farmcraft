using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz.Player
{
    public class PlayerPawn : Pawn
    {
        [Header("Components")]
        [SerializeField] private Animator spriteAnimator;

        private PlayerStageInstance playerStageInstance;
        private NodeSystem nodeSystem;

        public PlayerStageData PlayerStageData
        {
            get
            {
                return playerStageInstance.PlayerStageData;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            playerStageInstance.OnActivate -= PlayerStageInstance_OnActivate;
            playerStageInstance.OnDeactivate -= PlayerStageInstance_OnDeactivate;
        }

        private void Start()
        {
            if (StageSystemLocator.TryGetSystem(out nodeSystem) == false)
            {
                Debug.LogError("[PlayerPawn] NodeSystem needed in scene");
                return;
            }
            NodeSystem.OnNodeSystemInitialize += NodeSystem_OnNodeSystemInitialize;
        }

        private void NodeSystem_OnNodeSystemInitialize(NodeSystem nodeSystem)
        {
            NodeSystem.OnNodeSystemInitialize -= NodeSystem_OnNodeSystemInitialize;
            this.nodeSystem = nodeSystem;
            List<NodeBase> remainingNodes = nodeSystem.StageNodes.ToList();
            PlayerStageData.SetRemainingNodes(remainingNodes);
            PlayerStageData.SetEndNodeReached(false);
            NodeBase startNode = nodeSystem.GetNextEntranceNode();
            TeleportToNode(startNode);
        }

        public void Initialize(PlayerStageInstance PlayerStageInstance)
        {
            playerStageInstance = PlayerStageInstance;
            playerStageInstance.OnActivate += PlayerStageInstance_OnActivate;
            playerStageInstance.OnDeactivate += PlayerStageInstance_OnDeactivate;
            NodeMovement.OnArrival += NodeMovement_OnArrival;
        }

        private void NodeMovement_OnArrival(NodeBase nodeBase, NodePath nodePath)
        {
            PlayerStageData.RegisterNodeMovement(nodeBase);
            PlayerStageData.SleepingTime.Use(nodePath.SleepingTimeToTraverse);
        }

        private void LateUpdate()
        {
            spriteAnimator.SetFloat("DirectionX", NodeMovement.Direction.x);
            spriteAnimator.SetFloat("DirectionY", NodeMovement.Direction.y);
        }

        public void Activate()
        {
            nodeSystem.SetCurrentPawn(this);
        }

        public void Deactivate()
        {
            nodeSystem.SetCurrentPawn(null);
        }

        private void PlayerStageInstance_OnActivate()
        {
            nodeSystem.SetCurrentPawn(this);
        }

        private void PlayerStageInstance_OnDeactivate()
        {
            nodeSystem.SetCurrentPawn(null);
        }
    }
}