using UnityEngine;
using UnityEngine.UI;

public class ScreenColorMask : MonoBehaviour
{
    private Image screenMask;
    
    [Header("Color Settings")]
    public Color[] colors = new Color[6];
    
    [Range(0f, 1f)]
    public float opacity = 0.3f;
    
    private int currentColorIndex = 1;
    
    void Start()
    {
        screenMask = GetComponent<Image>();
        
        if (colors.Length == 0 || colors[0] == Color.clear)
        {
            colors = new Color[]
            {
                Color.red,
                Color.blue,
                Color.green,
                Color.yellow,
                Color.cyan,
                Color.magenta
            };
        }

        Color initialColor = colors[currentColorIndex];
        initialColor.a = opacity;
        screenMask.color = initialColor;
    }
    
    public void OnSwitch()
    {
        // Move to next color first
        currentColorIndex = (currentColorIndex + 1) % colors.Length;

        // Then apply the new color
        Color selectedColor = colors[currentColorIndex];
        selectedColor.a = opacity;
        screenMask.color = selectedColor;
    }
}