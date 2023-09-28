using UnityEngine;

public class WoodSpawn : MonoBehaviour, IInteract
{
    [SerializeField] private GameObject _wood;

    [SerializeField] private AudioManager _audioManager;

    public void Interact()
    {
        if (!_wood.activeInHierarchy)
        {
            _audioManager.PlaySound(_audioManager.WoodHit);
            RandomChance();
        }
    }

    private void RandomChance()
    {
        float chance = Random.value;

        if (chance > 0.5f)
        {
            _audioManager.PlaySound(_audioManager.WoodFall);

            _wood.SetActive(true);
        }
    }
}