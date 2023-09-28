using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CreateNewPlayer : MonoBehaviour
{
    private bool _canCreate = true;

    [SerializeField] private GameObject _errorGameObject;

    [SerializeField] private TextMeshProUGUI _errorText;

    [SerializeField] private TextMeshProUGUI _warningText;

    public TextMeshProUGUI PlayerText;

    public TMP_InputField InputField;

    public APIData APIData;

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(APIData.PlayerName))
        {
            _warningText.text = "This will overwrite the data from your existing farmer: " + APIData.PlayerName + "!";
        }

        APIData.OnPlayerCreatedFailed.AddListener(DisplayError);
    }

    private void OnDisable()
    {
        APIData.OnPlayerCreatedFailed.RemoveListener(DisplayError);
    }

    private void Update()
    {
        string playerName = InputField.text.Trim();

        if (Input.GetKeyDown(KeyCode.Return) && playerName.Length >= 3 && playerName.Length <= 8 && _canCreate)
        {
            CreatePlayer(playerName);
        }
    }

    public void CreatePlayer(string playerName)
    {
        _canCreate = false;
        DataPersistenceManager.Instance.NewGame();
        DataPersistenceManager.Instance.GameData.PlayerName = playerName;
        APIData.CreatePlayer();
        Debug.Log("Created player");
        DataPersistenceManager.Instance.SaveGame();

    }

    public void OnTextChanged(string text)
    {
        text = text.Trim();
        InputField.text = text;

        switch (text.Length)
        {
            case int length when length < 3:
                _errorText.text = "Your name cannot be less than 3 characters";
                _errorGameObject.SetActive(true);
                break;

            case int length when length > 8:
                _errorText.text = "Your name cannot be more than 8 characters";
                _errorGameObject.SetActive(true);
                break;

            default:
                _errorGameObject.SetActive(false);
                break;
        }
    }

    public void DisplayError()
    {
        _errorText.text = "Error: Name already taken!";
        _errorGameObject.SetActive(true);
        _canCreate = true;
    }
}