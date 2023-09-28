using UnityEngine;

[CreateAssetMenu(fileName = "New Story fragment", menuName = "Gameplay/Story/Story fragment")]
public class StoryFragmentSO : ScriptableObject
{
    [TextArea(5, 20)] public string text = "";
}
