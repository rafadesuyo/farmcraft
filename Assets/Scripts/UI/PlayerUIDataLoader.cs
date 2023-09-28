using TMPro;
using UnityEngine;

public class PlayerUIDataLoader : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject _playerCreationUI;

    [SerializeField] private GameObject _continueButton;

    [SerializeField] private TextMeshProUGUI _createCharacterButton;

    [SerializeField] private APIData _apiData;

    private bool _playerCreated;

    private void Start()
    {
        _apiData.OnPlayerDeleted.AddListener(EnableContinueButton);

        if (_playerCreated)
        {
            _continueButton.SetActive(true);
        }
        else
        {
            _continueButton.SetActive(false);
        }
    }

    private void EnableContinueButton()
    {
        if (DataPersistenceManager.Instance.GameData.PlayerCreated)
        {
            _continueButton.SetActive(true);
        }
        else
        {
            _continueButton.SetActive(false);
        }
    }

    public void CreateCharacter()
    {
        _playerCreationUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void LoadData(GameData gameData)
    {
        _playerCreated = gameData.PlayerCreated;
    }

    
    public void SaveData(GameData gameData)
    {
        //we are only using data persistence to load data.
    }
}