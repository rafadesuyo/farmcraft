using UnityEngine;
using UnityEngine.Pool;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _collectablePrefab;

    [SerializeField] private int _defaultSpawnAmount = 6;

    [SerializeField] private float _spawnTimer = 3f;

    private ObjectPool<GameObject> _pool;

    public Transform[] SpawnLocation;

    private void Start()
    {
        _pool = new ObjectPool<GameObject>(CreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, true, _defaultSpawnAmount, _defaultSpawnAmount * 2);

        for (int i = 0; i < _defaultSpawnAmount; i++)
        {
            SpawnObject();
        }
    }

    public GameObject CreateObject()
    {
        return Instantiate(_collectablePrefab);
    }

    public void OnGetObject(GameObject prefabObject)
    {
        prefabObject.gameObject.SetActive(true);
    }

    public void OnReleaseObject(GameObject prefabObject)
    {
        prefabObject.gameObject.SetActive(false);
    }

    public void OnDestroyObject(GameObject prefabObject)
    {
        Destroy(prefabObject.gameObject);
    }

    public void SpawnObject()
    {
        GameObject newObject = _pool.Get();
        Transform randomPosition = SpawnLocation[Random.Range(0, SpawnLocation.Length)];
        Vector3 newPosition = randomPosition.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
        newObject.transform.position = newPosition;
    }

    public void DespawnObject(GameObject prefabObject)
    {
        _pool.Release(prefabObject);
        Invoke(nameof(SpawnObject), _spawnTimer);
    }
}