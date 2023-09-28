using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public APIData APIData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        APIData.OnPlayerCreated.AddListener(LoadFarmScene);
    }

    private IEnumerator LoadLevel(int level)
    {
        _animator.SetTrigger("GameStart");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(level);

        _animator.SetTrigger("WipeBack");
    }

    public void LoadFarmScene()
    {
        StartCoroutine(LoadLevel(1));
    }
}