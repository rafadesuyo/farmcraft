using UnityEngine;
using UnityEngine.Events;

public class APIData : MonoBehaviour
{
    public string PlayerName;

    public UnityEvent OnPlayerDeleted = new();

    public UnityEvent OnPlayerCreated = new();

    public UnityEvent OnPlayerCreatedFailed = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (DataPersistenceManager.Instance.GameData.PlayerCreated)
        {
        }
        else
        {
            Debug.Log("No local player data found");
        }
    }

    public void CreatePlayer()
    {
        OnPlayerCreated.Invoke();
    }
 
}
