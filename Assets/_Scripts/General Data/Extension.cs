using UnityEngine;

public static class Extension
{

    public static string ColorToHex(this Color color) 
        =>  $"{(byte)(color.r * 255):X2}" +
            $"{(byte)(color.g * 255):X2}" +
            $"{(byte)(color.b * 255):X2}";

    public static Color HexToColor(this string hex)
    {
        string hexFormat = "#" + hex;
        Color color = new Color();
        if (ColorUtility.TryParseHtmlString(hexFormat, out color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning("Invalid hex color format.");
            return Color.white; // Hoặc màu mặc định khác
        }
    }

    public static float Percent(this int value) => value / 100.0f;

 


}

