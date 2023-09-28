using System.Collections;
using TMPro;
using UnityEngine;

public class TimeElapsed : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TextMeshProUGUI _timeElapsedText;
    private float _timeElapsedSeconds;

    private void Start()
    {
        //Load last timer of gameplay.
        _timeElapsedText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateElapsedTime());
    }

    private IEnumerator UpdateElapsedTime()
    {
        while (true)
        {
            int hours = Mathf.FloorToInt(_timeElapsedSeconds / 3600f);
            int minutes = Mathf.FloorToInt(_timeElapsedSeconds % 3600f / 60f);
            int seconds = Mathf.FloorToInt(_timeElapsedSeconds % 60f);

            _timeElapsedText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            yield return new WaitForSeconds(1f);

            _timeElapsedSeconds += 1f;
        }
    }

    public void LoadData(GameData gameData)
    {
        _timeElapsedSeconds = gameData.TimeSpent;
    }

    public void SaveData(GameData gameData)
    {
        gameData.TimeSpent = _timeElapsedSeconds;
    }
}