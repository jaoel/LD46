using UnityEngine;
public static class ColorPalette {
    public static float LEDEmissiveStrength = 5f;

    public static Vector3 OK = new Vector3(0f, 1f, 0f);
    public static Vector3 WARNING = new Vector3(1f, 1f, 0f);
    public static Vector3 BAD = new Vector3(0.5f, 0f, 0f);

    public static Vector3 GetLEDEmissiveColor(Vector3 color) {
        return color * LEDEmissiveStrength;
    }

    public static Vector3 GetLEDEmissiveColor(Color color) {
        return GetLEDEmissiveColor(new Vector3(color.r, color.g, color.b));
    }
}
