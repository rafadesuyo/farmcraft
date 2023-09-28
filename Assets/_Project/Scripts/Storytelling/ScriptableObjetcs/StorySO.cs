using UnityEngine;

[CreateAssetMenu(fileName = "New Story", menuName = "Gameplay/Story/Story")]
public class StorySO : ScriptableObject
{
    public Sprite speakgerSprite = null;
    public StoryFragmentSO[] storyFragments = null;
}
