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
    
    [Tooltip("Hide the item after anim")]
    public float hideSpriteDelay = 0.3f;

    [Tooltip("Destroy the item after collection")]
    public bool destroyAfterCollection = true;

    [Tooltip("Delay before destroying (time to show pressed sprite)")]
    public float destroyDelay = 0.3f;




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
        if (other.CompareTag("Player"))
        {
            if (pressedSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = pressedSprite;
                StartCoroutine(HideSpriteAfterDelay());
            }

            if (colorMask != null)
            {
                colorMask.OnSwitch();
            }

            if (destroyAfterCollection)
            {
                Destroy(gameObject, destroyDelay);
            }
        }
    }

    IEnumerator HideSpriteAfterDelay()
    {
        yield return new WaitForSeconds(hideSpriteDelay);
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
