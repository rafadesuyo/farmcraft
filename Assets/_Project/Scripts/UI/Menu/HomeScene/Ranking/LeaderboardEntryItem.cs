using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardEntryItem : MonoBehaviour
{
    [SerializeField] private bool isPlayerEntry = false;
    [SerializeField] private Image backgroundImg = null;
    [SerializeField] private TextMeshProUGUI positionTxt = null;
    [SerializeField] private TextMeshProUGUI scoreTxt = null;
    [SerializeField] private TextMeshProUGUI usernameTxt = null;

    [Header("Player display")]
    [SerializeField] private Sprite playerBackgroud = null;
    [SerializeField] private Color playerUsernameColor = Color.white;
    [SerializeField] private Color playerPositionColor = Color.white;
    [SerializeField] private Color playerScoreColor = Color.white;

    [Header("Top3 display")]
    [SerializeField] private Sprite top3Background = null;
    [SerializeField] private Color top3UsernameColor = Color.white;
    [SerializeField] private Color top3PositionColor = Color.white;
    [SerializeField] private Color top3ScoreColor = Color.white;

    [Header("Default display")]
    [SerializeField] private Sprite defaultBackgroud = null;
    [SerializeField] private Color defaultUsernameColor = Color.white;
    [SerializeField] private Color defaultPositionColor = Color.white;
    [SerializeField] private Color defaultScoreColor = Color.white;

    public void Setup(string username, int position, int score, bool isLocalPlayer = false)
    {
        // https://ocarinastudios.atlassian.net/browse/DQG-1520?atlOrigin=eyJpIjoiYWRlZWU0MzRlMDU1NDQ4ZDg4YjFiODMzOGZiYTY5N2QiLCJwIjoiaiJ9
        // not updated by the server yet
        if (position <= 0)
        {
            positionTxt.text = "???";
        }
        else
        {
            positionTxt.text = $"{position}";
        }

        scoreTxt.text = $"{score}";
        usernameTxt.text = username;

        if (isLocalPlayer)
        {
            SetuPlayerDisplay();
        }
        else if (position <= 3)
        {
            SetupTop3Display();
        }
        else
        {
            SetuDefaultDisplay();
        }
    }

    private void OnDisable()
    {
        if (isPlayerEntry)
        {
            return;
        }

        this.ReleaseItem();
    }

    private void SetuPlayerDisplay()
    {
        backgroundImg.sprite = playerBackgroud;
        usernameTxt.color = playerUsernameColor;
        positionTxt.color = playerPositionColor;
        scoreTxt.color = playerScoreColor;
    }

    private void SetupTop3Display()
    {
        backgroundImg.sprite = top3Background;
        usernameTxt.color = top3UsernameColor;
        positionTxt.color = top3PositionColor;
        scoreTxt.color = top3ScoreColor;
    }

    private void SetuDefaultDisplay()
    {
        backgroundImg.sprite = defaultBackgroud;
        usernameTxt.color = defaultUsernameColor;
        positionTxt.color = defaultPositionColor;
        scoreTxt.color = defaultScoreColor;
    }
}
