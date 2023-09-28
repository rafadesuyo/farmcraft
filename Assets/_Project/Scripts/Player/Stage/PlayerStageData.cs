using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DreamQuiz.Player
{
    public class PlayerStageData
    {
        public List<Collectible> Team { get; private set; }
        public List<AbilityId> UsedAbilities { get; private set; }
        public Dictionary<QuizDifficulty.Level, int> CorrectAnswerCountDict { get; private set; }
        public int TotalCorrectAnswers { get; private set; }
        public int CurrentCorrectAnswerStreak { get; private set; }
        public int CorrectAnswerBestStreak { get; private set; }

        public Dictionary<QuizDifficulty.Level, int> WrongAnswerCountDict { get; private set; }
        public int TotalWrongAnswers { get; private set; }

        public List<NodeBase> RemainingNodes { get; private set; }
        public NodeBase CurrentNode { get; private set; }
        public bool EndNodeReached { get; private set; }
        public int NumberOfNodeMovements { get; private set; }

        public float PoisonMultiplier { get; private set; }
        public float ArmorMultiplier { get; private set; }

        public SleepingTimeData SleepingTime { get; private set; }

        public int SheepsCollected { get; private set; }
        public int WolvesCollected { get; private set; }
        public int PowerCount { get; private set; }
        public int KeyCount { get; private set; }
        public int GoldCount { get; private set; }
        public int AntidoteCount { get; private set; }
        public int TotalStageScore { get; private set; }

        public event Action<PlayerAnswerEventArgs> OnCorrectAnswer;
        public event Action<PlayerAnswerEventArgs> OnWrongAnswer;

        public event Action<NodeBase> OnNodeMove;
        public event Action<int> OnNodeRemainingChange;
        public event Action OnReachedEndNode;

        public event Action<float> OnPoisonChanged;
        public event Action<float> OnArmorChanged;

        public event Action<int> OnSheepsCollectedChanged;
        public event Action<int> OnWolvesCollectedChanged;
        public event Action<int> OnPowerCountChanged;
        public event Action<int> OnKeyCountChanged;
        public event Action<int> OnGoldCountChanged;
        public event Action<int> OnAntidoteCountChanged;
        public event Action<int> OnScoreChanged;

        public PlayerStageData(PlayerData playerData)
        {
            Team = playerData.Team;
            SleepingTime = new SleepingTimeData(playerData.MaxSleepingTime, playerData.MaxSleepingTime);
            UsedAbilities = new List<AbilityId>();

            CorrectAnswerCountDict = new Dictionary<QuizDifficulty.Level, int>();
            TotalCorrectAnswers = 0;

            WrongAnswerCountDict = new Dictionary<QuizDifficulty.Level, int>();
            TotalWrongAnswers = 0;

            CurrentCorrectAnswerStreak = 0;
            CorrectAnswerBestStreak = 0;

            RemainingNodes = new List<NodeBase>();
            NumberOfNodeMovements = 0;
        }

        //
        // Ability
        //
        public void RegisterAbilityUse(AbilityId abilityId)
        {
            UsedAbilities.Add(abilityId);
        }

        //
        // Correct answers
        //
        public void AddCorrectAnswer(QuizDifficulty.Level difficulty)
        {
            if (!CorrectAnswerCountDict.ContainsKey(difficulty))
            {
                CorrectAnswerCountDict.Add(difficulty, 0);
            }

            CorrectAnswerCountDict[difficulty]++;
            TotalCorrectAnswers++;

            CurrentCorrectAnswerStreak++;
            if (CurrentCorrectAnswerStreak > CorrectAnswerBestStreak)
            {
                CorrectAnswerBestStreak = CurrentCorrectAnswerStreak;
            }

            int scoreToAdd = StageHelper.GetAnswerScoreMultiplier(difficulty);

            if (CurrentCorrectAnswerStreak > 1)
            {
                scoreToAdd += StageHelper.StreakScore;
            }

            AddScore(scoreToAdd);

            OnCorrectAnswer?.Invoke(
                new PlayerAnswerEventArgs(
                    difficulty,
                    TotalCorrectAnswers,
                    TotalWrongAnswers,
                    CurrentCorrectAnswerStreak
                    )
                );
        }

        //
        // Wrong answers
        //
        public void AddWrongAnswer(QuizDifficulty.Level difficulty)
        {
            if (!WrongAnswerCountDict.ContainsKey(difficulty))
            {
                WrongAnswerCountDict.Add(difficulty, 0);
            }

            WrongAnswerCountDict[difficulty]++;
            TotalWrongAnswers++;

            CurrentCorrectAnswerStreak = 0;

            OnCorrectAnswer?.Invoke(
                 new PlayerAnswerEventArgs(
                     difficulty,
                     TotalCorrectAnswers,
                     TotalWrongAnswers,
                     CurrentCorrectAnswerStreak
                     )
                 );
        }

        //
        // Node
        //
        public void RegisterNodeMovement(NodeBase node)
        {
            if (CurrentNode != null)
            {
                NumberOfNodeMovements++;
            }

            CurrentNode = node;

            if (RemainingNodes.Contains(node))
            {
                RemainingNodes.Remove(node);
                OnNodeRemainingChange?.Invoke(RemainingNodes.Count);
            }

            OnNodeMove?.Invoke(node);
        }

        public void SetEndNodeReached(bool value)
        {
            EndNodeReached = value;

            if (EndNodeReached)
            {
                OnReachedEndNode?.Invoke();
            }
        }

        public void SetRemainingNodes(List<NodeBase> remainingNodes)
        {
            RemainingNodes = remainingNodes;
        }

        //
        // Poison
        //
        private void SetPoison(float value)
        {
            if (PoisonMultiplier == value) return;

            PoisonMultiplier = Mathf.Max(0, value);

            if (PoisonMultiplier > 0 && AntidoteCount == 0)
            {
                SleepingTime.AddModifier(SleepingTimeData.SleepingTimeModifier.Poison, PoisonMultiplier);
            }
            else
            {
                if (PoisonMultiplier > 0 && AntidoteCount > 0)
                {
                    PoisonMultiplier = 0;
                    RemoveAntidoteCount(1);
                }

                SleepingTime.RemoveModifier(SleepingTimeData.SleepingTimeModifier.Poison);
            }

            OnPoisonChanged?.Invoke(PoisonMultiplier);
        }

        public void AddPoison(float value)
        {
            SetPoison(PoisonMultiplier + value);
        }

        public void RemovePoison(float value)
        {
            SetPoison(PoisonMultiplier - value);
        }

        public void RemovePoison()
        {
            SetPoison(0);
        }

        //
        // Armor
        //
        private void SetArmor(float value)
        {
            if (ArmorMultiplier == value) return;

            ArmorMultiplier = Mathf.Max(0, value);

            if (ArmorMultiplier > 0)
            {
                SleepingTime.AddModifier(SleepingTimeData.SleepingTimeModifier.Armor, ArmorMultiplier);
            }
            else
            {
                SleepingTime.RemoveModifier(SleepingTimeData.SleepingTimeModifier.Armor);
            }

            OnArmorChanged?.Invoke(ArmorMultiplier);
        }

        public void AddArmor(float value)
        {
            SetArmor(ArmorMultiplier + value);
        }

        public void RemoveArmor(float value)
        {
            SetArmor(ArmorMultiplier - value);
        }

        public void RemoveArmor()
        {
            SetArmor(0);
        }

        //
        // Sheeps collected
        //
        public void AddSheepsCollected(int value)
        {
            SetSheepsCollected(SheepsCollected + value);
        }

        public void SetSheepsCollected(int value)
        {
            if (SheepsCollected == value)
            {
                return;
            }

            SheepsCollected = value;

            OnSheepsCollectedChanged?.Invoke(value);
        }

        //
        // Wolves collected
        //
        public void SetWolvesCollected(int value)
        {
            if (WolvesCollected == value)
            {
                return;
            }
            WolvesCollected = value;

            OnWolvesCollectedChanged?.Invoke(value);
        }
        public void AddWolvesCollected(int value)
        {
            SetWolvesCollected(WolvesCollected + value);
        }

        //
        // Power count
        //
        public void SetPowerCount(int value)
        {
            if (PowerCount == value)
            {
                return;
            }

            PowerCount = Mathf.Max(0, value);

            OnPowerCountChanged?.Invoke(PowerCount);
        }

        public void AddPowerCount(int value)
        {
            SetPowerCount(PowerCount + value);
        }

        public void RemovePowerCount(int value)
        {
            SetPowerCount(PowerCount - value);
        }

        //
        // Key count
        //
        public void SetKeyCount(int value)
        {
            if (KeyCount == value)
            {
                return;
            }

            KeyCount = Mathf.Max(0, value);

            OnKeyCountChanged?.Invoke(KeyCount);
        }

        public void AddKeyCount(int value)
        {
            SetKeyCount(KeyCount = value);
        }

        public void RemoveKeyCount(int value)
        {
            SetKeyCount(KeyCount -= value);
        }

        //
        // Gold count
        //
        public void SetGoldCount(int value)
        {
            if (GoldCount == value)
            {
                return;
            }

            GoldCount = value;

            OnGoldCountChanged?.Invoke(GoldCount);
        }

        public void AddGoldCount(int value)
        {
            SetGoldCount(GoldCount + value);
        }

        //
        // Antidote count
        //
        public void SetAntidoteCount(int value)
        {
            if (AntidoteCount == value)
            {
                return;
            }

            AntidoteCount = Mathf.Max(0, value);

            if (AntidoteCount > 0 && PoisonMultiplier > 0)
            {
                AntidoteCount--;
                RemovePoison();
            }

            OnAntidoteCountChanged?.Invoke(AntidoteCount);
        }

        public void AddAntidoteCount(int value)
        {
            SetAntidoteCount(AntidoteCount + value);
        }

        public void RemoveAntidoteCount(int value)
        {
            SetAntidoteCount(AntidoteCount - value);
        }

        //
        // Score
        // Reference for scores: https://ocarinastudios.atlassian.net/wiki/spaces/DQ/pages/2031943719/GDD+Ranking
        //
        private void SetScore(int value)
        {
            if (TotalStageScore == value)
            {
                return;
            }

            TotalStageScore = Mathf.Max(0, value);

            OnScoreChanged?.Invoke(value);
        }

        private void AddScore(int value)
        {
            if (value < 0)
            {
                Debug.LogError("Negative score being added");
                return;
            }

            SetScore(TotalStageScore + value);
        }
    }
}