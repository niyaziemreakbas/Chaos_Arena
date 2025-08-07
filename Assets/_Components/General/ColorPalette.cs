using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "UI/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    public Color background;
    public Color primary;
    public Color secondary;
    public Color accent;
    public Color text;
}