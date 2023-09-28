using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private AudioManager _audioManager;

    [SerializeField] private CollectableSpawner _coinSpawner;

    [SerializeField] private CollectableSpawner _chestSpawner;

    [SerializeField] private UIDataLoader _uiDataLoader;

    private readonly IPlayerService _playerService = new PlayerService();

    private Chest _chestScript;

    public event EventHandler OnGoldUpdate;

    public static event Action<string> OnError;

    [HideInInspector]
    public int PlayerGold;

    [HideInInspector]
    public int PlayerGoldLimit = 10;

    [HideInInspector]
    public int PlayerWood;

    [HideInInspector]
    public int PlayerWoodLimit = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Coin":
                if (other.TryGetComponent(out Collectable collectable))
                {
                    if (PlayerGold < PlayerGoldLimit)
                    {
                        OnError?.Invoke("+1 Gold");
                        _audioManager.PlaySound(_audioManager.CollectItem);
                        UpdateLocalGold(1);
                        _coinSpawner.DespawnObject(collectable.gameObject);
                    }
                    else
                    {
                        OnError?.Invoke("Inventory Full.");
                        _audioManager.PlaySound(_audioManager.Error);
                    }
                }
                break;

            case "ChestCoin":
                if (PlayerGold < PlayerGoldLimit)
                {
                    _chestScript = other.gameObject.GetComponentInParent<Chest>();

                    OnError?.Invoke("+1 Gold");

                    UpdateLocalGold(1);

                    _audioManager.PlaySound(_audioManager.CollectItem);
                    other.gameObject.SetActive(false);
                    _chestScript.CheckRespawn();

                    if (_chestScript.CanRespawn)
                    {
                        _chestSpawner.DespawnObject(_chestScript.gameObject);
                    }
                }
                else
                {
                    OnError?.Invoke("Inventory Full.");
                    _audioManager.PlaySound(_audioManager.Error);
                }
                break;

            case "Wood":
                if (PlayerWood < PlayerWoodLimit)
                {
                    UpdateWood(1);
                    OnError?.Invoke("+1 Wood");
                    _audioManager.PlaySound(_audioManager.CollectItem);
                    other.gameObject.SetActive(false);
                }
                else
                {
                    OnError?.Invoke("Inventory Full.");
                    _audioManager.PlaySound(_audioManager.Error);
                }
                break;

            default:
                break;
        }
    }

    public void UpdateWood(int amount)
    {
        PlayerWood += amount;
        OnGoldUpdate?.Invoke(this, EventArgs.Empty);
    }

    //Local gold is only called whenever you dont need to save the API, or in this case, upgrading the house.
    public void UpdateLocalGold(int amount)
    {
        PlayerGold += amount;
        OnGoldUpdate?.Invoke(this, EventArgs.Empty);
    }

    public void GetAPIGold()
    {
        ICommandAsync<GetPlayerDataResponse> getPlayerDataCommand = new GetPlayerPlayerDataCommand(_uiDataLoader.PlayerName, _playerService);
        getPlayerDataCommand.Execute();
    }

    public void LoadData(GameData gameData)
    {
        PlayerWood = gameData.PlayerWood;
        PlayerGold = gameData.PlayerGold;
    }

    public void SaveData(GameData gameData)
    {
        gameData.PlayerWood = PlayerWood;
        gameData.PlayerGold = PlayerGold;
    }
}