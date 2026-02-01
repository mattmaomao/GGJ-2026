using System.Collections;
using UnityEngine;

public class ColorSwapItem : MonoBehaviour
{
    private BackgroundSprite backgroundSprite;
    private SpriteRenderer spriteRenderer;

    [Header("Sprite Settings")]
    [Tooltip("Sprite to display when pressed (leave empty to keep current)")]
    public Sprite readySprite_Cycle;
    public Sprite pressedSprite_Cycle;
    public Sprite readySprite_Single;
    public Sprite pressedSprite_Single;

    [Header("Behavior Settings")]

    [Tooltip("Hide the item after anim")]
    public float hideSpriteDelay = 0.3f;

    [Tooltip("Destroy the item after collection")]
    public bool destroyAfterCollection = true;

    [Tooltip("Delay before destroying (time to show pressed sprite)")]
    public float destroyDelay = 0.3f;

    [Tooltip("Which color index the button will change it to (-1 if you want to cycle through colors instead)")]
    public int colorIndex = -1;



    void Start()
    {
        backgroundSprite = FindFirstObjectByType<BackgroundSprite>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (backgroundSprite == null)
        {
            Debug.LogWarning("ColorSwapItem: No BackgroundSprite found in scene!");
        }
    }

    public void Init(int colorIndex, bool destroyAfterCollection)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.colorIndex = colorIndex;
        if (colorIndex == -1)
        {
            spriteRenderer.color = Color.white;
        }
        else
            spriteRenderer.color = BackgroundSprite.Instance.colors[colorIndex >= 0 && colorIndex < BackgroundSprite.Instance.colors.Length ? colorIndex : 0];
        this.destroyAfterCollection = destroyAfterCollection;
        spriteRenderer.sprite = destroyAfterCollection ? readySprite_Single : readySprite_Cycle;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Sprite tempSprite = destroyAfterCollection ? pressedSprite_Single : pressedSprite_Cycle;
            if (tempSprite != null && spriteRenderer != null)
            {
                Sprite originalSprite = spriteRenderer.sprite;
                spriteRenderer.sprite = tempSprite;
                if (destroyAfterCollection)
                {
                    StartCoroutine(HideSpriteAfterDelay());
                }
                else
                {
                    StartCoroutine(UnpressSpriteAfterDelay(originalSprite));
                }

            }

            if (backgroundSprite != null)
            {
                AudioManager.Instance.PlaySE(AudioManager.SE_CLICK);
                backgroundSprite.OnSwitch(colorIndex);
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
    IEnumerator UnpressSpriteAfterDelay(Sprite originalSprite)
    {
        yield return new WaitForSeconds(hideSpriteDelay);
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
