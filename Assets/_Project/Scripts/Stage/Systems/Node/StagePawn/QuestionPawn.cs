using DreamQuiz.Player;
using System;
using UnityEngine;

namespace DreamQuiz
{
    public class QuestionPawn : Pawn
    {
        [Header("Quiz")]
        [SerializeField] protected int questionCount;
        [SerializeField] protected QuizCategory quizCategory;
        [SerializeField] protected QuizDifficulty.Level quizDifficulty;

        protected QuizSystem quizSystem;
        protected PlayerPawn pawnInQuiz;
        protected bool isSolved = false;
        protected bool isCanceled = false;

        public int QuestionCount
        {
            get
            {
                return questionCount;
            }
        }

        public QuizCategory QuizCategory
        {
            get
            {
                return quizCategory;
            }
        }

        public event Action OnSolved;

        private void Start()
        {
            isSolved = false;
            isCanceled = false;
        }

        public override void SetPawnToNode(NodeBase nodeBase)
        {
            base.SetPawnToNode(nodeBase);
            ChangeNodeBaseModel();
            nodeBase.OnPawnAdded += NodeBase_OnPawnAdded;
        }

        private void NodeBase_OnPawnAdded(Pawn pawn)
        {
            if (isSolved)
            {
                return;
            }

            if (pawn is PlayerPawn)
            {
                if (questionCount < 1)
                {
                    Solve();
                    return;
                }

                pawnInQuiz = pawn as PlayerPawn;

                if (quizSystem == null)
                {
                    quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
                }

                quizSystem.OnQuizStateChange += QuizSystem_OnQuizStateChange;
                quizSystem.StartQuiz(questionCount, quizCategory, quizDifficulty);
            }
        }

        private void QuizSystem_OnQuizStateChange(QuizState quizState)
        {
            OnQuizStateChanged(quizState);
        }

        protected virtual void OnQuizStateChanged(QuizState quizState)
        {
            if (quizState == QuizState.Passed)
            {
                Solve();
            }
            else if (quizState == QuizState.Canceled)
            {
                isCanceled = true;
            }

            if (quizState == QuizState.None)
            {
                if (isCanceled)
                {
                    pawnInQuiz.ReturnToPreviousNode();
                    isCanceled = false;
                }
                QuizExit();
            }
        }

        protected void Solve()
        {
            isSolved = true;
            PlaySolvedAnimation();
            OnSolved?.Invoke();
        }

        protected virtual void PlaySolvedAnimation()
        {
            CurrentNode.NodeBaseModel.PlayAnimation(NodeBaseModel.NodeAnimation.Win, () =>
            {
                CurrentNode.NodeBaseModel.PlayAnimation(NodeBaseModel.NodeAnimation.Idle);
                CurrentNode.NodeBaseModel.UpdateSkin(NodeBaseModel.goldenSkinName);
            });
        }

        protected void QuizExit()
        {
            quizSystem.OnQuizStateChange -= QuizSystem_OnQuizStateChange;
            pawnInQuiz = null;
        }

        public override string GetDescription()
        {
            return $"Category: {quizCategory}\nQuestions remaining: {questionCount}";
        }

        public void ChangeNodeBaseModel()
        {
            NodeBaseModelType nodeBaseModelType;

            switch (quizCategory)
            {
                case QuizCategory.Random:
                    nodeBaseModelType = NodeBaseModelType.RandomQuizCategory;
                    break;
                case QuizCategory.GeneralKnowledge:
                    nodeBaseModelType = NodeBaseModelType.GeneralKnowledgeQuizCategory;
                    break;
                case QuizCategory.ArtsAndEntertainment:
                    nodeBaseModelType = NodeBaseModelType.ArtsAndEntertainmentQuizCategory;
                    break;
                case QuizCategory.Science:
                    nodeBaseModelType = NodeBaseModelType.ScienceQuizCategory;
                    break;
                case QuizCategory.Puzzles:
                    nodeBaseModelType = NodeBaseModelType.PuzzleQuizCategory;
                    break;
                case QuizCategory.HumanSciences:
                    nodeBaseModelType = NodeBaseModelType.HumanScienceQuizCategory;
                    break;
                case QuizCategory.Math:
                    nodeBaseModelType = NodeBaseModelType.MathQuizCategory;
                    break;
                case QuizCategory.Sports:
                    nodeBaseModelType = NodeBaseModelType.SportsQuizCategory;
                    break;
                default:
                    nodeBaseModelType = NodeBaseModelType.DefaultModel;
                    break;
            }

            CurrentNode.SetNodeBaseModelType(nodeBaseModelType);
        }

        public void SetQuestionOptions(int questionCount, QuizCategory quizCategory, QuizDifficulty.Level quizDifficulty)
        {
            this.questionCount = questionCount;
            this.quizCategory = quizCategory;
            this.quizDifficulty = quizDifficulty;
        }
    }
}