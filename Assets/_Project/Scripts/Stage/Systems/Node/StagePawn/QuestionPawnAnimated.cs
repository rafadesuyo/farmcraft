using System.Collections;
using UnityEngine;

namespace DreamQuiz
{
    public class QuestionPawnAnimated : QuestionPawn
    {
        //Components
        [Header("Components")]
        [SerializeField] protected StagePawnModel stagePawnModel;

        protected void WaitForDisappearingAnimation()
        {
            StartCoroutine(RemovePawnFromBoardAfterModelGetsDisabled());
        }

        protected IEnumerator RemovePawnFromBoardAfterModelGetsDisabled()
        {
            yield return new WaitUntil(() => stagePawnModel.gameObject.activeSelf == false);

            RemovePawnFromBoard();
        }
    }
}
