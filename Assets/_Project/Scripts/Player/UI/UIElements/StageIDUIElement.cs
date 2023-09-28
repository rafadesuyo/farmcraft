using TMPro;
using UnityEngine;

namespace DreamQuiz.Player
{
    public class StageIDUIElement : PlayerUIElement
    {
        [SerializeField] private TextMeshProUGUI StageIDText = null;

        protected override void InitializeUI()
        {
            SetupId();
        }

        private void SetupId()
        {
            StageIDText.text = playerStageInstance.stageId.ToString();
        }
    }
}