using UnityEngine;

public class BackgroundSprite : MonoBehaviour
{
    // instance
    public static BackgroundSprite Instance { get; private set; }

    [Header("Color Settings")]
    public Color[] colors = new Color[]
    {
        Color.red,
        Color.orange,
        Color.yellow,
        Color.green,
        Color.blue,
        Color.magenta
    };

    private PlatformColorType[] colorTypes = new PlatformColorType[]
    {
        PlatformColorType.Red,
        PlatformColorType.Orange,
        PlatformColorType.Yellow,
        PlatformColorType.Green,
        PlatformColorType.Blue,
        PlatformColorType.Magenta
    };

    private int currentColorIndex = 0;
    private Camera mainCamera;
    private ColorMaskable[] allMaskables;

    public Color CurrentColor => colors[currentColorIndex];
    public PlatformColorType CurrentColorType => colorTypes[currentColorIndex];

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        mainCamera = Camera.main;
        allMaskables = FindObjectsByType<ColorMaskable>(FindObjectsSortMode.None);

        UpdateColor();
        // UpdateAllMaskables();
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

        if (allMaskables != null)
            foreach (ColorMaskable maskable in allMaskables)
            {
                maskable.UpdateVisibility(currentType);
            }
    }
}
