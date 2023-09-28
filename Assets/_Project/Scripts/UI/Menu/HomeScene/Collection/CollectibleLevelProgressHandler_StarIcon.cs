using UnityEngine;
using UnityEngine.UI;

public class CollectibleLevelProgressHandler_StarIcon : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image star_Background;
    [SerializeField] private Image star_Filled;

    //Getters
    public Image Star_Background => star_Background;
    public Image Star_Filled => star_Filled;
}
