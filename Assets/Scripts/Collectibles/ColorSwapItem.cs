using UnityEngine;

public class ColorSwapItem : MonoBehaviour
{
    private ScreenColorMask colorMask;

    [Header("Settings")]
    [Tooltip("Destroy the item after collection")]
    public bool destroyAfterCollection = true;

    void Start()
    {
        colorMask = FindFirstObjectByType<ScreenColorMask>();

        if (colorMask == null)
        {
            Debug.LogWarning("ColorSwapItem: No ScreenColorMask found in scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (colorMask != null)
            {
                colorMask.OnSwitch();
            }

            if (destroyAfterCollection)
            {
                Destroy(gameObject);
            }
        }
    }
}
