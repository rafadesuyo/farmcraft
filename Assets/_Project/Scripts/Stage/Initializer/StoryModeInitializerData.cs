using DreamQuiz.Player;
using System.Collections.Generic;

namespace DreamQuiz
{
    public class StoryModeInitializerData
    {
        public StageInfoSO StageInfoSO;
        public List<PlayerData> PlayerDataList;

        public StoryModeInitializerData(StageInfoSO stageInfoSO, List<PlayerData> playerDataList)
        {
            StageInfoSO = stageInfoSO;
            PlayerDataList = playerDataList;
        }
    }
}