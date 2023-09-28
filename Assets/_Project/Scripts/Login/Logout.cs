using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    public class Logout : MonoBehaviour
    {
        public void ProcessLogout()
        {
            LoginManager.Instance.SignOut();
        }
    }
}