using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerStageInstance playerStageInstancePrefab;

        Dictionary<int, PlayerStageInstance> playerDict = new Dictionary<int, PlayerStageInstance>();

        public static PlayerStageInstance CurrentPlayerInstance { get; private set; }

        public void SetupPlayers(List<PlayerData> playerData, StageInfoSO stageInfo)
        {
            for (int i = 0; i < playerData.Count; i++)
            {
                var playerStageInstance = Instantiate(playerStageInstancePrefab, Vector3.zero, Quaternion.identity);

                playerStageInstance.InitializePlayer(
                    this,
                    playerData[i],
                    stageInfo
                    );

                playerDict.Add(playerStageInstance.Id, playerStageInstance);
            }
        }

        public void SetCurrentPlayerInstance(PlayerStageInstance playerStageInstance)
        {
            CurrentPlayerInstance = playerStageInstance;
        }

        public List<PlayerStageInstance> GetPlayerStageInstances()
        {
            return playerDict.Values.ToList();
        }
    }
}