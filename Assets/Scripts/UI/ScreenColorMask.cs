using UnityEngine;
using UnityEngine.UI;

public class ScreenColorMask : MonoBehaviour
{
    private Image screenMask;
    
    [Header("Color Settings")]
    public Color[] colors = new Color[6];

    [Range(0f, 1f)]
    public float opacity = 0.3f;

    private int currentColorIndex = 0;

    // Maps index to color type (must match colors array order)
    private PlatformColorType[] colorTypes = new PlatformColorType[]
    {
        PlatformColorType.Red,
        PlatformColorType.Blue,
        PlatformColorType.Green,
        PlatformColorType.Yellow,
        PlatformColorType.Cyan,
        PlatformColorType.Magenta
    };
    
    // Add this
    private ColoredPlatform[] allPlatforms;
    
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
        
        // Find all platforms in the scene (updated API)
        allPlatforms = FindObjectsByType<ColoredPlatform>(FindObjectsSortMode.None);
        
        Color initialColor = colors[currentColorIndex];
        initialColor.a = opacity;
        screenMask.color = initialColor;
        
        // Update platforms with initial color
        UpdateAllPlatforms();
    }
    
    public void OnSwitch(int colorIndex)
    {
        if(colorIndex < 0 || colorIndex >= colors.Length)
        {
            // Move to next color first
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
        else 
        {
            currentColorIndex = colorIndex;
        }
                
        // Then apply the new color
        Color selectedColor = colors[currentColorIndex];
        selectedColor.a = opacity;
        screenMask.color = selectedColor;
        
        // Update all platforms
        UpdateAllPlatforms();
    }
    
    private void UpdateAllPlatforms()
    {
        PlatformColorType currentType = colorTypes[currentColorIndex];

        foreach (ColoredPlatform platform in allPlatforms)
        {
            platform.UpdateVisibility(currentType);
        }
    }
}
