using System.Collections;
using UnityEngine;

public class ColorSwapItem : MonoBehaviour
{
    private ScreenColorMask colorMask;
    private SpriteRenderer spriteRenderer;

    [Header("Sprite Settings")]
    [Tooltip("Sprite to display when pressed (leave empty to keep current)")]
    public Sprite pressedSprite;

    [Header("Behavior Settings")]
    
    [Tooltip("Show press animation for how long")]
    public float pressDelay = 0.3f;

    [Tooltip("Destroy the item after collection")]
    public bool destroyAfterCollection = true;

    [Tooltip("Delay before destroying (time to show pressed sprite)")]
    public float destroyDelay = 0.3f;

    [Header("Button Color Settings")]
    [Tooltip("Color Index (use -1 to cycle through colors instead)")]
    public int colorIndex = -1;



    void Start()
    {
        colorMask = FindFirstObjectByType<ScreenColorMask>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (colorMask == null)
        {
            Debug.LogWarning("ColorSwapItem: No ScreenColorMask found in scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (pressedSprite != null && spriteRenderer != null)
        {
            Sprite originalSprite = spriteRenderer.sprite;
            spriteRenderer.sprite = pressedSprite;
            if (destroyAfterCollection)
            {
                StartCoroutine(HideSpriteAfterDelay());
            }
            else
            {
                StartCoroutine(UnpressSpriteAfterDelay(originalSprite));
            }
        }

        if (colorMask != null)
        {
            colorMask.OnSwitch(colorIndex);
        }

        if (destroyAfterCollection)
        {
            Destroy(gameObject, destroyDelay);
        }
    }


    IEnumerator HideSpriteAfterDelay()
    {
        yield return new WaitForSeconds(pressDelay);
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
    
    IEnumerator UnpressSpriteAfterDelay(Sprite originalSprite)
    {
        yield return new WaitForSeconds(pressDelay);
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
