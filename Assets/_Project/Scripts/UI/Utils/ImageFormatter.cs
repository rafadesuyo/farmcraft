using UnityEngine;
using UnityEngine.UI;

public static class ImageFormatter
{
    public static void ChangeImageColor(Image image, string hexCode)
    {
        Color color = ColorUtility.TryParseHtmlString(hexCode, out Color newColor) ? newColor : Color.white;

        image.color = color;
    }

    public static void ChangeImageOpacity(Image image, float opacity)
    {
        Color currentColor = image.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);
        image.color = newColor;
    }
}