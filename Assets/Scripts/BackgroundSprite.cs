using UnityEngine;

public class BackgroundSprite : MonoBehaviour
{
    [Header("Color Settings")]
    public Color[] colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    private PlatformColorType[] colorTypes = new PlatformColorType[]
    {
        PlatformColorType.Red,
        PlatformColorType.Blue,
        PlatformColorType.Green,
        PlatformColorType.Yellow,
        PlatformColorType.Cyan,
        PlatformColorType.Magenta
    };

    private int currentColorIndex = 0;
    private Camera mainCamera;
    private ColorMaskable[] allMaskables;

    public Color CurrentColor => colors[currentColorIndex];
    public PlatformColorType CurrentColorType => colorTypes[currentColorIndex];

    void Start()
    {
        mainCamera = Camera.main;
        allMaskables = FindObjectsByType<ColorMaskable>(FindObjectsSortMode.None);

        UpdateColor();
        UpdateAllMaskables();
    }

    public void ReLoadMaskableObj()
    {
        allMaskables = FindObjectsByType<ColorMaskable>(FindObjectsSortMode.None);
    }

    public void OnSwitch(int colorIndex)
    {
        if (colorIndex < 0 || colorIndex >= colors.Length)
        {
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
        else
        {
            currentColorIndex = colorIndex;
        }

        UpdateColor();
        UpdateAllMaskables();
    }

    private void UpdateColor()
    {
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = colors[currentColorIndex];
        }
    }

    private void UpdateAllMaskables()
    {
        PlatformColorType currentType = colorTypes[currentColorIndex];

        foreach (ColorMaskable maskable in allMaskables)
        {
            maskable.UpdateVisibility(currentType);
        }
    }
}
