using UnityEngine;

//Change to LocalSingleton when merged with ScenePreloader
public class StorytellingManager : PersistentSingleton<StorytellingManager>
{
    [SerializeField] private GameObject contentContaier = null;
    [SerializeField] private StoryController storyController = null;

    //Example
    [SerializeField] private StorySO story;

    //Example
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartStory(story);
        }
    }

    public void StartStory(StorySO story)
    {
        // Pause gameplay
        contentContaier.SetActive(true);
        storyController.SetupStory(story);
    }

    public void FinishStory()
    {
        // Resume gameplay
        contentContaier.SetActive(false);
    }
}
