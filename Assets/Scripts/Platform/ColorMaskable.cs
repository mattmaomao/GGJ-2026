using UnityEngine;

public enum PlatformColorType
{
    Black,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Magenta,
    Null
}

public class ColorMaskable : MonoBehaviour
{
    [Header("Color Mask Settings")]
    public PlatformColorType colorType = PlatformColorType.Red;

    Renderer renderer;
    Collider2D collider;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider2D>();
    }

    public void UpdateVisibility(PlatformColorType maskColorType)
    {
        bool shouldBeActive = colorType != maskColorType;

        if (renderer != null)
            renderer.enabled = shouldBeActive;
        if (collider != null)
            collider.enabled = shouldBeActive;
    }
}
