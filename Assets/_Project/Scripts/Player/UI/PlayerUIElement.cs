using UnityEngine;

namespace DreamQuiz.Player
{
    public abstract class PlayerUIElement : MonoBehaviour
    {
        protected PlayerStageInstance playerStageInstance;
        protected bool initialized = false;

        public void SetupPlayerStageInstance(PlayerStageInstance playerStageInstance)
        {
            this.playerStageInstance = playerStageInstance;

            InitializeUI();

            initialized = true;
        }

        protected abstract void InitializeUI();
    }
}