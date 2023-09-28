using UnityEngine;

public class Chest : MonoBehaviour, IInteract
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _coin1;
    [SerializeField] private GameObject _coin2;
    [SerializeField] private GameObject _coin3;

    public bool CanRespawn;

    public void Interact()
    {
        if (!_animator.GetBool("isOpen"))
        {
            _animator.SetBool("isOpen", true);
            _coin1.SetActive(true);
            _coin2.SetActive(true);
            _coin3.SetActive(true);
            CanRespawn = false;
        }
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void CheckRespawn()
    {
        if (_coin1.activeInHierarchy == false && _coin2.activeInHierarchy == false && _coin3.activeInHierarchy == false && _animator.GetBool("isOpen"))
        {
            CanRespawn = true;
            _animator.SetBool("isOpen", false);
            _coin1.SetActive(false);
            _coin2.SetActive(false);
            _coin3.SetActive(false);
        }
    }
}