using System;
using UnityEngine;
using TMPro;

public static class TextFormatter
{
    public static string InlineColor(string text, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }

    public static string TimeToMMSS(int time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return timeSpan.ToString(@"mm\:ss");
    }

    public static void ChangeTextColor(string hexCode, TextMeshProUGUI text)
    {
        Color color = ColorUtility.TryParseHtmlString(hexCode, out Color newColor) ? newColor : Color.white; 

        text.color = color;
    }
}
