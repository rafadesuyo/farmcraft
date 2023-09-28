using UnityEngine;

namespace DreamQuiz
{
    public class StageSoundPlayer : MonoBehaviour
    {
        void Start()
        {
            StageManager.Instance.OnStageInitialized += Instance_OnStageInitialized;
        }

        private void OnDestroy()
        {
            StageManager.Instance.OnStageInitialized -= Instance_OnStageInitialized;
        }

        private void Instance_OnStageInitialized()
        {
            PlayBackgroundSound();
        }

        void PlayBackgroundSound()
        {
            StageInfoSO stageInfo = StageManager.Instance.StageInfo;
            if (stageInfo == null)
            {
                return;
            }
            SoundManager.Instance.PlayMusic(stageInfo.BackgroundMusic.audioClip);
        }
    }
}