using TMPro;
using UnityEngine;

public class ErrorHandlerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _errorText;

    [SerializeField] private CurrencyManager _currencyManager;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CurrencyManager.OnError += CurrencyManager_OnError;
    }

    private void CurrencyManager_OnError(string obj)
    {
        _errorText.text = obj;
        _animator.SetTrigger("Idle");
        _animator.SetTrigger("Event");
    }

    //This function is called on the Animation trigger ''Event''
    private void Idle()
    {
        _animator.SetTrigger("Idle");
    }
}