using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IUpgradable _upgradable = null;

    private IInteract _interactable = null;

    private CurrencyManager _currencyManager;

    [SerializeField] private HouseManager HouseManager;
    private bool InteractionButton => Input.GetKeyDown(KeyCode.Space);

    private bool _canInteract = false;

    private bool _canUpgrade = false;

    private float _movementSpeed = 5f;

    [SerializeField] private float _playerBaseSpeed;

    private Vector2 _direction;

    private Rigidbody2D _rb;

    public Animator Animator;

    private void Start()
    {
        _currencyManager = GetComponent<CurrencyManager>();

        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        InputCommand();
        AnimateCharacter();
        if (InteractionButton && _canUpgrade)
        {
            if ((_currencyManager.PlayerGold > 0 || _currencyManager.PlayerWood > 0) && HouseManager.CurrentHouseLevel < 3)
            {
                _upgradable.HouseGoldUpdate(_currencyManager.PlayerGold, _currencyManager.PlayerWood);
                _currencyManager.UpdateLocalGold(-_currencyManager.PlayerGold);
                _currencyManager.UpdateWood(-_currencyManager.PlayerWood);
            }
        }
        if (InteractionButton && _canInteract)
        {
            _interactable.Interact();
        }
    }

    private void InputCommand()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        _movementSpeed = Mathf.Clamp(_direction.magnitude, 0.0f, 1.0f);
        _direction.Normalize();
    }

    private void Move()
    {
        _rb.velocity = new Vector2(_direction.x * _movementSpeed * _playerBaseSpeed, _direction.y * _movementSpeed * _playerBaseSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((_currencyManager.PlayerGold > 0 || _currencyManager.PlayerWood > 0) && other.gameObject.tag == "Upgradable")
        {
            _canUpgrade = true;
            _upgradable = other.GetComponent<IUpgradable>();
            Debug.Log("Inside house interactable");
        }
        if (other.gameObject.CompareTag("Interactable"))
        {
            _canInteract = true;
            _interactable = other.GetComponent<IInteract>();
            Debug.Log("Found Interactable");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _canInteract = false;
        _upgradable = null;
        if (other.gameObject.CompareTag("Interactable"))
        {
            _interactable = null;
            _canInteract = false;
        }
        if (other.gameObject.CompareTag("Upgradable"))
        {
            _upgradable = null;
            _canUpgrade = false;
        }
    }

    private void AnimateCharacter()
    {
        Animator.SetFloat("Horizontal", _direction.x);
        Animator.SetFloat("Vertical", _direction.y);
        Animator.SetFloat("Speed", _movementSpeed);
    }
}