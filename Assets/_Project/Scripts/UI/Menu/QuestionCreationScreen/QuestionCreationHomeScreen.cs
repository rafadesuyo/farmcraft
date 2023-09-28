using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    public class QuestionCreationHomeScreen : MonoBehaviour
    {
        [SerializeField] GameObject homeScreen;

        private void Start()
        {
            ShowHomeScreen();
        }

        public void ShowHomeScreen()
        {
            homeScreen.SetActive(true);
        }

        public void HideHomeScreen()
        {
            homeScreen.SetActive(false);
        }
    }
}
