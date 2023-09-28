using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressFeedback : MonoBehaviour
{
    [SerializeField] private GameObject loadingContainer = null;

    public void EnableProgressFeedback()
    {
        loadingContainer.SetActive(true);
    }

    public void DisableProgressFeedback()
    {
        loadingContainer.SetActive(false);
    }
}
