using UnityEngine;

public enum PlatformColorType
{
    Red,
    Blue,
    Green,
    Yellow,
    Cyan,
    Magenta
}

public class ColoredPlatform : MonoBehaviour
{
    [Header("Platform Color")]
    public PlatformColorType colorType = PlatformColorType.Red;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }
    
    public void UpdateVisibility(PlatformColorType maskColorType)
    {
        // Platform is disabled when color type matches the mask
        bool shouldBeActive = colorType != maskColorType;

        if (spriteRenderer != null)
            spriteRenderer.enabled = shouldBeActive;
        if (platformCollider != null)
            platformCollider.enabled = shouldBeActive;
    }
}