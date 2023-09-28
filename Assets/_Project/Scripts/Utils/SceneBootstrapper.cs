using UnityEngine;

public class SceneBootstrapper
{
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SetupScene()
    {
        Object.Instantiate(Resources.Load("Managers/Systems"));
    }
}
